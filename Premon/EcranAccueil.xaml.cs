﻿using System;
using System.Collections.Generic;
using System.IO;
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
            File.Delete("inventaire.json");
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
