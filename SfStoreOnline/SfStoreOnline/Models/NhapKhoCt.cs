//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SfStoreOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NhapKhoCt
    {
        public long PhieuNhapId { get; set; }
        public long SanPhamId { get; set; }
        public Nullable<decimal> SoLuong { get; set; }
        public Nullable<decimal> GiaBan { get; set; }
        public Nullable<long> ChietKhauId { get; set; }
        public Nullable<decimal> Tien { get; set; }
    
        public virtual NhapKho NhapKho { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual ChietKhau ChietKhau { get; set; }
    }
}
