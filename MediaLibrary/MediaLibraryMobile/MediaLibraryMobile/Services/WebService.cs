using MediaLibraryMobile.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MediaLibraryMobile.Services
{
    [Export(typeof(IWebService))]
    public class WebService : IWebService
    {
        [ImportingConstructor]
        public WebService()
        { 

        }

        public async Task<IEnumerable<T>> Get<T>(Uri baseUri, string relativePath, string username, string password)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();
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
                    results = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
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
