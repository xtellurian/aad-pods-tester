// Build request to acquire MSI token

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace src
{
    static class MsiHelper
    {
        public static string GetToken(string resource)
        {
// https://storage.azure.com/
// https://vault.azure.net
// https://management.azure.com/
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=" + resource);

            request.Headers["Metadata"] = "true";
            request.Method = "GET";

            try
            {
                // Call /token endpoint
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Pipe response Stream to a StreamReader, and extract access token
                StreamReader streamResponse = new StreamReader(response.GetResponseStream());
                string stringResponse = streamResponse.ReadToEnd();
                var list = (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(stringResponse);
                string accessToken = list["access_token"];
                return accessToken;
            }
            catch (Exception e)
            {
                var errorText = String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : "Acquire token failed");
                return errorText;
            }
        }
    }
}