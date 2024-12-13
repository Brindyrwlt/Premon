using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Premon
{
    /// <summary>
    /// Logique d'interaction pour InventaireObjet.xaml
    /// </summary>
    public partial class InventaireObjet : Window
    {
        private List<Objet> InventaireObjets = new List<Objet>();

        public InventaireObjet()
        {
            InitializeComponent();
        }

        private void AffichageInventaire(List<Objet> InventaireObjets)
        {
            for (int i = 1; i < InventaireObjets.Count; i++)
            {
                
            }
        }



    }
}
