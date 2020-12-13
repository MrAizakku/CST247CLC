using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTService
{
    [DataContract]
    public class ScoreDTO
    {
        public ScoreDTO()
        {
        }

        public ScoreDTO(int errorCode, string errorMessage, List<PlayerStat> scores)
        {
            this.ErrorCode = errorCode;
            this.ErrorMsg = errorMessage;
            this.Scores = scores;
        }

        [DataMember]
        public int ErrorCode { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }
        [DataMember]
        public List<PlayerStat> Scores { get; set; }



    }
}