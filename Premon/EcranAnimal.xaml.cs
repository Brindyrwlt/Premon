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
    /// Interaction logic for EcranAnimal.xaml
    /// </summary>
    public partial class EcranAnimal : Window
    {

        internal Animal animalSelectionne;
        internal bool enCombat = false;

        public EcranAnimal()
        {

            InitializeComponent();
            ListeAnimal.ItemsSource = MainWindow.animauxPossedes;

            if (!enCombat)
                BoutonSelectionner.IsEnabled = false;
            
        }

        private void BoutonSelectionner_Click(object sender, RoutedEventArgs e)
        {

            animalSelectionne = (Animal) ListeAnimal.SelectedItem;

            if(animalSelectionne != null )
                DialogResult = true;

        }

        private void BoutonPromouvoir_Click(object sender, RoutedEventArgs e)
        {

            Animal animalEnTete;

            animalSelectionne = (Animal) ListeAnimal.SelectedItem;

            if (animalSelectionne != null)
            {

                animalEnTete = MainWindow.animauxPossedes[0];
                MainWindow.animauxPossedes[0] = animalSelectionne;
                MainWindow.animauxPossedes.Add(animalEnTete);
                DialogResult = true;

            }

        }
    }
}
