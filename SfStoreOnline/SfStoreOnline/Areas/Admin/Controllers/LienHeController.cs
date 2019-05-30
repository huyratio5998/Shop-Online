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
    public class LienHeController : Controller
    {
        // GET: Admin/LienHe
        public ActionResult Index()
        {
            var lstLienHe = dataProvider.Entities.LienHes.ToList().OrderBy(p => p.Id);
            return View(lstLienHe);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            var entity = dataProvider.Entities.LienHes.Find(id);
            if (entity != null)
            {
                dataProvider.Entities.LienHes.Remove(entity);
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
            var entity = dataProvider.Entities.LienHes.Find(id);
            if (entity != null)
            {
                return PartialView("_Detail", entity);
            }

            return Json(false, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Doitt(int id)
        {
            var entity = dataProvider.Entities.LienHes.Find(id);
            var objLienHe = dataProvider.Entities.LienHes.Find(id);
            if (objLienHe == null)
                return Json(new { success = false, JsonRequestBehavior.AllowGet });
            objLienHe.Da_phan_hoi_yn = !objLienHe.Da_phan_hoi_yn;
            dataProvider.Entities.Entry(entity).CurrentValues.SetValues(objLienHe);
            dataProvider.Entities.SaveChanges();
            return Json(new { success = true, TrangThai = objLienHe.Da_phan_hoi_yn }, JsonRequestBehavior.AllowGet);

        }
    }
}