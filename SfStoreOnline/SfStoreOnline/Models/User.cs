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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.GioHangs = new HashSet<GioHang>();
            this.NhapKhoes = new HashSet<NhapKho>();
            this.TinTucs = new HashSet<TinTuc>();
        }
    
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public long NhomId { get; set; }
        public string HoTen { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public bool IsAdmin { get; set; }
        public Nullable<long> UserId0 { get; set; }
        public Nullable<System.DateTime> Date0 { get; set; }
        public Nullable<System.TimeSpan> Time0 { get; set; }
        public long UserId2 { get; set; }
        public Nullable<System.DateTime> Date2 { get; set; }
        public Nullable<System.TimeSpan> Time2 { get; set; }
        public string AvatarName { get; set; }
        public bool TruyCapQuanTri { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NhapKho> NhapKhoes { get; set; }
        public virtual NhomNguoiDung NhomNguoiDung { get; set; }
        public virtual TinTuc TinTuc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TinTuc> TinTucs { get; set; }
    }
}