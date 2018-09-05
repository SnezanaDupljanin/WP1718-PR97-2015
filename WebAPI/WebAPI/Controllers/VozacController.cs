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
    public class VozacController : ApiController
    {
        //api/Dispecer/VratiSortiraneVoznje
        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/VratiSortiraneVoznje/")]
        public List<Voznja> VratiSortiraneVoznje(List<Voznja> sveVoznje)
        {
            var ret = Sortiraj(sveVoznje, Get().Lokacija.KoordinataX, Get().Lokacija.KoordinataY);
            return ret;
        }
        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/VratiAutomobil/")]
        public TipoviAutomobila VratiAutomobil(string korIme)
        {
            return Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == korIme).Automobil.TipAutomobila;
        }
        public List<Voznja> Sortiraj(List<Voznja> zaSortiranje, string x, string y)
        {
            var ret = new List<Vozac>();
            zaSortiranje.Sort(
                   delegate (Voznja b1, Voznja b2)
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
        [MyAuthorization(Roles = "Vozac")]
        [HttpPost]
        [Route("api/Vozac/GetLokacija/")]
        public Lokacija GetLokacija([FromBody]JObject jsonResult)
        {
            if (jsonResult != null)
            {
                string korisnicko = "";
                string s = jsonResult.ToString();
                IList<JToken> addresses = jsonResult["jsonResult"]["address"].Children().ToList();
                //string b="";
                string grad = "";
                string ulica = "";
                string posta = "";
                string broj = "";
                foreach (var item in addresses)
                {
                    string ssss = item.ToString().Replace("\"", "");
                    if (ssss.Split(':')[0] == "city")
                    {
                        grad = ssss.Split(':')[1].Trim();
                    }
                    else if (ssss.Split(':')[0] == "road")
                    {
                        ulica = ssss.Split(':')[1].Trim();
                    }
                    else if (ssss.Split(':')[0] == "postcode")
                    {
                        posta = ssss.Split(':')[1].Trim();
                    }
                    else if (ssss.Split(':')[0] == "house_number")//ako nema broj moram stavit bb
                    {
                        broj = ssss.Split(':')[1].Trim();
                    }
                }
                IList<JToken> koordinate = jsonResult["jsonResult"]["boundingbox"].Children().ToList();

                string x = koordinate[3].ToString().Trim(new char[] { '{', '}' });
                string y = koordinate[0].ToString().Trim(new char[] { '{', '}' });
                if (broj.Trim() == "")
                {
                    broj = "bb";
                }
                Lokacija lok = new Lokacija() { KoordinataX = x, KoordinataY = y, Adresa = new Adresa() { NaseljenoMjesto = grad, Ulica = ulica, PozivniBrojMjesta = posta, Broj = broj } };
                korisnicko = Get().KorisnickoIme;
                return lok;
            }
            return new Lokacija();
        }

        [MyAuthorization(Roles = "Vozac")]
        [HttpPost]
        [Route("api/Vozac/IzmijeniLokaciju/")]
        public void IzmijeniLokaciju([FromBody]JObject jsonResult)
        {
            if (jsonResult != null)
            {
                //isparsiraj i username jos
                string korisnicko = "";
                string s = jsonResult.ToString();
                IList<JToken> addresses = jsonResult["jsonResult"]["address"].Children().ToList();
                //string b="";
                string grad = "";
                string ulica = "";
                string posta = "";
                string broj = "";
                foreach (var item in addresses)
                {
                    string ssss = item.ToString().Replace("\"", "");
                    if (ssss.Split(':')[0] == "city")
                    {
                        grad = ssss.Split(':')[1].Trim();
                    }
                    else if (ssss.Split(':')[0] == "road")
                    {
                        ulica = ssss.Split(':')[1].Trim();
                    }
                    else if (ssss.Split(':')[0] == "postcode")
                    {
                        posta = ssss.Split(':')[1].Trim();
                    }
                    else if (ssss.Split(':')[0] == "house_number")//ako nema broj moram stavit bb
                    {
                        broj = ssss.Split(':')[1].Trim();
                    }
                }
                IList<JToken> koordinate = jsonResult["jsonResult"]["boundingbox"].Children().ToList();
                //TripObject item = JsonConvert.DeserializeObject<TripObject>(jsonResult.ToString());
                //return jsonResult;
                string x = koordinate[0].ToString().Trim(new char[] { '{', '}' });
                string y = koordinate[3].ToString().Trim(new char[] { '{', '}' });
                if (broj.Trim() == "")
                {
                    broj = "bb";
                }
                Lokacija lok = new Lokacija() { KoordinataX = x, KoordinataY = y, Adresa = new Adresa() { NaseljenoMjesto = grad, Ulica = ulica, PozivniBrojMjesta = posta, Broj = broj } };
                korisnicko = Get().KorisnickoIme;
                Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == korisnicko).Lokacija = lok;
                if (File.Exists(Korisnici.PutanjaVozaci))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                    }
                }
            }
        }

        //"/api/Vozac/PromijeniLokaciju/"        
        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/PromijeniLokaciju/")]
        public void PromijeniLokaciju(string idVozaca, string broj, string ulica, string grad, string x, string y, string posta)
        {
            //string b=broj;
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Lokacija = new Lokacija() { KoordinataX = x, KoordinataY = y, Adresa = new Adresa() { Broj = broj, NaseljenoMjesto = grad, PozivniBrojMjesta = posta, Ulica = ulica } };
            if (File.Exists(Korisnici.PutanjaVozaci))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                }
            }
            System.Web.HttpContext.Current.Session["mojaSesija"] = Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca);
        }

        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/VratiKreirane")]
        public List<Voznja> VratiKreirane()
        {
            List<Voznja> ret = new List<Voznja>();
            foreach (var item in Korisnici.ListaMusterija)
            {
                ret.AddRange(item.Voznje.Where(d => d.StatusVoznje == StatusiVoznje.Kreirana_NaCekanju));
            }

            return ret;
        }

        // GET: api/Vozac/5
        //[MyAuthorization(Roles = "Vozac")]
        public Vozac Get()
        {
            Vozac k = (Vozac)System.Web.HttpContext.Current.Session["mojaSesija"];

            return k;
        }

        ///api/Vozac/PrihvatiVoznju
        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/PrihvatiVoznju/")]
        public void PrihvatiVoznju(string idVoznje, string idVozaca)
        {
            //od 11 substring
            string id = idVoznje.Substring(11);
            Voznja v = null;
            foreach (var item in Korisnici.ListaMusterija)
            {
                foreach (var item2 in item.Voznje)
                {
                    if (item2.Id == int.Parse(id))
                    {
                        item2.Vozac = idVozaca;
                        item2.StatusVoznje = StatusiVoznje.Prihvacena;
                        v = item2;
                    }
                }
            }
            Korisnici.ListaVozaca.FirstOrDefault(vo => vo.KorisnickoIme == idVozaca).Voznje.Add(v);
            if (File.Exists(Korisnici.PutanjaVozaci))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                }
            }
        }

        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/ObradiVoznju/")]
        public void ObradiVoznju(string idVoznje, string idVozaca)
        {
            //od 11 substring
            string id = idVoznje.Substring(11);
            Voznja v = null;
            foreach (var item in Korisnici.ListaMusterija)
            {
                foreach (var item2 in item.Voznje)
                {
                    if (item2.Id == int.Parse(id))
                    {
                        item2.Vozac = idVozaca;
                        item2.StatusVoznje = StatusiVoznje.Prihvacena;
                        v = item2;
                    }
                }
            }
            Korisnici.ListaVozaca.FirstOrDefault(vo => vo.KorisnickoIme == idVozaca).Voznje.Add(v);
            if (File.Exists(Korisnici.PutanjaVozaci))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                }
            }
        }

        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/NeuspjesnaVoznja/")]
        public void NeuspjesnaVoznja(string idVoznje, string idVozaca, string koment)
        {
            string j = koment;
            string id = idVoznje.Substring(9);
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).StatusVoznje = StatusiVoznje.Neuspjesna;
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Opis = koment;
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Zauzet = false;
            if (File.Exists(Korisnici.PutanjaVozaci))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                }
            }
            if (Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija != null && Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija != "" && Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija != "nepoznato")
            {
                string korImeMust = Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).StatusVoznje = StatusiVoznje.Neuspjesna;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Opis = koment;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Voznja = id;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.DatumObjave = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Korisnik = idVozaca;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Ocjena = Ocjene.Neocijenjeno;
                if (File.Exists(Korisnici.PutanjaMusterije))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Musterija>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaMusterije, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaMusterija);
                    }
                }
            }
            if (Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Dispecer != null && Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Dispecer != "")
            {
                string korImeDisp = Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Dispecer;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).StatusVoznje = StatusiVoznje.Neuspjesna;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Opis = koment;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Voznja = id;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.DatumObjave = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Korisnik = idVozaca;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Ocjena = Ocjene.Neocijenjeno;
                if (File.Exists(Korisnici.PutanjaDispeceri))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Dispecer>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaDispeceri, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaDispecera);
                    }
                }
            }
            System.Web.HttpContext.Current.Session["mojaSesija"] = Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca);
            //Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Opis=koment;
        }

        [MyAuthorization(Roles = "Vozac")]
        [HttpGet]
        [Route("api/Vozac/UspjesnaVoznja/")]
        public void UspjesnaVoznja(string idVoznje, string idVozaca, string iznos, string broj, string ulica, string grad, string x, string y, string posta)
        {
            string j = ulica;
            string id = idVoznje.Substring(9);
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).StatusVoznje = StatusiVoznje.Uspjesna;
            Lokacija odr = new Lokacija() { KoordinataX = x, KoordinataY = y, Adresa = new Adresa() { Ulica = ulica, Broj = broj, NaseljenoMjesto = grad, PozivniBrojMjesta = posta } };
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Odrediste = odr;
            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Iznos = double.Parse(iznos);

            Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Zauzet = false;
            if (Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija != null && Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija != "" && Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija != "nepoznato")
            {
                string korImeMust = Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Musterija;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).StatusVoznje = StatusiVoznje.Uspjesna;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Odrediste = odr;
                Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == korImeMust).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Iznos = double.Parse(iznos);
            }
            if (Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Dispecer != null && Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Dispecer != "")
            {
                string korImeDisp = Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Dispecer;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).StatusVoznje = StatusiVoznje.Uspjesna;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Odrediste = odr;
                Korisnici.ListaDispecera.FirstOrDefault(v => v.KorisnickoIme == korImeDisp).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Iznos = double.Parse(iznos);
            }
            System.Web.HttpContext.Current.Session["mojaSesija"] = Korisnici.ListaVozaca.FirstOrDefault(v => v.KorisnickoIme == idVozaca);
            //Korisnici.ListaMusterija.FirstOrDefault(v => v.KorisnickoIme == idVozaca).Voznje.FirstOrDefault(v => v.Id == int.Parse(id)).Komentar.Opis=koment;
        }      

        [MyAuthorization(Roles = "Vozac")]
        // POST: api/Vozac
        public HttpResponseMessage Post([FromBody]Vozac vozac)
        {
            string ime = vozac.Ime;
            HttpResponseMessage mess = new HttpResponseMessage();
            //Musterija m = musterija;
            if (Korisnici.ListaVozaca.FirstOrDefault(d => vozac.KorisnickoIme == d.KorisnickoIme) != null)
            {

                int ind = Korisnici.ListaVozaca.IndexOf(Korisnici.ListaVozaca.FirstOrDefault(d => vozac.KorisnickoIme == d.KorisnickoIme));
                Automobil a = Korisnici.ListaVozaca.FirstOrDefault(d => vozac.KorisnickoIme == d.KorisnickoIme).Automobil;
                //ne postoji korisnicko ime do sad
                vozac.Uloga = Uloge.Vozac;
                vozac.Automobil = a;
                vozac.Voznje = Korisnici.ListaVozaca.FirstOrDefault(d => vozac.KorisnickoIme == d.KorisnickoIme).Voznje;
                vozac.Lokacija = Korisnici.ListaVozaca.FirstOrDefault(d => vozac.KorisnickoIme == d.KorisnickoIme).Lokacija;
                vozac.Banovan = Korisnici.ListaVozaca.FirstOrDefault(d => vozac.KorisnickoIme == d.KorisnickoIme).Banovan;
                vozac.Zauzet = Korisnici.ListaVozaca.FirstOrDefault(d => vozac.KorisnickoIme == d.KorisnickoIme).Zauzet;
                Korisnici.ListaVozaca[ind] = vozac;
                if (File.Exists(Korisnici.PutanjaVozaci))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaVozaci, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaVozaca);
                    }
                }
                System.Web.HttpContext.Current.Session["mojaSesija"] = vozac;
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
