using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Premon
{
    /// <summary>
    /// Interaction logic for EcranAttaque.xaml
    /// </summary>
    public partial class EcranAttaque : Window
    {

        // Attaques de l'animal du joueur
        internal Attaques[] attaques;

        internal Attaques attaqueChoisie;

        // Boutons d'attaques et leur ordre
        // Utilisation d'un dictionnaire pour pouvoir récupérer les attaques et les descriptions plus simplement avec l'ordre du bouton
        internal Dictionary<Button, int> boutons = new Dictionary<Button, int>();

        public EcranAttaque()
        {
            InitializeComponent();
        }

        internal void InitBoutons()
        {

            // Assignation des différents boutons dans le dictionnaire
            boutons[BoutonAttaque1] = 0;
            boutons[BoutonAttaque2] = 1;
            boutons[BoutonAttaque3] = 2;
            boutons[BoutonAttaque4] = 3;
            
            // Pour chaque bouton et son ordre dans le dictionnaire
            foreach(KeyValuePair<Button, int> bouton in boutons)
            {

                // Assignation des attaques à chaque bouton
                if(bouton.Value < attaques.Length)
                {

                    bouton.Key.IsEnabled = true;
                    bouton.Key.Content = MainWindow.FormatageNomAttaque(attaques[bouton.Value]);

                } else // Quand il n'y a plus d'attaques à afficher, désactivation des boutons
                {

                    bouton.Key.Visibility = Visibility.Hidden;

                }

            }

        }

        /// <summary>
        /// Assigne l'attaque effectuée et ferme l'écran
        /// </summary>
        /// <param name="button"></param>
        private void Attaque(object button)
        {

            attaqueChoisie = attaques[boutons[(Button) button]];
            DialogResult = true;

        }

        /// <summary>
        /// Change la description d'attaques en fonction du numéro de l'attaque
        /// </summary>
        /// <param name="nombreBouton"></param>
        private void ChangeDescription(int nombreBouton = 0)
        {

            if(nombreBouton != 0) // Affichage de la description
            {

                Attaques attaque = attaques[nombreBouton - 1];
                NomAttaque.Content = MainWindow.FormatageNomAttaque(attaque);
                DescriptionAttaque.Text = Animal.descriptionsAttaques[attaque];

            } else // Quand la souris n'est pas sur un bouton
            {

                NomAttaque.Content = "Sélectionnez une attaque";
                DescriptionAttaque.Text = "";

            }
            
        }

        // Ferme le menu d'attaque
        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        // Change la description pour celle de la première attaque quand la souris survole le bouton d'attaque
        private void BoutonAttaque1_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(1);
        private void BoutonAttaque1_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

        // Change la description pour celle de la deuxième attaque quand la souris survole le bouton d'attaque
        private void BoutonAttaque2_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(2);
        private void BoutonAttaque2_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

        // Change la description pour celle de la troisième attaque quand la souris survole le bouton d'attaque
        private void BoutonAttaque3_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(3);
        private void BoutonAttaque3_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

        // Change la description pour celle de la quatrième attaque quand la souris survole le bouton d'attaque
        private void BoutonAttaque4_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(4);
        private void BoutonAttaque4_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

        // Attaque selon le bouton cliqué
        private void BoutonAttaque1_Click(object sender, RoutedEventArgs e)
            => Attaque(sender);
        private void BoutonAttaque2_Click(object sender, RoutedEventArgs e)
            => Attaque(sender);
        private void BoutonAttaque3_Click(object sender, RoutedEventArgs e)
            => Attaque(sender);
        private void BoutonAttaque4_Click(object sender, RoutedEventArgs e)
            => Attaque(sender);
    }
}
