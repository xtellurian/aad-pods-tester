// Build request to acquire MSI token

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace src
{
    static class AzureStorageHelper
    {
        static string baseUri = @"https://{account}.blob.core.windows.net/";
        public static string GetAllContainerNamesXml(string token, string storageAccount)
        {
            Console.WriteLine("Getting container names for account: " + storageAccount);

            var uri = baseUri.Replace("{account}", storageAccount) + "?comp=list";

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
                return PrettyXml(stringResponse);
            }
            catch (Exception e)
            {
                var errorText = String.Format("{0} \n\n{1}", e.Message, e.InnerException != null ? e.InnerException.Message :
                     $"Reading Blob Containers for {storageAccount} Failed");
                return errorText;
            }
        }

        private static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}