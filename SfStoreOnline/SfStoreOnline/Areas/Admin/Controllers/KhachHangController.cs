
using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class KhachHangController : Controller
    {
        // GET: Admin/KhachHang
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            PhanQuyen phanquyen = user.NhomNguoiDung.PhanQuyens.Where(t => t.MenuId.ToUpper().Trim() == "A00050").FirstOrDefault();
            ViewBag.phanquyen = phanquyen;
            ViewBag.isadmin = user.IsAdmin;
            var lstKhachHang = dataProvider.Entities.KhachHangs.ToList().OrderBy(t => t.Id);
            return View(lstKhachHang);
        }
        public ActionResult ViewThemMoi()
        {
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(KhachHang objKhachHang)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objKhachHang);
            }
            else
            {
                objKhachHang.Date0 = DateTime.Now.Date;
                objKhachHang.Time0 = DateTime.Now.TimeOfDay;
                objKhachHang.Date2 = DateTime.Now.Date;
                objKhachHang.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objKhachHang.UserId0 = _objectUser.Id;
                    objKhachHang.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.KhachHangs.Add(objKhachHang);
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
            var entity = dataProvider.Entities.KhachHangs.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.KhachHangs.Remove(entity);
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
            var entity = dataProvider.Entities.KhachHangs.Find(id);
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
            var entity = dataProvider.Entities.KhachHangs.Find(id);
            if (entity != null)
            {
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(KhachHang objKhachHang)
        {
            var entity = dataProvider.Entities.KhachHangs.Find(objKhachHang.Id);
            if (!ModelState.IsValid)
            {
                return PartialView("_Sua", objKhachHang);
            }
            else
            {
                objKhachHang.Date2 = DateTime.Now.Date;
                objKhachHang.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objKhachHang.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objKhachHang);
                dataProvider.Entities.SaveChanges();
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }


        }
    }
}