using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Komentar
    {
        public String Opis { get; set; }
        public DateTime DatumObjave { get; set; }
        public String Korisnik { get; set; }
        public String Voznja { get; set; }
        public Ocjene Ocjena { get; set; }
    }
}