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
    public class DonHangBanController : Controller
    {
        // GET: Admin/DonHangBan
        public ActionResult Index()
        {
            var lstDonHangBan = dataProvider.Entities.DonHangBans.ToList().OrderBy(t => t.Id);
            return View(lstDonHangBan);
        }

        [HttpGet]
        public ActionResult ViewThemMoi()
        {
           
            DonHangBan objDonHangBan = new DonHangBan();

            try
            {
                objDonHangBan.SoDh = "PN" + dataProvider.Entities.DonHangBans.Max(t => t.Id);
            }catch(Exception e)
            {
                objDonHangBan.SoDh = "PN000";
            }
            List<KhachHang> lstKhachHang = dataProvider.Entities.KhachHangs.ToList();
            ViewBag.lstKhachHang = new SelectList(lstKhachHang, "Id", "HoTen");
            //
            List<SanPham> lstSanPham = dataProvider.Entities.SanPhams.ToList();
            ViewBag.lstSanPham = new SelectList(lstSanPham, "Id", "TenSp");
            List<ChietKhau> lstChietKhau = dataProvider.Entities.ChietKhaus.ToList();
            ViewBag.lstChietKhau = new SelectList(lstChietKhau, "Id", "TenCK");
            List<DonHangBanCt> lstDonHangBanCT = new List<DonHangBanCt>();
            DonHangBanCt objDonHangBanCt = new DonHangBanCt();


            lstDonHangBanCT.Add(objDonHangBanCt);

            ViewBag.lstDonHangBanCt = lstDonHangBanCT;
            return PartialView("_ThemMoi", objDonHangBan);
        }

        [HttpGet]
        public ActionResult LayTong(string getepassdata)
        {
            List<itemDonHangBan> serializeData = JsonConvert.DeserializeObject<List<itemDonHangBan>>(getepassdata);

            List<ChietKhau> lstCk = dataProvider.Entities.ChietKhaus.ToList();

            foreach (itemDonHangBan item in serializeData)
            {
                if (item.ChietKhauId != 0)
                {
                    item.ptck = lstCk.Where(t => t.Id == item.ChietKhauId).FirstOrDefault().PtCk.GetValueOrDefault(0);
                }
                else
                {
                    item.ptck = 0;
                }
            }

            SumVoucher _sumVoucher = new SumVoucher();
            _sumVoucher.TongSL = serializeData.Sum(t => t.SoLuong);
            _sumVoucher.TongTien = serializeData.Sum(t => t.SoLuong * t.GiaBan);
            _sumVoucher.TongCK = serializeData.Sum(t => (t.SoLuong * t.GiaBan) * t.ptck / 100);
            _sumVoucher.Tongtt = _sumVoucher.TongTien - _sumVoucher.TongCK;
            return Json(_sumVoucher, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(DonHangBan objDonHangBan)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objDonHangBan);
            }
            else
            {

                objDonHangBan.Date0 = DateTime.Now.Date;
                objDonHangBan.Time0 = DateTime.Now.TimeOfDay;
                objDonHangBan.Date2 = DateTime.Now.Date;
                objDonHangBan.Time2 = DateTime.Now.TimeOfDay;
                objDonHangBan.NgayBan = DateTime.Now.Date;
                User _objectUser = (User)Session[CommonConstants.USER_SESSION];

                if (_objectUser != null)
                {
                    objDonHangBan.Status = 1;
                    objDonHangBan.giohang_yn = true;
                    objDonHangBan.UserId0 = _objectUser.Id;
                    objDonHangBan.UserId2 = _objectUser.Id;
                    
                }

                dataProvider.Entities.DonHangBans.Add(objDonHangBan);
                //dataProvider.Entities.SaveChanges();

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Id = objDonHangBan.Id });
            }
        }
        [HttpPost]
        public ActionResult SaveChiTiet(string getepassdata)
        {
            List<DonHangBanCt> serializeData = JsonConvert.DeserializeObject<List<DonHangBanCt>>(getepassdata);
            long _PhieuNhapId = 0;
            bool _isSuccess = true;
            decimal SoLuong = 0, GiaBan = 0, ChietKhau = 0, tong_tt = 0, tong_ck = 0;
            try
            {
                foreach (DonHangBanCt item in serializeData)
                {
                    if (item.SanPhamId.Equals(null))
                    {
                        continue;
                    }
                    _PhieuNhapId = item.DonHangId;

                    dataProvider.Entities.DonHangBanCts.Add(item);
                    SoLuong = item.SoLuong.GetValueOrDefault(0);
                    GiaBan = item.GiaBan.GetValueOrDefault(0);
                    if (!item.ChietKhauId.Equals(null))
                    {
                        ChietKhau objChietKhau = dataProvider.Entities.ChietKhaus.Find(item.ChietKhauId);
                        ChietKhau = objChietKhau.PtCk.GetValueOrDefault(0);
                    }

                    tong_tt += (SoLuong * GiaBan) - (SoLuong * GiaBan) * ChietKhau / 100;
                    tong_ck += (SoLuong * GiaBan) * ChietKhau / 100;
                    dataProvider.Entities.DonHangBanCts.Add(item);
                }

                DonHangBan objDonHangBan = dataProvider.Entities.DonHangBans.Find(_PhieuNhapId);
                objDonHangBan.TongSL = serializeData.Sum(t => t.SoLuong);
                objDonHangBan.TongTien = serializeData.Sum(t => t.SoLuong * t.GiaBan); ;
                objDonHangBan.TongCk = tong_ck;
                objDonHangBan.TongTt = tong_tt - objDonHangBan.PhiVc;
                dataProvider.Entities.DonHangBans.Add(objDonHangBan);
            }
            catch
            {
                _isSuccess = false;
                //var _lstPQ = dataProvider.Entities.NhapKhoCts.Where(t => t.PhieuNhapId == _PhieuNhapId).ToList();
                //_lstPQ.ForEach(x => dataProvider.Entities.NhapKhoCts.Remove(x));

                //var entity = dataProvider.Entities.NhapKhoCts.Find(_PhieuNhapId);
                //if (entity != null)
                //{
                //    dataProvider.Entities.NhapKhoCts.RemoveRange(dataProvider.Entities.NhapKhoCts.Where(t => t.PhieuNhapId == _PhieuNhapId));
                //    dataProvider.Entities.NhapKhoCts.Remove(entity);
                //}
            }
            if (_isSuccess)
            {
                dataProvider.Entities.SaveChanges();
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
        public ActionResult Xoa(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            DonHangBan entity = dataProvider.Entities.DonHangBans.Find(id);

            if (entity != null)
            {
                dataProvider.Entities.DonHangBanCts.RemoveRange(dataProvider.Entities.DonHangBanCts.Where(t => t.DonHangId == id));
                dataProvider.Entities.DonHangBans.Remove(entity);
                dataProvider.Entities.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Sua(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                DonHangBan objDonBanHang = dataProvider.Entities.DonHangBans.Find(id);
                List<SanPham> lstSanPham = dataProvider.Entities.SanPhams.ToList();
                ViewBag.lstSanPham = lstSanPham;
                List<ChietKhau> lstChietKhau = dataProvider.Entities.ChietKhaus.ToList();
                ViewBag.lstChietKhau = lstChietKhau;

                List<KhachHang> lstKhachHang = dataProvider.Entities.KhachHangs.ToList();
                ViewBag.lstKhachHang = new SelectList(lstKhachHang, "Id", "HoTen");

                return View(objDonBanHang);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSua(DonHangBan objDonBanHang)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objDonBanHang);
            }
            else
            {
                DonHangBan objDonBanHangSua = dataProvider.Entities.DonHangBans.Find(objDonBanHang.Id);
                objDonBanHang.Date0 = DateTime.Now.Date;
                objDonBanHang.Time0 = DateTime.Now.TimeOfDay;
                objDonBanHang.Date2 = DateTime.Now.Date;
                objDonBanHang.Time2 = DateTime.Now.TimeOfDay;
                objDonBanHang.NgayBan = objDonBanHangSua.NgayBan;
                User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objDonBanHang.Status = 1;
                    objDonBanHang.giohang_yn = true;
                    objDonBanHang.UserId2 = _objectUser.Id;
                    objDonBanHang.UserId0 = _objectUser.Id;

                    
                }
                dataProvider.Entities.Entry(objDonBanHangSua).CurrentValues.SetValues(objDonBanHang);

                return Json(new { Id = objDonBanHang.Id });
            }
        }
        //save sua chi chiet


        public ActionResult SaveSuaChiTiet(string getepassdata, int Id)
        {

            List<DonHangBanCt> serializeData = JsonConvert.DeserializeObject<List<DonHangBanCt>>(getepassdata);
            //List<NhapKhoCt> lstNhapKhoFull = dataProvider.Entities.NhapKhoCts.Where(p=>p.PhieuNhapId==Id).ToList();
            decimal SoLuong = 0, GiaBan = 0, ChietKhau = 0;
            long _PhieuNhapId = 0;
            bool _isSuccess = true;
            try
            {

                foreach (var item in serializeData)
                {

                    if (item.SanPhamId.Equals(null))
                    {
                        continue;
                    }
                    item.DonHangId = Id;
                    _PhieuNhapId = item.DonHangId;
                    SoLuong = item.SoLuong.GetValueOrDefault(0);
                    GiaBan = item.GiaBan.GetValueOrDefault(0);
                    if (item.ChietKhauId != 0)
                    {
                        ChietKhau objChietKhau = dataProvider.Entities.ChietKhaus.Find(item.ChietKhauId);
                        ChietKhau = objChietKhau.PtCk.GetValueOrDefault(0);
                    }
                    else
                    {
                        item.ChietKhauId = null;
                        ChietKhau = 0;
                    }
                    var entity = dataProvider.Entities.DonHangBanCts.Find(Id, item.SanPhamId);
                    // lam the nao lay du lieu hien tai de thay the;
                    dataProvider.Entities.Entry(entity).CurrentValues.SetValues(item);
                }


            }
            catch
            {
                _isSuccess = false;
                //var _lstPQ = dataProvider.Entities.NhapKhoCts.Where(t => t.PhieuNhapId == _PhieuNhapId).ToList();
                //_lstPQ.ForEach(x => dataProvider.Entities.NhapKhoCts.Remove(x));

                //var entity = dataProvider.Entities.NhapKhoCts.Find(_PhieuNhapId);
                //if (entity != null)
                //{
                //    dataProvider.Entities.NhapKhoCts.RemoveRange(dataProvider.Entities.NhapKhoCts.Where(t => t.PhieuNhapId == _PhieuNhapId));
                //    dataProvider.Entities.NhapKhoCts.Remove(entity);
                //}
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