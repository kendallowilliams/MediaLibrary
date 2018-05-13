using MusicLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TagLib;

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
            IEnumerable<string> directories = await Task.Run(() => Directory.GetDirectories(path));

            foreach(string directory in directories)
            {
                directories = directories.Concat(await GetDirectories(path));
            }

            return directories;
        }
    }
}