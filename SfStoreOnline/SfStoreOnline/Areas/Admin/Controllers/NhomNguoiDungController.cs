using Newtonsoft.Json;
using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    [Authorize]
    public class NhomNguoiDungController : Controller
    {
        // GET: Admin/NhomNguoiDung
        public ActionResult Index()
        {
            var user = (User)Session[CommonConstants.USER_SESSION];
            PhanQuyen phanquyen = user.NhomNguoiDung.PhanQuyens.Where(t => t.MenuId.ToUpper().Trim() == "A00081").FirstOrDefault();
            ViewBag.phanquyen = phanquyen;
            ViewBag.isadmin = user.IsAdmin;
            var lstNhomUser = dataProvider.Entities.NhomNguoiDungs.ToList().OrderBy(t => t.TenNhom);
            return View(lstNhomUser);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            
            //NhomNguoiDung nhomNguoiDung = new NhomNguoiDung();

            var lstMenu = dataProvider.Entities.MenuAdmins.ToList().OrderBy(t => t.Stt);
           
            List<PhanQuyen> lstPhanQuyen = new List<PhanQuyen>();
            foreach (var item in lstMenu)
            {
                PhanQuyen phanQuyen = new PhanQuyen();
                phanQuyen.MenuId = item.MenuId;
                MenuAdmin menu = new MenuAdmin();
                menu = dataProvider.Entities.MenuAdmins.Find(item.MenuId);
                phanQuyen.MenuAdmin = menu;
                phanQuyen.Xem = true;
                phanQuyen.Them = true;
                phanQuyen.Sua = true;
                phanQuyen.Xoa = true;
                lstPhanQuyen.Add(phanQuyen);
            }
            ViewBag.lstPhanQuyen = lstPhanQuyen;
            //if (dataProvider.Entities.Entry(nhomNguoiDung).State == EntityState.Detached)
            //{
            //    dataProvider.Entities.Entry(nhomNguoiDung).State = EntityState.Detached;

            //    dataProvider.Entities.Set<NhomNguoiDung>().Attach(nhomNguoiDung);
            //    dataProvider.Entities.SaveChanges();
            //}
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(NhomNguoiDung objnhomnguoidung)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objnhomnguoidung);
            }
            else
            {
                objnhomnguoidung.Date0 = DateTime.Now.Date;
                objnhomnguoidung.Time0 = DateTime.Now.TimeOfDay;
                objnhomnguoidung.Date2 = DateTime.Now.Date;
                objnhomnguoidung.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objnhomnguoidung.UserId0 = _objectUser.Id;
                    objnhomnguoidung.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.NhomNguoiDungs.Add(objnhomnguoidung);
                dataProvider.Entities.SaveChanges();
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Id = objnhomnguoidung.Id});
            }


        }
        [HttpPost]
        public ActionResult SavePhanQuyen(string getepassdata)
        {
            var serializeData = JsonConvert.DeserializeObject<List<PhanQuyen>>(getepassdata);
            long _NhomId = 0;
            bool _isSuccess = true;
            try
            {
                foreach (PhanQuyen item in serializeData)
                {
                    if (item.MenuId == null)
                        continue;
                    _NhomId = item.NhomId;
                    dataProvider.Entities.PhanQuyens.Add(item);
                }
            }
            catch
            {
                _isSuccess = false;

                var _lstPQ = dataProvider.Entities.PhanQuyens.Where(t => t.NhomId == _NhomId).ToList();
                _lstPQ.ForEach(x => dataProvider.Entities.PhanQuyens.Remove(x));

                var entity = dataProvider.Entities.NhomNguoiDungs.Find(_NhomId);
                if (entity != null)
                {

                    dataProvider.Entities.NhomNguoiDungs.Remove(entity);
                }
            }
            if (_isSuccess)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                dataProvider.Entities.SaveChanges();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.NhomNguoiDungs.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.PhanQuyens.RemoveRange(dataProvider.Entities.PhanQuyens.Where(t => t.NhomId == id));
                dataProvider.Entities.SaveChanges();

                dataProvider.Entities.NhomNguoiDungs.Remove(entity);
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
            var entity = dataProvider.Entities.NhomNguoiDungs.Find(id);
            if (entity != null)
            {
                return PartialView("_Sua", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveSua(NhomNguoiDung objnhomnguoidung)
        {
            var entity = dataProvider.Entities.NhomNguoiDungs.Find(objnhomnguoidung.Id);
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objnhomnguoidung);
            }
            else
            {
                objnhomnguoidung.Date2 = DateTime.Now.Date;
                objnhomnguoidung.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objnhomnguoidung.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objnhomnguoidung);
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Id = objnhomnguoidung.Id });
            }
        }
        [HttpPost]
        public ActionResult SavePhanQuyenSua(string getepassdata, int Id)
        {
            var serializeData = JsonConvert.DeserializeObject<List<PhanQuyen>>(getepassdata);
            bool _isSuccess = true;
            try
            {
                foreach (PhanQuyen item in serializeData)
                {
                    if (item.MenuId == null)
                        continue;
                    var entity = dataProvider.Entities.PhanQuyens.Find(Id,item.MenuId);
                    dataProvider.Entities.Entry(entity).CurrentValues.SetValues(item);
                   
                }
            }
            catch
            {
                _isSuccess = false;
            }
            if (_isSuccess)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                dataProvider.Entities.SaveChanges();
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, JsonRequestBehavior.AllowGet });
        }


    }
}