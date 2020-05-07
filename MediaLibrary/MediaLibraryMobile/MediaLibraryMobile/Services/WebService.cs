using MediaLibraryMobile.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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

        public async Task<IEnumerable<T>> Get<T>(string baseUrl, string relativePath)
        {
            IEnumerable<T> results = Enumerable.Empty<T>();
            HttpResponseMessage response = default;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "");
                response = await client.GetAsync(relativePath);

                if (response.IsSuccessStatusCode)
                {
                    results = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
                }
            }

            return results;
        }
    }
}
