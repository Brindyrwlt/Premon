using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Premon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly int PAS_DEPLACEMENT = 50;
        private static DispatcherTimer intervalleDeplacement;
        private static readonly int INTERVALLE_DEPLACEMENT = 500;
        private static double gauche;
        private static double haut;
        public MainWindow()
        {
            InitializeComponent();
            InitIntervalleDeplacement();
        }

        private void fenetre_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    gauche -= PAS_DEPLACEMENT;
                    break;
                case Key.Right:
                    gauche += PAS_DEPLACEMENT;
                    break;
                case Key.Up:
                    haut -= PAS_DEPLACEMENT;
                    break;
                case Key.Down:
                    haut += PAS_DEPLACEMENT;
                    break;
            }
        }

        private void InitIntervalleDeplacement()
        {
            gauche = Canvas.GetLeft(perso);
            haut = Canvas.GetTop(perso);
            intervalleDeplacement = new DispatcherTimer();
            intervalleDeplacement.Interval = TimeSpan.FromMilliseconds(INTERVALLE_DEPLACEMENT);
            intervalleDeplacement.Tick += DeplacerVers;
            intervalleDeplacement.Start();
        }

        private void DeplacerVers(object? sender, EventArgs e)
        {
            Canvas.SetLeft(perso, gauche);
            Canvas.SetTop(perso, haut);
        }
    }
}