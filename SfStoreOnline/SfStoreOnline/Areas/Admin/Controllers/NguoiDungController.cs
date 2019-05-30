using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class NguoiDungController : Controller
    {
      
        // GET: Admin/NguoiDung
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            PhanQuyen phanquyen = user.NhomNguoiDung.PhanQuyens.Where(t => t.MenuId.ToUpper().Trim() == "A00082").FirstOrDefault();
            ViewBag.phanquyen = phanquyen;
            ViewBag.isadmin = user.IsAdmin;
            var lstUser = dataProvider.Entities.Users.ToList().OrderBy(t => t.Id);
            return View(lstUser);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            //Lấy list nhóm người dùng
            var lstNhom = dataProvider.Entities.NhomNguoiDungs.ToList();
            ViewBag.lstNhom = new SelectList(lstNhom, "Id", "TenNhom");
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(User objNguoiDung)
        {
            if (!IsUserExists(objNguoiDung.UserName))
            {
                var lstNhom = dataProvider.Entities.NhomNguoiDungs.ToList();
                ViewBag.lstNhom = new SelectList(lstNhom, "Id", "TenNhom");
                ModelState.AddModelError("", "UserName đã tồn tại.");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objNguoiDung);
            }
            if (!ModelState.IsValid)
            {
                var lstNhom = dataProvider.Entities.NhomNguoiDungs.ToList();
                ViewBag.lstNhom = new SelectList(lstNhom, "Id", "TenNhom");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objNguoiDung);
            }
            else
            {
                objNguoiDung.AvatarName = "";
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(Server.MapPath("~/Image/NguoiDung"), fileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        file.SaveAs(path);
                        objNguoiDung.AvatarName = fileName;
                    }
                }
                ///Xử lý băm password
                objNguoiDung.Password = Public.Get_HASH_SHA512(objNguoiDung.Password);
                objNguoiDung.Date0 = DateTime.Now.Date;
                objNguoiDung.Time0 = DateTime.Now.TimeOfDay;
                objNguoiDung.Date2 = DateTime.Now.Date;
                objNguoiDung.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objNguoiDung.UserId0 = _objectUser.Id;
                    objNguoiDung.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Users.Add(objNguoiDung);
                try
                {
                    dataProvider.Entities.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {

                    throw;
                }
               
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { id = objNguoiDung.Id });
            }


        }

        [HttpGet]
        public ActionResult ViewSua(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.Users.Find(id);
            if (entity != null)
            {
                //Lấy list nhóm người dùng
                var lstNhom = dataProvider.Entities.NhomNguoiDungs.ToList();
                ViewBag.lstNhom = new SelectList(lstNhom, "Id", "TenNhom");
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(User objNguoiDung)
        {
            var entity = dataProvider.Entities.Users.Find(objNguoiDung.Id);
            if (!ModelState.IsValid)
            {
                var lstNhom = dataProvider.Entities.NhomNguoiDungs.ToList();
                ViewBag.lstNhom = new SelectList(lstNhom, "Id", "TenNhom");
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objNguoiDung);
            }
            else
            {
                objNguoiDung.AvatarName = entity.AvatarName;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = Path.Combine(Server.MapPath("~/Image/NguoiDung"), fileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        file.SaveAs(path);
                        objNguoiDung.AvatarName = fileName;
                    }
                }
                //Kiểm tra nếu người dùng sửa password thì băm lại password
                if (entity.Password != objNguoiDung.Password)
                {
                    objNguoiDung.Password = Public.Get_HASH_SHA512(objNguoiDung.Password);
                }
                objNguoiDung.Date2 = DateTime.Now.Date;
                objNguoiDung.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objNguoiDung.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objNguoiDung);
                dataProvider.Entities.SaveChanges();
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
            var entity = dataProvider.Entities.Users.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.Users.Remove(entity);
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
            var entity = dataProvider.Entities.Users.Find(id);
            if (entity != null)
            {
                return PartialView("_Detail", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }

        public bool IsUserExists(string UserName)
        {
            return !dataProvider.Entities.Users.Any(x => x.UserName == UserName);
        }

       

    }
}