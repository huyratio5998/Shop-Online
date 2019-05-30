using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SfStoreOnline.Models
{
    public class login
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [DisplayName("Tên đăng nhập")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DisplayName("Mật khẩu")]
        public string Password { get; set; }
        public string ReturnURL { get; set; }
        public bool isRemember { get; set; }
    }
}