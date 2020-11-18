﻿using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class MinesweeperController : Controller
    {
        static public Board board = new Board(10);
        // GET: Minesweeper
        public ActionResult Index()
        {
            return View("Minesweeper", board);
        }
    }
}