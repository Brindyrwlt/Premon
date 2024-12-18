﻿using System.IO;
using System.Text.Json;

namespace Premon
{
    internal class Inventaire
    {

        private static string cheminFichier = "inventaire.json";

        /*public List<Animal> animauxPossedes { get; set; }
        public List<Objet> objetsPossedes { get; set; }*/

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

        internal static void InitInventaire(out List<Animal> animaux, out List<Objet> objets)
        {

            Inventaire inventaire;

            animaux = [];
            objets = [];

            if (File.Exists(cheminFichier))
            {

                StreamReader lectureFichier = File.OpenText(cheminFichier);
                try 
                { 

                    inventaire = JsonSerializer.Deserialize<Inventaire>(lectureFichier.ReadToEnd());
                    for (int i = 0; i < inventaire.AnimauxPossedes.Count; i++)
                        animaux.Add(Animal.CreerAnimal(inventaire.AnimauxPossedes[i], inventaire.NomAnimaux[i], inventaire.HPAnimaux[i]));
                    for (int i = 0; i < inventaire.ObjetsPossedes.Count; i++)
                        objets.Add(Objet.CreerObjet(inventaire.ObjetsPossedes[i], inventaire.QuantiteObjets[i]));
                    Console.WriteLine("Restauration effectuée");
                }
                catch { inventaire = new(); }
                lectureFichier.Close();

            }

            if(animaux.Count == 0)
                animaux.Add(Animal.CreerAnimal(Animaux.Mammouth));
            if (objets.Count == 0)
            {

                objets.Add(Objet.CreerObjet(Objets.Morceau_de_viande, 5));
                objets.Add(Objet.CreerObjet(Objets.Graine, 5));
                objets.Add(Objet.CreerObjet(Objets.Herbe_Medicinale, 3));

            }

        }

        internal static void SauvegardeInventaire(List<Animal> animaux, List<Objet> objets)
        {

            Inventaire inventaire = new();

            foreach (Animal animal in animaux)
            {

                inventaire.AnimauxPossedes.Add(animal.TypeAnimal);
                inventaire.NomAnimaux.Add(animal.Nom);
                inventaire.HPAnimaux.Add(animal.HP);

            }

            foreach(Objet objet in objets)
            {

                inventaire.ObjetsPossedes.Add(objet.TypeObjet);
                inventaire.QuantiteObjets.Add(objet.Quantite);

            }

            string text = JsonSerializer.Serialize(inventaire);            

            File.WriteAllText(cheminFichier, JsonSerializer.Serialize(inventaire, new JsonSerializerOptions { WriteIndented = true }));
#if DEBUG
            Console.WriteLine(JsonSerializer.Serialize(inventaire));
#endif
        }

    }
}
