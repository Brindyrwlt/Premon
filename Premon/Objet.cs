﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Premon
{
    internal class Objet : ICloneable
    {

        internal Objets TypeObjet;
        internal string Nom;
        internal int Quantite;
        internal BitmapImage Image;

        internal static Dictionary<Objets, Objet> objets = new Dictionary<Objets, Objet>();
        internal static Random random = new();

        private static readonly double CHANCE_CAPTURE_VIANDE = 0.90;

        public Objet(Objets typeObjet, string nom, string nomImage, int quantite = 1)
        {

            TypeObjet = typeObjet;
            Nom = nom;
            Quantite = quantite;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Objets/{nomImage}"));

        }

        internal static void InitObjets()
        {

            objets.Add(Objets.Morceau_de_viande, new(Objets.Morceau_de_viande, "Viande", "Morceau_de_viande.png"));
            //objets.Add(Objets.Graine, new(Objets.Graine, "Graine", "Graine.png"));

        }

        internal static Objet CreerObjet(Objets typeObjet, int quantite = 1)
        {

            Objet objet = (Objet) objets[typeObjet].Clone();
            objet.Quantite = quantite;

            return objet;
        }

        internal static void AjouterObjet(List<Objet> objetsPossedes, Objet objetAjoute)
        {

            bool aEteAjoute = false;

            foreach (Objet objet in objetsPossedes)
            {

                if (objet.TypeObjet == objetAjoute.TypeObjet)
                {

                    objet.Quantite += objetAjoute.Quantite;
                    aEteAjoute = true;
                    break;

                }

            }

            if (!aEteAjoute)
                objetsPossedes.Add(objetAjoute);

        }

        internal static void AjouterObjet(List<Objet> objetsPossedes, Objet[] objetsAjoute)
        {

            bool aEteAjoute = false;

            foreach(Objet objetAAjouter in objetsAjoute)
            foreach (Objet objet in objetsPossedes)
            {

                if (objet.TypeObjet == objetAAjouter.TypeObjet)
                {

                    objet.Quantite += objetAAjouter.Quantite;
                    aEteAjoute = true;
                    break;

                }

                    if (!aEteAjoute)
                        objetsPossedes.Add(objetAAjouter);
            }

        }

        internal static TypeAction? ActionObjet(Objet objet, Animal animalJoueur, Animal animalSauvage)
        {

            switch(objet.TypeObjet)
            {

                case Objets.Morceau_de_viande:
                    if(random.Next(0, (int) (100 - CHANCE_CAPTURE_VIANDE * 100) * (animalSauvage.HP / 50)) == 0)
                    {

                        MainWindow.animauxPossedes.Add(animalSauvage);
                        return TypeAction.Capture;

                    }
                    break;

            }

            return null;

        }
             
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    enum Objets
    {
        Morceau_de_viande,
        Graine
    }

    enum TypeAction
    {

        Capture,
        Soin

    }
}
