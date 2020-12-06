using CST247CLC.Models;
using CST247CLC.Services.Business;
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
        static public Board myBoard;
        static public bool GameOver = false;
        public static User user; 
        // GET: Minesweeper
        public ActionResult Index()
        {
            user = Session["User"] as User;
            if (myBoard==null || GameOver==true)
            {
                GameOver = false;
                myBoard = new Board(10);
                myBoard.difficulty = 15;
                myBoard.setupLiveNeighbors();
                myBoard.calculateLiveNeighbors();
                myBoard.gameAlert = "New Game!";
            }
            return View("Minesweeper", myBoard);
        }

        public ActionResult OnButtonRightClick(string mine)
        {
            if (!GameOver)
            {
                string[] strArr = mine.Split('|');
                int r = int.Parse(strArr[0]);
                int c = int.Parse(strArr[1]);
                Cell currentCell = myBoard.grid[r, c];

                if (!currentCell.isVisited)
                    currentCell.isFlagged = !currentCell.isFlagged;
            }
            return PartialView("_Minesweeper", myBoard);
        }

        public ActionResult OnButtonClick(string mine)
        {
            if (!GameOver)
            {
                string[] strArr = mine.Split('|');
                int r = int.Parse(strArr[0]);
                int c = int.Parse(strArr[1]);
                Cell currentCell = myBoard.grid[r, c];

                if (!currentCell.isFlagged)
                    gameLogic(currentCell);
            }
            return PartialView("_Minesweeper", myBoard);
        }

        private void gameLogic(Cell currentCell)    //send logic to gameBoard for process and update the views accordingly.
        {
            if (!currentCell.isVisited)
            {
                //check if cell is a bomb
                if(myBoard.checkForBomb(currentCell)) //this will reveal and flood fill n everything AND let us know if a bomb was hit.
                {
                    myBoard.gameAlert = "You hit a bomb! Game Over!";
                    myBoard.revealBoard();
                    SaveScore("lose", user);
                    GameOver = true;
                }
                else if (myBoard.checkForVictory())
                {
                    SaveScore("win", user);
                    myBoard.gameAlert = "You Win! Game Over!";
                    GameOver = true;
                }
                else
                {
                    //Keep Playing...
                }
            }
        }

        private void SaveScore(string result, User user) //playerstat model expects "win" or anything else.
        {
            ScoreDAOService scoreService = new ScoreDAOService();
            PlayerStat newScore = new PlayerStat();
            newScore.difficulty = "Normal";
            newScore.gameResult = result;
            newScore.timeLapsed = 100;
            newScore.flaggedBombCount = getFlaggedBombCount();
            newScore.calculateScore();
            scoreService.SaveScore(user, newScore);
        }

        private int getFlaggedBombCount()
        {
            int flaggedBombCountNum = 0;
            foreach (var item in myBoard.bombList)
            {
                if (item.isFlagged)
                    flaggedBombCountNum++;
            }
            return flaggedBombCountNum;
        }

    }
}