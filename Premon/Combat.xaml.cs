using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Premon
{
    /// <summary>
    /// Interaction logic for Combat.xaml
    /// </summary>
    public partial class Combat : Window
    {

        private Animal animalJoueur, animalSauvage;

        // Attaque choisie par le joueur
        private Attaques? attaqueChoisie;

        // Délai d'action du joueur et de l'ennemi
        private static readonly double TEMPS_ATTAQUE_JOUEUR = 0.5,
                                       TEMPS_ATTAQUE_ENNEMI = 1.5;

        // Variable système permettant de savoir si le tour est à l'ennemi
        private static bool ennemiAttaque = false;

        // Minuterie permettant de rajouter un délai entre les actions
        private static DispatcherTimer minuterieAction;

        private Random random = new Random();

        /*
         * 
         * Etats de la variable "etatCombat"
         * 
         * Dans Combat.xaml.cs
         * 0 : Partie en cours
         * 1 : Si le joueur a gagné la partie
         * 2 : Si l'ennemi a gagné la partie
         * 3 : Si les deux ont perdu la partie
         * 10 : Animal adverse capturé
         * 
         * Dans MainWindow.xaml.cs
         * 4 : Récupère le butin laché par l'ennemi
         * 5 : Retire l'animal du joueur utilisé
         * 6 : Fais les deux actions ci-dessus
         * 
         */
        internal byte etatCombat = 0;

        // Index dans le tableau animalPossedes de l'animal sur le terrain
        internal int indexAnimalSelectionne = 0;

        public Combat()
        {

            InitializeComponent();
            InitTimer();
            
        }

        private void InitTimer()
        {

            minuterieAction = new DispatcherTimer();
            //minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(1);
            minuterieAction.Tick += Action;

        }

        internal void InitAnimaux(Animal animalJoueur, Animal animalSauvage)
        {

            this.animalJoueur = animalJoueur;
            this.animalSauvage = animalSauvage;

            // Affiche de l'image des animaux actifs
            ImageAnimalJoueur.Source = animalJoueur.Image;
            ImageAnimalSauvage.Source = animalSauvage.Image;

            ActualiserPV();

#if DEBUG
            Console.WriteLine(animalJoueur.AttaquesAnimal.ToString());
#endif
        }


        // Permet de désactiver les boutons tant que le tour n'est pas au joueur
        private void BoutonsActifs(bool actifs)
        {

            BoutonAttaque.IsEnabled = actifs;
            BoutonFuite.IsEnabled = actifs;
            BoutonObjets.IsEnabled = actifs;

        } 

        // Défini l'état du combat en fonction des PV des animaux sur le terrain
        private byte CombatFini()
        {

            if (animalSauvage.PV <= 0 && animalJoueur.PV <= 0) // Si match nul
                return 3;
            if (animalJoueur.PV <= 0) // Si joueur perdu
                return 2;
            else if (animalSauvage.PV <= 0) // Si joueur gagné
                return 1;
            else
                return 0; // Combat toujours pas fini

        }
        
        // Méthode invoquée pour chaque action du joueur ou de l'ennemi, dans la minuterie
        private void Action(object? sender, EventArgs e)
        {

            if (ennemiAttaque && etatCombat == 0) // Tour de l'ennemi
            {

                Attaques attaqueChoisie = animalSauvage.AttaquesAnimal[random.Next(0, animalSauvage.AttaquesAnimal.Length)];

                animalSauvage.Attaque(attaqueChoisie, animalJoueur);
                TexteAction.Content = $"{animalSauvage.Nom} sauvage a utilisé {MainWindow.FormatageNomAttaque(attaqueChoisie)}.";
                
                // Vérification de si le combat est fini ou non
                etatCombat = CombatFini();

                if (etatCombat == 0) // Si le combat n'est toujours pas fini, on continue le combat
                {

                    ennemiAttaque = false;
                    BoutonsActifs(true);
                    minuterieAction.Stop();

                }
                else // Si combat perdu on met à 0 les PV du joueur (pour ne pas dépasser 0)
                    animalJoueur.PV = 0;

                ActualiserPV();
                Inventaire.SauvegardeInventaire(MainWindow.animauxPossedes, MainWindow.objetsPossedes);

            }
            else if (etatCombat == 0) // Tour du joueur
            {

                if(attaqueChoisie != null)
                {

                    animalJoueur.Attaque((Attaques) attaqueChoisie, animalSauvage);
                    TexteAction.Content = $"{animalJoueur.Nom} a utilisé {MainWindow.FormatageNomAttaque((Attaques) attaqueChoisie)}.";

                }                   

                ennemiAttaque = true;

                // Changement d'intervale de la minuterie pour le tour de l'ennemi
                minuterieAction.Interval = TimeSpan.FromSeconds(TEMPS_ATTAQUE_ENNEMI);

                // Vérification de si le combat est fini ou non
                etatCombat = CombatFini();

                // Changement des PV en fonction de l'état du combat
                switch(etatCombat)
                {

                    case 1:
                        animalSauvage.PV = 0;
                        break;

                    case 2:
                        animalJoueur.PV = 0;
                        break;

                    case 3:
                        animalJoueur.PV = 0;
                        animalSauvage.PV = 0;
                        break;

                }
                ActualiserPV();

            }
            else // Combat fini
            {

                switch (etatCombat) // Affichage en fonction de l'état du combat et changement d'état
                {

                    case 1:
                        TexteAction.Content = $"{animalJoueur.Nom} a gagné !";
                        etatCombat = 4;
                        break;

                    case 2:
                        TexteAction.Content = $"{animalSauvage.Nom} a gagné !";
                        etatCombat = 5;
                        break;

                    case 3:
                        TexteAction.Content = "Les deux animaux sont morts !";
                        etatCombat = 6;
                        break;

                    default: // Quand l'état du combat est supérieur à 3, fin du combat
                        minuterieAction.Stop();
                        DialogResult = true;
                        break;
                }

            }

        }

        private void ActualiserPV()
        {

            PVJoueur.Content = $"PV : {animalJoueur.PV}";
            PVEnnemi.Content = $"PV : {animalSauvage.PV}";

        }

        // Si l'attaque est null, passage du tour du joueur (pour les objets et le changement d'animal actif)
        private void DeclencherAttaqueJoueur(Attaques? attaque)
        {

            attaqueChoisie = attaque;
            minuterieAction.Interval = TimeSpan.FromSeconds(TEMPS_ATTAQUE_JOUEUR);
            minuterieAction.Start();
            BoutonsActifs(false);

        }

        private void BoutonObjets_Click(object sender, RoutedEventArgs e)
        {

            // Ouverture de l'inventaire du joueur
            InventaireObjet inventaireObjet = new();
            inventaireObjet.AffichageInventaire(MainWindow.objetsPossedes);

            // Changement de couleurs des bordures de la nourriture en fonction de l'alimentation de l'animal sauvage
            for(int i = 0; i < MainWindow.objetsPossedes.Count; i++)
            {

                Objets typeObjet = MainWindow.objetsPossedes[i].TypeObjet;
                Rectangle rectangle = inventaireObjet.cases[i];

                switch (animalSauvage.AlimentationAnimal)
                {

                    // Si l'animal sauvage est carnivore, changement en doré de la bordure de l'icône de la viande
                    case Alimentation.Carnivore:
                        if (typeObjet == Objets.Morceau_de_viande)
                            rectangle.Stroke = new SolidColorBrush(Colors.Gold);
                        break;

                    // Si l'animal sauvage est herbivore, changement en doré de la bordure de l'icône des graines
                    case Alimentation.Herbivore:
                        if (typeObjet == Objets.Graine)
                            rectangle.Stroke = new SolidColorBrush(Colors.Gold);
                        break;

                    // Si l'animal sauvage est herbivore, changement en doré de la bordure de l'icône des graines et de la viande
                    case Alimentation.Omnivore:
                        if (typeObjet == Objets.Morceau_de_viande || typeObjet == Objets.Graine)
                            rectangle.Stroke = new SolidColorBrush(Colors.Gold);
                        break;

                }

            }

            inventaireObjet.EnCombat();
            inventaireObjet.ShowDialog();

            if (inventaireObjet.DialogResult == true) // Effet de l'objet utilisé
            {

                Objet objetClique = inventaireObjet.objetClique;
                TypeAction? action = Objet.ActionObjet(objetClique, animalJoueur, animalSauvage);

                if (action == TypeAction.Capture) // Si l'animal sauvage est capturé
                {

                    TexteAction.Content = $"{animalSauvage.Nom} a été capturé !";
                    etatCombat = 10;

                } else if(action == TypeAction.Soin) // Si le joueur s'est soigné
                {

                    TexteAction.Content = $"{animalJoueur.Nom} a été soigné !";

                } else // Si la capture a écouché
                {

                    TexteAction.Content = $"La capture n'a pas fonctionné...";

                }

                // Passage du tour du joueur
                DeclencherAttaqueJoueur(null);

            }
        }

        // Fuite du combat
        private void BoutonFuite_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;

        private void BoutonAnimaux_Click(object sender, RoutedEventArgs e)
        {

            // Affichage des animaux du joueur
            EcranAnimal ecranAnimal = new();
            ecranAnimal.EnCombat();
            ecranAnimal.ShowDialog();

            if(ecranAnimal.DialogResult == true)
            {

                // Changement des animaux en fonction du nouvel animal actif
                InitAnimaux(ecranAnimal.animalSelectionne, animalSauvage);
                TexteAction.Content = $"{animalJoueur.Nom} est arrivé sur le terrain !";
                indexAnimalSelectionne = ecranAnimal.ListeAnimal.SelectedIndex;

                // Passage du tour du joueur
                DeclencherAttaqueJoueur(null);

            }

        }

        private void BoutonAttaque_Click(object sender, RoutedEventArgs e)
        {

            // Affichage de l'écran des attaques
            EcranAttaque ecranAttaque = new();

            // Assignation des attaques de l'animal du joueur aux attaques de l'écran et affichage des boutons
            ecranAttaque.attaques = animalJoueur.AttaquesAnimal;
            ecranAttaque.InitBoutons();
            
            ecranAttaque.ShowDialog();

            if(ecranAttaque.DialogResult == true)
            {

                attaqueChoisie = ecranAttaque.attaqueChoisie;
                DeclencherAttaqueJoueur(attaqueChoisie);

            }

        }

    }
}
