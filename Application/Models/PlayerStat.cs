using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperModels
{
    public class PlayerStat : IComparable
    {
        public string playerName { get; set; }
        public string difficulty { get; set; }      //to keep track of difficulty played
        public string gameResult { get; set; }      //not really used at this time.
        public int flaggedBombCount { get; set; }   //used to assess points earned
        public int timeLapsed { get; set; }
        public int score { get; set; }              //calulated in method below...

        public PlayerStat()     //prebuild the stat with empty fields.
        {
            playerName = "-----";
            difficulty = "-----";
            timeLapsed = 0;
            flaggedBombCount = 0;
        }

        public void calculateScore()
        {
            //each bomb is worth 500 points minus the time taken to identify all bombs if win, else 250 minus the time. lowest 100
            int scoreModifier = (gameResult == "win" ? (500 - timeLapsed) : (250 - timeLapsed));
            if (scoreModifier < 100) scoreModifier = 100;
            score =  flaggedBombCount * scoreModifier;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            PlayerStat p2 = (PlayerStat)obj;

            if (this.score < p2.score)
                return 1;
            if (this.score > p2.score)
                return -1;
            else
                return 0;
        }
    }
}
