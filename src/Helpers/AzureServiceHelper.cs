using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace src
{

    public static class AzureServiceHelper
    {
        private static async Task GetSecretFromKeyVault(AzureServiceTokenProvider azureServiceTokenProvider)
        {
            KeyVaultClient keyVaultClient =
                new KeyVaultClient(
                    new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));


            var keyVaultName = "gotta add this";

            try
            {
                var secret = await keyVaultClient
                    .GetSecretAsync($"https://{keyVaultName}.vault.azure.net/secrets/secret")
                    .ConfigureAwait(false);

                Console.WriteLine($"Secret: {secret.Value}");

            }
            catch (Exception exp)
            {
                Console.WriteLine($"Something went wrong: {exp.Message}");
            }
        }

        public static async Task<IEnumerable<ResourceGroup>> GetResourceGroups(string token, string subscriptionId)
        {
            if(string.IsNullOrEmpty(token)){
                throw new NullReferenceException("Null token in method GetResourceGroups()");
            }
            try
            {
                var serviceCreds = new TokenCredentials(token);

                var resourceManagementClient =
                    new ResourceManagementClient(serviceCreds) { SubscriptionId = subscriptionId };

                var resourceGroups = await resourceManagementClient.ResourceGroups.ListAsync();
                
                return resourceGroups;

            }
            catch (Exception exp)
            {
                Console.WriteLine($"Something went wrong: {exp.Message}");
            }
            return null;
        }
    }
}