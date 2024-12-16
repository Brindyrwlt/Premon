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

        internal readonly Animaux TypeAnimal;
        public readonly int HPMax;
        public int HP { get; set; }
        public string Nom { get; set; }
        internal Attaques[] AttaquesAnimal;
        internal BitmapImage Image;
        internal int ChanceComplementaire = 1;
        private double multiplicateur = 1;

        private static Dictionary<Animaux, Animal> animaux = new Dictionary<Animaux, Animal>();
        internal static Dictionary<Attaques, string> descriptionsAttaques = new Dictionary<Attaques, string>();

        // Multiplicateur d'attaques

        // Coup de pied
        internal static readonly int DEGAT_EMPALEMENT = 40;
        internal static readonly int DEGAT_COUP_DE_GRIFFE = 35;
        internal static readonly int DEGAT_COUP_DE_PIED = 25;
        internal static readonly int DEGAT_ATTAQUE_FURTIVE = 20;
        internal static readonly int DEGAT_MORSURE = 20;
        internal static readonly int DEGAT_ECRASEMENT = 15;
        internal static readonly int DEGAT_CHARGE = 10;

        public Animal(Animaux typeAnimal, string nom, string nomImage, int hpMax, int chanceComplementaire, params Attaques[] attaques)
        {

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

            descriptionsAttaques[Attaques.COUP_DE_PIED] = $"L'animal fonce vers l'ennemi et lui lance un gros coup de pied, infligeant {Animal.DEGAT_COUP_DE_PIED} dégâts.";
            descriptionsAttaques[Attaques.EMPALEMENT] = $"L'animal utilise sa corne pour transperser son ennemi, infligeant {Animal.DEGAT_EMPALEMENT} dégats.";
            descriptionsAttaques[Attaques.COUP_DE_GRIFFE] = $"L'animal jette sa patte vers l'avant et griffe son ennemi, infligeant {Animal.DEGAT_COUP_DE_GRIFFE} dégats.";
            descriptionsAttaques[Attaques.CHARGE] = $"L'animal charge son ennemi pour le faire tomber, infligeant {Animal.DEGAT_CHARGE} dégats.";
            descriptionsAttaques[Attaques.ATTAQUE_FURTIVE] = $"Cacher derriere son ennemi, l'animal bondit en surprenant son ennemi, infligeant {Animal.DEGAT_ATTAQUE_FURTIVE} dégats.";
            descriptionsAttaques[Attaques.ECRASEMENT] = $"L'animal lève ses pattes avants puis met tout son poids en retombant, écrasant son ennemi, infligeant {Animal.DEGAT_ECRASEMENT} dégats.";

        }

        internal static void InitAnimaux()
        {

            animaux.Add(Animaux.Mammouth, new Animal(Animaux.Mammouth, "Mammouth", "Mammouth.png", 200, 1, Attaques.COUP_DE_PIED));
            animaux.Add(Animaux.Bouquetin, new Animal(Animaux.Bouquetin, "Bouquetin", "Bouquetin.png", 80, 1, Attaques.EMPALEMENT, Attaques.AIGUISAGE));


        }

    }

    enum Attaques
    {

        COUP_DE_PIED,
        EMPALEMENT,
        COUP_DE_GRIFFE,
        CHARGE,
        ATTAQUE_FURTIVE,
        ECRASEMENT,
        MORSURE,
        AIGUISAGE,
        PROTECTION

    }

    enum Animaux
    {

        Mammouth,
        Bouquetin,
        Smilodon,
        Megaceros,
        Diprotodon,
        Deinotherium,
        Gastronis,
        Lion_des_cavernes,
        Rhinoceros_laineux,
        Megalonix,
        Glyptodon
    }
}
