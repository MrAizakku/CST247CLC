using CST247CLC.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MinesweeperModels
{
    public class PlayerStat : IComparable
    {
        [Display(Name = "Name")]
        public string PlayerName { get; set; }
        [Display(Name = "Difficulty")]
        public string Difficulty { get; set; }      //to keep track of difficulty played
        public string GameResult { get; set; }      //not really used at this time.
        public int FlaggedBombCount { get; set; }   //used to assess points earned
        [Display(Name = "Clicks")]
        public int Clicks { get; set; }     //formerly time
        [Display(Name = "Score")]
        public int Score { get; set; }              //calulated in method below...

        public PlayerStat()     //prebuild the stat with empty fields.
        {
            Difficulty = "-----";
            Clicks = 0;
            FlaggedBombCount = 0;
        }

        public void CalculateScore()
        {
            //each bomb is worth 500 points minus the time taken to identify all bombs if win, else 250 minus the time. lowest 100
            int scoreModifier = (GameResult == "win" ? (500 - Clicks) : (250 - Clicks));
            if (scoreModifier < 100) scoreModifier = 100;
            Score =  FlaggedBombCount * scoreModifier;

        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            PlayerStat p2 = (PlayerStat)obj;
            if (this.Score < p2.Score)
                return 1;
            if (this.Score > p2.Score)
                return -1;
            else
                return 0;
        }
    }
}
