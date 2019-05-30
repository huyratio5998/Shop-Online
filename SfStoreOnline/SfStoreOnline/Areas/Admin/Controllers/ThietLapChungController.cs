using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Areas.Admin.Controllers
{
    public class ThietLapChungController : Controller
    {
        // GET: Admin/ThietLapChung
        public ActionResult Index()
        {
            var dsloaisp = new SelectList(dataProvider.Entities.LoaiSanPhams.Where(t => t.Status == true).ToList(), "Id", "TenLoaiSp");
            ViewBag.dsloaisp = dsloaisp;
            int count = dataProvider.Entities.ThietLapChungs.Count();
            if (count > 0)//Sửa
            {
                ThietLapChung objThietlap = dataProvider.Entities.ThietLapChungs.FirstOrDefault();
                return View(objThietlap);
            }
            return View();
        }
        public ActionResult Save(ThietLapChung objThietLapChung)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            int count = dataProvider.Entities.ThietLapChungs.Count();
            if (count > 0)//Sửa
            {
                ThietLapChung entity = dataProvider.Entities.ThietLapChungs.FirstOrDefault();
                objThietLapChung.Logo = entity.Logo;
                objThietLapChung.SliderAnh1 = entity.SliderAnh1;
                objThietLapChung.SliderAnh2 = entity.SliderAnh2;
                objThietLapChung.SliderAnh3 = entity.SliderAnh3;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = "";
                    if (i == 0)
                        path = Path.Combine(Server.MapPath("~/Image/ThietLapChung/Logo"), fileName);
                    if (i == 1 || i == 2 || i == 3)
                        path = Path.Combine(Server.MapPath("~/Image/ThietLapChung/Slider"), fileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        file.SaveAs(path);
                        if (i == 0)
                            objThietLapChung.Logo = fileName;
                        if (i == 1)
                            objThietLapChung.SliderAnh1 = fileName;
                        if (i == 2)
                            objThietLapChung.SliderAnh2 = fileName;
                        if (i == 3)
                            objThietLapChung.SliderAnh3 = fileName;
                    }
                }
                objThietLapChung.Date2 = DateTime.Now.Date;
                objThietLapChung.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objThietLapChung.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objThietLapChung);
                dataProvider.Entities.SaveChanges();
            }
            else//Thêm mới
            {
                objThietLapChung.Logo = "";
                objThietLapChung.SliderAnh1 = "";
                objThietLapChung.SliderAnh2 = "";
                objThietLapChung.SliderAnh3 = "";
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    var fileName = Path.GetFileName(file.FileName);

                    var path = "";
                    if (i == 0)
                        path = Path.Combine(Server.MapPath("~/Image/ThietLapChung/Logo"), fileName);
                    if (i == 1 || i == 2 || i == 3)
                        path = Path.Combine(Server.MapPath("~/Image/ThietLapChung/Slider"), fileName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        file.SaveAs(path);
                        if (i == 0)
                            objThietLapChung.Logo = fileName;
                        if (i == 1)
                            objThietLapChung.SliderAnh1 = fileName;
                        if (i == 2)
                            objThietLapChung.SliderAnh2 = fileName;
                        if (i == 3)
                            objThietLapChung.SliderAnh3 = fileName;
                    }
                }
                objThietLapChung.Date0 = DateTime.Now.Date;
                objThietLapChung.Time0 = DateTime.Now.TimeOfDay;
                objThietLapChung.Date2 = DateTime.Now.Date;
                objThietLapChung.Time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objThietLapChung.UserId0 = _objectUser.Id;
                    objThietLapChung.UserId2 = _objectUser.Id;
                }
                dataProvider.Entities.ThietLapChungs.Add(objThietLapChung);
                dataProvider.Entities.SaveChanges();
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}