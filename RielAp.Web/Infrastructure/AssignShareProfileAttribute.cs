using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Infrastructure
{
    public class AssignShareProfileAttribute:FilterAttribute, IActionFilter
    {
        
        private string _shareProfileName;
        private DateTime? _shareProfileExpires;
        private DateTime? _shareTill;

        public AssignShareProfileAttribute(string shareProfileName,string shareAvailableTillKey, string shareProfileExpiresKey)
        {
            
            _shareProfileName = shareProfileName;
            _shareProfileExpires = null;
            string strProfileExpires = ConfigurationManager.AppSettings[shareProfileExpiresKey];
            if (!string.IsNullOrEmpty(strProfileExpires))
            {
                DateTime expires;
                if (DateTime.TryParse(strProfileExpires, out expires))
                {
                    _shareProfileExpires = expires;
                }
            }

            string strShareTill = ConfigurationManager.AppSettings[shareAvailableTillKey];
            if (!string.IsNullOrEmpty(strShareTill))
            {
                DateTime till;
                if (DateTime.TryParse(strShareTill, out till))
                {
                    _shareTill = till;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        { }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(_shareTill.HasValue){
                if (DateTime.Now.Date.CompareTo(_shareTill.Value.Date) <=0)
                {
                    IUsersRepository usersRepository = NinjectWebCommon.GetInstance<IUsersRepository>();
                    IProfilesRepository profilesRepository = NinjectWebCommon.GetInstance<IProfilesRepository>();
                    User authenticatedUser = usersRepository.GetUserByPhone(filterContext.HttpContext.User.Identity.Name);
                    if (authenticatedUser != null)
                    {
                        if (authenticatedUser.Apartments.Count < 1)
                        {
                            Profile shareProfile = profilesRepository.GetProfileByName(_shareProfileName);
                            if (shareProfile != null && _shareProfileExpires != null)
                            {
                                authenticatedUser.Profile = shareProfile;
                                authenticatedUser.ProfileExpires = _shareProfileExpires;
                                usersRepository.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}