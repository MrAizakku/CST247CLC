using CST247CLC.Models;
using CST247CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            SecurityDAOService sservice = new SecurityDAOService();
            bool results = sservice.Authenticate(model);
            if (results)
            {
                return View("LoginSuccess", model);
            }
            else
            {
                return View("LoginFailed");
            }
        }
    }
}