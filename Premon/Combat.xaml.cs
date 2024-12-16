using System;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;

namespace Premon
{
    /// <summary>
    /// Interaction logic for Combat.xaml
    /// </summary>
    public partial class Combat : Window
    {

        Animal animalJoueur, animalSauvage;
        Attaques attaqueChoisie;

        private static readonly double TEMPS_ATTAQUE_JOUEUR = 0.5,
                                       TEMPS_ATTAQUE_ENNEMI = 1.5;

        private static DispatcherTimer minuterieActionEnnemi;
        private static bool ennemiAttaque = false;
        Random random = new Random();
        internal int combatFini = 0;

        public Combat()
        {

            InitializeComponent();
            InitTimer();
            
        }

        private void InitTimer()
        {

            minuterieActionEnnemi = new DispatcherTimer();
            //minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(1);
            minuterieActionEnnemi.Tick += ActionEnnemi;

        }

        private void BoutonsActifs(bool actifs)
        {

            BoutonAttaque.IsEnabled = actifs;
            BoutonFuite.IsEnabled = actifs;
            BoutonObjets.IsEnabled = actifs;

        } 

        private int CombatFini()
        {

            if (animalSauvage.HP <= 0 && animalJoueur.HP <= 0)
                return 3;
            if (animalJoueur.HP <= 0)
                return 2;
            else if (animalSauvage.HP <= 0)
                return 1;
            else
                return 0;

        }
        
        private void ActionEnnemi(object? sender, EventArgs e)
        {

            if (ennemiAttaque && combatFini == 0)
            {

                Attaques attaqueChoisie = animalSauvage.AttaquesAnimal[random.Next(0, animalSauvage.AttaquesAnimal.Length)];

                animalSauvage.Attaque(attaqueChoisie, animalJoueur);
                TexteAction.Content = $"{animalSauvage.Nom} sauvage a utilisé {MainWindow.FormatageNomAttaque(attaqueChoisie)}.";
                combatFini = CombatFini();
                ActualiserHP();

                if (combatFini == 0)
                {

                    ennemiAttaque = false;
                    BoutonsActifs(true);
                    minuterieActionEnnemi.Stop();

                }

            }
            else if (combatFini == 0)
            {

                animalJoueur.Attaque(attaqueChoisie, animalSauvage);
                ActualiserHP();
                ennemiAttaque = true;
                TexteAction.Content = $"{animalJoueur.Nom} a utilisé {MainWindow.FormatageNomAttaque(attaqueChoisie)}.";
                minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(TEMPS_ATTAQUE_ENNEMI);
                combatFini = CombatFini();

            }
            else
            {

                switch (combatFini)
                {

                    case 1:
                        animalSauvage.HP = 0;
                        TexteAction.Content = $"{animalJoueur.Nom} a gagné !";
                        combatFini = 4;
                        break;

                    case 2:
                        animalJoueur.HP = 0;
                        TexteAction.Content = $"{animalSauvage.Nom} a gagné !";
                        combatFini = 5;
                        break;

                    case 3:
                        animalJoueur.HP = 0;
                        animalSauvage.HP = 0;
                        TexteAction.Content = "Les deux animaux sont morts !";
                        combatFini = 6;
                        break;

                    default:
                        minuterieActionEnnemi.Stop();
                        DialogResult = true;
                        break;
                }

                ActualiserHP();
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

            HPJoueur.Content = $"HP : {animalJoueur.HP}";
            HPEnnemi.Content = $"HP : {animalSauvage.HP}";

        }

        private void BoutonFuite_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
        
        private void BoutonAttaque_Click(object sender, RoutedEventArgs e)
        {
            EcranAttaque ecranAttaque = new EcranAttaque();
            ecranAttaque.attaques = animalJoueur.AttaquesAnimal;
            ecranAttaque.InitBoutons();
            ecranAttaque.ShowDialog();

            if(ecranAttaque.DialogResult == true)
            {

                attaqueChoisie = ecranAttaque.attaqueChoisie;
                minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(TEMPS_ATTAQUE_JOUEUR);
                minuterieActionEnnemi.Start();
                BoutonsActifs(false);

            }

        }
    }
}
