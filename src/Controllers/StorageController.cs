using System;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class StorageController : Controller
    {
        string subscription_id = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
        string storage_account = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT");
        public async System.Threading.Tasks.Task<IActionResult> Index()
        {
            ViewData["SubscriptionId"] = subscription_id;
            try
            {
                var storageToken = await MsiHelper.GetToken("https://storage.azure.com/");
                ViewData["BlobTokenStatus"] = string.IsNullOrEmpty(storageToken) ? "Failed to get token for storage.azure.com/" : "Got an ARM token for storage.azure.com/";


                if (string.IsNullOrEmpty(storageToken))
                {
                    ViewData["Containers"] = "Get StorageToken was unsuccessful";
                }
                else
                {
                    ViewData["ContainersXML"] = AzureStorageHelper.GetAllContainerNamesXml(storageToken, storage_account);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewData["Exception"] = ex?.Message;
                ViewData["InnerException"] = ex?.InnerException?.Message;
            }

            return View();
        }
    }
}