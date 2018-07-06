using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace src
{
    static class CosmosHelper
    {
        private static string KeyUrl = "https://management.azure.com/subscriptions/<SUB-ID>/resourceGroups/<RG>/providers/Microsoft.DocumentDB/databaseAccounts/<COSMOS>/listKeys/?api-version=2015-04-08";

        public static async Task<Dictionary<string, string>> GetKeysAsync(string token, string subscriptionId, string rg, string cosmosName)
        {
            var url = KeyUrl.Replace("<SUB-ID>", subscriptionId).Replace("<RG>", rg).Replace("<COSMOS>", cosmosName);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var response = await client.PostAsync(url, null);
            var contentBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var list = JsonConvert.DeserializeObject<Dictionary<string, string>>(contentBody);
                return list;
            } else{
                Console.WriteLine($"Error getting keys for Cosmos {cosmosName}, service returned:" );
                Console.WriteLine(contentBody);
                return null;
            }
        }
    }
}