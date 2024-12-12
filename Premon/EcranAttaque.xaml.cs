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
    /// Interaction logic for EcranAttaque.xaml
    /// </summary>
    public partial class EcranAttaque : Window
    {

        internal Attaques[] attaques;
        internal Dictionary<Button, int> boutons = new Dictionary<Button, int>();

        public EcranAttaque()
        {

            InitializeComponent();
            InitBoutons();

        }
        
        private void InitBoutons()
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
                    bouton.Key.Content = attaques[bouton.Value];

                }

            }

        }

        private void ChangeDescription(int nombreBouton = 0)
        {

            
            Attaques attaque = attaques[nombreBouton];
            NomAttaque.Content = nombreBouton != 0 ? attaque.ToString() : "Sélectionnez une attaque";
            DescriptionAttaque.Text = nombreBouton != 0 ? Animal.descriptionsAttaques[attaque] : "";

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
    }
}
