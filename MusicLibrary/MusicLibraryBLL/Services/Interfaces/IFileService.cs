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
    }
}
