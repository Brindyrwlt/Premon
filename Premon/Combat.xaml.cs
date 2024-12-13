using System.Globalization;
using System.Windows;

namespace Premon
{
    /// <summary>
    /// Interaction logic for Combat.xaml
    /// </summary>
    public partial class Combat : Window
    {

        Animal animalJoueur;
        Animal animalSauvage;

        public Combat()
        {
            InitializeComponent();
            
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

                Attaques attaqueChoisie = ecranAttaque.attaqueChoisie;

                animalJoueur.Attaque(attaqueChoisie, animalSauvage);
                ActualiserHP();
                TexteAction.Content = $"{animalJoueur.Nom} a utilisé {MainWindow.FormatageNomAttaque(attaqueChoisie)}.";

            }

        }
    }
}
