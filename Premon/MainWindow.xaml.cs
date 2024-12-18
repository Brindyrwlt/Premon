using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Premon
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Constantes ----------------------------------------

        // Mouvement
        private static readonly int PAS_DEPLACEMENT = 50;
        private static readonly int INTERVALLE_DEPLACEMENT = 200;

        // Probabilité
        private static readonly double POURCENTAGE_RENCONTRE_BUISSON = 0.1;

        // Variables système  --------------------------------

        // Mouvement
        private static DispatcherTimer intervalleDeplacement; // Minuteur permettant un déplacement 
        private static bool? gauche = null; // Quand false, il va à droite, quand true il va à gauche, quand null il ne se déplace pas
        private static bool? haut = null; // Quand false il va en bas, quand true il va en haut, quand null il ne se déplace pas
        private static bool aAppuye = false; // Permet de se déplacer sans délai d'une case à l'appui d'une touche

        // Collisions
        // Listes contenant les objets sujets aux collisions pour les détecter à chaque mouvement
        private List<Rectangle> buissons = new List<Rectangle>();
        private List<Rectangle> obstacles = new List<Rectangle>();

        // Inventaire du joueur
        internal static List<Animal> animauxPossedes;
        internal static List<Objet> objetsPossedes;

        // Images
        private static BitmapImage imgPersonnageDroite;
        private static BitmapImage imgPersonnageGauche;
        private static BitmapImage imgPersonnageDevant;
        private static BitmapImage imgPersonnageDerriere;
        private static BitmapImage imgPersonnageDevantBuisson;
        private static BitmapImage imgPersonnageDerriereBuisson;
        private static BitmapImage imgPersonnageDroiteBuisson;
        private static BitmapImage imgPersonnageGaucheBuisson;

        // Image actuelle du personnage
        private static ImageBrush imgPerso = new();

        // Musiques 
        private static MediaPlayer musiqueFond;
        private static MediaPlayer musiqueForet;
        private static MediaPlayer musiqueCombat;
        internal static double volume = 0.5; // Volume des musiques, par défaut à 50 %

        // Autre
        private Random aleatoire = new Random();

        public MainWindow()
        {

            EcranAccueil ecranAccueil = new();
            ecranAccueil.ShowDialog();
            Eteindre(ecranAccueil.quitterJeu);
            InitializeComponent();
            InitIntervalleDeplacement();
            InitBuissons();
            InitObstacles();
            InitBitmap();
            InitMusiqueFond();
            InitMusiqueCombat();
            ChangementSon(volume);
            Objet.InitObjets();
            Animal.InitAnimaux();
            Animal.InitDescriptions();
            Inventaire.InitInventaire(out animauxPossedes, out objetsPossedes);
            imgPerso.ImageSource = imgPersonnageDevant;

        }

        /// <summary>
        /// Initialise les musiques d'arrière plan du jeu.
        /// </summary>
        private void InitMusiqueFond()
        {
            musiqueFond = new MediaPlayer();
            musiqueForet = new MediaPlayer();
            musiqueFond.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Musique_fond.mp3"));
            musiqueForet.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Bruit_foret.mp3"));
            
            // Relance les musiques à la fin de celles-ci
            musiqueFond.MediaEnded += RelanceMusiqueFond;
            musiqueForet.MediaEnded += RelanceMusiqueFond;

            musiqueFond.Play();
            musiqueForet.Play();
        }

        /// <summary>
        /// Relanche les musiques d'arrière plan.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RelanceMusiqueFond(object? sender, EventArgs e)
        {
            musiqueFond.Position = TimeSpan.Zero;
            musiqueForet.Position = TimeSpan.Zero;
            musiqueFond.Play();
            musiqueForet.Play();
        }

        /// <summary>
        /// Initialise la musique de combat contre un animal sauvage.
        /// </summary>
        private void InitMusiqueCombat()
        {
            musiqueCombat = new MediaPlayer();
            musiqueCombat.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Musiques/Musique_combat.mp3"));
            musiqueCombat.MediaEnded += RelanceMusiqueCombat;
        }

        /// <summary>
        /// Relanche la musique de combat.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RelanceMusiqueCombat(object? sender, EventArgs e)
        {
            musiqueCombat.Position = TimeSpan.Zero;
            musiqueCombat.Play();
        }

        /// <summary>
        /// Initialise les images des différents états du personnage.
        /// </summary>
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

        /// <summary>
        /// Insert les différents buissons de la carte dans une liste.
        /// </summary>
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

        /// <summary>
        /// Insert les différents obstacles de la carte dans une liste.
        /// </summary>
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

        /// <summary>
        /// Initialise la minuterie pour les déplacements.
        /// </summary>
        private void InitIntervalleDeplacement()
        {
            intervalleDeplacement = new();
            intervalleDeplacement.Interval = TimeSpan.FromMilliseconds(INTERVALLE_DEPLACEMENT);
            intervalleDeplacement.Tick += Jeu;
            intervalleDeplacement.Start();
        }

        /// <summary>
        /// Invoque le déplacement du personnage à chaque itération de la minuterie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Jeu(object? sender, EventArgs e)
            => DeplacementPerso();

        /// <summary>
        /// Déplace le personnage dans les limites de la fenêtre en fonction des variables directionnelles.
        /// </summary>
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

            // Déplacement du personnage selon les changements si les changements sont possibles
            if (gauchePerso >= 0 && gauchePerso < ActualWidth - Personnage.Width)
                Canvas.SetLeft(Personnage, gauchePerso);
            if (hautPerso >= 0 && hautPerso < ActualHeight - Personnage.Height)
                Canvas.SetTop(Personnage, hautPerso);

            // Retour en arrière si collision
            CollisionObstacles(ancienneGauche, ancienHaut);

        }

        /// <summary>
        /// Démarre un combat et administre les récompenses ou les malus selon l'issue du combat.
        /// </summary>
        private void DebutCombat()
        {

            // Démarre la musique de combat
            musiqueFond.Stop();
            musiqueCombat.Play();

            // Arrête le déplacement du personnage
            gauche = null;
            haut = null;

            Animal animalSauvage;

            /* 
             * Quand l'animal est sélectionné aléatoirement, si la chance complémentaire est supérieure à 1,
             * le tirage a une chance de se réeffectuer
             */
            do animalSauvage = Animal.CreerAnimal((Animaux) aleatoire.Next(0, Animal.nombreAnimaux));
            while (!(aleatoire.Next(0, animalSauvage.ChanceComplementaire - 1) == 0));

            Combat combat = new();
            combat.InitAnimaux(animauxPossedes[0], animalSauvage);
            combat.ShowDialog();

            // Remets les multiplicateurs de l'animal du joueur par défaut
            animauxPossedes[combat.indexAnimalSelectionne].multiplicateur = 1;
            animauxPossedes[combat.indexAnimalSelectionne].multiplicateurDegatRecu = 1;

            // Administre les récompenses en fonction de l'issue du combat
            switch (combat.etatCombat)
            {

                case 4: // Tué l'animal
                    Objet.AjouterObjet(objetsPossedes, animalSauvage.Butin);
                    break;

                case 5: // Joueur tué par l'animal
                    animauxPossedes.RemoveAt(combat.indexAnimalSelectionne);
                    break;

                case 6: // Les deux sont morts
                    Objet.AjouterObjet(objetsPossedes, animalSauvage.Butin);
                    animauxPossedes.RemoveAt(combat.indexAnimalSelectionne);
                    break;

            }

            // Si le joueur n'a plus d'animal, suppression de sauvegarde et extinction du jeu
            if (animauxPossedes.Count == 0)
            {

                Inventaire.SuppressionSauvegarde();
                Eteindre(true);

            }

            Inventaire.SauvegardeInventaire(animauxPossedes, objetsPossedes);

        }

        /// <summary>
        /// Détecte la collision du personnage avec un buisson, change son apparence et lance un combat.
        /// </summary>
        private void RencontreBuisson()
        {

            // Représentation mathématique de la boîte de collision du personnage
            System.Drawing.Rectangle perso = new System.Drawing.Rectangle
                (
                
                    (int) Canvas.GetLeft(Personnage), // Coordonnées x
                    (int) Canvas.GetTop(Personnage), // Coordonnées y
                    (int) Personnage.Width, // Taille x
                    (int) Personnage.Height // Taille y

                );

            // Vérification d'une collision avec chaque buisson de la carte
            foreach(Rectangle buisson in buissons)
            {

                // Représentation mathématique du buisson
                System.Drawing.Rectangle buissonRect = new System.Drawing.Rectangle
                (

                    (int) Canvas.GetLeft(buisson),
                    (int) Canvas.GetTop(buisson),
                    (int) buisson.Width,
                    (int) buisson.Height

                );

                // Si les deux représentations mathématiques des boîtes de collision se touchent
                if(perso.IntersectsWith(buissonRect))
                {
#if DEBUG
                    Console.WriteLine("Intersection");
#endif
                    // Changement de l'image du personnage en fonction de sa direction
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

                    // Possibilité de rencontrer un animal dans les buissons
                    if (aleatoire.NextDouble() < POURCENTAGE_RENCONTRE_BUISSON)
                    {
                        DebutCombat();

                        // Redémarre la musique de fond après un combat
                        musiqueFond.Play();
                        musiqueCombat.Stop();

                    }

                }

            }
            
        }

        /// <summary>
        /// Détecte la collision avec des obstacles et arrête le joueur.
        /// </summary>
        /// <param name="ancienneGauche"></param>
        /// <param name="ancienHaut"></param>
        private void CollisionObstacles(double ancienneGauche, double ancienHaut)
        {

            // Représentation mathématique de la boîte de collision du personnage
            System.Drawing.Rectangle perso = new System.Drawing.Rectangle
                (

                    (int)Canvas.GetLeft(Personnage),
                    (int)Canvas.GetTop(Personnage),
                    (int)Personnage.Width,
                    (int)Personnage.Height

                );

            foreach (Rectangle obstacle in obstacles)
            {

                // Représentation mathématique de la boîte de collision des obstacles de la carte
                System.Drawing.Rectangle obstacleRect = new System.Drawing.Rectangle
                (

                    (int)Canvas.GetLeft(obstacle),
                    (int)Canvas.GetTop(obstacle),
                    (int)obstacle.Width,
                    (int)obstacle.Height

                );

                // Si les deux représentations mathématiques des boîtes de collision se touchent
                if (perso.IntersectsWith(obstacleRect))
                {
#if DEBUG
                    Console.WriteLine("Collision avec obstacle");
#endif
                    // Annulation du déplacement
                    Canvas.SetLeft(Personnage, ancienneGauche);
                    Canvas.SetTop(Personnage, ancienHaut);
                    break;

                }
            }
        }

        /// <summary>
        /// Eteints le jeu et sauvegarde l'inventaire du joueur.
        /// </summary>
        /// <param name="resultatDialogue"></param>
        private void Eteindre(bool resultatDialogue)
        {
            if(resultatDialogue)
            {

                Environment.Exit(0); // Ferme la fenêtre
                Inventaire.SauvegardeInventaire(animauxPossedes, objetsPossedes);

            }

        }

        /// <summary>
        /// Renvoie le nom d'une attaque en français à partir du nom de l'énumérateur.
        /// </summary>
        /// <param name="attaque"></param>
        /// <returns></returns>
        internal static string FormatageNomAttaque(Attaques attaque)
        {

            string nomAttaque = attaque.ToString().Replace("_", " ").ToLower();
            nomAttaque = nomAttaque[0].ToString().ToUpper()[0] + nomAttaque.Substring(1);
            return nomAttaque;

        }

        /// <summary>
        /// Change le volume des musiques du jeu.
        /// </summary>
        /// <param name="volume"></param>
        private void ChangementSon(double volume)
        {

            musiqueFond.Volume = volume;
            musiqueForet.Volume = volume;
            musiqueCombat.Volume = volume;

        }

        // Gestion de l'appui des touches
        private void fenetre_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {

                // Se déplacer à gauche
                case Key.Q:
                case Key.Left:
                    gauche = true;
                    RencontreBuisson();
                    break;

                // Se déplacer à droite
                case Key.D:
                case Key.Right:
                    gauche = false;
                    RencontreBuisson();
                    break;

                // Se déplacer en haut
                case Key.Z:
                case Key.Up:
                    haut = true;
                    RencontreBuisson();
                    break;

                // Se déplacer en bas
                case Key.S:
                case Key.Down:
                    haut = false;
                    RencontreBuisson();
                    break;

                // Ouvre l'inventaire des objets
                case Key.E:
                    InventaireObjet inventaireObjet = new();
                    inventaireObjet.AffichageInventaire(objetsPossedes);
                    inventaireObjet.ShowDialog();
                    break;

                // Ouvre l'écran des animaux
                case Key.A:
                    EcranAnimal ecranAnimal = new();
                    ecranAnimal.ShowDialog();
                    break;

                // Ouvre l'écran d'accueil sans la possibilité de démarrer une nouvelle partie ou de charger la partie
                case Key.Escape:
                    EcranAccueil ecranAccueil = new();
                    ecranAccueil.BoutonNouvellePartie.IsEnabled = false;
                    ecranAccueil.BoutonChargerPartie.IsEnabled = false;
                    ecranAccueil.ShowDialog();

                    // Extinction de l'application en fonction de la fin du dialogue avec la page d'accueil
                    Eteindre(ecranAccueil.quitterJeu);
                    
                    ChangementSon(volume);
                    break;
            }

            // Extinction de la minuterie, déplacement intantané du personnage et redémarrage de la minuterie
            if (aAppuye == false)
            {

                intervalleDeplacement.Stop();
                aAppuye = true;
                DeplacementPerso();
                intervalleDeplacement.Start();
                RencontreBuisson();

            }

        }

        

        // Réinitialisation des déplacements si aucune touche n'est pressée
        private void fenetre_KeyUp(object sender, KeyEventArgs e)
        {
            gauche = null;
            haut = null;
            aAppuye = false;
        }

        // Ouverture de l'inventaire des objets à l'appui de l'icône
        private void IconeInventaire_MouseDown(object sender, MouseButtonEventArgs e) 
        { 
        
            InventaireObjet inventaireObjet = new();
            inventaireObjet.AffichageInventaire(objetsPossedes);
            inventaireObjet.ShowDialog();
            
        }

        // Ouverture de la liste des animaux à l'appui de l'icône
        private void IconeAnimaux_MouseDown(object sender, MouseButtonEventArgs e)
        {

            EcranAnimal ecranAnimal = new();
            ecranAnimal.ShowDialog();

        }
    }
}