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
    public class ProfileController : Controller
    {
        // GET: Profile
        [CustomAuthorization]
        public ActionResult Index()
        {
            ScoreDAOService s = new ScoreDAOService();
            User user = (User) Session["User"];
            Tuple<User, List<PlayerStat>> tuple = new Tuple<User, List<PlayerStat>>(user, s.GetGlobalScores().Take(5).ToList());
            return View("Profile", tuple);
        }
    }
}