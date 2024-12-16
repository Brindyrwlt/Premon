using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Premon
{
    internal class Animal : ICloneable
    {

        internal readonly Animaux TypeAnimal;
        public readonly int HPMax;
        public int HP { get; set; }
        public string Nom { get; set; }
        internal Attaques[] AttaquesAnimal;
        internal Objet[] Butin;
        internal BitmapImage Image;
        internal int ChanceComplementaire = 1;
        private double multiplicateur = 1;

        private static Dictionary<Animaux, Animal> animaux = new Dictionary<Animaux, Animal>();
        internal static Dictionary<Attaques, string> descriptionsAttaques = new Dictionary<Attaques, string>();
        internal static int nombreAnimaux = Animaux.GetValues(typeof(Animaux)).Length);

        // Multiplicateur d'attaques

        // Coup de pied
        internal static readonly int DEGAT_COUP_DE_PIED = 25;

        public Animal(Animaux typeAnimal, string nom, string nomImage, int hpMax, int chanceComplementaire, Objet[] butin, params Attaques[] attaques)
        {

            if (butin.Length == 0)
                throw new ArgumentException("L'animal doit au moins posséder un butin.");
            if (attaques.Length == 0)
                throw new ArgumentException("L'animal doit posséder des attaques");

            TypeAnimal = typeAnimal;
            Nom = nom;
            HPMax = hpMax;
            HP = HPMax;
            ChanceComplementaire = chanceComplementaire;
            AttaquesAnimal = attaques;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Animal/{nomImage}"));

        }

        public void Attaque(Attaques attaque, Animal? cible = null)
        {

            if (!AttaquesAnimal.Contains(attaque))
                throw new ArgumentException("Le Premon ne possède pas l'attaque spécifiée.");

            if(cible != null)
            {

                switch (attaque)
                {

                    case Attaques.EMPALEMENT:
                        break;

                    case Attaques.COUP_DE_PIED:
                        cible.HP -= DEGAT_COUP_DE_PIED;
                        break;

                    case Attaques.AIGUISAGE:
                        break;

                }

            }

        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        internal static Animal CreerAnimal(Animaux animal)
        {

            return (Animal) animaux[animal].Clone();

        }

        internal static Animal CreerAnimal(Animaux animal, string nom, int hp)
        {

            Animal animalCree = (Animal) animaux[animal].Clone();
            animalCree.Nom = nom;
            animalCree.HP = hp;

            return animalCree;

        }

        internal static void InitDescriptions()
        {

            descriptionsAttaques[Attaques.COUP_DE_PIED] = $"L'animal charge vers l'ennemi et lui lance un gros coup de pied, infligeant {Animal.DEGAT_COUP_DE_PIED} dégâts.";

        }

        internal static void InitAnimaux()
        {

            animaux.Add(Animaux.Mammouth, new Animal(Animaux.Mammouth, "Mammouth", "Mammouth.png", 200, 1, [Objet.CreerObjet(Objets.Morceau_de_viande)],  Attaques.COUP_DE_PIED));
            animaux.Add(Animaux.Bouquetin, new Animal(Animaux.Bouquetin, "Bouquetin", "Bouquetin.png", 80, 1, [Objet.CreerObjet(Objets.Morceau_de_viande)],  Attaques.EMPALEMENT, Attaques.AIGUISAGE));

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
