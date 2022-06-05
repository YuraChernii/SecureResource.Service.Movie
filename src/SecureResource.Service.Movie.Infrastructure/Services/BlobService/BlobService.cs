using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.API.Services.BlobService
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient blobServiceClient;
        private readonly string containerName = "movies-container";
        public BlobService(BlobServiceClient blobServiceClient)
        {
            this.blobServiceClient = blobServiceClient;
        }
        public async Task DeleteAsync(string id)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(id);

            await blobClient.DeleteIfExistsAsync();

        }

        public Task<List<string>> GetAllIdsAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<BlobDownloadInfo> GetBlobAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task UploadAsync(IFormFile content, string fileName = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
