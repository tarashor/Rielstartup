using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RielAp.Web.Models;
using RielAp.Web.Infrastructure.Abstract;
using RielAp.Domain.Repositories;
using RielAp.Domain.Models;
using System.Text.RegularExpressions;
using RielAp.Core;
using RielAp.Web.Infrastructure;
using System.Configuration;
using RielAp.Web.Utils;
using System.Xml;
using System.Security.Cryptography;
using RielAp.Web.Services.Account;

namespace RielAp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoginService loginService;
        private readonly IAuthProvider _authProvider;
        private readonly IPasswordValidator _passwordValidator;
        private readonly IPasswordEncrypter _passwordEncryptor;
        private readonly IUsersRepository _userRepository;
        private readonly IProfilesRepository _profilesRepository;
        private readonly IMobileNumbersRepository _mobileNumbersRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IRolesRepository _rolesRepository;

        public AccountController(IAuthProvider authProvider,
            IPasswordValidator passwordValidator,
            IPasswordEncrypter passwordEncryptor,
            IUsersRepository userRepository, 
            IMobileNumbersRepository mobileNumberRepository, 
            IProfilesRepository profilesRepository, 
            IOrdersRepository ordersRepository,
            IRolesRepository rolesRepository,
            ILoginService loginService
        )
        {
            _authProvider = authProvider;
            _passwordValidator = passwordValidator;
            _passwordEncryptor = passwordEncryptor;
            _userRepository = userRepository;
            _mobileNumbersRepository = mobileNumberRepository;
            _profilesRepository = profilesRepository;
            _ordersRepository = ordersRepository;
            _rolesRepository = rolesRepository;
            this.loginService = loginService;
        }

        #region Register
        public ActionResult Register(string returnUrl)
        {
            HttpCookie c = new HttpCookie("showInfo", "false");
            Response.SetCookie(c);
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.MaxMobileNumbers = int.Parse(ConfigurationManager.AppSettings["MaxAdditionalMobileNumbers"]);
            return View();
        }

        
        [HttpPost]
        public ActionResult Register(RegisterViewModel model, ICollection<MobileNumber> mobileNumbers, string returnUrl)
        {
            string error;
            if (ModelState.IsValid)
            {
                if (model.IsValid(out error))
                {
                    //Get only numbers
                    string phone = StringHelper.GetOnlyNumbers(model.Phone);
                    if (phone.Length == 12)
                    {
                        if (_userRepository.GetUserByPhone(phone) == null)
                        {
                            User newUser = new User();
                            newUser.Username = model.UserName;
                            newUser.Phone = phone;
                            newUser.Password = _passwordEncryptor.EncryptPassword(model.Password);
                            newUser.ReceiveNewAnnouncements = model.CanReceiveEmail;
                            newUser.EmailAddress = model.Email;
                            newUser.RegisterDate = DateTime.Now;
                            newUser.Role = _rolesRepository.GetById("User");
                            newUser.ConfirmationCode = Guid.NewGuid();
                            newUser.Profile = _profilesRepository.GetBasicProfile();
                            newUser.ProfileExpires = null;

                            newUser.IsConfirmed = true;

                            _userRepository.Add(newUser);
                            _userRepository.SaveChanges();

                            if (mobileNumbers == null)
                            {
                                mobileNumbers = new List<MobileNumber>();
                            }
                            foreach (MobileNumber mobile in mobileNumbers)
                            {
                                mobile.UserId = newUser.UserID;
                                _mobileNumbersRepository.Add(mobile);
                            }
                            _mobileNumbersRepository.SaveChanges();

                            //sendConfirmEmail(newUser);
                            //return RedirectToAction("RegisterConfirm", new { userphone = newUser.Phone, confirmationCode = newUser.ConfirmationCode });

                            if (_authProvider.Authenticate(newUser) == AuthResult.Success)
                            {
                                return RedirectToLocal(string.Empty);
                            }
                        }
                        else
                        {
                            error = Translation.Translation.RegisterPagePhoneNumberIsUsedAlready;
                        }
                    }
                    else {
                        error = Translation.Translation.RegisterPageShortPhoneNumber;
                    }
                }
            }
            else{
                error = Translation.Translation.RegisterPageError;
            }
            ModelState.AddModelError("", error);
            ViewBag.MaxMobileNumbers = int.Parse(ConfigurationManager.AppSettings["MaxAdditionalMobileNumbers"]);
            return View(model);
        }

        private void sendConfirmEmail(User user) {
            MailService.SendConfirmationEmail(user.EmailAddress, Url.Action("RegisterConfirm", "Account", new { userphone = user.Phone, confirmationCode = user.ConfirmationCode }, this.Request.Url.Scheme));
        }

        /*public ActionResult RegisterConfirm(string userphone, Guid? confirmationCode)
        {
            User user = _userRepository.GetUserByPhone(userphone);

            if (user != null)
            {
                if (confirmationCode.HasValue && confirmationCode.Value == user.ConfirmationCode)
                {
                    if (!user.IsConfirmed)
                    {
                        user.IsConfirmed = true;
                        _userRepository.SaveChanges();
                    }

                    bool hasRights;
                    if (_authProvider.Authenticate(user.Phone, user.Password, out hasRights))
                    {
                        return RedirectToLocal(string.Empty);
                    }
                }
            }

            throw new Exception(Translation.Translation.AccessIsDeniedMessage);
        }
        */

        #endregion

        #region Forget Password
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(string userphone)
        {
            User authentacatedUser = _userRepository.GetUserByPhone(StringHelper.GetOnlyNumbers(userphone));

            if (authentacatedUser != null)
            {
                string temporaryPassword = System.Web.Security.Membership.GeneratePassword(6, 2);
                authentacatedUser.TemporaryPassword = _passwordEncryptor.EncryptPassword(temporaryPassword);
                authentacatedUser.TemporaryPasswordExpires = DateTime.Now.AddDays(2);
                _userRepository.SaveChanges();
                MailService.SendTemporaryPassword(authentacatedUser.EmailAddress, temporaryPassword, authentacatedUser.TemporaryPasswordExpires.Value);
                TempData["LoginMessage"] = Translation.Translation.ForgetPasswordSuccess;
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", Translation.Translation.UserIsNotFoundMessage);
            return View();

        }

        /*public ActionResult SendConfirmEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendConfirmEmail(string userphone)
        {
            User authentacatedUser = _userRepository.GetUserByPhone(StringHelper.GetOnlyNumbers(userphone));

            if (authentacatedUser != null)
            {
                sendConfirmEmail(authentacatedUser);
                TempData["LoginMessage"] = Translation.Translation.SendConfirmEmailSuccess;

                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", Translation.Translation.UserIsNotFoundMessage);
            return View();

        }*/

        #endregion

        #region Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            string message = Translation.Translation.LoginPageLoginError;
            if (ModelState.IsValid)
            {
                if (loginService.Login(model.Phone, model.Password))
                {
                    return RedirectToLocal(returnUrl);
                }
            }

            ModelState.AddModelError("", message);
            return View(model);
        }

        [HttpPost]
        public ActionResult Logoff() {
            if (Request.IsAuthenticated) {
                FormsAuthentication.SignOut();
            }
            return Redirect("/");
        }
        #endregion

        #region Update limit

        [Authorize]
        public ActionResult UpdateLimit()
        {
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);

            if (authentacatedUser != null)
            {
                UpdateLimitModel model = new UpdateLimitModel();
                model.CurrentUser = authentacatedUser;
                model.Profiles = _profilesRepository.GetAll();
                ViewBag.IsMyCabinet = true;
                ViewBag.InfoMessage = TempData["UpdateLimitSuccessMessage"];
                ViewBag.UserPhone = authentacatedUser.Phone;
                return View(model);
            }
            else {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateLimitSuccess(string operation_xml, string signature)
        {
            string message = string.Format(Translation.Translation.UpdateFail, ConfigurationManager.AppSettings["ContactNumbers"]);
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                string signatureFromAnswer = LiqPay.GetSignFromAnswer(operation_xml);
                if (signatureFromAnswer == signature)
                {
                    Order order = new Order();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(LiqPay.GetOperationXmlFromAnswer(operation_xml));
                    XmlNode nodeOrderId = doc.SelectSingleNode("response/order_id");
                    if (nodeOrderId != null)
                    {
                        order.OrderId = Guid.Parse(nodeOrderId.InnerText);
                    }

                    XmlNode nodeGoodId = doc.SelectSingleNode("response/goods_id");
                    if (nodeGoodId != null)
                    {
                        order.ProfileId = int.Parse(nodeGoodId.InnerText);
                    }

                    XmlNode nodeAmmount = doc.SelectSingleNode("response/amount");
                    if (nodeAmmount != null)
                    {
                        order.Ammount = decimal.Parse(nodeAmmount.InnerText);
                    }

                    XmlNode nodeStatus = doc.SelectSingleNode("response/status");
                    if (nodeStatus != null)
                    {
                        order.Status = nodeStatus.InnerText;
                    }

                    order.User = authenticatedUser;

                    order.Created = DateTime.Now;

                    _ordersRepository.Add(order);
                    _ordersRepository.SaveChanges();

                    if (order.Status == "success")
                    {
                        Profile profile = _profilesRepository.GetById(order.ProfileId);
                        authenticatedUser.Profile = profile;
                        if (authenticatedUser.ProfileExpires.HasValue)
                        {
                            authenticatedUser.ProfileExpires = authenticatedUser.ProfileExpires.Value.AddMonths(1);
                        }
                        else
                        {
                            authenticatedUser.ProfileExpires = DateTime.Now.AddMonths(1);
                        }

                        IEnumerable<Announcement> addAnnouncements = authenticatedUser.Apartments.OrderByDescending(an => an.Created).Skip(profile.MaxAnnouncements);
                        foreach (Announcement announcement in addAnnouncements)
                        {
                            announcement.IsVisible = true;
                        }

                        foreach (Announcement announcement in authenticatedUser.Apartments)
                        {
                            IEnumerable<Photo> addPhotos = announcement.Photos.OrderByDescending(p => p.Created).Skip(profile.MaxPhotosPerAnnouncements);
                            foreach (Photo photo in addPhotos)
                            {
                                photo.IsVisible = true;
                            }
                        }
                        _userRepository.SaveChanges();

                        message = Translation.Translation.UpdateSuccess;
                    }
                }
            }

            TempData["UpdateLimitSuccessMessage"] = message;

            return RedirectToAction("UpdateLimit");
        }

        [HttpPost]
        public ActionResult UpdateLimitServer(string operation_xml, string signature)
        {
            return View();
        }

        #endregion

        #region User profile
        [Authorize]
        public ActionResult UserProfile()
        {
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authentacatedUser != null)
            {
                UserProfileModel model = new UserProfileModel();
                model.Email = authentacatedUser.EmailAddress;
                model.UserName = authentacatedUser.Username;
                model.CanReceiveEmail = authentacatedUser.ReceiveNewAnnouncements;
                return View(model);
            }
            else {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }
            
        }

        [HttpPost]
        [Authorize]
        public ActionResult UserProfile(UserProfileModel model)
        {
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authentacatedUser != null)
            {

                string error;
                if (model.IsValid(out error))
                {
                    authentacatedUser.EmailAddress = model.Email;
                    authentacatedUser.Username = model.UserName;
                    authentacatedUser.ReceiveNewAnnouncements = model.CanReceiveEmail;
                    _userRepository.SaveChanges();
                    TempData["message"] = Translation.Translation.UserDataSavedMessage;
                }

                else
                {
                    ModelState.AddModelError("", error);
                }

                return View(model);

            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }

        }

        #endregion

        #region Password change

        [Authorize]
        public ActionResult PasswordChange()
        {
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authentacatedUser != null)
            {
                ChangePasswordModel model = new ChangePasswordModel();
                return View(model);
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult PasswordChange(ChangePasswordModel model)
        {
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authentacatedUser != null)
            {
                string error;
                
                if (_passwordValidator.IsPasswordValid(model.OldPassword, authentacatedUser))
                {
                    if (model.IsValid(out error))
                    {
                        authentacatedUser.Password = _passwordEncryptor.EncryptPassword(model.Password);
                        _userRepository.SaveChanges();
                        TempData["message"] = Translation.Translation.UserDataSavedMessage;
                    }
                    else
                    {
                        ModelState.AddModelError("", error);
                    }
                }
                else
                {
                    error = Translation.Translation.ProfileOldPasswordIsNotCorrectMessage;
                    ModelState.AddModelError("", error);
                }

                return View(model);
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }

        }

        #endregion

        #region Edit phones

        [Authorize]
        public ActionResult EditPhones()
        {
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authentacatedUser != null)
            {
                EditPhonesModel model = new EditPhonesModel(authentacatedUser.Phone, authentacatedUser.AdditionalNumbers);
                return View(model);
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }

        }

        [HttpPost]
        [Authorize]
        public ActionResult EditPhones(string phone, ICollection<MobileNumber> mobileNumbers)
        {
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authentacatedUser != null)
            {
                if (mobileNumbers == null)
                    mobileNumbers = new List<MobileNumber>();

                ICollection<MobileNumber> deleteList = new List<MobileNumber>();
                ICollection<MobileNumber> changeList = new List<MobileNumber>();
                ICollection<MobileNumber> newList = new List<MobileNumber>();

                foreach (MobileNumber mobileNumber in authentacatedUser.AdditionalNumbers)
                {
                    deleteList.Add(mobileNumber);
                }

                foreach (MobileNumber mobileNumber in mobileNumbers)
                {
                    string mobileNumberOnlyNumbers = StringHelper.GetOnlyNumbers(mobileNumber.Number);
                    mobileNumber.Number = mobileNumberOnlyNumbers;
                    mobileNumber.User = authentacatedUser;

                    MobileNumber existingNumber = deleteList.FirstOrDefault(m => m.MobileNumberId == mobileNumber.MobileNumberId);
                    if (existingNumber == null)
                    {
                        newList.Add(mobileNumber);
                    }
                    else
                    {
                        deleteList.Remove(existingNumber);
                        if (existingNumber.Number != mobileNumber.Number)
                        {
                            existingNumber.Number = mobileNumber.Number;
                            changeList.Add(existingNumber);
                        }
                    }
                }

                foreach (MobileNumber mobileNumber in deleteList)
                {
                    _mobileNumbersRepository.Delete(mobileNumber);
                }

                foreach (MobileNumber mobileNumber in newList)
                {
                    _mobileNumbersRepository.Add(mobileNumber);
                }

                TempData["message"] = Translation.Translation.UserDataSavedMessage;
                _mobileNumbersRepository.SaveChanges();

                string phoneOnlyNumbers = StringHelper.GetOnlyNumbers(phone);
                if (authentacatedUser.Phone != phoneOnlyNumbers)
                {
                    authentacatedUser.Phone = phoneOnlyNumbers;
                    _userRepository.SaveChanges();
                    _authProvider.Authenticate(authentacatedUser);
                }
                

                return RedirectToAction("EditPhones");
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }

        }

        #endregion

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Apartments", "Announcements");
            }
        }

        /*
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }*/
        #endregion

        public ActionResult License()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _authProvider.Dispose();
                _mobileNumbersRepository.Dispose();
                _userRepository.Dispose();
                _profilesRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
