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
        
        public Tuple<double, double, double> CoinGeckoHinnat()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            string historia = projectDirectory + "\\historia.json";
            try
            {
               
                var URL = new UriBuilder(@"https://api.coingecko.com/api/v3/simple/price?ids=litecoin%2Cethereum%2Cbitcoin&vs_currencies=eur&include_last_updated_at=true");
                var webClient = new WebClient();
                var json = webClient.DownloadString(URL.ToString());
                //hyv� muistaa ett� macissa n�m� kenoviivat menee toisinp�in ja tuo polun hakeminne j�rjestelm�st� ei toimi oikeuksien takia oikein
               // System.IO.File.WriteAllText(@"C:\Users\User\source\repos\cryprtot_sovellus\cryptot_sovellus\tallennus.json", json);
                System.IO.File.WriteAllText(@historia, json);
                Console.WriteLine(historia);
                var result = JsonConvert.DeserializeObject<Root>(json);

                double BTC = result.bitcoin.eur;
                double LTC = result.litecoin.eur;
                double ETHEREUM = result.ethereum.eur;

                /* testataan ett� onko selke�mp�� ett� p�iv�ys n�kyy vain kun on k�yt�ss� paikallisesti tallennettu tieto
                DateTimeOffset tiedotPaivitettyBTC = DateTimeOffset.FromUnixTimeSeconds(result.bitcoin.last_updated_at);
                DateTimeOffset tiedotPaivitettyLTC = DateTimeOffset.FromUnixTimeSeconds(result.litecoin.last_updated_at);
                DateTimeOffset tiedotPaivitettyETH = DateTimeOffset.FromUnixTimeSeconds(result.ethereum.last_updated_at);
                 Console.WriteLine("******************************************************");
                Console.WriteLine(" BTC hinta p�ivitetty {0}", tiedotPaivitettyBTC);
                Console.WriteLine(" LTC hinta p�ivitetty {0}", tiedotPaivitettyLTC);
                Console.WriteLine(" ETH hinta p�ivitetty {0}", tiedotPaivitettyETH);
                 Console.WriteLine("******************************************************");*/

                var HinnatTuple = Tuple.Create(BTC, LTC, ETHEREUM);
                return HinnatTuple;
            }
            catch
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
                    Console.WriteLine("******************************************************");
                    Console.WriteLine(" HUOM! vanhat hinnat");
                    Console.WriteLine(" BTC hinta p�ivitetty {0}", tiedotPaivitettyBTC);
                    Console.WriteLine(" LTC hinta p�ivitetty {0}", tiedotPaivitettyLTC);
                    Console.WriteLine(" ETH hinta p�ivitetty {0}", tiedotPaivitettyETH);
                    Console.WriteLine("******************************************************");

                    var HinnatTuple = Tuple.Create(BTC, LTC, ETHEREUM);
                    return HinnatTuple;
                }
            }
        }
    }
}
