using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Helper.Storage
{
    internal interface IFileService
    {
        Task<bool> UploadAsync(string directoryName, string targetFileName, byte[] sourceFileBytes);
        Task<byte[]> DownloadAsync(string directoryName, string targetFileName, string sourceFileName = null);
        Task<IEnumerable<string>> GetAllAsync(string directoryName, string targetFileName);
        Task<bool> RemoveAllFilesAsync(string directoryName);
        Task<bool> RemoveAsync(string directoryName, string targetFileName);
        Task<bool> FileExistAsync(string directoryName, string targetFileName);
    }
}
