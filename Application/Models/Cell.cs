using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperModels
{
    public class Cell
    {
        public int Row { get; set; }                    //rows stack, so go up n down
        public int Column { get; set; }                 //columns line up, go left n right
        public bool IsVisited { get; set; }
        public bool IsLive { get; set; }                //indicates is a "live bomb" cell
        public bool IsFlagged { get; set; }                //indicates if cell is suspected by user
        public int NumOfLiveNeighbors { get; set; }     //expexted < 9: norm = 0-8, 9 = bomb

        public Cell()
        {
            this.Row = this.Column = -1;
            this.IsVisited = this.IsLive = this.IsFlagged = false;
            this.NumOfLiveNeighbors = 0;
        }
    }
}
