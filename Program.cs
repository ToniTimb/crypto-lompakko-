

namespace cryptot_sovellus
{
    class Program
    {
        static void Main(string[] args)
        {   
            Lompakko lompakko = new Lompakko();
            TiedotApi tiedot = new TiedotApi();
            Kayttoliittyma kayttoliittyma = new Kayttoliittyma(lompakko, tiedot);
            kayttoliittyma.kaynnista();
        }
    }
}
