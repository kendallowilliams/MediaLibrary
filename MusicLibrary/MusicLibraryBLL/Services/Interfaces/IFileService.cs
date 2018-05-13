using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<string>> GetDirectories(string path);

        Task<IEnumerable<string>> GetSubDirectories(string path);

        Task<IEnumerable<string>> EnumerateFiles(string path, string searchPattern = null);

        Task Write(string path, string data);

        Task Write(string path, byte[] data);
    }
}
