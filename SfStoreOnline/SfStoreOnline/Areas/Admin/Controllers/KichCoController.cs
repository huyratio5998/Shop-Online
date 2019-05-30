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
    public class KichCoController : Controller
    {
        // GET: Admin/KichCo
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            PhanQuyen phanquyen = user.NhomNguoiDung.PhanQuyens.Where(t => t.MenuId.ToUpper().Trim() == "A00053").FirstOrDefault();
            ViewBag.phanquyen = phanquyen;
            ViewBag.isadmin = user.IsAdmin;
            var lstKichCo = dataProvider.Entities.KichCoes.ToList().OrderBy(t => t.Id);
            return View(lstKichCo);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(KichCo objKichCo)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objKichCo);
            }
            else
            {
                objKichCo.Date0 = DateTime.Now.Date;
                objKichCo.Time0 = DateTime.Now.TimeOfDay;
                objKichCo.Date2 = DateTime.Now.Date;
                objKichCo.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objKichCo.UserId0 = _objectUser.Id;
                    objKichCo.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.KichCoes.Add(objKichCo);
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
            var entity = dataProvider.Entities.KichCoes.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.KichCoes.Remove(entity);
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
            var entity = dataProvider.Entities.KichCoes.Find(id);
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
            var entity = dataProvider.Entities.KichCoes.Find(id);
            if (entity != null)
            {
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(KichCo objKichCo)
        {
            var entity = dataProvider.Entities.KichCoes.Find(objKichCo.Id);
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objKichCo);
            }
            else
            {
                objKichCo.Date2 = DateTime.Now.Date;
                objKichCo.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objKichCo.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objKichCo);
                dataProvider.Entities.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }


        }
    }
}