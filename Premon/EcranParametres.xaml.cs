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

        // Change le volume des musiques en fonction de la valeur de la glissière
        private void GlissiereVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            MainWindow.volume = GlissiereVolume.Value;

        }

        // Ferme la fenêtre des paramètres
        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
