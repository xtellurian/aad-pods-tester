

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;
using Microsoft.Rest.Azure.OData;

namespace src.Controllers
{
    public class ManagementController : Controller
    {
        string subscription_id = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
        public async Task<IActionResult> Index()
        {
            ViewData["SubscriptionId"] = subscription_id;
            try
            {
                var token = await GetToken("https://management.azure.com/");
                ViewData["ARMTokenStatus"] = "Got a token!";
                ViewData["DecodedToken"] = JwtHelper.DecodeToJson(token);

                var rgs = await GetResourceGroups(token);
                ViewData["ResourceGroups"] = new List<string>(rgs.Select(r => r.Name)); // must cast correctly

                var providers = await GetProviders(token);
                ViewData["Providers"] = new List<string>(providers.Select(p => p.Id));

                var resources = await GetAllResources(token);
                ViewData["Resources"] = new List<string>(resources.Select(r => r.Type + "-" + r.Name));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["ARMTokenStatus"] = "Error Getting ARM Token";
            }

            return View();
        }

        private async Task<string> GetToken(string resource)
        {
            return await MsiHelper.GetToken(resource); // throws on error
        }

        private async Task<IList<Provider>> GetProviders(string token)
        {
            var serviceCreds = new TokenCredentials(token);

            var rmClient =
                new ResourceManagementClient(serviceCreds) { SubscriptionId = subscription_id };

            var providers = await rmClient.Providers.ListAsync();

            return providers.ToList();
        }

        private async Task<IList<ResourceGroup>> GetResourceGroups(string token)
        {
            var serviceCreds = new TokenCredentials(token);

            var rmClient =
                new ResourceManagementClient(serviceCreds) { SubscriptionId = subscription_id };

            var resourceGroups = await rmClient.ResourceGroups.ListAsync();


            return resourceGroups.ToList();
        }

        private async Task<IList<GenericResource>> GetAllResources(string token)
        {
            var serviceCreds = new TokenCredentials(token);

            var rmClient =
                new ResourceManagementClient(serviceCreds) { SubscriptionId = subscription_id };

            var resources = await rmClient.Resources.ListAsync();

            return resources.ToList();
        }
    }
}