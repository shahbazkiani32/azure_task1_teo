using System;
using AzureService.AzureHttpClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Task_AzureStorageJob
{
    public class Function1
    {
        private readonly AzureHttpClientClass _client;
        public Function1(AzureHttpClientClass client)
        {
            _client = client;
        }
        [FunctionName("Function1")]
        public void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _client.GetData();
        }
    }
}
