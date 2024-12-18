using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Premon
{
    /// <summary>
    /// Logique d'interaction pour InventaireObjet.xaml
    /// </summary>
    public partial class InventaireObjet : Window
    {

        internal Rectangle[] cases;
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

        /// <summary>
        /// Initialise les cases de la fenêtre dans un tableau.
        /// </summary>
        private void InitCases()
            => cases = [ObjetInv1, ObjetInv2, ObjetInv3, ObjetInv4, ObjetInv5, ObjetInv6, ObjetInv7, ObjetInv8];

        /// <summary>
        /// Initialise les textes des cases de la fenêtre dans un tableau
        /// </summary>
        private void InitNomCases()
            => nomCases = [LabObjet1, LabObjet2, LabObjet3, LabObjet4, LabObjet5, LabObjet6, LabObjet7, LabObjet8];

        /// <summary>
        /// Initialise les chiffres des quantités d'objets dans un tableau
        /// </summary>
        private void InitQuantiteCases()
            => quantiteCases = [QuantiteObjet1, QuantiteObjet2, QuantiteObjet3, QuantiteObjet4, QuantiteObjet5, QuantiteObjet6, QuantiteObjet7, QuantiteObjet8];

        /// <summary>
        /// Affiche les objets de la liste dans les cases de la fenêtre.
        /// </summary>
        /// <param name="objetsPossedes"></param>
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

        /// <summary>
        /// Active la possibilité de cliquer sur les objets pour les utiliser en combat.
        /// </summary>
        internal void EnCombat()
            => enCombat = true;

        /// <summary>
        /// Utilise un objet à l'indice donné si la fenêtre est invoquée en combat.
        /// </summary>
        /// <param name="indice"></param>
        internal void InteractionObjet(byte indice)
        {

            if (enCombat)
            {

                Objet.UtiliserObjet(out objetClique, indice);
                DialogResult = true;

            }

        }

        // Interaction avec l'objet en fonction de la case appuyée
        private void ObjetInv1_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(0);

        private void ObjetInv2_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(1);

        private void ObjetInv3_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(2);

        private void ObjetInv4_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(3);

        private void ObjetInv5_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(4);

        private void ObjetInv6_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(5);

        private void ObjetInv7_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(6);

        private void ObjetInv8_MouseDown(object sender, MouseButtonEventArgs e)
            => InteractionObjet(7);

        // Ferme la fenêtre
        private void BoutonRetour_Click(object sender, RoutedEventArgs e)
            => DialogResult = false;
        
    }
}
