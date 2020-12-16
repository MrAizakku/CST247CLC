using CST247CLC.Models;
using CST247CLC.Services.Business;
using CST247CLC.Services.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ILogger logger;

        public RegisterController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: Register
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View("Register");
            }
            catch
            {
                logger.Error("Failure at RegisterController Index().");
                return View("Error");
            }
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
