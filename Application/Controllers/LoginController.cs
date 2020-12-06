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
            List<MinesweeperModels.PlayerStat> global_temp_list = new List<MinesweeperModels.PlayerStat>();
            PlayerStat newScore = new PlayerStat();
            newScore.difficulty = "Normal";
            newScore.playerName = "Bob Odenkirk";
            newScore.gameResult = "win";
            newScore.timeLapsed = 100;
            newScore.flaggedBombCount = 2;
            newScore.calculateScore();
            global_temp_list.Add(newScore);

            newScore = new PlayerStat();
            newScore.difficulty = "Hard";
            newScore.gameResult = "lose";
            newScore.playerName = "David Cross";
            newScore.timeLapsed = 70;
            newScore.flaggedBombCount = 4;
            newScore.calculateScore();
            global_temp_list.Add(newScore);

            newScore = new PlayerStat();
            newScore.difficulty = "Easy";
            newScore.gameResult = "win";
            newScore.playerName = "Arthur Windsor";
            newScore.timeLapsed = 44;
            newScore.flaggedBombCount = 10;
            newScore.calculateScore();
            global_temp_list.Add(newScore);

            return global_temp_list;
        }
    }
}