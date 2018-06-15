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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            //var rgs = await AzureServiceHelper.GetResourceGroups(new Microsoft.Azure.Services.AppAuthentication.AzureServiceTokenProvider());
            var ARMtoken = MsiHelper.GetToken("https://management.azure.com/");
            ViewData["ARMToken"] = ARMtoken;
            var storageToken = MsiHelper.GetToken("https://storage.azure.com/");
            ViewData["BlobToken"] = storageToken;
            var cosmosKey = CosmosHelper.GetKeys(ARMtoken,"e39a92b5-b9a4-43d1-97a3-c31c819a583a", "istiotest", "msitester-table" );
            ViewData["NumKeys"] = cosmosKey?.Keys?.Count;
            ViewData["Containers"] = AzureStorageHelper.GetAllContainerNames(storageToken);
            ViewData["ResourceGroups"] = new List<string>{"None showing on purpose here"}; //rgs.Select(r => r.Name).ToList();

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
