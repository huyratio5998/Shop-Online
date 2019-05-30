using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SfStoreOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var thongtinchung = dataProvider.Entities.ThietLapChungs.FirstOrDefault();
            ViewBag.thongtinchung = thongtinchung;

            MucNoiBat mucnoibat = new MucNoiBat();

            var loaisp1 = dataProvider.Entities.LoaiSanPhams.Where(t => t.Id == thongtinchung.LoaiSpId1).FirstOrDefault();
            mucnoibat.loaisp1 = loaisp1;

            var loaisp2 = dataProvider.Entities.LoaiSanPhams.Where(t => t.Id == thongtinchung.LoaiSpId2).FirstOrDefault();
            mucnoibat.loaisp2 = loaisp2;

            var loaisp3 = dataProvider.Entities.LoaiSanPhams.Where(t => t.Id == thongtinchung.LoaiSpId3).FirstOrDefault();
            mucnoibat.loaisp3 = loaisp3;

            var loaisp4 = dataProvider.Entities.LoaiSanPhams.Where(t => t.Id == thongtinchung.LoaiSpId4).FirstOrDefault();
            mucnoibat.loaisp4 = loaisp4;

            var loaisp5 = dataProvider.Entities.LoaiSanPhams.Where(t => t.Id == thongtinchung.LoaiSpId5).FirstOrDefault();
            mucnoibat.loaisp5 = loaisp5;

            var loaisp6 = dataProvider.Entities.LoaiSanPhams.Where(t => t.Id == thongtinchung.LoaiSpId6).FirstOrDefault();
            mucnoibat.loaisp6 = loaisp6;

            ViewBag.mucnoibat = mucnoibat;

            return View();
        }
        [ChildActionOnly]
        public ActionResult Header()
        {
            var lstLoaiSanPham = dataProvider.Entities.LoaiSanPhams.Where(t => t.Status == true).ToList().OrderBy(t=>t.TenLoaiSp);
            ViewBag.lstLoaiSanPham = lstLoaiSanPham;
            return PartialView("_Header");
        }
        [ChildActionOnly]
        public ActionResult Footer()
        {
            var thongtinchung = dataProvider.Entities.ThietLapChungs.FirstOrDefault();

            var loaisp = dataProvider.Entities.LoaiSanPhams.Where(t => t.Status == true).ToList().OrderBy(t => t.Date2).Take(4);

            ViewBag.loaisp = loaisp;

            return PartialView("_Footer", thongtinchung);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveContact(LienHe objLienHe)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("Contact", objLienHe);
            }
            else
            {
                var x = dataProvider.Entities.LienHes.Max(t => t.Id);
                x++;
                objLienHe.Id = x;
                objLienHe.NgayTao = DateTime.Now;                
                dataProvider.Entities.LienHes.Add(objLienHe);
                dataProvider.Entities.SaveChanges();
                Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new { success = true, JsonRequestBehavior.AllowGet });

            }
        }
        public ActionResult GetSession()
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (authCookie == null)
                    return null;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                var cookie = ticket.Name;

                var user = dataProvider.Entities.Users.Where(t => t.UserName == cookie).SingleOrDefault();
                Session[CommonConstants.USER_SESSION] = user;
                if (user != null)
                {
                    var cart = dataProvider.Entities.GioHangs.Where(t => t.UserId == user.Id).ToList();
                    if (cart == null)
                        cart = new List<GioHang>();
                    Session[CommonConstants.CART_SESSION] = cart;
                }
            }

            return null;
        }
        [ChildActionOnly]
        public ActionResult SanPhamMoiNhat()
        {
            var lstmoi = dataProvider.Entities.SanPhams.Where(t => t.Status == true && t.SpMoi == true).ToList().OrderBy(t => t.Date2).Take(8);
            return PartialView("SanPhamMoiNhat", lstmoi);
        }
        [ChildActionOnly]
        public ActionResult SanPhamNoiBat()
        {
            var lstnoibat = dataProvider.Entities.SanPhams.Where(t => t.Status == true && t.SpNoiBat == true).ToList().OrderBy(t => t.Date2).Take(8);
            return PartialView("SanPhamNoiBat", lstnoibat);
        }
        [ChildActionOnly]
        public ActionResult TinTucNb()
        {
            var lstTintuc = dataProvider.Entities.TinTucs.Where(t=>t.Status == true).OrderBy(t=>t.Date0).Take(3);
            return PartialView("TinTucNb", lstTintuc);
        }

    }
}