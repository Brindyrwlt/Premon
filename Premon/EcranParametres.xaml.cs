using System.Windows;

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
