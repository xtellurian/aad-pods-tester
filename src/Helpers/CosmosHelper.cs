using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace src {
    class CosmosHelper {

        private static string KeyUrl = "https://management.azure.com/subscriptions/<SUB-ID>/resourceGroups/<RG>/providers/Microsoft.DocumentDb/databaseAccounts/<COSMOS>/listKeys/?api-version=2016-12-01";

        public static Dictionary<string,string> GetKeys(string token, string subscriptionId, string rg, string cosmosName){

            var url = KeyUrl.Replace("<SUB-ID>",subscriptionId).Replace("<RG>",rg).Replace("<COSMOS>",cosmosName);
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers["Authorization"] = "Bearer " + token;
            request.Method = "POST";

            try
            {
                // Call /token endpoint
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Pipe response Stream to a StreamReader, and extract access token
                StreamReader streamResponse = new StreamReader(response.GetResponseStream());
                string stringResponse = streamResponse.ReadToEnd();
                var list = JsonConvert.DeserializeObject<Dictionary<string, string>>(stringResponse);
                return list;
            }
            catch (Exception e)
            {
                var errorText = String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : "Acquire COSMOS key failed");
                return null;
            }

        }
    }
}