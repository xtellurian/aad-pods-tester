using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Rest;
using Microsoft.Rest.Azure.OData;
using Newtonsoft.Json;

namespace src.Controllers
{
    public class CosmosController : Controller
    {
        string cosmos_rg = Environment.GetEnvironmentVariable("COSMOS_RG");
        string cosmos_name = Environment.GetEnvironmentVariable("COSMOS_NAME");
        string subscription_id = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
        public async Task<IActionResult> Index()
        {
            var ARMtoken = await MsiHelper.GetToken("https://management.azure.com/");

            var allCosmos = await GetAllCosmosDb(ARMtoken);
            ViewData["AllCosmosNames"] = new List<string>(allCosmos.Select(c=>c.Name));
            ViewData["AllCosmosJson"] = PrettyStringHelper.JsonPrettify(JsonConvert.SerializeObject(allCosmos));
            // replace if in query, otherwise use defaults
            if (HttpContext.Request.Query.ContainsKey("rg"))
            {
                cosmos_rg = HttpContext.Request.Query["rg"];
            }
            if (HttpContext.Request.Query.ContainsKey("name"))
            {
                cosmos_name = HttpContext.Request.Query["name"];
            }

            var cosmosKey = await CosmosHelper.GetKeysAsync(ARMtoken, subscription_id, cosmos_rg, cosmos_name);
            ViewData["NumKeys"] = cosmosKey?.Keys?.Count.ToString() ?? "Failed to get keys for " + cosmos_name + " in rg " + cosmos_rg;

            return View();
        }
        private async Task<IList<GenericResource>> GetAllCosmosDb(string token)
        {
            var serviceCreds = new TokenCredentials(token);

            var rmClient =
                new ResourceManagementClient(serviceCreds) { SubscriptionId = subscription_id };
            
            var res = await rmClient.Resources.ListAsync(new ODataQuery<GenericResourceFilter>(f => f.ResourceType == "Microsoft.DocumentDb/databaseAccounts"));
            
            return res.ToList();
        }
    }

}