using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet, Route("")]
        public RedirectResult Index()
        {
            var requestUri = Request.RequestUri;
            Korisnik kSesija = (Korisnik)System.Web.HttpContext.Current.Session["mojaSesija"];
            if (kSesija != null)
            {
                return Redirect(requestUri.AbsoluteUri + kSesija.Uloga.ToString() + ".html");
            }
            else
            {
                var cookie = Request.Headers.GetCookies("mojKuki").FirstOrDefault();
                if (cookie == null)
                {
                    return Redirect(requestUri.AbsoluteUri + "Login.html");
                }
                else//zavisi od toga koji je korisnik
                {
                    string imeKukija = cookie["mojKuki"].Value;//dobijamo username
                    Korisnik korisnik;
                    if ((Korisnici.ListaDispecera.FirstOrDefault(dispecer => dispecer.KorisnickoIme == imeKukija)) != null)
                    {
                        korisnik = Korisnici.ListaDispecera.FirstOrDefault(dispecer => dispecer.KorisnickoIme == imeKukija);

                        System.Web.HttpContext.Current.Session["mojaSesija"] = (Dispecer)korisnik;
                        return Redirect(requestUri.AbsoluteUri + "Dispecer.html");
                    }
                    else if ((Korisnici.ListaVozaca.FirstOrDefault(vozac => vozac.KorisnickoIme == imeKukija)) != null)
                    {
                        korisnik = Korisnici.ListaVozaca.FirstOrDefault(vozac => vozac.KorisnickoIme == imeKukija);
                        System.Web.HttpContext.Current.Session["mojaSesija"] = korisnik;
                        return Redirect(requestUri.AbsoluteUri + "Vozac.html");
                    }
                    else if ((Korisnici.ListaMusterija.FirstOrDefault(musterija => musterija.KorisnickoIme == imeKukija)) != null)
                    {
                        korisnik = Korisnici.ListaMusterija.FirstOrDefault(musterija => musterija.KorisnickoIme == imeKukija);
                        System.Web.HttpContext.Current.Session["mojaSesija"] = korisnik;
                        return Redirect(requestUri.AbsoluteUri + "Musterija.html");
                    }
                    else
                    {
                        //null je - ne postoji u listama, mozda je u medjuvremenu izbrisan
                        //return "Pogresno korisnicko ime ili lozinka";
                    }
                    System.Web.HttpContext.Current.Session["mojaSesija"] = null;
                    return Redirect(requestUri.AbsoluteUri + "Login.html");

                }
            }
        }
    }
}
