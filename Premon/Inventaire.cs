using System.IO;
using System.Text.Json;

namespace Premon
{
    internal class Inventaire
    {

        private static string cheminFichier = "inventaire.json";

        public List<Animal> animauxPossedes { get; set; }
        public List<Objet> objetsPossedes { get; set; }

        public Inventaire()
        {

            animauxPossedes = new List<Animal>();
            objetsPossedes = new List<Objet>();

        }

        internal static void InitInventaire(out Inventaire inventaire)
        {

            inventaire = new();
            inventaire.animauxPossedes = new List<Animal>();
            inventaire.objetsPossedes = new List<Objet>();

            Inventaire inventaireIntermediaire = new();

            if (File.Exists(cheminFichier))
            {

                StreamReader lectureFichier = File.OpenText(cheminFichier);
                try { inventaire = JsonSerializer.Deserialize<Inventaire>(lectureFichier.ReadToEnd()); }
                catch { inventaire = new(); }
                lectureFichier.Close();

            }

            if (inventaireIntermediaire.animauxPossedes != null)
            {

                inventaire = inventaireIntermediaire;

            }

        }

        internal static void SauvegardeInventaire(Inventaire inventaire)
        {

            string text = JsonSerializer.Serialize(inventaire);

            File.WriteAllText(cheminFichier, JsonSerializer.Serialize(inventaire));
            Console.WriteLine(JsonSerializer.Serialize(inventaire));

        }

    }
}
