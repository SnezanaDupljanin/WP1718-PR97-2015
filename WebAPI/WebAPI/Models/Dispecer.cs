using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class Dispecer:Korisnik
    {
        public Dispecer()
        {
            Voznje = new List<Voznja>();
        }
    }
}