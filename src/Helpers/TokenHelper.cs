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
        public static async Task<string> TestMethod()
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            return JsonConvert.SerializeObject(await GetResourceGroups(azureServiceTokenProvider));
        }
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

        public static async Task<IEnumerable<ResourceGroup>> GetResourceGroups(AzureServiceTokenProvider azureServiceTokenProvider)
        {
            var subscriptionId = "c5760548-23c2-4223-b41e-5d68a8320a0c";

            try
            {
                var serviceCreds = new TokenCredentials(await azureServiceTokenProvider.GetAccessTokenAsync("https://management.azure.com/").ConfigureAwait(false));

                var resourceManagementClient =
                    new ResourceManagementClient(serviceCreds) { SubscriptionId = subscriptionId };

                var resourceGroups = await resourceManagementClient.ResourceGroups.ListAsync();

                foreach (var resourceGroup in resourceGroups)
                {
                    Console.WriteLine($"Resource group {resourceGroup.Name}");
                }
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