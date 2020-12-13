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
                user.Stats = scoreDO.GetUserScores(user).Take(5).ToList();
                Session["User"] = user;
                Tuple<User, List<PlayerStat>> tuple = new Tuple<User, List<PlayerStat>>(user, ReturnGlobalStats()); //pass the user and the global stats
                return View("~/Views/Profile/Profile.cshtml", tuple);
                //return View("LoginSuccess", model);
            }
            else
            {
                return View("LoginFailed");
            }
        }

        private List<PlayerStat> ReturnGlobalStats()
        {

            ScoreDAOService scoreDAOService = new ScoreDAOService();
            List<MinesweeperModels.PlayerStat> global_temp_list = scoreDAOService.GetAllScores().Take(5).ToList();
            return global_temp_list;
        }
    }
}