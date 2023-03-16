using AzureService;
using AzureService.AzureHttpClient;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;


[assembly: FunctionsStartup(typeof(Task_AzureStorageJob.Startup))]
namespace Task_AzureStorageJob
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<AzureHttpClientClass>();
            builder.Services.AddSingleton<IAzureLocalStorage, AzureLocalStorage>();
        }
    }
}
