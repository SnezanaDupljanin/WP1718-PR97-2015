using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public abstract class Korisnik
    {
        public String KorisnickoIme { get; set; }
        public String Lozinka { get; set; }
        public String Ime { get; set; }
        public String Prezime { get; set; }
        public Polovi Pol { get; set; }
        public String Jmbg { get; set; }
        public String KontaktTelefon { get; set; }
        public String Email { get; set; }
        public Uloge Uloga { get; set; }
        public List<Voznja> Voznje { get; set; }
    }
}