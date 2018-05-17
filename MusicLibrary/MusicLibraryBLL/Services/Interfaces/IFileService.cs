using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicLibraryBLL.Services.Interfaces
{
    public interface IFileService
    {
        Task<IEnumerable<string>> EnumerateDirectories(string path, string searchPattern = "*", bool recursive = false);

        Task<IEnumerable<string>> EnumerateFiles(string path, string searchPattern = "*", bool recursive = false);

        Task Write(string path, string data);

        Task Write(string path, byte[] data);

        Task ReadDirectory(string path, bool recursive = true, bool copyFiles = false);

        Task ReadMediaFile(string path, bool copyFiles = false);
    }
}
