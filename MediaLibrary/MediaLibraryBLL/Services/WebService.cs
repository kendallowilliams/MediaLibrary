using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Fody;
using MediaLibraryBLL.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace MediaLibraryBLL.Services
{
    [ConfigureAwait(false)]
    [Export(typeof(IWebService))]
    public class WebService : IWebService
    {
        [ImportingConstructor]
        public WebService()
        {
        }

        public async Task<byte[]> DownloadData(string address)
        {
            TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();

            using (WebClient client = new WebClient())
            {
                try
                {
                    Uri uri = new Uri(address);

                    client.DownloadDataCompleted += (sender, args) =>
                    {
                        if (args.Error == null) { tcs.SetResult(args.Result); }
                        else { tcs.SetException(args.Error); }
                    };
                    client.DownloadDataAsync(uri);
                }
                catch(Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            return await tcs.Task;
        }

        public async Task<bool> DownloadFile(string address, string filename)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            using (WebClient client = new WebClient())
            {
                try
                {
                    Uri uri = new Uri(address);

                    client.DownloadFileCompleted += (sender, args) =>
                    {
                        if (args.Error == null) { tcs.SetResult(true); }
                        else { tcs.SetException(args.Error); }
                    };
                    client.DownloadFileAsync(uri, filename);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }

            return await tcs.Task;
        }

        public async Task<T> Get<T>(Uri baseUri, string relativePath, string username, string password)
        {
            T results = default;
            HttpResponseMessage response = default;
            string credentials = $"{username}:{password}",
                   authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                response = await client.GetAsync(relativePath);

                if (response.IsSuccessStatusCode)
                {
                    results = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                }
            }

            return results;
        }

        public async Task<bool> IsAuthorized(Uri baseUri, string relativePath, string username, string password)
        {
            bool authorized = false;
            HttpResponseMessage response = default;
            string credentials = $"{username}:{password}",
                   authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

            using (var client = new HttpClient())
            {
                client.BaseAddress = baseUri;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                response = await client.GetAsync(relativePath);
                authorized = response.IsSuccessStatusCode;
            }

            return authorized;
        }
    }
}