﻿using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    public class AuthenticationController : Controller
    {

        // GET: Authentication
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login(string returnURL)
        {
            var userinfo = new login();

            try
            {
                EnsureLoggedOut();
                userinfo.ReturnURL = returnURL;
                return View(userinfo);
            }
            catch
            {
                throw;
            }
        }
        private void EnsureLoggedOut()
        {

            if (Request.IsAuthenticated)
                Logout();
        }

        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();
                return RedirectToLocal();
            }
            catch
            {
                throw;
            }
        }
        private void SignInRemember(string userName, bool isPersistent = false)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.SetAuthCookie(userName, isPersistent);

        }
        private ActionResult RedirectToLocal(string returnURL = "")
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(returnURL) && Url.IsLocalUrl(returnURL))
                    return Redirect(returnURL);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(login entity)
        {
            string OldHASHValue = string.Empty;

            try
            {
                if (!ModelState.IsValid)
                    return View(entity);
                var userInfo = dataProvider.Entities.Users.Where(s => s.UserName == entity.UserName.Trim()).FirstOrDefault();

                //Assign HASH Value
                if (userInfo != null)
                {
                    OldHASHValue = userInfo.Password;
                }

                bool isLogin = CompareHashValue(entity.Password, OldHASHValue);

                if (isLogin)
                {
                    //Login Success
                    //For Set Authentication in Cookie (Remeber ME Option)
                    SignInRemember(entity.UserName, entity.isRemember);

                    Session[CommonConstants.USER_SESSION] = userInfo;
                    if (userInfo.TruyCapQuanTri)
                    {
                        return RedirectToLocal(entity.ReturnURL);
                    }
                    else
                    {
                        return View("Error403", "ErrorPages");
                    }


                }
                else
                {
                    //Login Fail
                    TempData["ErrorMSG"] = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                    return View(entity);
                }

            }
            catch
            {
                throw;
            }

        }

        public static bool CompareHashValue(string password, string OldHASHValue)
        {
            try
            {
                string expectedHashString = Public.Get_HASH_SHA512(password);

                return (OldHASHValue == expectedHashString);
            }
            catch
            {
                return false;
            }
        }

    }
}