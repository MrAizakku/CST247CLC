using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace MinesweeperModels
{
    public class Board
    {
        public int Size { get; set; }   //every board is square so only one value needed
        public Cell[,] Grid { get; set; }   //to hold the collection of cells that is the board
        public List<Cell> BombList { get; set; }    //hold the list of bombs
        public int Difficulty { get; set; }     //indicate amount of bombs
        public string GameAlert { get; set; }     //message to user
        public Timer Timer { get; set; }
        public DateTime TimeStarted { get; set; }
        public int Clicks { get; set; }

        public Board(int size)
        {
            /*The constructor for the Board should have a single parameter 
             * to set the size of the Grid. In its constructor, the Grid 
             * should be initialized so that a Cell object is stored at each location.*/
            this.Size = size;
            Grid = new Cell[this.Size, this.Size];
            this.Timer = new Timer();
            TimeStarted = DateTime.Now;
            InitializeBoard();
        }

        public bool CheckForBomb(Cell queryLoc)
        {
            if(queryLoc.IsLive) //if it a bomb
            {
                return true;
            } 
            else           //it not a bomb
            {
                queryLoc.IsVisited = true; //set to visisted
                if (queryLoc.NumOfLiveNeighbors == 0) { FloodFill(queryLoc); } //in actual game it only expands if this is 0
            }
            return false;
        }

        public bool CheckForVictory()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (!Grid[row, col].IsVisited && !Grid[row, col].IsLive) //if non bomb cells exist that are not bombs, game isn't over
                    { return false; }
                }
            }
            return true;
        }
        public void RevealBoard() //set all to visisted so view will display all
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Grid[row, col].IsVisited = true;
                }
            }
        }

        /*Instructios: Add a method to the Board class called floodFill(int row, int col) This method should be recursive. Inside the floodFill method, mark cells as “visited” = true when they are included in the block of affected cells. Recursively call floodfill on surrounding cells
         
        I am doing floorFill(Cell loc) instead as i find it better to pass an object that holds variables than numerous objects themselves. ANd, convienantly, cells already exist in this project.
         */
        private void FloodFill(Cell queryLoc)  //checking to see if chosen cell with 0 no of live neighbors has neighbors with 0 no of live neighbors
        {
            int row_expected;  //to hold the new proposed row value
            int col_expected;  //to hold the new proposed col value.  these are to check if on grid before using.
            for (int row = -1; row < 2; row++)  //only searching touching cells
            {
                row_expected = queryLoc.Row + row;
                for (int col = -1; col < 2; col++) //only searching touching cells
                {
                    col_expected = queryLoc.Column + col;
                    if (row_expected != -1 && row_expected != Size && col_expected != -1 && col_expected != Size)   //if within the grid
                    {
                        Cell testCell = Grid[queryLoc.Row + row, queryLoc.Column + col];    //for easy reference
                        if (!testCell.IsVisited && !testCell.IsFlagged)    //need to ONLY visit unvisited cells, else forever recursion and stack overflow.
                        {
                            testCell.IsVisited = true;
                            if (testCell.NumOfLiveNeighbors == 0)    //if no live neighbors, check for other no live neighbors with recursion.
                            {
                                FloodFill(testCell);           //recursion
                            }
                        }
                    }
                    
                }
            }
        }
        private void InitializeBoard()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    Grid[row, column] = new Cell
                    {
                        Column = column,
                        Row = row
                    };
                }
            }
        }

        public void SetupLiveNeighbors()
        {
            /*A method to randomly initialize the grid with live bombs. 
             * The method should utilize the Difficulty property to determine 
             * what percentage of the cells in the grid will be set to "live" status. */
            int numOfBombs = ((Size * Size) * Difficulty) / 100;    //total num of expected bombs
            int bombCounter = 0;                                    //count how many bombs have been set
            BombList = new List<Cell>();                            //means of tracking all cells that are bombs

            Random rand = new Random();                             //tool to randomly select cells
            int tmpRow, tmpCol;

            //Console.Write("Bombs: \n"); //uncomment this and line below to list coord of bombs
            while (bombCounter < numOfBombs)
            {
                tmpRow = rand.Next(Size);
                tmpCol = rand.Next(Size);
                //Console.Write($"({tmpRow},{tmpCol})"); //uncomment to show bomb locations
                if(Grid[tmpRow, tmpCol].IsLive == false)
                {
                    Grid[tmpRow, tmpCol].IsLive = true;
                    BombList.Add(Grid[tmpRow, tmpCol]);
                    bombCounter++;              //only increase if bomb is actually set
                }
            }
        }
        public void CalculateLiveNeighbors()
        {
            /*A method to calculate the live neighbors for every cell on the grid. 
             * A cell should have between 0 and 8 live neighbors. 
             * If a cell itself is "live", then you can set the neighbor count to 9.*/
            int row_expected;                                                      //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
            int col_expected;                                                      //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
            for (int bombCounter = 0; bombCounter < BombList.Count; bombCounter++)                    //go through the bomb list
            {
                for (int row_mod = -1; row_mod < 2; row_mod++)                   //cycle -1 0 1
                {
                    row_expected = BombList[bombCounter].Row + row_mod;                         //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
                    for (int col_mod = -1; col_mod < 2; col_mod++)               //cycle -1 0 1
                    {
                        col_expected = BombList[bombCounter].Column + col_mod;                  //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
                        //if not outside bounds then...
                        if (row_expected != -1 && row_expected != Size && col_expected != -1 && col_expected != Size && Grid[BombList[bombCounter].Row + row_mod, BombList[bombCounter].Column + col_mod].IsLive == false)
                            Grid[BombList[bombCounter].Row + row_mod, BombList[bombCounter].Column + col_mod].NumOfLiveNeighbors++;   //will be 0 - 8
                        else if (BombList[bombCounter].IsLive == true)            //set live bombs to 9
                            BombList[bombCounter].NumOfLiveNeighbors = 9;         
                    }
                }
            }
        }
    }
}
