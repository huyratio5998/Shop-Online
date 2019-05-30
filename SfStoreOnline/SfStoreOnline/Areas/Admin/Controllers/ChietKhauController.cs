using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SfStoreOnline.Models;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    public class ChietKhauController : Controller
    {
        [Authorize]
        // GET: Admin/ChietKhau
        public ActionResult Index()
        {
            var lstChietKhau = dataProvider.Entities.ChietKhaus.ToList().OrderBy(t => t.Id);
            return View(lstChietKhau);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(ChietKhau objChietKhau)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objChietKhau);
            }
            else
            {
                objChietKhau.Date0 = DateTime.Now.Date;
                objChietKhau.Time0 = DateTime.Now.TimeOfDay;
                objChietKhau.Date2 = DateTime.Now.Date;
                objChietKhau.time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objChietKhau.UserId0 = _objectUser.Id;
                    objChietKhau.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.ChietKhaus.Add(objChietKhau);
                dataProvider.Entities.SaveChanges();
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }


        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.ChietKhaus.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.ChietKhaus.Remove(entity);
                dataProvider.Entities.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ViewChiTiet(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.ChietKhaus.Find(id);
            if (entity != null)
            {
                return PartialView("_Detail", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ViewSua(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.ChietKhaus.Find(id);
            if (entity != null)
            {
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(ChietKhau objChietKhau)
        {
            var entity = dataProvider.Entities.ChietKhaus.Find(objChietKhau.Id);
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objChietKhau);
            }
            else
            {
                objChietKhau.Date2 = DateTime.Now.Date;
                objChietKhau.time2 = DateTime.Now.TimeOfDay;
                //SfStoreOnline.Models.User _objectUser = (User)Session["User"];
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objChietKhau.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objChietKhau);
                dataProvider.Entities.SaveChanges();
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }


        }
    }
}