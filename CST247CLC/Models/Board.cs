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
    public class Board
    {
        public int size { get; set; }   //every board is square so only one value needed
        public Cell[,] grid { get; set; }   //to hold the collection of cells that is the board
        public List<Cell> bombList { get; set; }    //hold the list of bombs
        public int difficulty { get; set; }     //indicate amount of bombs

   
        public Board(int size)
        {
            /*The constructor for the Board should have a single parameter 
             * to set the size of the Grid. In its constructor, the Grid 
             * should be initialized so that a Cell object is stored at each location.*/
            this.size = size;
            grid = new Cell[this.size, this.size];
            initializeBoard();
        }

        public bool checkForBomb(Cell queryLoc)
        {
            if(queryLoc.isLive) //if it a bomb
            {
                return true;
            } 
            else           //it not a bomb
            {
                queryLoc.isVisited = true; //set to visisted
                if (queryLoc.numOfLiveNeighbors == 0) { floodFill(queryLoc); } //in actual game it only expands if this is 0
            }
            return false;
        }

        public bool checkForVictory()
        {
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    if(!grid[row,col].isVisited&&!grid[row, col].isLive) //if non bomb cells exist that are not bombs, game isn't over
                    { return false; }
                }
            }
            return true;
        }

        /*Instructios: Add a method to the Board class called floodFill(int row, int col) This method should be recursive. Inside the floodFill method, mark cells as “visited” = true when they are included in the block of affected cells. Recursively call floodfill on surrounding cells
         
        I am doing floorFill(Cell loc) instead as i find it better to pass an object that holds variables than numerous objects themselves. ANd, convienantly, cells already exist in this project.
         */
        private void floodFill(Cell queryLoc)  //checking to see if chosen cell with 0 no of live neighbors has neighbors with 0 no of live neighbors
        {
            int row_expected;  //to hold the new proposed row value
            int col_expected;  //to hold the new proposed col value.  these are to check if on grid before using.
            for (int row = -1; row < 2; row++)  //only searching touching cells
            {
                row_expected = queryLoc.row + row;
                for (int col = -1; col < 2; col++) //only searching touching cells
                {
                    col_expected = queryLoc.column + col;
                    if (row_expected != -1 && row_expected != size && col_expected != -1 && col_expected != size)   //if within the grid
                    {
                        Cell testCell = grid[queryLoc.row + row, queryLoc.column + col];    //for easy reference
                        if (!testCell.isVisited && !testCell.isFlagged)    //need to ONLY visit unvisited cells, else forever recursion and stack overflow.
                        {
                            testCell.isVisited = true;
                            if (testCell.numOfLiveNeighbors == 0)    //if no live neighbors, check for other no live neighbors with recursion.
                            {
                                floodFill(testCell);           //recursion
                            }
                        }
                    }
                    
                }
            }
        }
        private void initializeBoard()
        {
            for (int row = 0; row < size; row++)
            {
                for (int column = 0; column < size; column++)
                {
                    grid[row, column] = new Cell();
                    grid[row, column].column = column;
                    grid[row, column].row = row;
                }
            }
        }

        public void setupLiveNeighbors()
        {
            /*A method to randomly initialize the grid with live bombs. 
             * The method should utilize the Difficulty property to determine 
             * what percentage of the cells in the grid will be set to "live" status. */
            int numOfBombs = ((size * size) * difficulty) / 100;    //total num of expected bombs
            int bombCounter = 0;                                    //count how many bombs have been set
            bombList = new List<Cell>();                            //means of tracking all cells that are bombs

            Random rand = new Random();                             //tool to randomly select cells
            int tmpRow, tmpCol;

            //Console.Write("Bombs: \n"); //uncomment this and line below to list coord of bombs
            while (bombCounter < numOfBombs)
            {
                tmpRow = rand.Next(size);
                tmpCol = rand.Next(size);
                //Console.Write($"({tmpRow},{tmpCol})"); //uncomment to show bomb locations
                if(grid[tmpRow, tmpCol].isLive == false)
                {
                    grid[tmpRow, tmpCol].isLive = true;
                    bombList.Add(grid[tmpRow, tmpCol]);
                    bombCounter++;              //only increase if bomb is actually set
                }
            }
        }
        public void calculateLiveNeighbors()
        {
            /*A method to calculate the live neighbors for every cell on the grid. 
             * A cell should have between 0 and 8 live neighbors. 
             * If a cell itself is "live", then you can set the neighbor count to 9.*/
            int row_expected;                                                      //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
            int col_expected;                                                      //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
            for (int bombCounter = 0; bombCounter < bombList.Count; bombCounter++)                    //go through the bomb list
            {
                for (int row_mod = -1; row_mod < 2; row_mod++)                   //cycle -1 0 1
                {
                    row_expected = bombList[bombCounter].row + row_mod;                         //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
                    for (int col_mod = -1; col_mod < 2; col_mod++)               //cycle -1 0 1
                    {
                        col_expected = bombList[bombCounter].column + col_mod;                  //for checking bounds.  0 + -1 = -1, 9 + 1 = 10
                        //if not outside bounds then...
                        if (row_expected != -1 && row_expected != size && col_expected != -1 && col_expected != size && grid[bombList[bombCounter].row + row_mod, bombList[bombCounter].column + col_mod].isLive == false)
                            grid[bombList[bombCounter].row + row_mod, bombList[bombCounter].column + col_mod].numOfLiveNeighbors++;   //will be 0 - 8
                        else if (bombList[bombCounter].isLive == true)            //set live bombs to 9
                            bombList[bombCounter].numOfLiveNeighbors = 9;         
                    }
                }
            }
        }
    }
}
