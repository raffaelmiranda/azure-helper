using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Helper.Storage
{
    public class AzureStorageBlob: IFileService
    {
        private BlobClient _blob;
        private readonly BlobContainerClient _container;

        public AzureStorageBlob(string connectionString, string containerName)
        {
            _container = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<byte[]> DownloadAsync(string directoryName, string targetFileName, string sourceFileName = null)
        {
            _blob = _container.GetBlobClient($"{directoryName}/{targetFileName}");
            var downloaded = await _blob.DownloadAsync();
            byte[] byteArray = new byte[downloaded.Value.ContentLength];
            using (var ms = new MemoryStream())
            {
                int bit;
                while ((bit = downloaded.Value.Content.Read(byteArray, 0, byteArray.Length)) > 0)
                {
                    ms.Write(byteArray, 0, bit);
                }
                return ms.ToArray();
            }
        }

        public async Task<IEnumerable<string>> GetAllAsync(string directoryName, string targetFileName)
        {
            var files = new List<string>();
            await foreach (var blob in _container.GetBlobsAsync(prefix: $"{directoryName}/{targetFileName}"))
            {
                files.Add(blob.Name);
            }

            return files;
        }

        public async Task<bool> RemoveAllFilesAsync(string directoryName)
        {
            _blob = _container.GetBlobClient($"{directoryName}");
            var removed = await _blob.DeleteAsync();
            return removed.Status == 202;
        }

        public async Task<bool> RemoveAsync(string directoryName, string targetFileName)
        {
            _blob = _container.GetBlobClient($"{directoryName}/{targetFileName}");
            var removed = await _blob.DeleteAsync();
            return removed.Status == 202;
        }

        public async Task<bool> UploadAsync(string directoryName, string targetFileName, byte[] sourceFileBytes)
        {
            _container.CreateIfNotExists();
            _blob = _container.GetBlobClient($"{directoryName}/{targetFileName}");
            bool uploaded;
            using (var ms = new MemoryStream(sourceFileBytes, false))
            {
                var uploadeAsync = await _blob.UploadAsync(ms, true);

                uploaded = uploadeAsync.GetRawResponse().Status == 201;
            }

            return uploaded;
        }

        public async Task<bool> FileExistAsync(string directoryName, string targetFileName)
        {
            _blob = _container.GetBlobClient($"{directoryName}/{targetFileName}");
            var exists = await _blob.ExistsAsync();
            return exists;
        }
    }
}
