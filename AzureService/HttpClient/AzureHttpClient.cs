using AzureService.Models;
using Newtonsoft.Json;

namespace AzureService.AzureHttpClient
{
    public class AzureHttpClientClass
    {
        static readonly HttpClient client = new HttpClient();
        private readonly IAzureLocalStorage _azureStorage;

        public AzureHttpClientClass(IAzureLocalStorage azureStorage)
        {
            _azureStorage = azureStorage;
        }

        public async Task GetData()
        {
            ApiLogs entity;
            var tableName = "ApiLoggingTable";
            var containerName = "api-logging-container";
            var blobFileName = "apiloggingfile";
            var storageConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                using HttpResponseMessage response = await client.GetAsync("https://api.publicapis.org/random?auth=null");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    entity = new ApiLogs
                    {
                        PartitionKey = Guid.NewGuid().ToString(),
                        RowKey = Guid.NewGuid().ToString(),
                        Error = string.Empty,
                        Success = true,
                        Timestamp = DateTime.UtcNow

                    };
                    await _azureStorage.CreateAzureStorageTableIfNotExist(storageConnectionString, tableName);
                    await _azureStorage.InsertApiLogs(storageConnectionString, tableName, entity);
                    await _azureStorage.CreateNWriteBlobIfNotExist(storageConnectionString, containerName, blobFileName, responseBody,entity.PartitionKey);
                }
                else
                {
                    entity = new ApiLogs()
                    {
                        PartitionKey = Guid.NewGuid().ToString(),
                        RowKey = Guid.NewGuid().ToString(),
                        Error = "Api Error",
                        Success = false,
                        Timestamp = DateTime.UtcNow

                    };
                    await _azureStorage.CreateAzureStorageTableIfNotExist(storageConnectionString, tableName);
                    await _azureStorage.InsertApiLogs(storageConnectionString, tableName, entity);
                }
            }
            catch (HttpRequestException e)
            {
                entity = new ApiLogs
                {
                    PartitionKey = Guid.NewGuid().ToString(),
                    RowKey = Guid.NewGuid().ToString(),
                    Error = e.Message,
                    Success = false,
                    Timestamp = DateTime.UtcNow

                };
                await _azureStorage.CreateAzureStorageTableIfNotExist(storageConnectionString, tableName);
                await _azureStorage.InsertApiLogs(storageConnectionString, tableName, entity);
            }
        }
    }
}
