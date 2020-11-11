using CST247CLC.Models;
using CST247CLC.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST247CLC.Services.Business
{
    public class SecurityDAOService
    {
        public bool Authenticate(User user)
        {
            SecurityDAO service = new SecurityDAO();
            return service.FindByUser(user);
        }
    }
}