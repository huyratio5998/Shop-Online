using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class NhaCungCapController : Controller
    {
        // GET: Admin/NhaCungCap
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            PhanQuyen phanquyen = user.NhomNguoiDung.PhanQuyens.Where(t => t.MenuId.ToUpper().Trim() == "A0006").FirstOrDefault();
            ViewBag.phanquyen = phanquyen;
            ViewBag.isadmin = user.IsAdmin;
            var lstNhaCungCap = dataProvider.Entities.NhaCungCaps.ToList().OrderBy(t => t.Id);
            return View(lstNhaCungCap);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(NhaCungCap objNhaCungCap)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objNhaCungCap);
            }
            else
            {
                objNhaCungCap.Date0 = DateTime.Now.Date;
                objNhaCungCap.Time0 = DateTime.Now.TimeOfDay;
                objNhaCungCap.Date2 = DateTime.Now.Date;
                objNhaCungCap.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objNhaCungCap.UserId0 = _objectUser.Id;
                    objNhaCungCap.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.NhaCungCaps.Add(objNhaCungCap);
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
            var entity = dataProvider.Entities.NhaCungCaps.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.NhaCungCaps.Remove(entity);
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
            var entity = dataProvider.Entities.NhaCungCaps.Find(id);
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
            var entity = dataProvider.Entities.NhaCungCaps.Find(id);
            if (entity != null)
            {
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(NhaCungCap objNhaCungCap)
        {
            var entity = dataProvider.Entities.NhaCungCaps.Find(objNhaCungCap.Id);
            if (!ModelState.IsValid)
            {
                return PartialView("_Sua", objNhaCungCap);
            }
            else
            {
                objNhaCungCap.Date2 = DateTime.Now.Date;
                objNhaCungCap.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objNhaCungCap.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objNhaCungCap);
                dataProvider.Entities.SaveChanges();
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }


        }
    }
}