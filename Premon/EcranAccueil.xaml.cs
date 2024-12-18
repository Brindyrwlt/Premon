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

        // Charger la partie est l'action par défaut
        private void BoutonChargerPartie_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        // Suppression de la sauvegarde en cas de nouvelle partie
        private void BoutonNouvellePartie_Click(object sender, RoutedEventArgs e)
        {
            Inventaire.SuppressionSauvegarde();
            DialogResult = true;
        }

        // Accéder aux paramètres
        private void BoutonParametres_Click(object sender, RoutedEventArgs e)
        {
            EcranParametres ecranParametres = new();
            ecranParametres.ShowDialog();
        }

        // Quitter le jeu
        private void BoutonQuitter_Click(object sender, RoutedEventArgs e)
        {

            quitterJeu = true;
            DialogResult = true;

        }
    }
}
