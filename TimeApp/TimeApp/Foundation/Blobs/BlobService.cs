using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace TimeApp.Foundation.Blobs
{
    public sealed class BlobService : IBlobService
    {
        private const string AccountName = "azuretestdev9e8a";
        private const string SasToken = "";


        public async Task UploadImageAsync(Stream image, string filename)
        {
            var client = GetBlobServiceClient(AccountName);
            var containerClient = client.GetBlobContainerClient("images");
            await containerClient.UploadBlobAsync(filename, image);
        }


        private static BlobServiceClient GetBlobServiceClient(string accountName)
        {
            var client = new BlobServiceClient(new Uri($"https://{accountName}.blob.core.windows.net?{SasToken}"));

            return client;
        }
    }
}