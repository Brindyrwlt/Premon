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
    internal class Animal : ICloneable
    {

        public readonly int HPMax;
        public int HP;
        public string Nom;
        public Attaques[] Attaques;
        public BitmapImage Image;
        public int ChanceComplementaire = 1;
        private double multiplicateur = 1.0;

        // Multiplicateur d'attaques

        // Coup de pied
        internal static readonly int DEGAT_COUP_DE_PIED = 25;

        public Animal(string nom, int hpMax, int chanceComplementaire, params Attaques[] attaques)
        {

            Nom = nom;
            HPMax = hpMax;
            HP = HPMax;
            ChanceComplementaire = chanceComplementaire;
            Attaques = attaques;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Animal/{nom}.png"));

        }

        public void Attaque(Attaques attaque, Animal? cible = null)
        {

            if (!Attaques.Contains(attaque))
                throw new ArgumentException("Le Premon ne possède pas l'attaque spécifiée.");

            if(cible != null)
            {

                switch (attaque)
                {

                    case Premon.Attaques.EMPALEMENT:
                        break;

                    case Premon.Attaques.COUP_DE_PIED:
                        cible.HP -= DEGAT_COUP_DE_PIED;
                        break;

                    case Premon.Attaques.AIGUISAGE:
                        break;

                }

            }

        }

        public object Clone()
        {
            return this.MemberwiseClone();
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
