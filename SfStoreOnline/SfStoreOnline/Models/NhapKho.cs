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
    
    public partial class NhapKho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhapKho()
        {
            this.NhapKhoCts = new HashSet<NhapKhoCt>();
        }
    
        public long Id { get; set; }
        public string SoPhieuNhap { get; set; }
        public Nullable<long> NhaCungCapId { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
        public Nullable<long> NguoiNhapId { get; set; }
        public string GhiChu { get; set; }
        public Nullable<decimal> TongSL { get; set; }
        public Nullable<decimal> TongTien { get; set; }
        public Nullable<decimal> TongCk { get; set; }
        public Nullable<decimal> PhiVc { get; set; }
        public Nullable<decimal> TongTt { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> Date0 { get; set; }
        public Nullable<System.TimeSpan> Time0 { get; set; }
        public Nullable<long> UserId0 { get; set; }
        public Nullable<System.DateTime> Date2 { get; set; }
        public Nullable<System.TimeSpan> Time2 { get; set; }
        public Nullable<long> UserId2 { get; set; }
        public bool TraLai_Yn { get; set; }
    
        public virtual NhaCungCap NhaCungCap { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhapKhoCt> NhapKhoCts { get; set; }
    }
}
