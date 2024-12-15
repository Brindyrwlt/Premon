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

        internal Rectangle[] cases;
        internal Label[] nomCases;
        internal Label[] quantiteCases;

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

        internal void AffichageInventaire(List<Objet> objetsPosedes)
        {

            for (int i = 0; i < cases.Length; i++)
            {

                if(i < objetsPosedes.Count)
                {

                    cases[i].Fill = new ImageBrush(objetsPosedes[i].Image);
                    nomCases[i].Content = objetsPosedes[i].Nom;
                    quantiteCases[i].Content = objetsPosedes[i].Quantite;

                } 
                else
                {

                    cases[i].Stroke = new SolidColorBrush(Colors.Transparent);
                    nomCases[i].Content = "";
                    quantiteCases[i].Content = "";

                }

            }

        }

    }
}
