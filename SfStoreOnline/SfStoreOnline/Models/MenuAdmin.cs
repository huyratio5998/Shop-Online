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
    
    public partial class MenuAdmin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuAdmin()
        {
            this.PhanQuyens = new HashSet<PhanQuyen>();
        }
    
        public Nullable<int> Cap { get; set; }
        public string MenuId { get; set; }
        public string MenuIdCha { get; set; }
        public string TenMenu { get; set; }
        public string AswIcon { get; set; }
        public bool AnMenu { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public Nullable<int> Stt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; }
    }
}
