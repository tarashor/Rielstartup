using Ninject;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RielAp.Web.Infrastructure
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        [Inject]
        public IUsersRepository UsersRepository { get; set; }
        [Inject]
        public IRolesRepository RolesRepository { get; set; }

        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            User user = UsersRepository.GetUserByPhone(httpContext.User.Identity.Name);
            if (user.Role.HasAdminAccess)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Account",
                            action = "Login"
                        })
                    );
        }

    }
}