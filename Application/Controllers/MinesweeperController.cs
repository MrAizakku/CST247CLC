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
        static public Board myBoard = new Board(10);
        // GET: Minesweeper
        public ActionResult Index()
        {
            return View("Minesweeper", myBoard);
        }

        public ActionResult OnButtonClick(string MineID)
        {
            string[] strArr = MineID.Split('|');
            int r = int.Parse(strArr[0]);
            int c = int.Parse(strArr[1]);

            Cell currentCell = myBoard.grid[r, c];

            //run game logic
            if (!currentCell.isFlagged)
                gameLogic(currentCell);

            /*
            if (me.Button == MouseButtons.Left)
            {
                if (!currentCell.isFlagged)
                    gameLogic(currentCell);
            }
            else if (me.Button == MouseButtons.Right)
            {
                if (!currentCell.isVisited)
                {
                    currentCell.isFlagged = !currentCell.isFlagged;
                    updateButtonLabels();
                }
            } */

            return View("Minesweeper", myBoard);
        }

        private void gameLogic(Cell currentCell)    //send logic to gameBoard for process and update the views accordingly.
        {
            if (!currentCell.isVisited)
            {
                //check if cell is a bomb
                myBoard.checkForBomb(currentCell);
                //have all non bomb cells been visisted?
                //else if (myBoard.checkForVictory()) { GameOver("win"); return; }
                //update the buttons
                //else { //updateButtonLabels(); }
            }
        }
    }
}