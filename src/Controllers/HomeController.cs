using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using src.Models;

namespace src.Controllers
{
    public class HomeController : Controller
    {
        string cosmos_rg = Environment.GetEnvironmentVariable("COSMOS_RG");
        string cosmos_name = Environment.GetEnvironmentVariable("COSMOS_NAME");
        string subscription_id = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
        string storage_account = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT");
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Demo()
        {
            ViewData["SubscriptionId"] = subscription_id;
            try
            {
                var ARMtoken = MsiHelper.GetToken("https://management.azure.com/");
                ViewData["ARMTokenStatus"] = string.IsNullOrEmpty(ARMtoken) ? "Failed to get token for management.azure.com/" : "Got an ARM token for management.azure.com/";

                var storageToken = MsiHelper.GetToken("https://storage.azure.com/");
                ViewData["BlobTokenStatus"] = string.IsNullOrEmpty(storageToken) ?  "Failed to get token for storage.azure.com/" : "Got an ARM token for storage.azure.com/";
                
                if (string.IsNullOrEmpty(ARMtoken))
                {
                    ViewData["NumKeys"] = "Get ARMtoken was unsuccessful, won't attempt to get COSMOS keys";
                }
                else
                {
                    // let's get this from input
                    //var cosmosKey = CosmosHelper.GetKeys(ARMtoken, "e39a92b5-b9a4-43d1-97a3-c31c819a583a", "istiotest", "msitester-table");
                    var cosmosKey = CosmosHelper.GetKeys(ARMtoken, subscription_id, cosmos_rg, cosmos_name);
                    ViewData["NumKeys"] = cosmosKey?.Keys?.Count.ToString() ?? "Failed to get keys for " + cosmos_name + " in rg " + cosmos_rg;
                }
                if (string.IsNullOrEmpty(storageToken))
                {
                    ViewData["Containers"] = "Get StorageToken was unsuccessful";
                }
                else
                {
                    ViewData["ContainersXML"] = AzureStorageHelper.GetAllContainerNamesXml(storageToken, storage_account);
                }
                var rgs = await AzureServiceHelper.GetResourceGroups(ARMtoken, subscription_id);
                ViewData["ResourceGroups"] = new List<string> (rgs.Select(r => r.Name).ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["Exception"] = ex?.Message;
                ViewData["InnerException"] = ex?.InnerException?.Message;
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
