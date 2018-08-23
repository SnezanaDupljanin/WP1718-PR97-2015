using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class UlogujSe
    {
        public string username { get; set; }
        public string password { get; set; }
        public string rememberMe { get; set; }
        public UlogujSe()
        {

        }
    }
}