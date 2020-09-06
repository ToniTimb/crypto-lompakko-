using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using static cryptot_sovellus.Models.Lataus;
using cryptot_sovellus.Models;

namespace cryptot_sovellus
{
    class Program
    {
        static void Main(string[] args)
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            
            Lompakko lompakko = new Lompakko();
            TiedotApi tiedot = new TiedotApi();
            Kayttoliittyma kayttoliittyma = new Kayttoliittyma(lompakko, tiedot);
            kayttoliittyma.kaynnista();
        }
    }
}
