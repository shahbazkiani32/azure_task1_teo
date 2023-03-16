using AzureService.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureService
{
    public interface IAzureLocalStorage
    {
        Task<bool> CreateAzureStorageTableIfNotExist(string storageconn, string tableName);

        Task<TableResult> InsertApiLogs(string storageconn, string tableName, ApiLogs entity);
        Task<List<ApiLogs>> GetApiLogs(string storageconn, string tableName, DateTime from, DateTime to);

        Task<bool> CreateNWriteBlobIfNotExist(string connectionString, string containerName, string fileName, string content, string logKey);

        Task<List<ApiData>> GetApiData(string connectionString, string containerName, string fileName, string logid);


    }
}
