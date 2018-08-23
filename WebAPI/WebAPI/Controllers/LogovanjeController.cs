using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            return null;
        }

        // GET: api/Logovanje/5
        [HttpGet]
        [Route("api/Logovanje/ZapamtiMe/{value}")]
        public HttpResponseMessage ZapamtiMe(string value)
        {

            return null;
        }

        // POST: api/Logovanje

        public string Post([FromBody]UlogujSe value)
        {

            return null;
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