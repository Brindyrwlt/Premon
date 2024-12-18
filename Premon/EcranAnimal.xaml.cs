using System.Windows;

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

        private void BoutonPromouvoir_Click(object sender, RoutedEventArgs e)
        {

            Animal animalEnTete;

            animalSelectionne = (Animal) ListeAnimal.SelectedItem;

            if (animalSelectionne != null)
            {

                animalEnTete = MainWindow.animauxPossedes[0];
                MainWindow.animauxPossedes[0] = animalSelectionne;
                MainWindow.animauxPossedes[ListeAnimal.SelectedIndex] = animalEnTete;
                DialogResult = true;

            }

            Inventaire.SauvegardeInventaire(MainWindow.animauxPossedes, MainWindow.objetsPossedes);

        }

        internal void EnCombat()
        {

            BoutonSelectionner.IsEnabled = true;

        }

        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
