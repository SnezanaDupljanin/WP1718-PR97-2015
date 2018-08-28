using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class LogovanjeController : ApiController
    {
        // GET: api/Logovanje
        [HttpGet]
        [Route("api/Logovanje/IzlogujSe")]
        public HttpResponseMessage IzlogujSe()
        {
            HttpResponseMessage mess = new HttpResponseMessage();

            if (HttpContext.Current.Response.Cookies["mojKuki"] != null)
            {
                HttpContext.Current.Response.Cookies["mojKuki"].Value = null;
                HttpContext.Current.Response.Cookies["mojKuki"].Expires = DateTime.Now.AddMonths(-1);
            }
            HttpContext.Current.Session["mojaSesija"] = null;
            HttpContext.Current.Session.Abandon();
            mess.StatusCode = HttpStatusCode.OK;
            return mess;
        }

        // GET: api/Logovanje/5
        [HttpGet]
        [Route("api/Logovanje/ZapamtiMe/{value}")]
        public HttpResponseMessage ZapamtiMe(string value)
        {            
            var resp = new HttpResponseMessage();
            var cookie = new CookieHeaderValue("mojKuki", value);

            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";

            resp.Headers.AddCookies(new CookieHeaderValue[] { cookie });
            //resp.StatusCode = HttpStatusCode.OK;
            return resp;
        }

        // POST: api/Logovanje

        public string Post([FromBody]UlogujSe value)
        {

            string username = value.username;
            string password = value.password;
            string rememberMe = value.rememberMe;


            Korisnik korisnik;
            if ((Korisnici.ListaDispecera.FirstOrDefault(dispecer => dispecer.KorisnickoIme == username && dispecer.Lozinka == password)) != null)
            {
                korisnik = Korisnici.ListaDispecera.FirstOrDefault(dispecer => dispecer.KorisnickoIme == username && dispecer.Lozinka == password);

                HttpContext.Current.Session["mojaSesija"] = korisnik;

                return korisnik.Uloga.ToString();
            }
            else if ((Korisnici.ListaVozaca.FirstOrDefault(vozac => vozac.KorisnickoIme == username && vozac.Lozinka == password)) != null)
            {
                korisnik = Korisnici.ListaVozaca.FirstOrDefault(vozac => vozac.KorisnickoIme == username && vozac.Lozinka == password);
                //Vozac d = (Vozac)HttpContext.Current.Session["mojaSesija"];//ne moze se na login vratiti nikako drugacije osim sa log out
                //if (d == null)
                {
                    HttpContext.Current.Session["mojaSesija"] = korisnik;
                }
                return korisnik.Uloga.ToString();
            }
            else if ((Korisnici.ListaMusterija.FirstOrDefault(musterija => musterija.KorisnickoIme == username && musterija.Lozinka == password)) != null)
            {
                korisnik = Korisnici.ListaMusterija.FirstOrDefault(musterija => musterija.KorisnickoIme == username && musterija.Lozinka == password);
                //Musterija d = (Musterija)HttpContext.Current.Session["mojaSesija"];
                //if (d == null)
                {
                    HttpContext.Current.Session["mojaSesija"] = korisnik;
                }
                return korisnik.Uloga.ToString();
            }
            else
            {
                return "Pogresno korisnicko ime ili lozinka";
            }
        }

        // PUT: api/Logovanje/5
        public void Put(int id, [FromBody]string value)
        {

            
        }

        // DELETE: api/Logovanje/5
        public void Delete(int id)
        {
        }
    }
}