using CST247CLC.Models;
using CST247CLC.Services.Business;
using CST247CLC.Services.Data;
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
        [CustomAuthorization]
        public ActionResult Index()
        {
            user = Session["User"] as User;          //grab user from session.
            myBoard = user.savedBoard;              //force load the saved game, if it exists.
            if (myBoard==null || GameOver==true)    //if it doesn't exist or if game is over.
            {
                GameOver = false;       //new game so make sure set to false
                myBoard = new Board(10);
                myBoard.difficulty = 15;
                myBoard.setupLiveNeighbors();
                myBoard.calculateLiveNeighbors();
                myBoard.gameAlert = "New Game!";
                user.savedBoard = myBoard;          //make sure to set the new board to the user item so above check doesn't become an issue. 
            }
            return View("Minesweeper", myBoard);
        }

        public ActionResult OnButtonRightClick(string mine) //take mine from view
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

        public ActionResult OnButtonClick(string mine) //take mine from view
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
                    GameOver = true;            //Game is over
                    SaveScore("lose", user);
                    myBoard.gameAlert = "You hit a bomb! Game Over!";
                    myBoard.revealBoard();
                    GameSaveClear();     //Erase users saveState as game is over.
                }
                else if (myBoard.checkForVictory())
                {
                    GameOver = true;            //Game is over
                    SaveScore("win", user);
                    myBoard.gameAlert = "You Win! Game Over!";
                    
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

        public ActionResult onGameSave()
        {
            if (GameOver==false)        //Dont want to save a game that is over.
            {
                //update the database
                GameDAOService gameDAO = new GameDAOService();
                myBoard.gameAlert = "Game Saved!" + DateTime.Now;
                gameDAO.SaveGame(user, myBoard);
                //then update the session
                Session["User"] = user; //save to DB and inform the session
            }
            return PartialView("_Minesweeper", myBoard);
        }

    }
}