using System;
using System.Collections.Generic;
using System.Text;

namespace cryptot_sovellus
{  
    class Kolikko
    {
        public Kolikko(string nimi)
        {
            this.Saldo = 0.0;
            this.Nimi = nimi; 
        }
        public Kolikko(string nimi,double saldo)
        {
            this.Saldo =saldo;
            this.Nimi = nimi;
        }
        public string Nimi { get; set; }
        public double Saldo { get; set; }
   
        public void LisaaS(double saldo)
        {
           this.Saldo += saldo;
        }
        public void VahennaS(double saldo)
        {
            this.Saldo -= saldo;
        }
        public override string ToString()
        {
            return "Crypto: " + Nimi + "\n" + "Saldo: "+ Saldo;
        }
    }
}
