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
    /// Interaction logic for EcranParametres.xaml
    /// </summary>
    public partial class EcranParametres : Window
    {
        public EcranParametres()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            MainWindow.volume = GlissiereVolume.Value;

        }

        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
