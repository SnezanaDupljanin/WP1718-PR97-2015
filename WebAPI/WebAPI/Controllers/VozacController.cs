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
        // GET: api/Vozac/5
        //[MyAuthorization(Roles = "Vozac")]
        public Vozac Get()
        {
            Vozac k = (Vozac)System.Web.HttpContext.Current.Session["mojaSesija"];

            return k;
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
