using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace Premon
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Constantes ----------------------------------------

        // Mouvement
        private static readonly int PAS_DEPLACEMENT = 50;
        private static readonly int INTERVALLE_DEPLACEMENT = 200;

        // Probabilité
        private static readonly double POURCENTAGE_RENCONTRE_BUISSON = 0.2;

        // Images
        private static BitmapImage imgPersonnageDroite;
        private static BitmapImage imgPersonnageGauche;
        private static BitmapImage imgPersonnageDevant;
        private static BitmapImage imgPersonnageDerriere;
        // Variables système  --------------------------------

        // Mouvement
        private static DispatcherTimer intervalleDeplacement;
        private static bool? gauche = null;
        private static bool? haut = null;
        private static bool aAppuye = false;
        private static bool animalChoisi;

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
            InitObstacles();
            InitObjets();
            InitAnimaux();
            InitBitmap();

            animauxPossedes.Add(animaux[Animaux.Mammouth]);
        }

        private void InitBitmap()
        {
            imgPersonnageDevant = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_devant/Personnage_devant_1.png"));
            imgPersonnageDroite = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_droite/Personnage_droite_1.png"));
            imgPersonnageDerriere = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_derriere/Personnage_derriere_1.png"));
            imgPersonnageGauche = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_gauche/Personnage_gauche_1.png"));
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
            obstacles.Add(Arbre_1);
            obstacles.Add(Arbre_2);
            obstacles.Add(Arbre_3);
            obstacles.Add(Arbre_4);
            obstacles.Add(Butte_1);
        }

        private void InitAnimaux()
        {

            animaux.Add(Animaux.Mammouth, new Animal("Mammouth", 200, 1, Attaques.COUP_DE_PIED));
            animaux.Add(Animaux.Bouquetin, new Animal("Bouquetin", 80, 1, Attaques.EMPALEMENT, Attaques.AIGUISAGE));

        }

        private void InitObjets()
        {

        }

        private void Jeu(object? sender, EventArgs e)
        {

            DeplacementPerso();
            
    
        }

        private void RencontreBuisson()
        {

            System.Drawing.Rectangle perso = new System.Drawing.Rectangle
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

                if(perso.IntersectsWith(buissonRect))
                {
#if DEBUG
                    Console.WriteLine("Intersection");
#endif
                    if (random.NextDouble() < POURCENTAGE_RENCONTRE_BUISSON)
                        DebutCombat();

                }

            }
            
        }

        private void DebutCombat()
        {

            Animal animalSauvage;
            animalChoisi = false;

            do
            {

                animalSauvage = animaux[(Animaux) random.Next(0, Animaux.GetValues(typeof(Animaux)).Length)];

                if (random.Next(0, animalSauvage.ChanceComplementaire - 1) == 0)
                    animalChoisi = true;

            } while (!animalChoisi);

            Combat combat = new Combat(); 
            combat.InitAnimaux(animauxPossedes[0], animalSauvage);
            combat.ShowDialog();


        }

        private void CollisionObstacles(double ancienneGauche, double ancienHaut)
        {
            System.Drawing.Rectangle perso = new System.Drawing.Rectangle
                (

                    (int)Canvas.GetLeft(Personnage),
                    (int)Canvas.GetTop(Personnage),
                    (int)Personnage.Width,
                    (int)Personnage.Height

                );

            foreach (Rectangle obstacle in obstacles)
            {

                System.Drawing.Rectangle obstacleRect = new System.Drawing.Rectangle
                (

                    (int)Canvas.GetLeft(obstacle),
                    (int)Canvas.GetTop(obstacle),
                    (int)obstacle.Width,
                    (int)obstacle.Height

                );

                if (perso.IntersectsWith(obstacleRect))
                {
#if DEBUG
                    Console.WriteLine("Collision avec obstacle");
#endif
                    Canvas.SetLeft(Personnage, ancienneGauche);
                    Canvas.SetTop(Personnage, ancienHaut);
                    break;
                }
            }
        }

            private void fenetre_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Q:
                case Key.Left:
                    gauche = true;
                    break;

                case Key.D:
                case Key.Right:
                    gauche = false;
                    break;

                case Key.Z:
                case Key.Up:
                    haut = true;
                    break;

                case Key.S:
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

            // Coordonnées du personnage
            double gauchePerso = Canvas.GetLeft(Personnage);
            double hautPerso = Canvas.GetTop(Personnage);

            double ancienneGauche = gauchePerso;
            double ancienHaut = hautPerso;

            // Modification de la position en fonction de la touche appuyée
            if (gauche != null)
            {
                gauchePerso += gauche == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;
           /*     Personnage.Fill = imgPersonnageDroite;*/
            }
            if (haut != null)
            {
                hautPerso += haut == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;

            }

#if DEBUG
            Console.WriteLine($"Gauche : {gauchePerso}\n" +
                $"Haut : {hautPerso}");
#endif

            // Déplacement du personnage selon les changements si les changements sont possibles
            if(gauchePerso >= 0 && gauchePerso < ActualWidth - Personnage.Width)
                Canvas.SetLeft(Personnage, gauchePerso);
            if (hautPerso >= 0 && hautPerso < ActualHeight - Personnage.Height)
                Canvas.SetTop(Personnage, hautPerso);

            // Retour en arrière si collision
            CollisionObstacles(ancienneGauche, ancienHaut);

        }
    }
}