﻿using System.Windows.Media.Imaging;

namespace Premon
{
    internal class Animal : ICloneable
    {

        // Attributs de l'animal 

        // Identifiant de l'animal
        internal readonly Animaux TypeAnimal;

        public int PVMax { get; private set; }
        public int PV { get; set; }
        public string Nom { get; set; }
        internal Attaques[] AttaquesAnimal;
        internal Objet[] Butin;
        internal Alimentation AlimentationAnimal;
        internal BitmapImage Image { get; private set; }

        // Les multiplicateurs permettent de modifier les dégâts infligés ou reçus
        internal double multiplicateur = 1, multiplicateurDegatRecu = 1;

        // Plus la chance complémentaire est haute, plus l'animal est compliqué à rencontrer
        internal int ChanceComplementaire = 1;

        // Variables systèmes

        // Dictionnaire contenant tous les animaux du jeu
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
            PVMax = hpMax;
            PV = PVMax;
            Butin = butin;
            AlimentationAnimal = alimentation;
            ChanceComplementaire = chanceComplementaire;
            AttaquesAnimal = attaques;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Animal/{nomImage}"));

        }

        /// <summary>
        /// Insert dans le dictionnaire les différents animaux.
        /// </summary>
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
                [Objet.CreerObjet(Objets.Morceau_de_viande, 2)],
                Attaques.ECRASEMENT,
                Attaques.PROTECTION,
                Attaques.COUP_DE_PIED,
                Attaques.CHARGE));

            animaux.Add( // Bouquetin
                Animaux.Bouquetin,
                new Animal(Animaux.Bouquetin,
                "Bouquetin",
                "Bouquetin.png",
                80,
                1,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine, 2), Objet.CreerObjet(Objets.Herbe_Medicinale)],
                Attaques.EMPALEMENT,
                Attaques.AIGUISAGE,
                Attaques.CHARGE));

            animaux.Add( // Smilodon
                Animaux.Smilodon,
                new Animal(Animaux.Smilodon,
                "Smilodon",
                "Smilodon.png",
                90,
                2,
                Alimentation.Carnivore,
                [Objet.CreerObjet(Objets.Morceau_de_viande, 2), Objet.CreerObjet(Objets.Herbe_Medicinale, 2)],
                Attaques.ATTAQUE_FURTIVE,
                Attaques.AIGUISAGE,
                Attaques.COUP_DE_GRIFFE,
                Attaques.MORSURE));

            animaux.Add( // Megaceros
                Animaux.Megaceros,
                new Animal(Animaux.Megaceros,
                "Megaceros",
                "Megaceros.png",
                160,
                10,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Morceau_de_viande, 5), Objet.CreerObjet(Objets.Herbe_Medicinale, 4)],
                Attaques.EMPALEMENT,
                Attaques.PROTECTION,
                Attaques.CHARGE));

            animaux.Add( // Diprotodon
                Animaux.Diprotodon,
                new Animal(Animaux.Diprotodon,
                "Diprotodon",
                "Diprotodon.png",
                80,
                1,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine)],
                Attaques.PROTECTION,
                Attaques.AIGUISAGE));

            animaux.Add( // Deinotherium
                Animaux.Deinotherium,
                new Animal(Animaux.Deinotherium,
                "Deinotherium",
                "Deinotherium.png",
                220,
                4,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine)],
                Attaques.ECRASEMENT));

            animaux.Add( // Gastronis
                Animaux.Gastronis,
                new Animal(Animaux.Gastronis,
                "Gastronis",
                "Gastornis.png",
                110,
                2,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine), Objet.CreerObjet(Objets.Herbe_Medicinale)],
                Attaques.COUP_DE_PIED,
                Attaques.MORSURE));

            animaux.Add( // Lion des cavernes
                Animaux.Lion_des_cavernes,
                new Animal(Animaux.Lion_des_cavernes,
                "Lion des cavernes",
                "Lion_des_cavernes.png",
                100,
                1,
                Alimentation.Carnivore,
                [Objet.CreerObjet(Objets.Morceau_de_viande)],
                Attaques.COUP_DE_GRIFFE,
                Attaques.MORSURE,
                Attaques.ATTAQUE_FURTIVE));

            animaux.Add( // Rhinocéros laineux
                Animaux.Rhinoceros_laineux,
                new Animal(Animaux.Rhinoceros_laineux,
                "Rhinoceros laineux",
                "Rhinoceros_laineux.png",
                190,
                4,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine)],
                Attaques.EMPALEMENT,
                Attaques.ECRASEMENT,
                Attaques.CHARGE));

            animaux.Add( // Megalonyx
                Animaux.Megalonyx,
                new Animal(Animaux.Megalonyx,
                "Megalonyx", "Megalonyx.png",
                130,
                2,
                Alimentation.Omnivore,
                [Objet.CreerObjet(Objets.Morceau_de_viande)],
                Attaques.COUP_DE_GRIFFE,
                Attaques.MORSURE,
                Attaques.AIGUISAGE));

            animaux.Add( // Glyptodon
                Animaux.Glyptodon,
                new Animal(Animaux.Glyptodon,
                "Glyptodon", "Glyptodon.png",
                150,
                3,
                Alimentation.Herbivore,
                [Objet.CreerObjet(Objets.Graine)],
                Attaques.PROTECTION,
                Attaques.COUP_DE_GRIFFE,
                Attaques.AIGUISAGE));

        }

        /// <summary>
        /// Crée les descriptions des attaques.
        /// </summary>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Effectue une attaque de l'animal sur une cible.
        /// </summary>
        /// <param name="attaque"></param>
        /// <param name="cible"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Attaque(Attaques attaque, Animal? cible = null)
        {

            // Si l'animal n'a pas l'attaque spécifiée
            if (!AttaquesAnimal.Contains(attaque))
                throw new ArgumentException("Le Premon ne possède pas l'attaque spécifiée.");

            if(cible != null)
            {

                switch (attaque)
                {

                    case Attaques.EMPALEMENT:
                        cible.PV -= (int) (DEGAT_EMPALEMENT * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur = MULTIPLICATEUR_BASE;
                        break;

                    case Attaques.COUP_DE_PIED:
                        cible.PV -= (int)(DEGAT_COUP_DE_PIED * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur = MULTIPLICATEUR_BASE;
                        break;

                    case Attaques.COUP_DE_GRIFFE:
                        cible.PV -= (int)(DEGAT_COUP_DE_GRIFFE * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur = MULTIPLICATEUR_BASE;
                        break;

                    case Attaques.CHARGE:
                        cible.PV -= (int)(DEGAT_CHARGE * multiplicateur * cible.multiplicateurDegatRecu);
                        break;

                    case Attaques.ATTAQUE_FURTIVE:
                        cible.PV -= (int)(DEGAT_ATTAQUE_FURTIVE * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur *= MULTIPLICATEUR_ATTAQUE_FURTIVE;
                        break;

                    case Attaques.MORSURE:
                        cible.PV -= (int)(DEGAT_MORSURE * multiplicateur * cible.multiplicateurDegatRecu);
                        multiplicateur *= MULTIPLICATEUR_MORSURE;
                        break;

                    case Attaques.ECRASEMENT:
                        cible.PV -= (int)(DEGAT_ECRASEMENT * multiplicateur * cible.multiplicateurDegatRecu);
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

        /// <summary>
        /// Crée une instance d'un animal selon son identifiant à partir du dictionnaire "animaux".
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        internal static Animal CreerAnimal(Animaux animal)
        {
            return (Animal) animaux[animal].Clone();
        }

        /// <summary>
        /// Crée une instance d'un animal selon son identifiant à partir du dictionnaire "animaux" et lui change son nom et ses pv.
        /// </summary>
        /// <param name="animal"></param>
        /// <param name="nom"></param>
        /// <param name="pv"></param>
        /// <returns></returns>
        internal static Animal CreerAnimal(Animaux animal, string nom, int pv)
        {

            Animal animalCree = (Animal) animaux[animal].Clone();
            animalCree.Nom = nom;
            animalCree.PV = pv;

            return animalCree;

        }

        // Provient de l'interface ICloneable, permet de créer un clone d'un objet
        public object Clone()
        {
            return MemberwiseClone();
        }

    }

    // Identifiants des attaques
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

    // Identifiants des animaux
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

    // Identifiants du type d'alimentation des animaux
    enum Alimentation
    {

        Carnivore,
        Herbivore,
        Omnivore

    }
}
