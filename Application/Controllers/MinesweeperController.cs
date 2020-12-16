using CST247CLC.Models;
using CST247CLC.Services.Business;
using CST247CLC.Services.Data;
using CST247CLC.Services.Utility;
using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace CST247CLC.Controllers
{
    public class MinesweeperController : Controller
    {
        private readonly ILogger logger;
        static public Board myBoard;
        static public bool GameOver = false;
        public static User user;

        public MinesweeperController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: Minesweeper
        [CustomAuthorization]
        public ActionResult Index()
        {
            try
            {
                user = Session["User"] as User;          //grab user from session.
                myBoard = user.savedBoard;              //force load the saved game, if it exists.
                if (myBoard == null || GameOver == true)    //if it doesn't exist or if game is over.
                {
                    GameOver = false;       //new game so make sure set to false
                    myBoard = new Board(10)
                    {
                        Difficulty = 15
                    };
                    myBoard.SetupLiveNeighbors();
                    myBoard.CalculateLiveNeighbors();
                    myBoard.GameAlert = "New Game!";
                    user.savedBoard = myBoard;          //make sure to set the new board to the user item so above check doesn't become an issue. 
                }
                return View("Minesweeper", myBoard);
            }
            catch
            {
                logger.Error("Failure at MinesweeperController Index().");
                return View("Error");
            }
        }

        public ActionResult OnButtonRightClick(string mine) //take mine from view
        {
            if (!GameOver)
            {
                string[] strArr = mine.Split('|');
                int r = int.Parse(strArr[0]);
                int c = int.Parse(strArr[1]);
                Cell currentCell = myBoard.Grid[r, c];

                if (!currentCell.IsVisited)
                    currentCell.IsFlagged = !currentCell.IsFlagged;
                myBoard.Clicks++;
            }
            return PartialView("_Minesweeper", myBoard);
        }

        public ActionResult OnButtonClick(string mine) //take mine from view
        {
            if (!GameOver)
            {
                string[] strArr = mine.Split('|');
                int r = int.Parse(strArr[0]);
                int c = int.Parse(strArr[1]);
                Cell currentCell = myBoard.Grid[r, c];

                if (!currentCell.IsFlagged)
                    GameLogic(currentCell);
                myBoard.Clicks++;
            }
            return PartialView("_Minesweeper", myBoard);
        }

        private void GameLogic(Cell currentCell)    //send logic to gameBoard for process and update the views accordingly.
        {
            if (!currentCell.IsVisited)
            {
                //check if cell is a bomb
                if(myBoard.CheckForBomb(currentCell)) //this will reveal and flood fill n everything AND let us know if a bomb was hit.
                {
                    GameOver = true;            //Game is over
                    SaveScore("lose", user);
                    myBoard.GameAlert = "You hit a bomb! Game Over!";
                    myBoard.RevealBoard();
                    GameSaveClear();     //Erase users saveState as game is over.
                }
                else if (myBoard.CheckForVictory())
                {
                    GameOver = true;            //Game is over
                    SaveScore("win", user);
                    myBoard.GameAlert = "You Win! Game Over!";
                    
                    GameSaveClear();     //Erase users saveState as game is over.
                }
                else
                {
                    //Keep Playing...
                }
            }
        }

        private void GameSaveClear()
        {
            //We are doing this here rather than on the index so such a check is not done everytime the page reloads, only once we need to clear the gamesave.
            GameDAOService gameDAO = new GameDAOService(); 
            gameDAO.ClearSave(user);//save that null board to database, don't need to inform session as GameOver=true by here. WIll auto make new board.
        }

        private void SaveScore(string result, User user) //playerstat model expects "win" or anything else.
        {
            ScoreDAOService scoreService = new ScoreDAOService();
            PlayerStat newScore = new PlayerStat
            {
                Difficulty = "Normal",
                PlayerName = user.FirstName + " " + user.LastName,
                GameResult = result,
                Clicks = myBoard.Clicks,
                FlaggedBombCount = GetFlaggedBombCount()
            };
            newScore.CalculateScore();
            scoreService.SaveScore(user, newScore);
            user.Stats.Add(newScore);
        }

        private int GetFlaggedBombCount()
        {
            int flaggedBombCountNum = 0;
            foreach (var item in myBoard.BombList)
            {
                if (item.IsFlagged)
                    flaggedBombCountNum++;
            }
            return flaggedBombCountNum;
        }

        public ActionResult OnGameSave()
        {
            if (GameOver==false)        //Dont want to save a game that is over.
            {
                //update the database
                GameDAOService gameDAO = new GameDAOService();
                myBoard.GameAlert = "Game Saved!" + DateTime.Now;
                gameDAO.SaveGame(user, myBoard);
                //then update the session
                Session["User"] = user; //save to DB and inform the session
            }
            return PartialView("_Minesweeper", myBoard);
        }

    }
}