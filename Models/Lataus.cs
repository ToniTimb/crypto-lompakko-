using System;
using System.Collections.Generic;
using System.Text;

namespace cryptot_sovellus.Models
{
    class Lataus
    {

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class MyArray
        {
            public string Nimi { get; set; }
            public double Saldo { get; set; }
        }

        public class Root
        {
            public List<MyArray> MyArray { get; set; }
        }
    }
}
