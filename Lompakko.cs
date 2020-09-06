using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using static cryptot_sovellus.Models.Lataus;

namespace cryptot_sovellus
{
    class Lompakko : IEnumerable<Kolikko>
    {
        public List<Kolikko> kolikot;
        public Lompakko()
        {
            this.kolikot = new List<Kolikko>();
        }
        public Lompakko(List<Kolikko> kolikot)
        {
            this.kolikot = kolikot;
        }
        public IEnumerator<Kolikko> GetEnumerator()
        {
            return ((IEnumerable<Kolikko>)kolikot).GetEnumerator();
        }
        public void LisaaK(string nimi,double maara)
        {
            foreach (Kolikko kolikko in kolikot)
            {
                if (kolikko.Nimi.Equals(nimi))
                {
                    kolikko.Saldo += maara;
                    return;
                }
            }
            this.kolikot.Add(new Kolikko(nimi, maara));
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)kolikot).GetEnumerator();
        }
        public override string ToString()
        {
            if (kolikot.Count==0)
            {
                return "Lompakko on tyhjä, ole hyvä ja lisää saldoa L komennolla";
            }
            return "Lompakon kokonaisarvo (EUR): ";
        }
    }
}
