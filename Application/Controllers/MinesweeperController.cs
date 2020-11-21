using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CST247CLC.Controllers
{
    public class MinesweeperController : Controller
    {
        static public Board myBoard;
        // GET: Minesweeper
        public ActionResult Index()
        {
            if(myBoard==null)
            {
                myBoard = new Board(10);
                myBoard.difficulty = 15;
                myBoard.setupLiveNeighbors();
                myBoard.calculateLiveNeighbors();
            }
            return View("Minesweeper", myBoard);
        }

        public ActionResult OnButtonRightClick(string mine)
        {
            string[] strArr = mine.Split('|');
            int r = int.Parse(strArr[0]);
            int c = int.Parse(strArr[1]);
            Cell currentCell = myBoard.grid[r, c];

            if (!currentCell.isVisited)
                currentCell.isFlagged = !currentCell.isFlagged;
            return View("Minesweeper", myBoard);
        }

            public ActionResult OnButtonClick(string mine)
        {
            string[] strArr = mine.Split('|');
            int r = int.Parse(strArr[0]);
            int c = int.Parse(strArr[1]);
            Cell currentCell = myBoard.grid[r, c];

            if (!currentCell.isFlagged)
                gameLogic(currentCell);
            return View("Minesweeper", myBoard);
        }

        private void gameLogic(Cell currentCell)    //send logic to gameBoard for process and update the views accordingly.
        {
            if (!currentCell.isVisited)
            {
                //check if cell is a bomb
                if(myBoard.checkForBomb(currentCell)) //this will reveal and flood fill n everything AND let us know if a bomb was hit.
                {
                    ViewBag.Message = "You hit a bomb!";
                } 
                else if (myBoard.checkForVictory())
                {
                    //game over. win
                }
            }
        }
    }
}