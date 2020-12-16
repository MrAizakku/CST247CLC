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
    public class ProfileController : Controller
    {
        private readonly ILogger logger;

        public ProfileController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: Profile
        [CustomAuthorization]
        public ActionResult Index()
        {
            try
            {
                ScoreDAOService s = new ScoreDAOService();
                User user = (User)Session["User"];
                Tuple<User, List<PlayerStat>> tuple = new Tuple<User, List<PlayerStat>>(user, s.GetGlobalScores().Take(5).ToList());
                return View("Profile", tuple);
            }
            catch
            {
                logger.Error("Failure at ProfileController Index().");
                return View("Error");
            }
        }
    }
}