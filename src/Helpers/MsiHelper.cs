// Build request to acquire MSI token

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src
{
    static class MsiHelper
    {
        private static string BaseUrl = "http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=";
        public static async Task<string> GetToken(string resource)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Metadata", "true");

            var response = await client.GetAsync(BaseUrl + resource);
            var bodyContent = await response.Content.ReadAsStringAsync();
            var list = (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(bodyContent);
            string accessToken = list["access_token"];
            return accessToken;
        }
    }
}