using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Objet(Objets typeObjet, string nom, int quantite = 1)
        {

            TypeObjet = typeObjet;
            Nom = nom;
            Quantite = quantite;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Objets/{nom}.png"));

        }

        internal static void InitObjets()
        {

            objets.Add(Objets.Morceau_de_viande, CreerObjet(Objets.Morceau_de_viande));
            objets.Add(Objets.Graine, CreerObjet(Objets.Graine));

        }

        internal static Objet CreerObjet(Objets typeObjet, int quantite = 1)
        {

            Objet objet = (Objet) objets[typeObjet].Clone();
            objet.Quantite = quantite;

            return objet;
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
}
