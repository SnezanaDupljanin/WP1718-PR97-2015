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
    public class RegistracijaController : ApiController
    {
        // POST: api/Registracija
        public HttpResponseMessage Post([FromBody]Musterija musterija)
        {
            string ime = musterija.Ime;
            HttpResponseMessage mess = new HttpResponseMessage();
            //Musterija m = musterija;
            if (Korisnici.ListaDispecera.FirstOrDefault(dispecer => dispecer.KorisnickoIme == musterija.KorisnickoIme) == null && Korisnici.ListaVozaca.FirstOrDefault(dispecer => dispecer.KorisnickoIme == musterija.KorisnickoIme) == null && Korisnici.ListaMusterija.FirstOrDefault(dispecer => dispecer.KorisnickoIme == musterija.KorisnickoIme) == null)
            {
                //ne postoji korisnicko ime do sad
                musterija.Uloga = Uloge.Musterija;
                musterija.Voznje = new List<Voznja>();
                Korisnici.ListaMusterija.Add(musterija);
                mess.StatusCode = HttpStatusCode.OK;
                if (File.Exists(Korisnici.PutanjaMusterije))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Musterija>));
                    using (StreamWriter writer = new StreamWriter(Korisnici.PutanjaMusterije, false))
                    {
                        xmlSerializer.Serialize(writer, Korisnici.ListaMusterija);
                    }
                }
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
