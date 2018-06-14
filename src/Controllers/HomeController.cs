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
            var token = MsiHelper.GetToken();
            ViewData["Message"] = token;
            ViewData["HealthCheck"] = MsiHelper.HealthCheck();
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
