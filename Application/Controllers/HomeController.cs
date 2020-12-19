using CST247CLC.Models;
using CST247CLC.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger logger;

        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: Home
        public ActionResult Index()
        {
            try
            {
                logger.Info("Success at HomeController Index().");
                return View("Home");
            }
            catch
            {
                logger.Error("Failure at HomeController Index().");
                return View("Error");
            }
        }
    }
}