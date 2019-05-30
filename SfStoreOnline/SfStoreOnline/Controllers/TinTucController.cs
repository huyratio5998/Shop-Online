using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SfStoreOnline.Controllers
{
    public class TinTucController : Controller
    {
        // GET: TinTuc
        public ActionResult Index()
        {
            var lstTinTuc = dataProvider.Entities.TinTucs.ToList();
            return View(lstTinTuc);
        }
        //public ActionResult Index(int id)
        //{
        //    var date = dataProvider.Entities.TinTucs.Find(id).Date0;
        //    var lstTinTuc = dataProvider.Entities.TinTucs.GroupBy(t => t.Date0).ToList();
        //    return View(lstTinTuc);
        //}
        public ActionResult _Detail(int id)
        {
            var lstTinTuc = dataProvider.Entities.TinTucs.ToList();
            var objTinTuc = dataProvider.Entities.TinTucs.Find(id);
            ViewBag.objTinTuc = objTinTuc;
            return View(lstTinTuc);
        }
        [ChildActionOnly]
        public ActionResult LayoutTinTuc()
        {
            return PartialView("LayoutTinTuc");
        }
    }
}