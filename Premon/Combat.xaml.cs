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
        private Attaques? attaqueChoisie;

        private static readonly double TEMPS_ATTAQUE_JOUEUR = 0.5,
                                       TEMPS_ATTAQUE_ENNEMI = 1.5;

        private static DispatcherTimer minuterieActionEnnemi;
        private static bool ennemiAttaque = false;
        private Random random = new Random();

        /*
         * 
         * Etats de la variable
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
        internal int indexAnimalSelectionne = 0;

        public Combat()
        {

            InitializeComponent();
            InitTimer();
            
        }

        private void InitTimer()
        {

            minuterieActionEnnemi = new DispatcherTimer();
            //minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(1);
            minuterieActionEnnemi.Tick += Action;

        }

        private void BoutonsActifs(bool actifs)
        {

            BoutonAttaque.IsEnabled = actifs;
            BoutonFuite.IsEnabled = actifs;
            BoutonObjets.IsEnabled = actifs;

        } 

        private byte CombatFini()
        {

            if (animalSauvage.PV <= 0 && animalJoueur.PV <= 0)
                return 3;
            if (animalJoueur.PV <= 0)
                return 2;
            else if (animalSauvage.PV <= 0)
                return 1;
            else
                return 0;

        }
        
        private void Action(object? sender, EventArgs e)
        {

            if (ennemiAttaque && etatCombat == 0)
            {

                Attaques attaqueChoisie = animalSauvage.AttaquesAnimal[random.Next(0, animalSauvage.AttaquesAnimal.Length)];

                animalSauvage.Attaque(attaqueChoisie, animalJoueur);
                TexteAction.Content = $"{animalSauvage.Nom} sauvage a utilisé {MainWindow.FormatageNomAttaque(attaqueChoisie)}.";
                etatCombat = CombatFini();

                if (etatCombat == 0)
                {

                    ennemiAttaque = false;
                    BoutonsActifs(true);
                    minuterieActionEnnemi.Stop();

                }
                else
                    animalJoueur.PV = 0;

                ActualiserHP();
                Inventaire.SauvegardeInventaire(MainWindow.animauxPossedes, MainWindow.objetsPossedes);

            }
            else if (etatCombat == 0)
            {

                if(attaqueChoisie != null)
                {

                    animalJoueur.Attaque((Attaques) attaqueChoisie, animalSauvage);
                    TexteAction.Content = $"{animalJoueur.Nom} a utilisé {MainWindow.FormatageNomAttaque((Attaques) attaqueChoisie)}.";

                }                   

                ennemiAttaque = true;
                minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(TEMPS_ATTAQUE_ENNEMI);
                etatCombat = CombatFini();

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
                ActualiserHP();

            }
            else
            {

                switch (etatCombat)
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

                    default:
                        minuterieActionEnnemi.Stop();
                        DialogResult = true;
                        break;
                }

            }

        }

        internal void InitAnimaux(Animal animalJoueur, Animal animalSauvage)
        {

            this.animalJoueur = animalJoueur;
            this.animalSauvage = animalSauvage;
            ImageAnimalJoueur.Source = animalJoueur.Image;
            ImageAnimalSauvage.Source = animalSauvage.Image;
            ActualiserHP();

#if DEBUG
            Console.WriteLine(animalJoueur.AttaquesAnimal.ToString());
#endif
        }

        /*private void Attaque()*/

        private void ActualiserHP()
        {

            PVJoueur.Content = $"PV : {animalJoueur.PV}";
            PVEnnemi.Content = $"PV : {animalSauvage.PV}";

        }

        private void BoutonObjets_Click(object sender, RoutedEventArgs e)
        {

            InventaireObjet inventaireObjet = new();
            inventaireObjet.AffichageInventaire(MainWindow.objetsPossedes);
            for(int i = 0; i < MainWindow.objetsPossedes.Count; i++)
            {

                Objets typeObjet = MainWindow.objetsPossedes[i].TypeObjet;
                Rectangle rectangle = inventaireObjet.cases[i];

                switch (animalSauvage.AlimentationAnimal)
                {

                    case Alimentation.Carnivore:
                        if (typeObjet == Objets.Morceau_de_viande)
                            rectangle.Stroke = new SolidColorBrush(Colors.Gold);
                        break;

                    case Alimentation.Herbivore:
                        if (typeObjet == Objets.Graine)
                            rectangle.Stroke = new SolidColorBrush(Colors.Gold);
                        break;

                    case Alimentation.Omnivore:
                        if (typeObjet == Objets.Morceau_de_viande || typeObjet == Objets.Graine)
                            rectangle.Stroke = new SolidColorBrush(Colors.Gold);
                        break;

                }

            }

            inventaireObjet.EnCombat();
            inventaireObjet.ShowDialog();

            if (inventaireObjet.DialogResult == true)
            {

                Objet objetClique = inventaireObjet.objetClique;
                TypeAction? action = Objet.ActionObjet(objetClique, animalJoueur, animalSauvage);


                if (action == TypeAction.Capture)
                {

                    TexteAction.Content = $"{animalSauvage.Nom} a été capturé !";
                    etatCombat = 10;

                } else if(action == TypeAction.Soin)
                {

                    TexteAction.Content = $"{animalJoueur.Nom} a été soigné !";

                } else
                {

                    TexteAction.Content = $"La capture n'a pas fonctionné...";

                }

                DeclencherAttaqueJoueur(null);

            }
        }

        private void BoutonFuite_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;

        private void BoutonAnimaux_Click(object sender, RoutedEventArgs e)
        {

            EcranAnimal ecranAnimal = new();
            ecranAnimal.EnCombat();
            ecranAnimal.ShowDialog();

            if(ecranAnimal.DialogResult == true)
            {

                InitAnimaux(ecranAnimal.animalSelectionne, animalSauvage);
                TexteAction.Content = $"{animalJoueur.Nom} est arrivé sur le terrain !";
                indexAnimalSelectionne = ecranAnimal.ListeAnimal.SelectedIndex;
                DeclencherAttaqueJoueur(null);

            }

        }

        private void BoutonAttaque_Click(object sender, RoutedEventArgs e)
        {
            EcranAttaque ecranAttaque = new();
            ecranAttaque.attaques = animalJoueur.AttaquesAnimal;
            ecranAttaque.InitBoutons();
            ecranAttaque.ShowDialog();

            if(ecranAttaque.DialogResult == true)
            {

                attaqueChoisie = ecranAttaque.attaqueChoisie;
                DeclencherAttaqueJoueur(attaqueChoisie);

            }

        }

        private void DeclencherAttaqueJoueur(Attaques? attaque)
        {

            attaqueChoisie = attaque;
            minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(TEMPS_ATTAQUE_JOUEUR);
            minuterieActionEnnemi.Start();
            BoutonsActifs(false);

        }
    }
}
