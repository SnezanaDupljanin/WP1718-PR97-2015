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
        [MyAuthorization(Roles = "Administrator")]
        // GET: api/Dispecer/5
        public Dispecer Get()
        {
            Dispecer k = (Dispecer)System.Web.HttpContext.Current.Session["mojaSesija"];

            return k;
        }
        [MyAuthorization(Roles = "Administrator")]
        public HttpResponseMessage Get(string x, string y, string tip, string ulica, string broj, string posta, string grad, string korIme, string vozac)
        {
            HttpResponseMessage ret = new HttpResponseMessage();
            ret.StatusCode = HttpStatusCode.OK;
            string xxx = ulica;
            string usernameVozaca = vozac;
            Adresa adresa = new Adresa() { Ulica = ulica, Broj = broj, NaseljenoMjesto = grad, PozivniBrojMjesta = posta };
            Lokacija lokacija = new Lokacija() { Adresa = adresa, KoordinataX = x, KoordinataY = y };
            Voznja voznja = new Voznja() { DatumIVrijemePorudzbe = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), Musterija = "nepoznato", Vozac = vozac, Dispecer = korIme, StatusVoznje = StatusiVoznje.Formirana, Lokacija = lokacija, Komentar = new Komentar() { Opis = "", Ocjena = Ocjene.Neocijenjeno }, TipAutomobila = (TipoviAutomobila)(int.Parse(tip)) };
            Dispecer disp = Korisnici.ListaDispecera.FirstOrDefault(m => m.KorisnickoIme == korIme);
            
            Korisnici.ListaDispecera.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje.Add(voznja);
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == vozac).Voznje.Add(voznja);
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == vozac).Zauzet = true;
            if (File.Exists(Korisnici.PutanjaVozaci))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                }
            }
            if (File.Exists(Korisnici.PutanjaDispeceri))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Dispecer>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaDispeceri, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaDispecera);
                }
            }
            System.Web.HttpContext.Current.Session["mojaSesija"] = Korisnici.ListaDispecera.FirstOrDefault(m => m.KorisnickoIme == korIme);
            return ret;
        }

        [MyAuthorization(Roles = "Administrator")]
        // POST: api/Dispecer
        public HttpResponseMessage Post([FromBody]Dispecer dispecer)
        {
            string ime = dispecer.Ime;
            HttpResponseMessage mess = new HttpResponseMessage();
            //mora postojati ako je izmjena podataka
            if (Korisnici.ListaDispecera.FirstOrDefault(d => dispecer.KorisnickoIme == d.KorisnickoIme) != null)
            {
                int ind = Korisnici.ListaDispecera.IndexOf(Korisnici.ListaDispecera.FirstOrDefault(d => dispecer.KorisnickoIme == d.KorisnickoIme));
                //ne postoji korisnicko ime do sad
                dispecer.Uloga = Uloge.Dispecer;
                dispecer.Voznje = Korisnici.ListaDispecera[ind].Voznje;
                Korisnici.ListaDispecera[ind] = dispecer;
                if (File.Exists(Korisnici.PutanjaDispeceri))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Dispecer>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaDispeceri, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaDispecera);
                    }
                }
                System.Web.HttpContext.Current.Session["mojaSesija"] = dispecer;
                mess.StatusCode = HttpStatusCode.OK;
                return mess;
            }
            else
            {
                // postoji korisnicko ime vec
                mess.StatusCode = HttpStatusCode.NotAcceptable;
                return mess;
            }
        }
    }
}