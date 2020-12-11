using CST247CLC.Models;
using CST247CLC.Services.Data;
using MinesweeperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST247CLC.Services.Business
{
    public class GameDAOService
    {
        public bool SaveGame(User user, Board game)
        {
            GameDAO service = new GameDAO();
            return service.SaveGame(user, game);
        }
        public bool ClearSave(User user)
        {
            GameDAO service = new GameDAO();
            return service.ClearSave(user);
        }
    }
}