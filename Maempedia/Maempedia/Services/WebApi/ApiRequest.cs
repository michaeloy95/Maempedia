using Maempedia.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Maempedia.Services.WebApi
{
    public class ApiRequest
    {
#if PRODUCTION
        private const string BASE_URL = "https://api.maempedia.com/";
#else
        private const string BASE_URL = "https://dev.maempedia.com/";
#endif

        private const string API_KEY = Constant.MAEMPEDIA_WEB_API_KEY;

        private const int TIMEOUT = 5;

        private readonly HttpClient client;

        public ApiRequest()
        {
            this.client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(TIMEOUT)
            };
        }

        protected async Task<JObject> GetFromMaempedia(string endpoint)
        {
            var response = await this.client.GetAsync(
                this.ConstructUri(endpoint));

            return await HandleHttpResponse(response);
        }

        protected async Task<JObject> PostToMaempedia(string endpoint, IDictionary<string, string> values)
        {
            var formContents = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(
                this.ConstructUri(endpoint),
                formContents);

            return await HandleHttpResponse(response);
        }

        protected async Task<JObject> PostMediaToMaempedia(string endpoint, byte[] media, string id)
        {
            var byteArrayContent = new ByteArrayContent(media);
            byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");

            var formContents = new MultipartFormDataContent
            {
                { byteArrayContent, "image_main", $"{id}b.jpg" },
                { new StringContent(id), "id" }
            };

            var response = await client.PostAsync(
                this.ConstructUri(endpoint),
                formContents);

            return await HandleHttpResponse(response);
        }

        private async Task<JObject> HandleHttpResponse(HttpResponseMessage response)
        {
            if (!await ResponseIsValid(response))
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            return TryParseJson(content);
        }

        private async Task<bool> ResponseIsValid(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Maempedia HTTP request failed.");
                return false;
            }

            var content = await response.Content.ReadAsStringAsync();
            if (content == null)
            {
                Debug.WriteLine("Maempedia API has returned nothing.");
                return false;
            }

            return true;
        }

        private JObject TryParseJson(string content)
        {
            try
            {
                return JObject.Parse(content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return null;
            }
        }

        private string ConstructUri(string endpoint)
        {
            var connector = endpoint.EndsWith("html") ? "?" : "&";
            return $"{BASE_URL}{endpoint}{connector}key={API_KEY}";
        }
    }
}