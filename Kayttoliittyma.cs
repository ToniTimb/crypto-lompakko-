using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using static cryptot_sovellus.Models.Lataus;

namespace cryptot_sovellus
{
    class Kayttoliittyma
    {
        private static string tallennus = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\tallennus.json";
        private static string[] KolikotListaus = { "BTC", "LTC", "ETH" };
        private Lompakko lompakko;
        private TiedotApi tiedot;
        public Kayttoliittyma(Lompakko lompakko, TiedotApi tiedot)
        {
            this.lompakko = lompakko;
            this.tiedot = tiedot;
        }
        public void kaynnista()
        {
            Lataus();
            while (true)
            {
                Console.WriteLine("Komennot: L=Lisaa, V=Vahenna, A=Lompakon arvo, END=lopeta, RESET=lompakon nollaus");
                string komento = Kysymys("?").ToUpper();

                if (komento.Equals("END"))
                {
                    Tallennus();
                    break;
                }
                else if (komento.Equals("L"))
                {
                    LisaaSaldo();
                }
                else if (komento.Equals("V"))
                {
                    VahennaSaldo();
                }
                else if (komento.Equals("RESET"))
                {
                    Reset();
                }
                else if (komento.Equals("A"))
                {
                    LompakonArvo();
                }
                else
                {
                    Console.WriteLine("Virheellinen syöte, yritä uudelleen");
                }
            }
        }
        public String Kysymys(String kysymys)
        {
            Console.WriteLine(kysymys + " ");
            return Console.ReadLine().ToUpper();
        }
        public void LisaaSaldo()
        {
            while (true)
            {
                try
                {
                    string nimi = Kysymys("Kolikon nimi: (BTC, LTC, ETH)");
                    if (KolikotListaus.Contains(nimi))
                    {
                        double maara = double.Parse(Kysymys("Lisäys: "));
                        bool osuma = false;

                        foreach (Kolikko kolikko in lompakko)
                        {
                            if (kolikko.Nimi.Equals(nimi.ToUpper()))
                            {
                                kolikko.LisaaS(maara);
                                osuma = true;
                                break;
                            }
                        }
                        if (osuma == false)
                        {
                            lompakko.LisaaK(nimi.ToUpper(), maara);
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"{nimi} ei ole tuettujen Cryptojen listalla, koeta uudelleen");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Syöte virheellisessä muodossa");
                    Console.WriteLine("Muistathan että desimaali eroitin on pilkku");
                    Console.WriteLine("Yritä uudelleen");
                    continue;
                }
            }
            Console.WriteLine();
        }
        public void VahennaSaldo()
        {
            if (lompakko.Count() == 0)

            {
                Console.WriteLine(lompakko.ToString());
                return;
            }
            while (true)
            {
                string nimi = Kysymys("Kolikon nimi: (BTC, LTC, ETH)");
                if (KolikotListaus.Contains(nimi))
                {
                    bool tyhja = true;
                    int indeksi = -1;

                    foreach (Kolikko kolikko in lompakko)
                    {
                        if (kolikko.Nimi.Equals(nimi.ToUpper()))
                        {
                            Console.WriteLine($"Kolikon {nimi} saldo on tällä hetkellä {kolikko.Saldo}");
                            double maara = double.Parse(Kysymys("Vähennys: "));
                            if (maara >= kolikko.Saldo)
                            {
                                kolikko.VahennaS(kolikko.Saldo);
                                indeksi = lompakko.kolikot.IndexOf(kolikko);
                                tyhja = true;
                                break;
                            }
                            Console.WriteLine($"Kolikon {nimi} saldo on tämän jälkeen {kolikko.Saldo - maara}");
                            kolikko.VahennaS(maara);
                            tyhja = false;
                            indeksi = -1;
                            break;
                        }
                    }
                    if (indeksi != -1)
                    {
                        var poistettava = lompakko.kolikot[indeksi];
                        lompakko.kolikot.Remove(poistettava);
                        Console.WriteLine(indeksi);
                        Console.WriteLine("******************************************************");
                        Console.WriteLine($"Kolikon {nimi} saldo on nolla");
                        Console.WriteLine("******************************************************");
                    }
                    else if (tyhja)
                    {
                        Console.WriteLine("******************************************************");
                        Console.WriteLine($"Kolikkoa {nimi} ei ole lompakossa");
                        Console.WriteLine("******************************************************");
                    }
                }
                else
                {
                    Console.WriteLine($"Valinta {nimi} ei ole tuettu, yritä uudelleen");
                    continue;
                }
                Console.WriteLine();
                break;
            }
        }
        public void LompakonArvo()
        {
            double summa = 0.0;
            int sisalto = lompakko.Count();

            if (sisalto != 0)
            {
                Tuple<double, double, double> HintaTuple = tiedot.CoinGeckoHinnat();
                Console.WriteLine();
                Console.WriteLine("Lompakon sisältö:");
                Console.WriteLine("******************************************************");
                foreach (Kolikko kolikko in lompakko)
                {
                    double hinta = 0.0;
                    if (kolikko.Nimi.Equals("BTC"))
                    {
                        hinta = HintaTuple.Item1;
                        summa += hinta * kolikko.Saldo;
                    }
                    else if (kolikko.Nimi.Equals("LTC"))
                    {
                        hinta = HintaTuple.Item2;
                        summa += hinta * kolikko.Saldo;
                    }
                    else if (kolikko.Nimi.Equals("ETH"))
                    {
                        hinta = HintaTuple.Item3;
                        summa += hinta * kolikko.Saldo;
                    }
                    Console.WriteLine(kolikko.ToString());
                    Console.WriteLine($"Arvo (EUR): " + string.Format("{0:0.00}", hinta * kolikko.Saldo));
                    Console.WriteLine("******************************************************");
                }
                Console.WriteLine(lompakko.ToString() + string.Format("{0:0.00}", summa));
                Console.WriteLine();
                return;
            }
            Console.WriteLine(lompakko.ToString());
            Console.WriteLine();
        }
        public void Tallennus()
        {
            string json = JsonConvert.SerializeObject(lompakko.kolikot.ToArray());
            System.IO.File.WriteAllText(@tallennus, json);
        }
        public void Lataus()
        {
            using (StreamReader lukija = new StreamReader(@tallennus))
            {
                string json = lukija.ReadToEnd();
                List<MyArray> items = JsonConvert.DeserializeObject<List<MyArray>>(json);
                foreach (MyArray item in items)
                {
                    lompakko.LisaaK(item.Nimi, item.Saldo);
                }
            }
        }
        public void Reset()
        {
            lompakko.kolikot.Clear();
            Tallennus();
        }
    }
}



