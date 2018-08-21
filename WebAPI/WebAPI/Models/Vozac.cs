using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Vozac:Korisnik
    {
        public Vozac()
        {
            Lokacija = new Lokacija();
            Automobil = new Automobil();
            Voznje = new List<Voznja>();
            Zauzet = false;
            Banovan = false;
        }
        public Lokacija Lokacija { get; set; }
        public Automobil Automobil { get; set; }
        public bool Zauzet { get; set; }
        public bool Banovan { get; set; }
    }
}