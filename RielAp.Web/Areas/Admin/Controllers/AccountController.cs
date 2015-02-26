using RielAp.Core;
using RielAp.Domain.Repositories;
using RielAp.Domain.Models;
using RielAp.Web.Infrastructure;
using RielAp.Web.Infrastructure.Abstract;
using RielAp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RielAp.Web.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private IAuthProvider _authProvider;

        public AccountController(IAuthProvider authProvider, IUsersRepository userRepository)
        {
            _authProvider = authProvider;
        }

        #region Login

        //
        // GET: /Admin/Account/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            string message = Translation.Translation.LoginPageLoginError;
            if (ModelState.IsValid)
            {
                string phone = StringHelper.GetOnlyNumbers(model.Phone);
                model.Phone = phone;
                if (phone.Length == 12)
                {
                    AuthResult authResult = _authProvider.Authenticate(phone, model.Password);
                    if (authResult == AuthResult.Success)
                    {
                        return Redirect(Url.Action("Users", "Default"));
                    }
                    else if (authResult == AuthResult.NoRights)
                    {
                        message = Translation.Translation.AccessIsDeniedMessage;
                    }
                }
            }


            ModelState.AddModelError("", message);
            return View(model);
        }

        [HttpPost]
        public ActionResult Logoff()
        {
            if (Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            return Redirect("/");
        }

        #endregion
    }
}
