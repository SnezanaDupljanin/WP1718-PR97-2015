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

        [MyAuthorization(Roles = "Administrator")]
        [HttpGet]
        [Route("api/Dispecer/VratiSveVoznje")]
        public List<Voznja> VratiSveVoznje()
        {
            List<Voznja> ret = new List<Voznja>();
            foreach (var item in Korisnici.ListaMusterija)
            {
                ret.AddRange(item.Voznje);
            }
            foreach (var item in Korisnici.ListaDispecera)
            {
                foreach (var item2 in item.Voznje)
                {
                    if (!ret.Contains(item2))
                    {
                        ret.Add(item2);
                    }
                }
            }
            return ret;
        }

        [MyAuthorization(Roles = "Administrator")]
        [HttpGet]
        [Route("api/Dispecer/VratiSveKorisnike")]
        public List<string> VratiSveKorisnike()
        {
            List<string> ret = new List<string>();
            foreach (var item in Korisnici.ListaMusterija)
            {
                ret.Add(item.KorisnickoIme + "-musterija-" + item.Banovan.ToString());
            }
            foreach (var item in Korisnici.ListaVozaca)
            {
                ret.Add(item.KorisnickoIme + "-vozac-" + item.Banovan.ToString());
            }
            return ret;
        }

        [MyAuthorization(Roles = "Administrator")]
        [HttpGet]
        [Route("api/Dispecer/Blokiranje/")]
        public List<string> Blokiranje(string korIme)
        {
            if (Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme) != null)
            {
                Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Banovan = !Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Banovan;
                if (File.Exists(Korisnici.PutanjaMusterije))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Musterija>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaMusterije, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaMusterija);
                    }
                }
            }
            else if (Korisnici.ListaVozaca.FirstOrDefault(m => m.KorisnickoIme == korIme) != null)
            {
                Korisnici.ListaVozaca.FirstOrDefault(m => m.KorisnickoIme == korIme).Banovan = !Korisnici.ListaVozaca.FirstOrDefault(m => m.KorisnickoIme == korIme).Banovan;
                if (File.Exists(Korisnici.PutanjaVozaci))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                    }
                }
            }
            return VratiSveKorisnike();
        }
    }
}