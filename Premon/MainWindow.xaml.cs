using System.Printing;
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
        private static readonly int INTERVALLE_DEPLACEMENT = 100;
        private static bool? gauche = null;
        private static bool? haut = null;
        private static bool aAppuye = false;

        // 100 HP pour la base
        private Animal[] premons =
        {

            new Animal("Mammouth", 200, Attaques.COUP_DE_PIED),
            new Animal("Bouquetin", 80, Attaques.EMPALEMENT, Attaques.AIGUISAGE)

        };

        public MainWindow()
        {
            InitializeComponent();
            InitIntervalleDeplacement();
        }

        private void Jeu(object? sender, EventArgs e)
        {

            DeplacementPerso();

        }

        private void fenetre_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Left:
                    gauche = true;
                    break;
                case Key.Right:
                    gauche = false;
                    break;
                case Key.Up:
                    haut = true;
                    break;
                case Key.Down:
                    haut = false;
                    break;

            }

            if (aAppuye == false)
            {

                intervalleDeplacement.Stop();
                aAppuye = true;
                DeplacementPerso();
                intervalleDeplacement.Start();

            }
            
        }

        private void InitIntervalleDeplacement()
        {
            intervalleDeplacement = new DispatcherTimer();
            intervalleDeplacement.Interval = TimeSpan.FromMilliseconds(INTERVALLE_DEPLACEMENT);
            intervalleDeplacement.Tick += Jeu;
            intervalleDeplacement.Start();
        }

        private void fenetre_KeyUp(object sender, KeyEventArgs e)
        {
            gauche = null;
            haut = null;
            aAppuye = false;
        }

        private void DeplacementPerso()
        {

            double gauchePerso = Canvas.GetLeft(perso);
            double hautPerso = Canvas.GetTop(perso);

            if (gauche != null)
                gauchePerso += gauche == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;

            if (haut != null)
                hautPerso += haut == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;
#if DEBUG
            Console.WriteLine($"Gauche : {gauchePerso}\n" +
                $"Haut : {hautPerso}");
#endif
            if(gauchePerso >= 0 && gauchePerso < ActualWidth - perso.Width)
                Canvas.SetLeft(perso, gauchePerso);

            if (hautPerso >= 0 && hautPerso < ActualHeight - perso.Height)
                Canvas.SetTop(perso, hautPerso);

            
        }
    }
}