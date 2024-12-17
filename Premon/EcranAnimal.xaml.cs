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

        public EcranAnimal()
        {

            InitializeComponent();

            ListeAnimal.ItemsSource = MainWindow.animauxPossedes;
            
        }

        private void BoutonSelectionner_Click(object sender, RoutedEventArgs e)
        {

            animalSelectionne = (Animal) ListeAnimal.SelectedItem;

            if(animalSelectionne != null )
                DialogResult = true;

        }
    }
}
