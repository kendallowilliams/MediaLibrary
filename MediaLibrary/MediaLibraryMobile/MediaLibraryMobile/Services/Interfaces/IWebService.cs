using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryMobile.Services.Interfaces
{
    public interface IWebService
    {
        Task<IEnumerable<T>> Get<T>(Uri baseUri, string relativePath, string username = default, string password = default);
        Task<bool> IsAuthorized(Uri baseUri, string relativePath, string username = default, string password = default);
    }
}
