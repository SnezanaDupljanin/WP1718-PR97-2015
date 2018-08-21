using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Musterija:Korisnik
    {
        public Musterija()
        {
            Voznje = new List<Voznja>();
            Banovan = false;
        }
        public bool Banovan { get; set; }
    }
}