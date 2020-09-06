using System;


namespace cryptot_sovellus
{
    class Luokat
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        // var result = JsonConvert.DeserializeObject<Root>(jsonString);

        public class Litecoin
        {
            public double eur { get; set; }
            public double saldo { get; set; }
            public int last_updated_at { get; set; }
        }

        public class Ethereum
        {
            public double eur { get; set; }
            public double saldo { get; set; }
            public int last_updated_at { get; set; }
        }

        public class Bitcoin
        {
            public double eur { get; set; }
            public double saldo { get; set; }
            public int last_updated_at { get; set; }
        }

        public class Root
        {
            public Litecoin litecoin { get; set; }
            public Ethereum ethereum { get; set; }
            public Bitcoin bitcoin { get; set; }
        }
    }
}

