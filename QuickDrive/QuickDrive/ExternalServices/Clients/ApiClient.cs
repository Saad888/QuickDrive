using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Newtonsoft.Json;

namespace QuickDrive.ExternalServices.ServiceClients
{
    public static class ApiClient
    { 
        public static HttpClient Client = new HttpClient();

        public async static Task<T> QueryService<T>(string url, Dictionary<string, string> values, string auth_token = null)
        {
            // Content
            var content = new FormUrlEncodedContent(values);

            // Response
            var response = await Client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Var object
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            }

            throw new HttpRequestException($"Request Failed - {await response.Content.ReadAsStringAsync()}");
        }
    }

}
