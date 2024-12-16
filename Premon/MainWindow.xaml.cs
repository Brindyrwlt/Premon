using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Text.Json;

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
        private static readonly double POURCENTAGE_RENCONTRE_BUISSON = 0.1;

        // Images
        private static BitmapImage imgPersonnageDroite;
        private static BitmapImage imgPersonnageGauche;
        private static BitmapImage imgPersonnageDevant;
        private static BitmapImage imgPersonnageDerriere;
        private static BitmapImage imgPersonnageDevantBuisson;
        private static BitmapImage imgPersonnageDerriereBuisson;
        private static BitmapImage imgPersonnageDroiteBuisson;
        private static BitmapImage imgPersonnageGaucheBuisson;

        private static ImageBrush imgPerso = new();

        // Variables système  --------------------------------

        // Mouvement
        private static DispatcherTimer intervalleDeplacement;
        private static bool? gauche = null;
        private static bool? haut = null;
        private static bool aAppuye = false;
        private static bool animalChoisi;

        // Inventaire
        internal static List<Animal> animauxPossedes;
        internal static List<Objet> objetsPossedes;


        // Autre
        private Random aleatoire = new Random();

        private List<Rectangle> buissons = new List<Rectangle>();
        private List<Rectangle> obstacles = new List<Rectangle>();

        public MainWindow()
        {

            InitializeComponent();
            InitIntervalleDeplacement();
            InitBuissons();
            InitObstacles();
            InitBitmap();
            Objet.InitObjets();
            Animal.InitAnimaux();
            Animal.InitDescriptions();
            Inventaire.InitInventaire(out animauxPossedes, out objetsPossedes);
            imgPerso.ImageSource = imgPersonnageDevant;
            animauxPossedes.Add(Animal.CreerAnimal(Animaux.Mammouth));/*
            objetsPossedes.Add(Objet.CreerObjet(Objets.Morceau_de_viande));*/
            Objet.AjouterObjet(objetsPossedes, Objet.CreerObjet(Objets.Morceau_de_viande));

        }

        internal static string FormatageNomAttaque(Attaques attaque)
        {

            string nomAttaque = attaque.ToString().Replace("_", " ").ToLower();
            nomAttaque = nomAttaque[0].ToString().ToUpper()[0] + nomAttaque.Substring(1);
            return nomAttaque;

        }

        private void InitBitmap()
        {
            imgPersonnageDevant = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_devant/Personnage_devant_1.png"));
            imgPersonnageDroite = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_droite/Personnage_droite_1.png"));
            imgPersonnageDerriere = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_derriere/Personnage_derriere_1.png"));
            imgPersonnageGauche = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_gauche/Personnage_gauche_1.png"));
            imgPersonnageDevantBuisson = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_devant/Personnage_devant_1_buisson.png"));
            imgPersonnageDerriereBuisson = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_derriere/Personnage_derriere_1_buisson.png"));
            imgPersonnageDroiteBuisson = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_droite/Personnage_droite_1_buisson.png"));
            imgPersonnageGaucheBuisson = new BitmapImage(new Uri($"pack://application:,,,/Textures/Personnage/Personnage_gauche/Personnage_gauche_1_buisson.png"));
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
                    if (gauche != null)
                    {
                        if (gauche == false)
                            imgPerso.ImageSource = imgPersonnageDroiteBuisson;
                        else
                            imgPerso.ImageSource = imgPersonnageGaucheBuisson;
                    }
                    if (haut != null)
                    {
                        if (haut == false)
                            imgPerso.ImageSource = imgPersonnageDevantBuisson;
                        else
                            imgPerso.ImageSource = imgPersonnageDerriereBuisson;
                    }
                    Personnage.Fill = imgPerso;
                    if (aleatoire.NextDouble() < POURCENTAGE_RENCONTRE_BUISSON)
                        DebutCombat();

                }

            }
            
        }

        private void DebutCombat()
        {

            gauche = null;
            haut = null;
            Animal animalSauvage;
            bool animalTrouve = false;

            /* 
             * Quand l'animal est sélectionné aléatoirement, si la chance complémentaire est supérieure à 1,
             * le tirage a une chance de se réeffectuer
             */
            do
            {

                animalSauvage = Animal.CreerAnimal((Animaux) aleatoire.Next(0, Animal.nombreAnimaux));          

            }
            while (!(aleatoire.Next(0, animalSauvage.ChanceComplementaire - 1) == 0));

            Combat combat = new Combat(); 
            combat.InitAnimaux(animauxPossedes[0], animalSauvage);
            combat.ShowDialog();

            switch(combat.combatFini)
            {

                case 4:
                    Objet.AjouterObjet(objetsPossedes, animalSauvage.Butin);
                    break;

                case 5:
                    animauxPossedes.RemoveAt(0);
                    break;

                case 6:
                    Objet.AjouterObjet(objetsPossedes, animalSauvage.Butin);
                    animauxPossedes.RemoveAt(0);
                    break;

            }

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

                case Key.E:
                    
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

            Personnage.Fill = imgPerso;

            // Modification de la position en fonction de la touche appuyée
            if (gauche != null)
            {
                gauchePerso += gauche == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;
                if (gauche == false)
                    imgPerso.ImageSource = imgPersonnageDroite;
                else
                    imgPerso.ImageSource = imgPersonnageGauche;
            }
            if (haut != null)
            {
                hautPerso += haut == false ? PAS_DEPLACEMENT : -PAS_DEPLACEMENT;
                if (haut == false)
                    imgPerso.ImageSource = imgPersonnageDevant;
                else
                    imgPerso.ImageSource = imgPersonnageDerriere;
            }

            Personnage.Fill = imgPerso;
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

        private void IconeSauvegarde_MouseDown(object sender, MouseButtonEventArgs e)
        {

            Inventaire.SauvegardeInventaire(animauxPossedes, objetsPossedes);

        }

        private void IconeInventaire_MouseDown(object sender, MouseButtonEventArgs e) 
        { 
        
            InventaireObjet inventaireObjet = new InventaireObjet();
            inventaireObjet.AffichageInventaire(objetsPossedes);
            inventaireObjet.ShowDialog();
            
        }

    }
}