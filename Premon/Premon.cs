using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Premon
{
    internal class Premon
    {

        public readonly int HPMax;
        public int HP;
        public string Nom;
        public Attaques[] attaques;
        private double multiplicateur = 1.0;

        public Premon(string nom, int maxhp, params Attaques[] attaques)
        {

            Nom = nom;
            HPMax = maxhp;
            HP = HPMax;

        }

        public void Attaque(Attaques attaque, Premon cible)
        {

            switch(attaque)
            {

                case Attaques.EMPALEMENT:
                    break;

            }

        }
        
    }

    enum Attaques
    {

        COUP_DE_PIED,
        EMPALEMENT,
        AIGUISAGE


    }
}
