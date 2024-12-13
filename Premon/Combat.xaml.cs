using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

                animalJoueur.Attaque(ecranAttaque.attaqueChoisie, animalSauvage);
                ActualiserHP();

            }

        }
    }
}
