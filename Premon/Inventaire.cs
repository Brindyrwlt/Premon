using System.IO;
using System.Text.Json;

namespace Premon
{
    internal class Inventaire
    {

        // Chemin du fichier de sauvegarde
        private static string cheminFichier = "inventaire.json";

        // Attributs sauvegardés dans le Json
        public List<string> NomAnimaux { get; set; }
        public List<Animaux> AnimauxPossedes {  get; set; }
        public List<int> HPAnimaux { get; set; }

        public List<Objets> ObjetsPossedes { get; set; }
        public List<int> QuantiteObjets { get; set; }

        public Inventaire()
        {

            NomAnimaux = new List<string>();
            AnimauxPossedes = new List<Animaux>();
            HPAnimaux = new List<int>();

            ObjetsPossedes = new List<Objets>();
            QuantiteObjets = new List<int>();

        }

        /// <summary>
        /// Récupère l'inventaire sauvegardé dans le fichier de sauvegarde. Si le fichier est vide, une nouvelle sauvegarde sera crée.
        /// </summary>
        /// <param name="animaux"></param>
        /// <param name="objets"></param>
        internal static void InitInventaire(out List<Animal> animaux, out List<Objet> objets)
        {

            Inventaire inventaire;

            animaux = [];
            objets = [];

            if (File.Exists(cheminFichier)) // Si le fichier de sauvegarde existe
            {

                // Ouvre la lecture du fichier texte qui contient la sauvegarde
                StreamReader lectureFichier = File.OpenText(cheminFichier);

                // Essaie de récupérer l'inventaire et les animaux, sinon crée un nouvel inventaire
                try
                { 
                    // Décodage du fichier Json en un objet Inventaire
                    inventaire = JsonSerializer.Deserialize<Inventaire>(lectureFichier.ReadToEnd());

                    // Insertion dans les listes animaux et objets des objets créés à partir de l'objet Inventaire obtenu
                    for (int i = 0; i < inventaire.AnimauxPossedes.Count; i++)
                        animaux.Add(Animal.CreerAnimal(inventaire.AnimauxPossedes[i], inventaire.NomAnimaux[i], inventaire.HPAnimaux[i]));
                    for (int i = 0; i < inventaire.ObjetsPossedes.Count; i++)
                        objets.Add(Objet.CreerObjet(inventaire.ObjetsPossedes[i], inventaire.QuantiteObjets[i]));
                    Console.WriteLine("Restauration effectuée");
                }
                catch { inventaire = new(); } // Si la récupération a échoué, crée un nouvel inventaire
                lectureFichier.Close(); // Ferme la lecture du fichier

            }

            // Si le joueur n'a pas d'animal, en ajoute un par défaut
            if(animaux.Count == 0)
                animaux.Add(Animal.CreerAnimal(Animaux.Mammouth));

            // Si le joueur ne possède pas d'objets, en ajoute par défaut
            if (objets.Count == 0)
            {

                objets.Add(Objet.CreerObjet(Objets.Morceau_de_viande, 5));
                objets.Add(Objet.CreerObjet(Objets.Graine, 5));
                objets.Add(Objet.CreerObjet(Objets.Herbe_Medicinale, 3));

            }

        }

        /// <summary>
        /// Supprime le fichier de sauvegarde.
        /// </summary>
        internal static void SuppressionSauvegarde()
            => File.Delete("inventaire.json");

        /// <summary>
        /// Sauvegarde les animaux et les objets du joueur dans un fichier Json.
        /// </summary>
        /// <param name="animaux"></param>
        /// <param name="objets"></param>
        internal static void SauvegardeInventaire(List<Animal> animaux, List<Objet> objets)
        {

            Inventaire inventaire = new();

            // Sauvegarde des attributs des animaux possédés dans l'inventaire
            foreach (Animal animal in animaux)
            {

                inventaire.AnimauxPossedes.Add(animal.TypeAnimal);
                inventaire.NomAnimaux.Add(animal.Nom);
                inventaire.HPAnimaux.Add(animal.PV);

            }

            // Sauvegarde des attributs des objets possédés dans l'inventaire
            foreach(Objet objet in objets)
            {

                inventaire.ObjetsPossedes.Add(objet.TypeObjet);
                inventaire.QuantiteObjets.Add(objet.Quantite);

            }

            // Ecriture en Json de l'objet inventaire, sauvegardé dans le fichier inventaire.json
            // Le fichier est indenté grâce à l'option d'encodage
            File.WriteAllText(cheminFichier, JsonSerializer.Serialize(inventaire, new JsonSerializerOptions { WriteIndented = true }));
#if DEBUG
            Console.WriteLine(JsonSerializer.Serialize(inventaire));
#endif
        }

    }
}
