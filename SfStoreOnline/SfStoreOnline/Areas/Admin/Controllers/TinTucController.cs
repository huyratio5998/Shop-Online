
using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class TinTucController : Controller
    {
        // GET: Admin/TinTuc
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            PhanQuyen phanquyen = user.NhomNguoiDung.PhanQuyens.Where(t => t.MenuId.ToUpper().Trim() == "A0006").FirstOrDefault();
            ViewBag.phanquyen = phanquyen;
            ViewBag.isadmin = user.IsAdmin;
            var lstTinTuc = dataProvider.Entities.TinTucs.ToList().OrderBy(t => t.Id);
            return View(lstTinTuc);
        }
        public ActionResult ViewThemMoi()
        {
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(TinTuc objTinTuc)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objTinTuc);
            }
            else
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(Server.MapPath("~/Image/TinTuc"), fileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        file.SaveAs(path);
                        objTinTuc.ImageName = fileName;
                    }
                }
                objTinTuc.Date0 = DateTime.Now.Date;
                objTinTuc.Time0 = DateTime.Now.TimeOfDay;
                objTinTuc.Date2 = DateTime.Now.Date;
                objTinTuc.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objTinTuc.UserId0 = _objectUser.Id;
                    objTinTuc.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.TinTucs.Add(objTinTuc);
                dataProvider.Entities.SaveChanges();
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(true, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.TinTucs.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.TinTucs.Remove(entity);
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
            var entity = dataProvider.Entities.TinTucs.Find(id);
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
            var entity = dataProvider.Entities.TinTucs.Find(id);
            if (entity != null)
            {
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(TinTuc objTinTuc)
        {
            var entity = dataProvider.Entities.TinTucs.Find(objTinTuc.Id);
            if (!ModelState.IsValid)
            {
                return PartialView("_Sua", objTinTuc);
            }
            else
            {
                objTinTuc.ImageName = entity.ImageName;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(Server.MapPath("~/Image/TinTuc"), fileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        file.SaveAs(path);
                        objTinTuc.ImageName = fileName;
                    }
                }
                objTinTuc.Date2 = DateTime.Now.Date;
                objTinTuc.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objTinTuc.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objTinTuc);
                dataProvider.Entities.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }


        }
    }
}