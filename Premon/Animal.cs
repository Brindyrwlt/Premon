using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Printing;
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
        public int HPMax { get; private set; }
        public int HP { get; set; }
        public string Nom { get; set; }
        internal Attaques[] AttaquesAnimal;
        internal Objet[] Butin;
        internal Alimentation AlimentationAnimal;
        internal BitmapImage Image { get; private set; }
        internal int ChanceComplementaire = 1;
        private double multiplicateur = 1, multiplicateurDegatRecu = 1;

        private static Dictionary<Animaux, Animal> animaux = new Dictionary<Animaux, Animal>();
        internal static Dictionary<Attaques, string> descriptionsAttaques = new Dictionary<Attaques, string>();
        internal static int nombreAnimaux = Animaux.GetValues(typeof(Animaux)).Length ;
        internal static int nombreAttaques = Attaques.GetValues(typeof(Attaques)).Length;

        // Statistiques d'attaques

        // Multiplicateur de base
        private static readonly double MULTIPLICATEUR_BASE = 1;

        // Coup de pied
        private static readonly int DEGAT_COUP_DE_PIED = 25;

        // Empalement
        private static readonly int DEGAT_EMPALEMENT = 40;

        // Coup de griffe
        private static readonly int DEGAT_COUP_DE_GRIFFE = 35;
        
        // Attaque furtive
        private static readonly int DEGAT_ATTAQUE_FURTIVE = 20;
        private static readonly double MULTIPLICATEUR_ATTAQUE_FURTIVE = 1.1;
        
        // Morsure
        private static readonly int DEGAT_MORSURE = 20;
        private static readonly double MULTIPLICATEUR_MORSURE = 1.05;
        
        // Ecrasement
        private static readonly int DEGAT_ECRASEMENT = 15;
        private static readonly double MULTIPLICATEUR_ECRASEMENT = 1.05;

        // Aiguisage
        private static readonly double MULTIPLICATEUR_AIGUISAGE = 1.2;

        // Protection
        private static readonly double MULTIPLICATEUR_PROTECTION = 0.8;

        // Charge
        private static readonly int DEGAT_CHARGE = 10;

        public Animal(Animaux typeAnimal, string nom, string nomImage, int hpMax, int chanceComplementaire, Alimentation alimentation, Objet[] butin, params Attaques[] attaques)
        {

            if (butin.Length == 0)
                throw new ArgumentException("L'animal doit au moins posséder un butin.");
            if (attaques.Length == 0)
                throw new ArgumentException("L'animal doit posséder des attaques");

            TypeAnimal = typeAnimal;
            Nom = nom;
            HPMax = hpMax;
            HP = HPMax;
            Butin = butin;
            AlimentationAnimal = alimentation;
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
                        cible.HP -= (int) (DEGAT_EMPALEMENT * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur = MULTIPLICATEUR_BASE;
                        break;

                    case Attaques.COUP_DE_PIED:
                        cible.HP -= (int)(DEGAT_COUP_DE_PIED * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur = MULTIPLICATEUR_BASE;
                        break;

                    case Attaques.COUP_DE_GRIFFE:
                        cible.HP -= (int)(DEGAT_COUP_DE_GRIFFE * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur = MULTIPLICATEUR_BASE;
                        break;

                    case Attaques.CHARGE:
                        cible.HP -= (int)(DEGAT_CHARGE * multiplicateur * cible.multiplicateurDegatRecu);
                        break;

                    case Attaques.ATTAQUE_FURTIVE:
                        cible.HP -= (int)(DEGAT_ATTAQUE_FURTIVE * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur *= MULTIPLICATEUR_ATTAQUE_FURTIVE;
                        break;

                    case Attaques.MORSURE:
                        cible.HP -= (int)(DEGAT_MORSURE * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur *= MULTIPLICATEUR_MORSURE;
                        break;

                    case Attaques.ECRASEMENT:
                        cible.HP -= (int)(DEGAT_ECRASEMENT * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur *= MULTIPLICATEUR_ECRASEMENT;
                        break;

                    case Attaques.AIGUISAGE:
#if DEBUG
                        Console.WriteLine(multiplicateur);
#endif
                        if (multiplicateur < 1.5) // A vérifier
                            multiplicateur *= MULTIPLICATEUR_AIGUISAGE;
                        break;

                    case Attaques.PROTECTION:
#if DEBUG
                        Console.WriteLine(multiplicateurDegatRecu);
#endif
                        if (multiplicateurDegatRecu > 0.5)
                            multiplicateurDegatRecu *= MULTIPLICATEUR_PROTECTION;
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
            descriptionsAttaques[Attaques.MORSURE] = $"L'animal ouvre sa gueule et mord son ennemi, infligeant {Animal.DEGAT_MORSURE} dégats.";
            descriptionsAttaques[Attaques.AIGUISAGE] = $"L'animal aiguise ses crocs et griffes, augmentant les dégats infligés.";
            descriptionsAttaques[Attaques.PROTECTION] = $"L'animal se protège avec sa peau épaisse ou sa carapace, diminuant les dégats reçus.";

            if (descriptionsAttaques.Count != Animal.nombreAttaques)
                throw new Exception("Une ou plus attaques n'ont pas de descriptions");


        }

        internal static void InitAnimaux()
        {

            animaux.Add( // Mammouth
                Animaux.Mammouth,
                new Animal(Animaux.Mammouth,
                "Mammouth",
                "Mammouth.png",
                200,
                1,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Morceau_de_viande)],
                Attaques.ECRASEMENT,
                Attaques.PROTECTION,
                Attaques.COUP_DE_PIED));

            animaux.Add( // Bouquetin
                Animaux.Bouquetin,
                new Animal(Animaux.Bouquetin,
                "Bouquetin",
                "Bouquetin.png",
                80,
                1,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine)],
                Attaques.EMPALEMENT,
                Attaques.AIGUISAGE));

            animaux.Add(
                Animaux.Smilodon, 
                new Animal(Animaux.Smilodon, 
                "Smilodon", 
                "Smilodon.png", 
                90, 
                2, 
                Alimentation.Carnivore, 
                [Objet.CreerObjet(Objets.Morceau_de_viande)], 
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Megaceros, 
                new Animal(Animaux.Megaceros, 
                "Megaceros", 
                "Megaceros.png", 
                160, 
                10, 
                Alimentation.Herbivore, 
                [Objet.CreerObjet(Objets.Morceau_de_viande)], 
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Diprotodon, 
                new Animal(Animaux.Diprotodon, 
                "Diprotodon", 
                "Diprotodon.png", 
                80, 
                1, 
                Alimentation.Herbivore, 
                [Objet.CreerObjet(Objets.Graine)], 
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Deinotherium, 
                new Animal(Animaux.Deinotherium, 
                "Deinotherium", "Deinotherium.png", 
                220, 4, Alimentation.Herbivore, 
                [Objet.CreerObjet(Objets.Graine)], 
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Gastronis, 
                new Animal(Animaux.Gastronis, 
                "Gastronis", 
                "Gastornis.png", 
                110, 
                2, 
                Alimentation.Herbivore, 
                [Objet.CreerObjet(Objets.Graine)], 
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Lion_des_cavernes, 
                new Animal(Animaux.Lion_des_cavernes, 
                "Lion des cavernes", 
                "Lion_des_cavernes.png", 
                100, 
                1, 
                Alimentation.Carnivore, 
                [Objet.CreerObjet(Objets.Morceau_de_viande)], 
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Rhinoceros_laineux, 
                new Animal(Animaux.Rhinoceros_laineux, 
                "Rhinoceros laineux", "Rhinoceros_laineux.png", 
                190, 
                4, 
                Alimentation.Herbivore, 
                [Objet.CreerObjet(Objets.Graine)], 
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Megalonyx,
                new Animal(Animaux.Megalonyx,
                "Megalonyx", "Megalonyx.png",
                130,
                2,
                Alimentation.Omnivore,
                [Objet.CreerObjet(Objets.Morceau_de_viande)],
                Attaques.ECRASEMENT));

            animaux.Add(
                Animaux.Glyptodon,
                new Animal(Animaux.Glyptodon,
                "Glyptodon", "Glyptodon.png",
                150,
                3,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine)],
                Attaques.ECRASEMENT));

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
        Megalonyx,
        Glyptodon
    }

    enum Alimentation
    {

        Carnivore,
        Herbivore,
        Omnivore

    }
}
