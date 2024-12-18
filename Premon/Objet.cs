using System.Windows.Media.Imaging;

namespace Premon
{
    internal class Objet : ICloneable
    {

        internal Objets TypeObjet;
        internal string Nom;
        internal int Quantite;
        internal BitmapImage Image;

        internal static Dictionary<Objets, Objet> objets = new Dictionary<Objets, Objet>();
        internal static Random random = new();

        private static readonly double CHANCE_CAPTURE_VIANDE = 0.90;
        private static readonly double CHANCE_CAPTURE_GRAINE = 0.90;
        private static readonly int SOIN_HERBE_MEDICINALE = 40;

        public Objet(Objets typeObjet, string nom, string nomImage, int quantite = 1)
        {

            TypeObjet = typeObjet;
            Nom = nom;
            Quantite = quantite;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Objets/{nomImage}"));

        }

        /// <summary>
        /// Insert dans le dictionnaire les différents objets.
        /// </summary>
        internal static void InitObjets()
        {

            objets.Add(Objets.Morceau_de_viande, new(Objets.Morceau_de_viande, "Viande", "Morceau_de_viande.png"));
            objets.Add(Objets.Graine, new(Objets.Graine, "Graine", "Graines.png"));
            objets.Add(Objets.Herbe_Medicinale, new(Objets.Herbe_Medicinale, "Herbe médicinale", "Herbes_medicinale.png"));

        }

        /// <summary>
        /// Crée un objet en fonction de son identifiant et de sa quantité.
        /// </summary>
        /// <param name="typeObjet"></param>
        /// <param name="quantite"></param>
        /// <returns></returns>
        internal static Objet CreerObjet(Objets typeObjet, int quantite = 1)
        {

            Objet objet = (Objet) objets[typeObjet].Clone();
            objet.Quantite = quantite;

            return objet;
        }

        /// <summary>
        /// Ajoute un objet à la liste des objets possédés du joueur.
        /// </summary>
        /// <param name="objetsPossedes"></param>
        /// <param name="objetAjoute"></param>
        internal static void AjouterObjet(List<Objet> objetsPossedes, Objet objetAjoute)
        {

            bool aEteAjoute = false;

            foreach (Objet objet in objetsPossedes)
            {

                if (objet.TypeObjet == objetAjoute.TypeObjet)
                {

                    objet.Quantite += objetAjoute.Quantite;
                    aEteAjoute = true;
                    break;

                }

            }

            if (!aEteAjoute)
                objetsPossedes.Add(objetAjoute);

        }

        /// <summary>
        /// Ajoute plusieurs objets à la liste des objets possédés du joueur. 
        /// </summary>
        /// <param name="objetsPossedes"></param>
        /// <param name="objetsAjoute"></param>
        internal static void AjouterObjet(List<Objet> objetsPossedes, Objet[] objetsAjoute)
        {

            bool aEteAjoute = false;

            foreach(Objet objetAAjouter in objetsAjoute)
            {

                foreach (Objet objet in objetsPossedes)
                {

                    if (objet.TypeObjet == objetAAjouter.TypeObjet)
                    {

                        objet.Quantite += objetAAjouter.Quantite;
                        aEteAjoute = true;
                        break;

                    }

                    
                }

                if (!aEteAjoute)
                    objetsPossedes.Add(objetAAjouter);

            }
            

        }

        /// <summary>
        /// Lance un essai de capture d'un animal sauvage en fonction de sa chance de capture et de son alimentation.
        /// </summary>
        /// <param name="chanceCapture"></param>
        /// <param name="animalSauvage"></param>
        /// <param name="alimentationExclue"></param>
        /// <returns></returns>
        internal static bool Capture(double chanceCapture, Animal animalSauvage, params Alimentation[] alimentationExclue)
        {

            if (random.Next(0, (int) ((100 - CHANCE_CAPTURE_VIANDE * 100) * ((double) animalSauvage.PV / animalSauvage.PVMax))) == 0 && !alimentationExclue.Contains(animalSauvage.AlimentationAnimal))
            {

                MainWindow.animauxPossedes.Add(animalSauvage);
                Console.WriteLine("Animal capturé");
                return true;

            }

            return false;
        }

        /// <summary>
        /// Effectue une action sur un des animaux fourni en fonction de l'objet fourni.
        /// </summary>
        /// <param name="objet"></param>
        /// <param name="animalJoueur"></param>
        /// <param name="animalSauvage"></param>
        /// <returns></returns>
        internal static TypeAction? ActionObjet(Objet objet, Animal animalJoueur, Animal animalSauvage)
        {

            switch(objet.TypeObjet)
            {

                case Objets.Morceau_de_viande:
                    if(Capture(CHANCE_CAPTURE_VIANDE, animalSauvage, Alimentation.Herbivore))
                        return TypeAction.Capture;
                    break;

                case Objets.Graine:
                    if(Capture(CHANCE_CAPTURE_GRAINE, animalSauvage, Alimentation.Carnivore))
                        return TypeAction.Capture;
                    break;

                case Objets.Herbe_Medicinale:
                    if (animalJoueur.PV + SOIN_HERBE_MEDICINALE >= animalJoueur.PVMax)
                        animalJoueur.PV = animalJoueur.PVMax;
                    else
                        animalJoueur.PV += SOIN_HERBE_MEDICINALE;
                    return TypeAction.Soin;

            }

            return null;

        }
             
        /// <summary>
        /// Enlève un objet de l'inventaire du joueur en fonction de sa position dans la liste.
        /// </summary>
        /// <param name="objetUtilise"></param>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        internal static void UtiliserObjet(out Objet objetUtilise, byte index)
        {

            if(index < 0 || index > MainWindow.objetsPossedes.Count - 1) 
                throw new ArgumentOutOfRangeException("L'index fournit est plus grand que la liste d'objets");

            objetUtilise = MainWindow.objetsPossedes[index];
            MainWindow.objetsPossedes[index].Quantite--;

            if (MainWindow.objetsPossedes[index].Quantite <= 0)
                MainWindow.objetsPossedes.RemoveAt(index);

        }

        // Provient de l'interface ICloneable, permet de créer un clone d'un objet
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    // Identifiants des objets
    enum Objets
    {
        Morceau_de_viande,
        Graine,
        Herbe_Medicinale
    }

    // Identifiants des types d'actions
    enum TypeAction
    {
        Capture,
        Soin
    }
}
