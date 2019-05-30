using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DanhSach(int Id)
        {
            return View();
        }
        public ActionResult TimKiem(string text)
        {
            return View();
        }
        public ActionResult ChiTiet(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            SanPham entity = dataProvider.Entities.SanPhams.Find(Id);
            if (entity != null)
            {
                return View(entity);
            }
            return View();
        }
    }
}