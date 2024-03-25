using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using TimeApp.Options;

namespace TimeApp.Foundation.Blobs
{
    public sealed class BlobService : IBlobService
    {
        private const int PageSize = 50;

        private readonly AzureBlobOptions _options;


        public BlobService(IOptions<AzureBlobOptions> options)
        {
            _options = options.Value;
        }


        public async Task UploadImageAsync(Stream image, string filename)
        {
            var containerClient = GetBlobContainerClient();
            await containerClient.UploadBlobAsync(filename, image);
        }

        public async Task DeleteImageAsync(string filename)
        {
            var containerClient = GetBlobContainerClient();
            await containerClient.GetBlobClient(filename).DeleteIfExistsAsync();
        }

        public async Task DeleteAllImagesExceptAsync(IReadOnlyCollection<string> fileNames)
        {
            var containerClient = GetBlobContainerClient();
            var resultSegment = containerClient.GetBlobsAsync().AsPages(default, PageSize);
            var images = new List<BlobItem>();
            try
            {
                await foreach (var blobPage in resultSegment)
                {
                    images.AddRange(blobPage.Values);
                }

                images = images.Where(i => !fileNames.Contains(i.Name)).ToList();
                foreach (var image in images)
                {
                    await containerClient.GetBlobClient(image.Name).DeleteIfExistsAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        private BlobContainerClient GetBlobContainerClient()
        {
            var client = new BlobServiceClient(new Uri($"https://{_options.AccountName}.blob.core.windows.net?{_options.SasKey}"));
            var containerClient = client.GetBlobContainerClient("images");

            return containerClient;
        }
    }
}