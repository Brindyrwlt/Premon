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

        // Paramètres ---------------------------------------

        // Mouvement
        private static readonly int PAS_DEPLACEMENT = 50;
        private static readonly int INTERVALLE_DEPLACEMENT = 200;

        // Probabilité
        private static readonly double POURCENTAGE_RENCONTRE_BUISSON = 0.2;

        // Variables système  --------------------------------

        // Mouvement
        private static DispatcherTimer intervalleDeplacement;
        private static bool? gauche = null;
        private static bool? haut = null;
        private static bool aAppuye = false;

        // Inventaire
        private static List<Animal> animauxPossedes = new List<Animal>();

        // Autre
        private Random random = new Random();
        /*private Animal[] animaux = // 100 HP pour la base
        {

            new Animal("Mammouth", 200, "Mammouth.png", Attaques.COUP_DE_PIED),
            //new Animal("Bouquetin", 80, Attaques.EMPALEMENT, Attaques.AIGUISAGE)

        };*/

        private Dictionary<Animaux, Animal> animaux = new Dictionary<Animaux, Animal>();

        private List<Rectangle> buissons = new List<Rectangle>();
        private List<Rectangle> obstacles = new List<Rectangle>();

        public MainWindow()
        {
            InitializeComponent();
            InitIntervalleDeplacement();
            InitBuissons();
            InitAnimaux();

            animauxPossedes.Add(animaux[Animaux.Mammouth]);
        }

        private void InitBuissons()
        {
            
            buissons.Add(Buisson_1);
            buissons.Add(Buisson_2);
            buissons.Add(Buisson_3);
            buissons.Add(Buisson_4);
            buissons.Add(Buisson_5);
            buissons.Add(Buisson_6);
            buissons.Add(Buisson_7);
            buissons.Add(Buisson_8);
            buissons.Add(Buisson_9);

        }

        private void InitObstacles()
        {
            obstacles.Add(Eau_1);
            obstacles.Add(Eau_2);
            obstacles.Add(Eau_3);
            obstacles.Add(Eau_4);
            obstacles.Add(Eau_5);
            obstacles.Add(Pierre_1);
            obstacles.Add(Pierre_2);
        }

        private void InitAnimaux()
        {

            animaux.Add(Animaux.Mammouth, new Animal("Mammouth", 200, "Mammouth.png", Attaques.COUP_DE_PIED));

        }

        private void Jeu(object? sender, EventArgs e)
        {

            DeplacementPerso();
            
    
        }

        private void RencontreBuisson()
        {

            System.Drawing.Rectangle player = new System.Drawing.Rectangle
                (
                
                    (int) Canvas.GetLeft(Personnage),
                    (int) Canvas.GetTop(Personnage),
                    (int) Personnage.Width,
                    (int) Personnage.Height

                );

            foreach(Rectangle buisson in buissons)
            {

                System.Drawing.Rectangle buissonRect = new System.Drawing.Rectangle
                (

                    (int) Canvas.GetLeft(buisson),
                    (int) Canvas.GetTop(buisson),
                    (int) buisson.Width,
                    (int) buisson.Height

                );

                if(player.IntersectsWith(buissonRect))
                {

#if DEBUG
                    Console.WriteLine("Intersection");
#endif

                    if(random.NextDouble() < POURCENTAGE_RENCONTRE_BUISSON)
                    {

                        Combat combat = new Combat();
                        combat.ShowDialog();

                    }

                }

            }
            
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

            RencontreBuisson();

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

            double gauchePerso = Canvas.GetLeft(Personnage);
            double hautPerso = Canvas.GetTop(Personnage);

            if (gauche != null)
                gauchePerso += gauche == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;

            if (haut != null)
                hautPerso += haut == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;

#if DEBUG
            Console.WriteLine($"Gauche : {gauchePerso}\n" +
                $"Haut : {hautPerso}");
#endif

            if(gauchePerso >= 0 && gauchePerso < ActualWidth - Personnage.Width)
                Canvas.SetLeft(Personnage, gauchePerso);

            if (hautPerso >= 0 && hautPerso < ActualHeight - Personnage.Height)
                Canvas.SetTop(Personnage, hautPerso);

            

        }
    }
}