using System.Windows;

namespace Premon
{
    /// <summary>
    /// Interaction logic for EcranAccueil.xaml
    /// </summary>
    public partial class EcranAccueil : Window
    {

        internal bool quitterJeu = false;

        public EcranAccueil()
        {
            InitializeComponent();
        }

        private void BoutonChargerPartie_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BoutonNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            Inventaire.SuppressionSauvegarde();
            DialogResult = true;
        }

        private void BoutonParametres_Click(object sender, RoutedEventArgs e)
        {
            EcranParametres ecranParametres = new();
            ecranParametres.ShowDialog();
        }

        private void BoutonQuitter_Click(object sender, RoutedEventArgs e)
        {

            quitterJeu = true;
            DialogResult = true;

        }
    }
}
