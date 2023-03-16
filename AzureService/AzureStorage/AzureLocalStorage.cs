using AzureService.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Azure.Storage.Blobs;
using AzureService.AzureStorage;
using System.Data;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using System.Text;

namespace AzureService
{
    public class AzureLocalStorage : IAzureLocalStorage
    {
        public async Task<bool> CreateAzureStorageTableIfNotExist(string storageconn, string tableName)
        {
            try
            {
                CloudStorageAccount storageAcc = CloudStorageAccount.Parse(storageconn);
                CloudTableClient tblclient = storageAcc.CreateCloudTableClient();
                CloudTable table = tblclient.GetTableReference(tableName);
                await table.CreateIfNotExistsAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TableResult> InsertApiLogs(string storageconn, string tableName, ApiLogs entity)
        {
            try
            {
                CloudStorageAccount storageAcc = CloudStorageAccount.Parse(storageconn);
                CloudTableClient tblclient = storageAcc.CreateCloudTableClient();
                CloudTable table = tblclient.GetTableReference(tableName);
                return await table.ExecuteAsync(TableOperation.Insert(entity));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateNWriteBlobIfNotExist(string connectionString, string containerName, string fileName, string content, string logKey)
        {
            try
            {
                var container = BlobExtensions.GetContainer(connectionString, containerName);
                await AppendBlobFile(container.Result, connectionString, containerName, fileName, content, logKey);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public async Task<List<ApiData>> GetApiData(string connectionString, string containerName, string fileName, string logid)
        {
            var container = await BlobExtensions.GetContainer(connectionString, containerName);
            BlobClient blobClient = container.GetBlobClient(fileName);
            if (blobClient.Exists())
            {
                var content = await blobClient.DownloadContentAsync();
                ApiPayLoad apiResult = JsonConvert.DeserializeObject<ApiPayLoad>(content.Value.Content.ToString());
                if (!string.IsNullOrWhiteSpace(logid))
                return apiResult?.PayLoadData?.Where(x => x.Logkey == logid).ToList();
                else
                 return apiResult.PayLoadData;
                //return null;
            }
            else
            {
                return null;
            }

        }

        private async Task AppendBlobFile(BlobContainerClient blobServiceClient, string connectionString, string containerName, string fileName, string content, string logkey)
        {
            //AS I am using storage emulator so append is not supported by sorage emulator that's why using upload

            BlobClient blobClient = blobServiceClient.GetBlobClient(fileName);
            if (!blobClient.Exists())
            {                                                                                                                                                                 
                string trimedContent = content.TrimStart('{'); 
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(@"{""payLoadData"":[{" + trimedContent.Remove(trimedContent.LastIndexOf('}'), 1) + @",""logkey"":""" + logkey +"\"}]}")))
                {
                    await blobClient.UploadAsync(ms);
                }
                return;
            }
            var data = await blobClient.DownloadContentAsync();
            string oldcontent = data.Value.Content.ToString();
            string trimedoldcontent = oldcontent.Remove(oldcontent.LastIndexOf('}'), 1).TrimEnd(']');
            string updatedcontent = trimedoldcontent + "," + content.Remove(content.LastIndexOf('}'), 1) + @",""logkey"":""" + logkey + "\"}]}";
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(updatedcontent)))
            {
                await blobClient.DeleteIfExistsAsync();
                await blobClient.UploadAsync(ms);
            }
        }
        public static Stream Append(Stream destination, Stream source)
        {
            destination.Position = destination.Length;
            source.CopyTo(destination);
            destination.Position = 0;
            return destination;
        }

        public async Task<List<ApiLogs>> GetApiLogs(string storageconn, string tableName, DateTime from, DateTime to)
        {
            try
            {
                CloudStorageAccount storageAcc = CloudStorageAccount.Parse(storageconn);
                var cloudTableClient = storageAcc.CreateCloudTableClient();
                var mytable = cloudTableClient.GetTableReference(tableName);

                var query = new TableQuery<ApiLogs>();
                var enties = await mytable.ExecuteQuerySegmentedAsync(query, null);
                return enties.Results.Where(x => x.Timestamp.Date >= from && x.Timestamp.Date <= to).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
