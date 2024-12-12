using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Premon
{
    internal class Animal
    {

        public readonly int HPMax;
        public int HP;
        public string Nom;
        public Attaques[] attaques;
        public BitmapImage image;
        private double multiplicateur = 1.0;
        private Dictionary<Attaques, string> descriptionsAttaques = new Dictionary<Attaques, string>();

        public Animal(string nom, int hpMax, string nomImage, params Attaques[] attaques)
        {

            Nom = nom;
            HPMax = hpMax;
            HP = HPMax;
            image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Animal/{nomImage}"));

        }

        public void Attaque(Attaques attaque, Animal cible)
        {

            switch(attaque)
            {

                case Attaques.EMPALEMENT:
                    break;

            }

        }

        public static void InitDescriptionAttaques()
        {

            

        }
        
    }

    enum Attaques
    {

        COUP_DE_PIED,
        EMPALEMENT,
        AIGUISAGE

    }
}
