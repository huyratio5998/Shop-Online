using SfStoreOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SfStoreOnline.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Addcart(int Id, int sl)
        {
            if (Session[CommonConstants.CART_SESSION] == null)
            {
                List<GioHang> lstGioHang = new List<GioHang>();
                GioHang item = new GioHang();
                item.SanPhamId = Id;
                item.SanPham = dataProvider.Entities.SanPhams.FirstOrDefault(t => t.Id == Id);
                item.SoLuong = sl;
                item.Date0 = DateTime.Now;
                item.Date2 = DateTime.Now;
                SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];
                
                if (_objectUser != null)
                {
                    item.UserId = _objectUser.Id;
                    dataProvider.Entities.GioHangs.Add(item);
                    dataProvider.Entities.SaveChanges();
                }
               
                lstGioHang.Add(item);
                Session[CommonConstants.CART_SESSION] = lstGioHang;
                return PartialView("_Cart", lstGioHang);
            }
            else
            {
                List<GioHang> lstGioHang = (List<GioHang>)Session[CommonConstants.CART_SESSION];
                if (lstGioHang.Exists(x => x.SanPhamId == Id))//Nếu sản phẩm này đã có trong giỏ thì + thêm số lượng vào
                {

                    foreach (var item in lstGioHang)
                    {
                        if (item.SanPhamId == Id)
                        {
                            item.SoLuong += sl;
                            item.Date2 = DateTime.Now;
                        }
                    }
                }
                else
                {

                    //tạo mới đối tượng item giỏ hàng
                    GioHang item = new GioHang();
                    item.SanPhamId = Id;
                    item.SanPham = dataProvider.Entities.SanPhams.FirstOrDefault(t => t.Id == Id);
                    item.SoLuong = sl;
                    item.Date0 = DateTime.Now;
                    item.Date2 = DateTime.Now;
                    SfStoreOnline.Models.User _objectUser = (User)Session[CommonConstants.USER_SESSION];

                    if (_objectUser != null)
                    {
                        item.UserId = _objectUser.Id;
                        dataProvider.Entities.GioHangs.Add(item);
                        dataProvider.Entities.SaveChanges();
                    }
                    lstGioHang.Add(item);
                }
                //Gán vào session
                Session[CommonConstants.CART_SESSION] = lstGioHang;
                TempData["total"] = lstGioHang.Sum(t => t.SoLuong);
                
                return PartialView("_Cart", lstGioHang);

            }
        }
    }
}