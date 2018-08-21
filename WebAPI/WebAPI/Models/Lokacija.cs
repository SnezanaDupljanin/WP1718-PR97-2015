using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Lokacija
    {
        public Lokacija()
        {
            Adresa = new Adresa();
            KoordinataX = "";
            KoordinataY = "";
        }
        public String KoordinataX { get; set; }
        public String KoordinataY { get; set; }
        public Adresa Adresa { get; set; }
    }
}