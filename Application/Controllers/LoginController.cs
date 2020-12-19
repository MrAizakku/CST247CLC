using CST247CLC.Models;
using CST247CLC.Services.Business;
using CST247CLC.Services.Utility;
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
        private readonly ILogger logger;
        public LoginController(ILogger logg)
        {
            this.logger = logg;
        }

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            logger.Info("Success at LoginController Index().");
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            try
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
                    logger.Info("Success at LoginController Login() with login success.");
                    return View("~/Views/Profile/Profile.cshtml", tuple);
                    //return View("LoginSuccess", model);
                }
                else
                {
                    logger.Info("Success at LoginController Login() with login failure.");
                    return View("LoginFailed");
                }
            }
            catch
            {
                logger.Error("Failure at LoginController Login().");
                return View("Error");
            }
        }

        private List<PlayerStat> ReturnGlobalStats()
        {
            List<MinesweeperModels.PlayerStat> global_temp_list = new List<MinesweeperModels.PlayerStat>();
            try
            {
                ScoreDAOService scoreDAOService = new ScoreDAOService();
                global_temp_list = scoreDAOService.GetAllScores().Take(5).ToList();
                logger.Info("Success at LoginController ReturnGlobalStats()");
            }
            catch
            {
                logger.Error("Failure at LoginController ReturnGlobalStats()");
            }
            return global_temp_list;
        }
    }
}