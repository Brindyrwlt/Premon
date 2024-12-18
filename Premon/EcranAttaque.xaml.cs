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

        internal Attaques[] attaques;
        internal Attaques attaqueChoisie;
        internal Dictionary<Button, int> boutons = new Dictionary<Button, int>();

        public EcranAttaque()
        {

            InitializeComponent();

        }

        internal void InitBoutons()
        {

            boutons[BoutonAttaque1] = 0;
            boutons[BoutonAttaque2] = 1;
            boutons[BoutonAttaque3] = 2;
            boutons[BoutonAttaque4] = 3;
            
            foreach(KeyValuePair<Button, int> bouton in boutons)
            {

                if(bouton.Value < attaques.Length)
                {

                    bouton.Key.IsEnabled = true;
                    bouton.Key.Content = MainWindow.FormatageNomAttaque(attaques[bouton.Value]);

                } else
                {

                    bouton.Key.Visibility = Visibility.Hidden;

                }

            }

        }

        private void ChangeDescription(int nombreBouton = 0)
        {

            if(nombreBouton != 0)
            {

                Attaques attaque = attaques[nombreBouton - 1];
                NomAttaque.Content = MainWindow.FormatageNomAttaque(attaque);
                DescriptionAttaque.Text = Animal.descriptionsAttaques[attaque];

            } else
            {

                NomAttaque.Content = "Sélectionnez une attaque";
                DescriptionAttaque.Text = "";

            }
            

        }

        private void Attaque(object button)
        {

            attaqueChoisie = attaques[boutons[(Button) button]];
            DialogResult = true;

        }

        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BoutonAttaque1_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(1);

        private void BoutonAttaque1_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

        private void BoutonAttaque2_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(2);

        private void BoutonAttaque2_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

        private void BoutonAttaque3_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(3);

        private void BoutonAttaque3_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

        private void BoutonAttaque4_MouseEnter(object sender, MouseEventArgs e)
            => ChangeDescription(4);

        private void BoutonAttaque4_MouseLeave(object sender, MouseEventArgs e)
            => ChangeDescription();

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
