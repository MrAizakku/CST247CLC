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
        public ActionResult Login(User user)
        {
            SecurityDAOService sservice = new SecurityDAOService();
            bool results = sservice.Authenticate(user);
            if (results)
            {
                user = sservice.LoadUser(user);
                ScoreDAOService scoreDO = new ScoreDAOService();
                user.stats = scoreDO.GetUserScores(user);
                Session["User"] = user;
                Tuple<User, List<PlayerStat>> tuple = new Tuple<User, List<PlayerStat>>(user, returnGlobalStats()); //pass the user and the global stats
                return View("~/Views/Profile/Profile.cshtml", tuple);
                //return View("LoginSuccess", model);
            }
            else
            {
                return View("LoginFailed");
            }
        }

        private List<PlayerStat> returnGlobalStats()
        {

            ScoreDAOService scoreDAOService = new ScoreDAOService();
            List<MinesweeperModels.PlayerStat> global_temp_list = scoreDAOService.getAllScores();
            return global_temp_list;
        }
    }
}