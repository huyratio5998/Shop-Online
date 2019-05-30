using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult MenuLeft()
        {

            var model = dataProvider.Entities.MenuAdmins.Where(t => t.AnMenu == false).ToList();
            return PartialView("_MenuLeft", model);
        }
        public ActionResult GetSession()
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null)
                    return null;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                var cookie = ticket.Name;
                if (string.IsNullOrEmpty(cookie))
                {
                    return View("Login", "Authentication");
                }

                var user = dataProvider.Entities.Users.Where(t => t.UserName == cookie).FirstOrDefault();
                Session[CommonConstants.USER_SESSION] = user;
            }

            return null;
        }
    }
}