using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SfStoreOnline.Models
{
    public class SumVoucher
    {
        public decimal TongSL { get; set; }
        public decimal TongTien { get; set; }
        public decimal TongCK { get; set; }
        public decimal Tongtt { get; set; }
    }
    public class itemNhapKho
    {
        public decimal SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal Tien { get; set; }
        public int ChietKhauId { get; set; }
        public decimal ptck { get; set; }
    }
    public class itemDonHangBan
    {
        public decimal SoLuong { get; set; }
        public decimal GiaBan { get; set; }
        public decimal Tien { get; set; }
        public int ChietKhauId { get; set; }
        public decimal ptck { get; set; }
    }
}