using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Voznja
    {
        private static int brojac = 0;
        public Voznja()
        {
            Lokacija = new Lokacija();
            Odrediste = new Lokacija();
            Komentar = new Komentar();
            Id = brojac;
            brojac++;
        }

        public int Id { get; set; }//ne prikazujemo
        public DateTime DatumIVrijemePorudzbe { get; set; }
        public Lokacija Lokacija { get; set; }
        public TipoviAutomobila TipAutomobila { get; set; }
        public String Musterija { get; set; }
        public String Dispecer { get; set; }
        public String Vozac { get; set; }
        public Lokacija Odrediste { get; set; }
        public Double Iznos { get; set; }
        public Komentar Komentar { get; set; }
        public StatusiVoznje StatusVoznje { get; set; }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType() || obj == null)
            {
                return false;
            }
            return Id == ((Voznja)obj).Id;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}