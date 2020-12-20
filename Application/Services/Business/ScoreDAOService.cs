using CST247CLC.Models;
using CST247CLC.Services.Data;
using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST247CLC.Services.Business
{
    public class ScoreDAOService
    {
        public bool SaveScore(User user, PlayerStat score)
        {
            ScoreDAO service = new ScoreDAO();
            return service.SaveScore(user, score);
        }
        public List<PlayerStat> GetAllScores()
        {
            ScoreDAO service = new ScoreDAO();
            return service.GetAllScores();
        }
        public List<PlayerStat> GetUserScores(User user)
        {
            ScoreDAO service = new ScoreDAO();
            return service.GetUserScores(user);
        }
        public List<PlayerStat> GetGlobalScores()
        {
            ScoreDAO service = new ScoreDAO();
            return service.GetAllScores();
        }
    }
}