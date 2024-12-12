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
        public Attaques[] Attaques;
        public BitmapImage Image;
        public int ChanceComplementaire = 1;
        private double multiplicateur = 1.0;
        private Dictionary<Attaques, string> descriptionsAttaques = new Dictionary<Attaques, string>();

        public Animal(string nom, int hpMax, int chanceComplementaire, params Attaques[] attaques)
        {

            Nom = nom;
            HPMax = hpMax;
            HP = HPMax;
            ChanceComplementaire = chanceComplementaire;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Animal/{nom}.png"));

        }

        public void Attaque(Attaques attaque, Animal cible)
        {

            switch(attaque)
            {

                case Premon.Attaques.EMPALEMENT:
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

    enum Animaux
    {

        Mammouth,
        Bouquetin
    }
}
