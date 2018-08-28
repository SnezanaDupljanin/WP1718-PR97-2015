using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace WebAPI.Models
{
    public class MyAuthorization : System.Web.Http.AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (HttpContext.Current.Session["mojaSesija"] == null)
            {
                return false;
            }
            Korisnik k = (Korisnik)HttpContext.Current.Session["mojaSesija"];
            string uloga = "";
            switch (k.Uloga)
            {
                case Uloge.Musterija:
                    uloga = "Musterija";
                    Musterija m = (Musterija)HttpContext.Current.Session["mojaSesija"];
                    if (m.Banovan)
                        return false;
                    break;
                case Uloge.Dispecer:
                    uloga = "Administrator";
                    break;
                case Uloge.Vozac:
                    uloga = "Vozac";
                    Vozac v = (Vozac)HttpContext.Current.Session["mojaSesija"];
                    if (v.Banovan)
                        return false;
                    break;
                default:
                    break;
            }

            if (uloga != Roles)
                return false;

            return true;
        }
    }
}