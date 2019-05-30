using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SfStoreOnline.Models
{
    public class dataProvider
    {
        /// <summary>
        /// Khai báo 1 thuộc tính để truy xuất đến cơ sở dữ liệu
        /// </summary>
        private static SFOnlinestore_A0418Entities _Entities = new SFOnlinestore_A0418Entities();
        public static SFOnlinestore_A0418Entities Entities
        {
            get { return _Entities; }
            set { _Entities = value; }
        }
    }
}