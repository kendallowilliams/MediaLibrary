using MediaLibraryDAL.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IFileService
    {
        Task Write(string path, string data);

        Task Write(string path, byte[] data);

        Task ReadDirectory(Transaction transaction, string path, bool recursive = true, bool copyFiles = false);

        Task CheckForMusicUpdates(Transaction transaction);
    }
}
