using MediaLibraryDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryBLL.Services.Interfaces
{
    public interface IWebService
    {
        Task<byte[]> DownloadData(string address);
        Task<bool> DownloadFile(string address, string filename);
        Task<T> Get<T>(Uri baseUri, string relativePath, string username = default, string password = default);
        Task<bool> IsAuthorized(Uri baseUri, string relativePath, string username = default, string password = default);
    }
}
