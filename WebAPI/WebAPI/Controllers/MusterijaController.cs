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
    public class MusterijaController : ApiController
    {
        [MyAuthorization(Roles = "Musterija")]
        [HttpGet]
        [Route("api/Musterija/VratiVoznju/")]
        public Voznja VratiVoznju(string id, string korIme)
        {
            string idMusterija = id.Substring(11);
            int i = int.Parse(idMusterija);
            Musterija must = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme);
            //int indVoznja = must.Voznje.IndexOf(must.Voznje.FirstOrDefault(v => v.Id == i));
            return Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje.FirstOrDefault(v => v.Id == i);
        }

        [MyAuthorization(Roles = "Musterija")]
        public List<Voznja> Get(string korIme)
        {
            return Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje;
        }
        [MyAuthorization(Roles = "Musterija")]
        // GET: api/Musterija/5
        public Musterija Get()
        {
            Musterija k = (Musterija)System.Web.HttpContext.Current.Session["mojaSesija"];

            return k;
        }
        [MyAuthorization(Roles = "Musterija")]
        [HttpPost]
        [Route("api/Musterija/GetLokacija/")]
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
        [MyAuthorization(Roles = "Musterija")]
        [HttpGet]
        [Route("api/Musterija/IzmijeniVoznju/")]
        public void Get(string x, string y, string tip, string ulica, string broj, string posta, string grad, string korIme, string id)
        {
            int i = int.Parse(id);
            string xxx = ulica;

            Adresa adresa = new Adresa() { Ulica = ulica, Broj = broj, NaseljenoMjesto = grad, PozivniBrojMjesta = posta };
            Lokacija lokacija = new Lokacija() { Adresa = adresa, KoordinataX = x, KoordinataY = y };

            Voznja voz = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje.FirstOrDefault(v => v.Id == i);
            int indexVoznje = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje.IndexOf(voz);
            Komentar kom = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje[indexVoznje].Komentar;

            Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje[indexVoznje].Lokacija = lokacija;
            Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme).Voznje[indexVoznje].TipAutomobila = (TipoviAutomobila)(int.Parse(tip));

            if (File.Exists(Korisnici.PutanjaMusterije))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Musterija>));
                using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaMusterije, false))
                {
                    xmlSerializer.Serialize(writer, Korisnici.ListaMusterija);
                }
            }
            System.Web.HttpContext.Current.Session["mojaSesija"] = Korisnici.ListaMusterija.FirstOrDefault(m => m.KorisnickoIme == korIme);
        }

        [MyAuthorization(Roles = "Musterija")]
        // POST: api/Musterija
        public HttpResponseMessage Post([FromBody]Musterija musterija)
        {
            string ime = musterija.Ime;
            HttpResponseMessage mess = new HttpResponseMessage();
            //Musterija m = musterija;
            if (Korisnici.ListaMusterija.FirstOrDefault(d => musterija.KorisnickoIme == d.KorisnickoIme) != null)
            {
                int ind = Korisnici.ListaMusterija.IndexOf(Korisnici.ListaMusterija.FirstOrDefault(d => musterija.KorisnickoIme == d.KorisnickoIme));
                //ne postoji korisnicko ime do sad
                musterija.Uloga = Uloge.Musterija;
                musterija.Voznje = Korisnici.ListaMusterija[ind].Voznje;
                musterija.Banovan = Korisnici.ListaMusterija[ind].Banovan;
                Korisnici.ListaMusterija[ind] = musterija;
                if (File.Exists(Korisnici.PutanjaMusterije))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Musterija>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaMusterije, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaMusterija);
                    }
                }
                System.Web.HttpContext.Current.Session["mojaSesija"] = musterija;
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
