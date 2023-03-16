using Azure.Storage.Blobs;
namespace AzureService.AzureStorage
{
    public static class BlobExtensions
    {
        public static async Task<BlobContainerClient> GetContainer(string connectionString, string containerName)
        {
            BlobServiceClient blobServiceClient = new(connectionString);
           
            var blobcontainer = blobServiceClient.GetBlobContainerClient(containerName);
            if (!blobcontainer.Exists())
            {
                return await blobServiceClient.CreateBlobContainerAsync(containerName);
            }
            return blobcontainer;
        }
    }
}
