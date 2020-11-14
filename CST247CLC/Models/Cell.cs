/*
 * Isaac Tucker
 * CST-227 Enterprise Applications 2
 * Milestone 
 * 10/10/2020 update: I have separated Board.cs and Cell.cs into their own library to be referenced by the console and GUI version of the application.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperModels
{
    public class Cell
    {
        public int row { get; set; }                    //rows stack, so go up n down
        public int column { get; set; }                 //columns line up, go left n right
        public bool isVisited { get; set; }
        public bool isLive { get; set; }                //indicates is a "live bomb" cell
        public bool isFlagged { get; set; }                //indicates if cell is suspected by user
        public int numOfLiveNeighbors { get; set; }     //expexted < 9: norm = 0-8, 9 = bomb

        public Cell()
        {
            this.row = this.column = -1;
            this.isVisited = this.isLive = this.isFlagged = false;
            this.numOfLiveNeighbors = 0;
        }
    }
}
