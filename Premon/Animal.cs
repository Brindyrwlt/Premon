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
        public static Dictionary<Attaques, string> descriptionsAttaques = new Dictionary<Attaques, string>();
        private double multiplicateur = 1.0;

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

                case Premon.Attaques.COUP_DE_PIED:
                    break;

                case Premon.Attaques.AIGUISAGE:
                    break;

            }

        }

        public static void InitDescriptionAttaques()
        {

            descriptionsAttaques.Add(Premon.Attaques.COUP_DE_PIED, "Le premon charge vers l'avant et lance un gros coup de pied infligeant 20 degâts");
            descriptionsAttaques.Add(Premon.Attaques.EMPALEMENT, "Le premon utilise ses cornes pour transpercer son ennemi infligeant 40 degâts");
            descriptionsAttaques.Add(Premon.Attaques.AIGUISAGE, "Le premon aiguise ses pointes, augmentation de 10% de la prochaine attaque");

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
