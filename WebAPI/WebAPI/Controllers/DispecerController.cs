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
        [HttpGet]
        [Route("api/Dispecer/VratiSlobodneVozace")]
        public List<Vozac> VratiSlobodneVozace()
        {
            var ret = Korisnici.ListaVozaca.Where(v => !v.Zauzet).ToList();

            return ret;
        }

        [MyAuthorization(Roles = "Administrator")]
        [HttpGet]
        [Route("api/Dispecer/VratiSlobodneVozaceNajblize/")]
        public List<Vozac> VratiSlobodneVozaceNajblize(string idVoznje, string idMusterije)
        {
            string id2 = idVoznje;
            id2 = idVoznje.Substring(9);

            string x = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == idMusterije).Voznje.FirstOrDefault(v => v.Id == int.Parse(id2)).Lokacija.KoordinataX;
            string y = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == idMusterije).Voznje.FirstOrDefault(v => v.Id == int.Parse(id2)).Lokacija.KoordinataY;

            var ret = Korisnici.ListaVozaca.Where(v => !v.Zauzet).ToList();
            ret = Sortiraj(ret, x, y);

            var listaSlobodnihNajblizih = ret.Where(v => !v.Zauzet).ToList();

            if (listaSlobodnihNajblizih.Count <= 5)
            {
                return listaSlobodnihNajblizih;
            }
            else
            {
                return listaSlobodnihNajblizih.ToList().GetRange(0, 5);
            }
        }
        public List<Vozac> Sortiraj(List<Vozac> zaSortiranje, string x, string y)
        {
            var ret = new List<Vozac>();
            zaSortiranje.Sort(
                   delegate (Vozac b1, Vozac b2)
                   {
                       return ApsolutnoRastojanje(b1.Lokacija.KoordinataX, b1.Lokacija.KoordinataY, x, y).CompareTo(ApsolutnoRastojanje(b2.Lokacija.KoordinataX, b2.Lokacija.KoordinataY, x, y));
                   }
            );
            return zaSortiranje;
        }
        public double ApsolutnoRastojanje(string x1, string y1, string x2, string y2)
        {
            double kX1 = double.Parse(x1.Replace('.', ','));
            double kX2 = double.Parse(x2.Replace('.', ','));
            double kY1 = double.Parse(y1.Replace('.', ','));
            double kY2 = double.Parse(y2.Replace('.', ','));

            double apsRastojanje = Math.Sqrt(Math.Pow((kX1 - kX2), 2) + Math.Pow((kY1 - kY2), 2));

            return apsRastojanje;
        }

        [MyAuthorization(Roles = "Administrator")]
        [HttpGet]
        [Route("api/Dispecer/ObradiVoznju/")]
        public void ObradiVoznju(string id, string korImeDisp, string korImeMusterije, string vozac)
        {
            string id2 = id;
            id2 = id.Substring(9);
            Voznja voznja = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korImeMusterije).Voznje.FirstOrDefault(v => v.Id == int.Parse(id2));
            int indVoz = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korImeMusterije).Voznje.IndexOf(voznja);
            Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korImeMusterije).Voznje[indVoz].StatusVoznje = StatusiVoznje.Obradjena;
            Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korImeMusterije).Voznje[indVoz].Dispecer = korImeDisp;
            Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korImeMusterije).Voznje[indVoz].Vozac = vozac;
            voznja.StatusVoznje = StatusiVoznje.Obradjena;
            voznja.Dispecer = korImeDisp;
            voznja.Vozac = vozac;
            Korisnici.ListaDispecera.FirstOrDefault(d => d.KorisnickoIme == korImeDisp).Voznje.Add(voznja);
            Korisnici.ListaVozaca.FirstOrDefault(d => d.KorisnickoIme == vozac).Voznje.Add(voznja);
            Korisnici.ListaVozaca.FirstOrDefault(d => d.KorisnickoIme == vozac).Zauzet = true;

            if (File.Exists(Korisnici.PutanjaMusterije))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Musterija>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaMusterije, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaMusterija);
                }
            }
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