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
        string MusicFolder { get; }

        string PodcastFolder { get; }

        string RootFolder { get; }

        Task Write(string path, string data);

        Task Write(string path, byte[] data);

        Task ReadDirectory(Transaction transaction, string path, bool recursive = true);

        Task CheckForMusicUpdates(Transaction transaction);

        Task ReadMediaFile(string path);

        void Delete(string path);
    }
}
