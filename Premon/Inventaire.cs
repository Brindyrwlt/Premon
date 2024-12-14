using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace Premon
{
    internal class Inventaire
    {

        private static string cheminFichier = "inventaire.json";

        internal List<Animal> animauxPossedes;
        internal List<Objet> objetsPossedes;

        public Inventaire()
        {

            animauxPossedes = new List<Animal>();
            objetsPossedes = new List<Objet>();

        }

        internal static Inventaire? InitInventaire()
        {
            
            Inventaire? inventaire = new();

            if (File.Exists(cheminFichier))
            {

                StreamReader lectureFichier = File.OpenText(cheminFichier);
                try { inventaire = JsonSerializer.Deserialize<Inventaire>(lectureFichier.ReadToEnd()); }
                catch { inventaire = new(); }
                lectureFichier.Close();

            }

            return inventaire;
        }

        internal static void SauvegardeInventaire(Inventaire inventaire)
            => File.WriteAllText(cheminFichier, JsonSerializer.Serialize(inventaire));

    }
}
