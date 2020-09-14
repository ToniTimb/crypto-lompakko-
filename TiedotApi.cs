using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using static cryptot_sovellus.Luokat;
using System.IO;

namespace cryptot_sovellus
{
    class TiedotApi
    {
        private static string historia = Path.Combine(Directory.GetCurrentDirectory(), "Tiedostot", "historia.json");
        public Tuple<double, double, double, bool> CoinGeckoHinnat()
        {
            bool linjoilla = false;
            try
            {

                var URL = new UriBuilder(@"https://api.coingecko.com/api/v3/simple/price?ids=litecoin%2Cethereum%2Cbitcoin&vs_currencies=eur&include_last_updated_at=true");
                var webClient = new WebClient();
                var json = webClient.DownloadString(URL.ToString());
                System.IO.File.WriteAllText(@historia, json);
                var result = JsonConvert.DeserializeObject<Root>(json);

                double BTC = result.bitcoin.eur;
                double LTC = result.litecoin.eur;
                double ETHEREUM = result.ethereum.eur;
                linjoilla = true;

                var HinnatTuple = Tuple.Create(BTC, LTC, ETHEREUM, linjoilla);
                return HinnatTuple;
            }
            catch
            {
                if (!File.Exists(historia))
                {
                    Console.WriteLine();
                    Console.WriteLine("HUOM! Ei yhteyttä coingeckon apiin. Aiempia hintoja ei käytettävissä, joten arvoa ei voida laskea.");

                    return Tuple.Create(0.0, 0.0, 0.0, linjoilla);
                }
                else
                {
                    using (StreamReader lukija = new StreamReader(@historia))
                    {
                        string json = lukija.ReadToEnd();
                        var result = JsonConvert.DeserializeObject<Root>(json);

                        double BTC = result.bitcoin.eur;
                        DateTimeOffset tiedotPaivitettyBTC = DateTimeOffset.FromUnixTimeSeconds(result.bitcoin.last_updated_at);
                        double LTC = result.litecoin.eur;
                        DateTimeOffset tiedotPaivitettyLTC = DateTimeOffset.FromUnixTimeSeconds(result.litecoin.last_updated_at);
                        double ETHEREUM = result.ethereum.eur;
                        DateTimeOffset tiedotPaivitettyETH = DateTimeOffset.FromUnixTimeSeconds(result.ethereum.last_updated_at);
                        Console.WriteLine(new string('-', 40));
                        Console.WriteLine(" HUOM! vanhat hinnat");
                        Console.WriteLine(" BTC hinta päivitetty {0}", tiedotPaivitettyBTC.ToLocalTime());
                        Console.WriteLine(" LTC hinta päivitetty {0}", tiedotPaivitettyLTC.ToLocalTime());
                        Console.WriteLine(" ETH hinta päivitetty {0}", tiedotPaivitettyETH.ToLocalTime());
                        Console.WriteLine(new string('-', 40));

                        var HinnatTuple = Tuple.Create(BTC, LTC, ETHEREUM, linjoilla);
                        return HinnatTuple;
                    }
                }
            }
        }
    }
}
