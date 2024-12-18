using System.Windows;

namespace Premon
{
    /// <summary>
    /// Logique d'interaction pour EcranAnimal.xaml
    /// </summary>
    public partial class EcranAnimal : Window
    {

        internal Animal animalSelectionne;

        public EcranAnimal()
        {

            InitializeComponent();

            // Assignation des animaux possédés à la liste
            ListeAnimal.ItemsSource = MainWindow.animauxPossedes;
            
        }

        /// <summary>
        /// Active le bouton de sélection d'animal.
        /// </summary>
        internal void EnCombat()
            => BoutonSelectionner.IsEnabled = true;

        /// <summary>
        /// Récupération l'animal sélectionné
        /// </summary>
        private void RecuperationAnimalSelectionne()
            => animalSelectionne = (Animal) ListeAnimal.SelectedItem;

        private void BoutonSelectionner_Click(object sender, RoutedEventArgs e)
        {

            RecuperationAnimalSelectionne();

            if (animalSelectionne != null)
                DialogResult = true;

        }

        private void BoutonPromouvoir_Click(object sender, RoutedEventArgs e)
        {

            // Animal en première position dans l'équipe
            Animal animalEnTete;

            RecuperationAnimalSelectionne();

            // Echange entre l'animal en tête d'équipe et l'animal sélectionné
            if (animalSelectionne != null)
            {

                animalEnTete = MainWindow.animauxPossedes[0];
                MainWindow.animauxPossedes[0] = animalSelectionne;
                MainWindow.animauxPossedes[ListeAnimal.SelectedIndex] = animalEnTete;
                DialogResult = true;

            }

            Inventaire.SauvegardeInventaire(MainWindow.animauxPossedes, MainWindow.objetsPossedes);

        }

        // Retour en arrière
        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
