using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class LoaiSanPhamController : Controller
    {
        // GET: Admin/LoaiSanPham
        public ActionResult Index()
        {
            var lstLoaiSanPham = dataProvider.Entities.LoaiSanPhams.ToList().OrderBy(p => p.Id);
            return View(lstLoaiSanPham);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {

            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(LoaiSanPham objLoaiSanPham)
        {
            if (!ModelState.IsValid)
            {

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objLoaiSanPham);
            }
            else
            {
                objLoaiSanPham.ImagName = "";
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = System.IO.Path.GetFileName(file.FileName);

                    var path = System.IO.Path.Combine(Server.MapPath("~/Image/LoaiSp"), fileName);
                    if (!string.IsNullOrEmpty(path))
                    {
                        file.SaveAs(path);
                        objLoaiSanPham.ImagName = fileName;
                    }
                }


                objLoaiSanPham.Date0 = DateTime.Now.Date;
                objLoaiSanPham.Time0 = DateTime.Now.TimeOfDay;
                objLoaiSanPham.Date2 = DateTime.Now.Date;
                objLoaiSanPham.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objLoaiSanPham.UserId0 = _objectUser.Id;
                    objLoaiSanPham.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.LoaiSanPhams.Add(objLoaiSanPham);
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
            var entity = dataProvider.Entities.LoaiSanPhams.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.LoaiSanPhams.Remove(entity);
                dataProvider.Entities.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
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
            LoaiSanPham entity = dataProvider.Entities.LoaiSanPhams.Find(id);
            if (entity != null)
            {

                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(LoaiSanPham objLoaiSanPham, HttpPostedFileBase fUpload)
        {
            LoaiSanPham entity = dataProvider.Entities.LoaiSanPhams.Find(objLoaiSanPham.Id);

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objLoaiSanPham);
            }
            else
            {

                objLoaiSanPham.ImagName = entity.ImagName;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {

                        var fileName = System.IO.Path.GetFileName(file.FileName);

                        var path = System.IO.Path.Combine(Server.MapPath("~/Image/LoaiSp"), fileName);
                        if (!string.IsNullOrEmpty(path))
                        {
                            file.SaveAs(path);
                            objLoaiSanPham.ImagName = fileName;
                        }
                    }
                }

                objLoaiSanPham.Date2 = DateTime.Now.Date;
                objLoaiSanPham.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objLoaiSanPham.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objLoaiSanPham);
                dataProvider.Entities.SaveChanges();
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }


        }
        [HttpGet]
        public ActionResult ViewChiTiet(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.LoaiSanPhams.Find(id);
            if (entity != null)
            {
                return PartialView("_Detail", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
    }
}