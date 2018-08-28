using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Serialization;
using WebAPI.Models;
namespace WebAPI.Controllers
{
    public class DispecerController : ApiController
    {
        [MyAuthorization(Roles = "Administrator")]
        [HttpGet]
        [Route("api/Dispecer/VratiImeIPrezime/")]
        public string VratiImeIPrezime(string korIme)
        {
            if (Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme) != null)
                return Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Ime + "-" + Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Prezime;
            else if (Korisnici.ListaVozaca.FirstOrDefault(m => m.KorisnickoIme == korIme) != null)
                return Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == korIme).Ime + "-" + Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == korIme).Prezime;
            else
                return "";
        }
    }
}