using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public RedirectResult Index()
        {
            Session["User"] = null;
            return Redirect("/home");
        }
    }
}