using System;
using System.Web.Mvc;
using CST247CLC.Models;
using CST247CLC.Services.Business;

namespace CST247CLC.Controllers
{
    internal class CustomAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            SecurityDAOService securityService = new SecurityDAOService();
            User user = (User)filterContext.HttpContext.Session["User"];
            bool success = securityService.Authenticate(user);
            if (!success)
            {
                filterContext.Result = new RedirectResult("/login");
            }
        }
    }
}