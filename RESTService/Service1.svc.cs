using CST247CLC.Models;
using RESTService;
using CST247CLC.Services.Data;
using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RESTService {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1 {

        private readonly ScoreDAO scoreDAO = new ScoreDAO();
        public ScoreDTO GetAllScores()
        {
            ScoreDTO scoreDTO = new ScoreDTO();
            List<PlayerStat> returnList = scoreDAO.GetAllScores();
            scoreDTO.ErrorCode = 0;
            scoreDTO.ErrorMsg = "OK";
            scoreDTO.Scores = returnList.Count > 0 ? returnList : null;
            return scoreDTO;
        }

        public ScoreDTO GetUserScoresByName(string user)
        {
            ScoreDTO scoreDTO = new ScoreDTO();
            List<PlayerStat> returnList = scoreDAO.GetUserScoresByName(user);
            scoreDTO.ErrorCode = returnList.Count > 0 ? 0 : -1;
            scoreDTO.ErrorMsg = returnList.Count > 0 ? "OK" : $"User '{user}' Does Not Exist Or Does Not Have Scores";
            scoreDTO.Scores = returnList.Count > 0 ? returnList : null;
            return scoreDTO;
        }
    }
}