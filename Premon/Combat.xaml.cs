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
        public Combat()
        {
            InitializeComponent();
        }

        internal void InitAnimaux(Animal animalJoueur)
        {

            ImageAnimalJoueur.Source = animalJoueur.Image;

        }

        private void BoutonFuite_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
