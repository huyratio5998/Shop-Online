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
    public class NhapKhoController : Controller
    {
        // GET: Admin/NhapKho       



        public ActionResult Index()
        {
            
            var lstNhapKho = dataProvider.Entities.NhapKhoes.ToList().OrderBy(p => p.Id);
            

            return View(lstNhapKho);
        }
        [HttpGet]
        public ActionResult ViewThemMoi()
        {
            User _objectUser = (User)Session[CommonConstants.USER_SESSION];
            ViewBag.UserName = _objectUser.HoTen;
            NhapKho objNhapKho = new NhapKho();

           
            objNhapKho.SoPhieuNhap = "PN" + dataProvider.Entities.NhapKhoes.Max(t => t.Id);

            List<NhaCungCap> lstNhaCungCap = dataProvider.Entities.NhaCungCaps.ToList();
            ViewBag.lstNhaCungCap = new SelectList(lstNhaCungCap, "Id", "HoTen");
            //
            List<SanPham> lstSanPham = dataProvider.Entities.SanPhams.ToList();
            ViewBag.lstSanPham = new SelectList(lstSanPham, "Id", "TenSp");
            List<ChietKhau> lstChietKhau = dataProvider.Entities.ChietKhaus.ToList();
            ViewBag.lstChietKhau = new SelectList(lstChietKhau, "Id", "TenCK");

            List<NhapKhoCt> lstNhapKhoCt = new List<NhapKhoCt>();
            NhapKhoCt objNhapKhoCT = new NhapKhoCt();

            lstNhapKhoCt.Add(objNhapKhoCT);

            ViewBag.NhapKhoCT = lstNhapKhoCt;
            return PartialView("_ThemMoi", objNhapKho);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult SaveThemMoi(NhapKho objNhapKho)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_ThemMoi", objNhapKho);
            }
            else
            {

                objNhapKho.Date0 = DateTime.Now.Date;
                objNhapKho.Time0 = DateTime.Now.TimeOfDay;
                objNhapKho.Date2 = DateTime.Now.Date;
                objNhapKho.Time2 = DateTime.Now.TimeOfDay;
                User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                
                if (_objectUser != null)
                {
                    
                    objNhapKho.UserId0 = _objectUser.Id;
                    objNhapKho.UserId2 = _objectUser.Id;
                    objNhapKho.NguoiNhapId = _objectUser.Id;
                }
                
                dataProvider.Entities.NhapKhoes.Add(objNhapKho);
                //dataProvider.Entities.SaveChanges();

                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Id = objNhapKho.Id });
            }
        }
        [HttpPost]
        public ActionResult SaveChiTiet(string getepassdata)
        {
            List<NhapKhoCt> serializeData = JsonConvert.DeserializeObject<List<NhapKhoCt>>(getepassdata);
            long _PhieuNhapId = 0;
            bool _isSuccess = true;
            decimal SoLuong = 0, GiaBan = 0, ChietKhau = 0, tong_tt = 0, tong_ck = 0;
            try
            {
                foreach (NhapKhoCt item in serializeData)
                {
                    if (item.SanPhamId.Equals(null))
                    {                        
                        continue;
                    }
                    _PhieuNhapId = item.PhieuNhapId;

                    dataProvider.Entities.NhapKhoCts.Add(item);
                    SoLuong = item.SoLuong.GetValueOrDefault(0);
                    GiaBan = item.GiaBan.GetValueOrDefault(0);
                    if (!item.ChietKhauId.Equals(null))
                    {
                        ChietKhau objChietKhau = dataProvider.Entities.ChietKhaus.Find(item.ChietKhauId);
                        ChietKhau = objChietKhau.PtCk.GetValueOrDefault(0);
                    }

                    tong_tt += (SoLuong * GiaBan) - (SoLuong * GiaBan) * ChietKhau / 100;
                    tong_ck += (SoLuong * GiaBan) * ChietKhau / 100;
                    dataProvider.Entities.NhapKhoCts.Add(item);
                }

                NhapKho objNhapKho = dataProvider.Entities.NhapKhoes.Find(_PhieuNhapId);
                objNhapKho.TongSL = serializeData.Sum(t => t.SoLuong);
                objNhapKho.TongTien = serializeData.Sum(t => t.SoLuong * t.GiaBan); ;
                objNhapKho.TongCk = tong_ck;
                objNhapKho.TongTt = tong_tt - objNhapKho.PhiVc;
                dataProvider.Entities.NhapKhoes.Add(objNhapKho);
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
            NhapKho entity = dataProvider.Entities.NhapKhoes.Find(id);

            if (entity != null)
            {
                dataProvider.Entities.NhapKhoCts.RemoveRange(dataProvider.Entities.NhapKhoCts.Where(t => t.PhieuNhapId == id));
                dataProvider.Entities.NhapKhoes.Remove(entity);
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
                NhapKho objNhapKho = dataProvider.Entities.NhapKhoes.Find(id);
                List<SanPham> lstSanPham = dataProvider.Entities.SanPhams.ToList();
                ViewBag.lstSanPham = lstSanPham;
                List<ChietKhau> lstChietKhau = dataProvider.Entities.ChietKhaus.ToList();
                ViewBag.lstChietKhau = lstChietKhau;

                List<NhaCungCap> lstNhaCungCap = dataProvider.Entities.NhaCungCaps.ToList();
                ViewBag.lstNhaCungCap = new SelectList(lstNhaCungCap, "Id", "HoTen");

                return View(objNhapKho);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSua(NhapKho objNhapKho)
        {
            
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return PartialView("_Sua", objNhapKho);
            }
            else
            {
                NhapKho objNhapKhoSua = dataProvider.Entities.NhapKhoes.Find(objNhapKho.Id);
                objNhapKho.Date0 = DateTime.Now.Date;
                objNhapKho.Time0 = DateTime.Now.TimeOfDay;
                objNhapKho.Date2 = DateTime.Now.Date;
                objNhapKho.Time2 = DateTime.Now.TimeOfDay;
                User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                if (_objectUser != null)
                {
                    objNhapKho.UserId2 = _objectUser.Id;
                    objNhapKho.UserId0 = _objectUser.Id;
                    
                    objNhapKho.NguoiNhapId = _objectUser.Id;
                }
                dataProvider.Entities.Entry(objNhapKhoSua).CurrentValues.SetValues(objNhapKho);
                
                return Json(new { Id = objNhapKho.Id });
            }
        }
        //save sua chi chiet
        
       
        public ActionResult SaveSuaChiTiet(string getepassdata,int Id)
        {
                    
            List<NhapKhoCt> serializeData = JsonConvert.DeserializeObject<List<NhapKhoCt>>(getepassdata);
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
                    item.PhieuNhapId = Id;
                    _PhieuNhapId = item.PhieuNhapId;
                    SoLuong = item.SoLuong.GetValueOrDefault(0);
                    GiaBan = item.GiaBan.GetValueOrDefault(0);
                    if (item.ChietKhauId!=0)
                    {
                        ChietKhau objChietKhau = dataProvider.Entities.ChietKhaus.Find(item.ChietKhauId);
                        ChietKhau = objChietKhau.PtCk.GetValueOrDefault(0);
                    }
                    else
                    {
                        item.ChietKhauId = null;
                        ChietKhau = 0;
                    }
                    var entity = dataProvider.Entities.NhapKhoCts.Find(Id, item.SanPhamId);
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
        
       

        [HttpGet]
        public ActionResult LayTong(string getepassdata)
        {
            List<itemNhapKho> serializeData = JsonConvert.DeserializeObject<List<itemNhapKho>>(getepassdata);

            List<ChietKhau> lstCk = dataProvider.Entities.ChietKhaus.ToList();

            foreach (itemNhapKho item in serializeData)
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
    }
}