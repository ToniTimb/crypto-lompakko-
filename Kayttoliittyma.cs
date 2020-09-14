
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static cryptot_sovellus.Models.Lataus;



namespace cryptot_sovellus
{
    class Kayttoliittyma
    {
        private static string tiedostotPolku = Path.Combine(Directory.GetCurrentDirectory(), "Tiedostot");
        private static string tallennusPolku = Path.Combine(Directory.GetCurrentDirectory(), "Tiedostot", "tallennus.json");
        private static string raportointiPolku = Path.Combine(Directory.GetCurrentDirectory(), "Tiedostot", "raportit");


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
            Alustus();
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
                        if (maara < 0)
                        {
                            throw new System.ArgumentException();
                        }

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
                    Console.WriteLine("Syöte virheellisessä muodossa, tarkista alla ilmoitetut seikat:");
                    Console.WriteLine("Vain positiivisia lukuja");
                    Console.WriteLine("Desimaali eroitin on pilkku ei piste");
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
                try
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
                                if (maara < 0)
                                {
                                    throw new System.ArgumentException();
                                }
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
                            Console.WriteLine(new string('-', 40));
                            Console.WriteLine($"Kolikon {nimi} saldo on nolla");
                            Console.WriteLine(new string('-', 40));
                        }
                        else if (tyhja)
                        {
                            Console.WriteLine(new string('-', 40));
                            Console.WriteLine($"Kolikkoa {nimi} ei ole lompakossa");
                            Console.WriteLine(new string('-', 40));
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
                catch
                {
                    Console.WriteLine("Syöte virheellisessä muodossa, tarkista alla ilmoitetut seikat:");
                    Console.WriteLine("Vain positiivisia lukuja");
                    Console.WriteLine("Desimaali eroitin on pilkku ei piste");
                    Console.WriteLine("Yritä uudelleen");
                    continue;
                }
            }
        }
        public void LompakonArvo()
        {
            double summa = 0.0;
            int sisalto = lompakko.Count();

            if (sisalto != 0)
            {
                Tuple<double, double, double, bool> HintaTuple = tiedot.CoinGeckoHinnat();
                Console.WriteLine();
                Console.WriteLine("Lompakon sisältö:");
                Console.WriteLine(new string('-', 40));
                string tulostus = "";
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
                    Console.WriteLine(new string('-', 40));

                    tulostus = tulostus + kolikko.ToString() + "\n";
                    var apu = $"Arvo (EUR): " + string.Format("{0:0.00}", hinta * kolikko.Saldo);
                    tulostus = tulostus + apu + "\n";
                    tulostus = tulostus + new string('-', 40) + "\n";
                }
                Console.WriteLine(lompakko.ToString() + string.Format("{0:0.00}", summa));
                Console.WriteLine();
                tulostus = tulostus + lompakko.ToString() + string.Format("{0:0.00}", summa);

                if (HintaTuple.Item4)
                {
                    raportitTallennus(tulostus);
                    Console.WriteLine("raportti tallennettu kansioon: " + raportointiPolku);
                    Console.WriteLine();
                }
                return;
            }
            Console.WriteLine(lompakko.ToString());
            Console.WriteLine();
        }
        public void raportitTallennus(string tiedosto)
        {
            DateTime nyt = DateTime.Now;
            string otsikko = "lompakon arvo: " + nyt.ToString() + "\n" + new string('-', 40);
            string raportti = otsikko + "\n" + tiedosto;
            string tiedostoNimi = nyt.ToString() + ".txt";
            string raporttiTiedosto = Path.Combine(raportointiPolku, tiedostoNimi);

            StreamWriter sw = File.CreateText(raporttiTiedosto);
            string tiedostoSisalto = raportti;
            sw.WriteLine(tiedostoSisalto);
            sw.Close();
        }
        public void Tallennus()
        {
            string json = JsonConvert.SerializeObject(lompakko.kolikot.ToArray());
            System.IO.File.WriteAllText(tallennusPolku, json);
        }
        public void Lataus()
        {
            using (StreamReader lukija = new StreamReader(@tallennusPolku))
            {
                string json = lukija.ReadToEnd();

                List<MyArray> items = JsonConvert.DeserializeObject<List<MyArray>>(json);
                if (items == null)
                {
                    return;
                }
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
        public void Alustus()
        {
            if (!Directory.Exists(raportointiPolku))
            {
                Directory.CreateDirectory(raportointiPolku);
            }
            if (!File.Exists(tallennusPolku))
            {
                StreamWriter sw = File.CreateText(tallennusPolku);
                sw.WriteLine("");
                sw.Close();
            }
        }
    }
}



