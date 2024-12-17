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
    /// Logique d'interaction pour InventaireObjet.xaml
    /// </summary>
    public partial class InventaireObjet : Window
    {

        private Rectangle[] cases;
        private Label[] nomCases;
        private Label[] quantiteCases;
        internal Objet objetClique;
        private List<Objet> objetsPossedes;
        internal bool enCombat;

        public InventaireObjet()
        {

            InitializeComponent();
            InitCases();
            InitNomCases();
            InitQuantiteCases();

        }

        private void InitCases()
            => cases = [ObjetInv1, ObjetInv2, ObjetInv3, ObjetInv4, ObjetInv5, ObjetInv6, ObjetInv7, ObjetInv8];

        private void InitNomCases()
            => nomCases = [LabObjet1, LabObjet2, LabObjet3, LabObjet4, LabObjet5, LabObjet6, LabObjet7, LabObjet8];

        private void InitQuantiteCases()
            => quantiteCases = [QuantiteObjet1, QuantiteObjet2, QuantiteObjet3, QuantiteObjet4, QuantiteObjet5, QuantiteObjet6, QuantiteObjet7, QuantiteObjet8];

        internal void AffichageInventaire(List<Objet> objetsPossedes)
        {

            this.objetsPossedes = objetsPossedes;

            for (int i = 0; i < cases.Length; i++)
            {

                if(i < objetsPossedes.Count)
                {

                    cases[i].Fill = new ImageBrush(objetsPossedes[i].Image);
                    nomCases[i].Content = objetsPossedes[i].Nom;
                    quantiteCases[i].Content = objetsPossedes[i].Quantite;

                } 
                else
                {

                    cases[i].Stroke = new SolidColorBrush(Colors.Transparent);
                    nomCases[i].Content = "";
                    quantiteCases[i].Content = "";

                }

            }

        }

        private void ObjetInv1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 0);
                DialogResult = true;

            }

        }

        private void ObjetInv2_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 1);
                DialogResult = true;

            }

        }

        private void ObjetInv3_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 2);
                DialogResult = true;

            }

        }

        private void ObjetInv4_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 3);
                DialogResult = true;

            }

        }

        private void ObjetInv5_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 4);
                DialogResult = true;

            }

        }

        private void ObjetInv6_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 5);
                DialogResult = true;

            }

        }

        private void ObjetInv7_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 6);
                DialogResult = true;

            }

        }

        private void ObjetInv8_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, 7);
                DialogResult = true;

            }

        }

        internal void EnCombat()
        {

            BoutonRetour.IsEnabled = false;

        }

        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
