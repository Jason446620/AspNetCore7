using AspCore7_Web_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;
using System.Data;
using System.Diagnostics;
using System.Net;
using Toolkit;

namespace AspCore7_Web_Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISqlHelperService _sqlHelperService;

        public HomeController(ILogger<HomeController> logger, ISqlHelperService sqlHelperService)
        {
            _logger = logger;
            _sqlHelperService = sqlHelperService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignalR()
        {
            return View();
        }
        public IActionResult Rtmp()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult testsql()
        {
            DataTable dt = _sqlHelperService.GetDataTableBySql("SELECT *FROM [dbo].[AspNetUsers]");
            return Ok(dt.Rows[0]["Email"]);
            //return Ok();
        }
    }
}