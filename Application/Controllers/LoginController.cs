using CST247CLC.Models;
using CST247CLC.Services.Business;
using MinesweeperModels;
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
                model = sservice.LoadUser(model);
                Session["User"] = model;
                Tuple<User, List<PlayerStat>> tuple = new Tuple<User, List<PlayerStat>>(model, model.stats);
                return View("~/Views/Profile/Profile.cshtml", tuple);
            }
            else
            {
                return View("LoginFailed");
            }
        }
    }
}