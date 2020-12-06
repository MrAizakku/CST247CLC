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
        public ActionResult Index(User user)
        {
            ScoreDAOService s = new ScoreDAOService();
            Tuple<User, List<PlayerStat>> tuple = new Tuple<User, List<PlayerStat>>(user, s.getGlobalScores());
            return View("Profile", tuple);
        }
    }
}