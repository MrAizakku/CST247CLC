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
                logger.Info("RegisterController Index(), success.");
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
                logger.Info("RegisterController Register(), ModelState is valid.");
                SecurityDAOService sservice = new SecurityDAOService();
                bool results = sservice.Register(model);
                if (results)
                {
                    logger.Info("RegisterController Register(), Registration success.");
                    return View("RegistrationSuccess", model);
                }
                else
                {
                    logger.Info("RegisterController Register(), Registration failed.");
                    return View("RegistrationFailed");
                }
            }
            else
            {
                logger.Info("RegisterController Register(), ModelState is not valid.");
                return View(model);
            }
        }
    }
}
