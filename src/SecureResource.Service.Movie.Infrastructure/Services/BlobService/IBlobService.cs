using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.API.Services.BlobService
{
    public interface IBlobService
    {
        public Task<BlobDownloadInfo> GetBlobAsync(string id);

        public Task<List<string>> GetAllIdsAsync();

        public Task UploadAsync(IFormFile content, string fileName = null);

        public Task DeleteAsync(string id);
    }
}
