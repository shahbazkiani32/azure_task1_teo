using AzureService;
using AzureService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Task_AzureStorageApi.Controllers
{
    public class AzureController : Controller
    {
        private readonly IAzureLocalStorage _azureLocalStorage;
        private readonly static string tableName = "ApiLoggingTable";
        private readonly static string containerName = "api-logging-container";
        private readonly static string staticblobFileName = "apiloggingfile";
        private readonly static string blobFileName = "apiloggingfile";
        private readonly static string storageConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
        public AzureController(IAzureLocalStorage azureLocalStorage)
        {
            _azureLocalStorage = azureLocalStorage;
        }
        [HttpGet("GetLogs")]
        public async Task<List<ApiLogs>> GetLogs(DateTime from, DateTime to)
        {
            return await _azureLocalStorage.GetApiLogs(storageConnectionString, tableName, from, to);
        }
        [HttpGet("GetPayLoad")]
        public async Task<List<ApiData>> GetPayLoad(string log_id)
        {
            return await _azureLocalStorage.GetApiData(storageConnectionString, containerName, blobFileName, log_id);
        }

    }
}
