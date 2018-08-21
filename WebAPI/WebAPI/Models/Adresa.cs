using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Adresa
    {
        public Adresa()
        {
            Ulica = "";
            Broj = "";
            NaseljenoMjesto = "";
            PozivniBrojMjesta = "";
        }
        public String Ulica { get; set; }
        public String Broj { get; set; }
        public String NaseljenoMjesto { get; set; }
        public String PozivniBrojMjesta { get; set; }
    }
}