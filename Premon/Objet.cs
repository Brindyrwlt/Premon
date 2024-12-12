using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Premon
{
    internal class Objet
    {
        public static string Nom;
        public static int Quantite = 0;
        public BitmapImage Image;

        public Objet(string nom, int quantite) 
        {
            Nom = nom;
            Quantite = quantite;
            Image = new BitmapImage(new Uri($"pack://application:,,,/Textures/Objets/{nom}.png"));
        }


    }

    enum Objets
    {
        MORCEAU_DE_VIANDE,
        GRAINE,
        CARTE
    }
}
