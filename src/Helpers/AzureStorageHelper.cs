// Build request to acquire MSI token

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace src
{
    static class AzureStorageHelper
    {
        public static string GetAllContainerNames(string token)
        {
            var uri = @"https://misterstorage.blob.core.windows.net/?comp=list";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers["Authorization"] = "Bearer " + token;
            request.Headers["x-ms-version"] = "2017-11-09";
            request.Method = "GET";

            try
            {
                // Call /token endpoint
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Pipe response Stream to a StreamReader, and extract access token
                StreamReader streamResponse = new StreamReader(response.GetResponseStream());
                string stringResponse = streamResponse.ReadToEnd();
                return stringResponse;
            }
            catch (Exception e)
            {
                var errorText = String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message : "Reading Blob Containers Failed");
                return errorText;
            }
        }
    }
}