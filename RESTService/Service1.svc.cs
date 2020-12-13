using Activity1Part3.Models;
using CST247CLC.Models;
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



        public List<PlayerStat> getAllScores()
        {
            List<PlayerStat> allScores = new List<PlayerStat>();
            ScoreDAO scoreDAO = new ScoreDAO();
            allScores = scoreDAO.GetAllScores();
            return allScores;
        }

        public List<PlayerStat> getUserScores(User user)
        {
            List<PlayerStat> userScores = new List<PlayerStat>();
            ScoreDAO scoreDAO = new ScoreDAO();
            userScores = scoreDAO.GetUserScores(user);
            return userScores;
        }








        


    }
}
