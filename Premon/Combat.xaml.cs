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

        Animal animalJoueur;
        Animal animalSauvage;
        Attaques attaqueChoisie;

        private static readonly double TEMPS_ATTAQUE_JOUEUR = 0.5;
        private static readonly double TEMPS_ATTAQUE_ENNEMI = 1.5;

        private static DispatcherTimer minuterieActionEnnemi;
        private static bool ennemiAttaque = false;
        Random random = new Random();

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

        private void ActionEnnemi(object? sender, EventArgs e)
        {

            if (ennemiAttaque)
            {

                Attaques attaqueChoisie = animalSauvage.Attaques[random.Next(0, animalSauvage.Attaques.Length)];

                animalSauvage.Attaque(attaqueChoisie, animalJoueur);
                TexteAction.Content = $"{animalSauvage.Nom} sauvage a utilisé {MainWindow.FormatageNomAttaque(attaqueChoisie)}.";
                minuterieActionEnnemi.Stop();
                ActualiserHP();
                ennemiAttaque = false;
                BoutonsActifs(true);

            }
            else
            {

                ennemiAttaque = true;
                animalJoueur.Attaque(attaqueChoisie, animalSauvage);
                ActualiserHP();
                TexteAction.Content = $"{animalJoueur.Nom} a utilisé {MainWindow.FormatageNomAttaque(attaqueChoisie)}.";
                minuterieActionEnnemi.Interval = TimeSpan.FromSeconds(TEMPS_ATTAQUE_ENNEMI);

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
            Console.WriteLine(animalJoueur.Attaques.ToString());
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
            ecranAttaque.attaques = animalJoueur.Attaques;
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
