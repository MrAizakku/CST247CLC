using CST247CLC.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class LogoutController : Controller
    {
        private readonly ILogger logger;

        public LogoutController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: Logout
        public RedirectResult Index()
        {
            try
            {
                Session["User"] = null;
            }
            catch
            {
                logger.Error("Failure at LogoutController Index().");
            }
            return Redirect("/home");
        }
    }
}