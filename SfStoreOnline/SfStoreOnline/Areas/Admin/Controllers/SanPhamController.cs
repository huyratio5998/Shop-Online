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
    public class SanPhamController : Controller
    {
        // GET: Admin/SanPham
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            PhanQuyen phanquyen = user.NhomNguoiDung.PhanQuyens.Where(t => t.MenuId.ToUpper().Trim() == "A00040").FirstOrDefault();
            ViewBag.phanquyen = phanquyen;
            ViewBag.isadmin = user.IsAdmin;
            var lstSanPham = dataProvider.Entities.SanPhams.ToList().OrderBy(p => p.Id);
            
            return View(lstSanPham);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            List<LoaiSanPham> lstLoaiSanPham = dataProvider.Entities.LoaiSanPhams.ToList();
            ViewBag.ChuDe = new SelectList(lstLoaiSanPham, "Id", "TenLoaiSp");
            List<KichCo> lstKichCo = dataProvider.Entities.KichCoes.ToList();
            ViewBag.KichCo = new SelectList(lstKichCo, "Id", "Size");
            List<NhaCungCap> lstNhaCungCap = dataProvider.Entities.NhaCungCaps.ToList();
            ViewBag.NhaCungCap = new SelectList(lstNhaCungCap, "Id", "HoTen");
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(SanPham objSanPham, HttpPostedFileBase fUpload)
        {
            if (!ModelState.IsValid)
            {
                
                List<LoaiSanPham> lstLoaiSanPham = dataProvider.Entities.LoaiSanPhams.ToList();
                ViewBag.ChuDe = new SelectList(lstLoaiSanPham, "Id", "TenLoaiSp");
                List<KichCo> lstKichCo = dataProvider.Entities.KichCoes.ToList();
                ViewBag.KichCo = new SelectList(lstKichCo, "Id", "Size");
                List<NhaCungCap> lstNhaCungCap = dataProvider.Entities.NhaCungCaps.ToList();
                ViewBag.NhaCungCap = new SelectList(lstNhaCungCap, "Id", "HoTen");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objSanPham);
            }
            else
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = System.IO.Path.GetFileName(file.FileName);

                    var path = System.IO.Path.Combine(Server.MapPath("~/Image/SanPham"), fileName);
                    if (!string.IsNullOrEmpty(path))
                    {
                        file.SaveAs(path);
                        objSanPham.ImageName = fileName;
                    }
                }

                objSanPham.Date0 = DateTime.Now.Date;
                objSanPham.Time0 = DateTime.Now.TimeOfDay;
                objSanPham.Date2 = DateTime.Now.Date;
                objSanPham.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objSanPham.UserId0 = _objectUser.Id;
                    objSanPham.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.SanPhams.Add(objSanPham);
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
            var entity = dataProvider.Entities.SanPhams.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.SanPhams.Remove(entity);
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
            var entity = dataProvider.Entities.SanPhams.Find(id);
            if (entity != null)
            {
                List<LoaiSanPham> lstLoaiSanPham = dataProvider.Entities.LoaiSanPhams.ToList();
                ViewBag.ChuDe = new SelectList(lstLoaiSanPham, "Id", "TenLoaiSp");
                List<KichCo> lstKichCo = dataProvider.Entities.KichCoes.ToList();
                ViewBag.KichCo = new SelectList(lstKichCo, "Id", "Size");
                List<NhaCungCap> lstNhaCungCap = dataProvider.Entities.NhaCungCaps.ToList();
                ViewBag.NhaCungCap = new SelectList(lstNhaCungCap, "Id", "HoTen");
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(SanPham objSanPham)
        {
            var entity = dataProvider.Entities.SanPhams.Find(objSanPham.Id);
           
            if (!ModelState.IsValid)
            {
                List<LoaiSanPham> lstLoaiSanPham = dataProvider.Entities.LoaiSanPhams.ToList();
                ViewBag.ChuDe = new SelectList(lstLoaiSanPham, "Id", "TenLoaiSp");
                List<KichCo> lstKichCo = dataProvider.Entities.KichCoes.ToList();
                ViewBag.KichCo = new SelectList(lstKichCo, "Id", "Size");
                List<NhaCungCap> lstNhaCungCap = dataProvider.Entities.NhaCungCaps.ToList();
                ViewBag.NhaCungCap = new SelectList(lstNhaCungCap, "Id", "HoTen");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objSanPham);
            }
            else
            {
                objSanPham.ImageName = entity.ImageName;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = System.IO.Path.GetFileName(file.FileName);

                        var path = System.IO.Path.Combine(Server.MapPath("~/Image/SanPham"), fileName);


                        file.SaveAs(path);
                        objSanPham.ImageName = fileName;
                    }
                }

                objSanPham.Date2 = DateTime.Now.Date;
                objSanPham.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objSanPham.UserId2 = _objectUser.Id;
                }
                
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objSanPham);
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
            var entity = dataProvider.Entities.SanPhams.Find(id);
            if (entity != null)
            {
                return PartialView("_Detail", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
    }
}