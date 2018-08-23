using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WebAPI.Models
{
    public class Korisnici
    {
        public Korisnici(string putanja, string putanjaMusterija, string putanjaVozac)
        {
           
            PutanjaDispeceri = putanja;
            PutanjaMusterije = putanjaMusterija;
            PutanjaVozaci = putanjaVozac;
            ListaDispecera = new List<Dispecer>();
            ListaVozaca = new List<Vozac>();
            ListaMusterija = new List<Musterija>();

            if (File.Exists(putanja))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Dispecer>));
                using (StreamReader reader = new StreamReader(putanja))
                {
                    ListaDispecera = (List<Dispecer>)xmlSerializer.Deserialize(reader);
                }

            }
            if (File.Exists(putanjaMusterija))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Musterija>));
                using (StreamReader reader = new StreamReader(putanjaMusterija))
                {
                    ListaMusterija = (List<Musterija>)xmlSerializer.Deserialize(reader);
                }
            }
            if (File.Exists(putanjaVozac))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Vozac>));
                using (StreamReader reader = new StreamReader(putanjaVozac))
                {
                    ListaVozaca = (List<Vozac>)xmlSerializer.Deserialize(reader);
                }
            }

            
        }

        public static List<Musterija> ListaMusterija { get; set; }
        public static List<Vozac> ListaVozaca { get; set; }
        public static List<Dispecer> ListaDispecera { get; set; }
        public static String PutanjaDispeceri { get; set; }
        public static String PutanjaVozaci { get; set; }
        public static String PutanjaMusterije { get; set; }

    }
}