using Newtonsoft.Json;
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
    public class DotKhuyenMaiController : Controller
    {
        // GET: Admin/DotKhuyenMai
        public ActionResult Index()
        {
            var lstDotKhuyenMai = dataProvider.Entities.DotKms.ToList().OrderBy(t => t.MaDotKm);
            return View(lstDotKhuyenMai);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            var lstLoaiSP = dataProvider.Entities.LoaiSanPhams.ToList().OrderBy(t => t.Id);
            //Gán danh sách lên dropdownlist
            ViewBag.TenLoaiSp = new SelectList(lstLoaiSP, "Id", "TenLoaiSp");

            var lstChietKhau = dataProvider.Entities.ChietKhaus.ToList().OrderBy(t => t.Id);
            //Gán danh sách lên dropdownlist
            ViewBag.TenCk = new SelectList(lstChietKhau, "Id", "TenCk");
            List<DotKmCt> lstdotKmCt = new List<DotKmCt>();
            DotKmCt dotKmCt = new DotKmCt();
            dotKmCt.ChietKhauId = 0;
            dotKmCt.LoaiSpId = 0;
            lstdotKmCt.Add(dotKmCt);
            ViewBag.lstdotKmCt = lstdotKmCt;
            return PartialView("_ThemMoi");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(DotKm objdotkm)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objdotkm);
            }
            else
            {
                objdotkm.Date0 = DateTime.Now.Date;
                objdotkm.Time0 = DateTime.Now.TimeOfDay;
                objdotkm.Date2 = DateTime.Now.Date;
                objdotkm.time2 = DateTime.Now.TimeOfDay;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objdotkm.UserId0 = _objectUser.Id;
                    objdotkm.UserId2 = _objectUser.Id;
                }

                dataProvider.Entities.DotKms.Add(objdotkm);
                dataProvider.Entities.SaveChanges();

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { success = true, JsonRequestBehavior.AllowGet });
            }
        }
        [HttpPost]
        public ActionResult SaveChiTiet(string getepassdata)
        {
            var serializeData = JsonConvert.DeserializeObject<List<DotKmCt>>(getepassdata);
            var _MaDotKm = "";
            bool _isSuccess = true;
            try
            {
                foreach (DotKmCt item in serializeData)
                {
                    if (item.MaDotkm == null)
                        continue;
                    _MaDotKm = item.MaDotkm;
                    dataProvider.Entities.DotKmCts.Add(item);
                    dataProvider.Entities.SaveChanges();
                }
            }
            catch
            {
                _isSuccess = false;

                var _lstPQ = dataProvider.Entities.DotKmCts.Where(t => t.MaDotkm == _MaDotKm).ToList();
                _lstPQ.ForEach(x => dataProvider.Entities.DotKmCts.Remove(x));

                var entity = dataProvider.Entities.DotKms.Find(_MaDotKm);
                if (entity != null)
                {

                    dataProvider.Entities.DotKms.Remove(entity);
                }
            }
            if (_isSuccess)
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, JsonRequestBehavior.AllowGet });
        }
        [HttpPost]
        public ActionResult Xoa(string MaDotKm, string NgayBd, string NgayKt)
        {

            if (MaDotKm == string.Empty)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DateTime _ngay_bd = Convert.ToDateTime(NgayBd).Date;
            DateTime _ngay_kt = Convert.ToDateTime(NgayKt).Date;
            var entity = dataProvider.Entities.DotKms.Find(MaDotKm, _ngay_bd, _ngay_kt);
            if (entity != null)
            {
                dataProvider.Entities.DotKmCts.RemoveRange(dataProvider.Entities.DotKmCts.Where(t => t.MaDotkm == MaDotKm && t.NgayBd == _ngay_bd && t.NgayKt == _ngay_kt));
                dataProvider.Entities.SaveChanges();

                dataProvider.Entities.DotKms.Remove(entity);
                dataProvider.Entities.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult ViewSua(string MaDotKm, string NgayBd, string NgayKt)
        {
            if (MaDotKm == string.Empty)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            DateTime _ngay_bd = Convert.ToDateTime(NgayBd).Date;
            DateTime _ngay_kt = Convert.ToDateTime(NgayKt).Date;
            var entity = dataProvider.Entities.DotKms.Find(MaDotKm, _ngay_bd, _ngay_kt);
            //var lstdotKmCt = dataProvider.Entities.DotKmCts.ToList().Where(t => t.MaDotkm == MaDotKm && t.NgayBd == _ngay_bd && t.NgayKt == _ngay_kt).OrderBy(t => t.MaDotkm == MaDotKm);

            //ViewBag.lstdotKmCt = lstdotKmCt;
            var lstLoaiSP = dataProvider.Entities.LoaiSanPhams.ToList().OrderBy(t => t.Id);
            //Gán danh sách lên dropdownlist
            ViewBag.TenLoaiSp = new SelectList(lstLoaiSP, "Id", "TenLoaiSp");
            ViewBag.lstLoaiSP = lstLoaiSP;
            ViewBag.NgayBd = _ngay_bd;
            var lstChietKhau = dataProvider.Entities.ChietKhaus.ToList().OrderBy(t => t.Id);
            //Gán danh sách lên dropdownlist
            ViewBag.TenCk = new SelectList(lstChietKhau, "Id", "TenCk");
            ViewBag.lstChietKhau = lstChietKhau;
            ViewBag.NgayBd = _ngay_bd;
            ViewBag.NgayKt = _ngay_kt;
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
    }
}