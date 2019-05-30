using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
namespace SfStoreOnline.Controllers
{
    public class LienHeController : Controller
    {
        // GET: LienHe
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(LienHe objLienHe)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("Index", objLienHe);
            }
            else
            {
                var x=dataProvider.Entities.LienHes.Max(t => t.Id);
                x++;
                objLienHe.Id = x;
                objLienHe.NgayTao = DateTime.Now;
                objLienHe.GioTao = TimeSpan.FromHours(5);
                dataProvider.Entities.LienHes.Add(objLienHe);
                dataProvider.Entities.SaveChanges();
                Response.StatusCode = (int)HttpStatusCode.OK;

                return Json(new { success = true, JsonRequestBehavior.AllowGet });

            }


        }
    }
}