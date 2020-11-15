using CST247CLC.Models;
using CST247CLC.Services.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        [HttpGet]
        public ActionResult Index()
        {
            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                SecurityDAOService sservice = new SecurityDAOService();
                bool results = sservice.Register(model);
                if (results)
                {
                    return View("RegistrationSuccess", model);
                }
                else
                {
                    return View("RegistrationFailed");
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
