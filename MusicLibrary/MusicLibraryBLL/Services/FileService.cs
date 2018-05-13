using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MusicLibraryBLL.Services
{
    [Export(typeof(IFileService))]
    public class FileService : IFileService
    {
        [ImportingConstructor]
        public FileService()
        {
        }

        public async Task<IEnumerable<string>> GetDirectories(string path)
        {
            return await Task.Run(() => Directory.GetDirectories(path));
        }

        public async Task<IEnumerable<string>> GetSubDirectories(string path)
        {
            IEnumerable<string> directories = await Task.Run(() => Directory.GetDirectories(path));

            foreach(string directory in directories)
            {
                directories = directories.Concat(await GetSubDirectories(path));
            }

            return directories;
        }

        public async Task<IEnumerable<string>> EnumerateFiles(string path, string searchPattern = null)
        {
            return await Task.Run(() => Directory.EnumerateFiles(path, searchPattern));
        }

        public async Task Write(string path, byte[] data)
        {
            await Task.Run(() => File.WriteAllBytes(path, data));
        }

        public async Task Write(string path, string data)
        {
            await Task.Run(() => File.WriteAllText(path, data));
        }
    }
}