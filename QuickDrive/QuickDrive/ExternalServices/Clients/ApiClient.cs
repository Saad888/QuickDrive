using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Linq;
using System.Net.Http.Headers;

using Newtonsoft.Json;

namespace QuickDrive.ExternalServices.ServiceClients
{
    public static class ApiClient
    {
        public static HttpClient Client = new HttpClient();
        public static HttpClient Client308 = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });

        static ApiClient()
        {
            Client.Timeout = new TimeSpan(0, 0, 2);
        }

        public async static Task<T> QueryService<T>(string url, HttpMethod method = null, Dictionary<string, string> queryParams = null, object data = null, string auth_token = null, byte[] uploadData = null, ContentRangeHeaderValue contentRange = null, bool Expect308 = false)
        {
            var response = await QueryService(url, method, queryParams, data, auth_token, uploadData, contentRange, Expect308);

            if (response.IsSuccessStatusCode)
            {
                // Var object
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }

            throw new HttpRequestException($"Request Failed - {await response.Content.ReadAsStringAsync()}");
        }

        public async static Task<HttpResponseMessage> QueryService(string url, HttpMethod method = null, Dictionary<string, string> queryParams = null, object data = null, string auth_token = null, byte[] uploadData = null, ContentRangeHeaderValue contentRange = null, bool Expect308 = false)
        {
            // Generate URL with query string
            if (queryParams != null)
            {
                var builder = new UriBuilder(url);
                builder.Port = -1;
                var query = HttpUtility.ParseQueryString(builder.Query);
                foreach (var key in queryParams.Keys)
                    query[key] = queryParams[key];
                builder.Query = query.ToString();
                url = builder.ToString();
            }

            // Get request
            var request = new HttpRequestMessage(method ?? HttpMethod.Get, url);

            // Form Request Data
            if (data != null)
            {
                var body = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            // File upload data (warning: will override json body)
            if (uploadData != null)
                request.Content = new ByteArrayContent(uploadData);

            // Content Range
            if (contentRange != null)
            {
                if (request.Content == null)
                    request.Content = new StringContent("");
                request.Content.Headers.ContentRange = contentRange;
            }

            // Add Authorization header
            if (auth_token != null)
                request.Headers.Add("Authorization", auth_token);

            // Request
            if (Expect308)
                return await Client308.SendAsync(request);
            
            // Return response
            return await Client.SendAsync(request);
        }
    }

}
