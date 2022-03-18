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
using GstBdd;

namespace projetGSB
{
    /// <summary>
    /// Logique d'interaction pour AjouterTin.xaml
    /// </summary>
    public partial class AjouterTin : Window
    {
        public AjouterTin(GstBDD unGst)
        {
            InitializeComponent();
        }
        GstBDD gst;

        private void Button_Click(object sender,  RoutedEventArgs e)
        {
            string a = libelleTin.Text; // "a" récupere ce qui à été saisit dans le champ texte
            int b = 0;

            for (int i = 0; i < a.Length; i++)
            {   //si un caratère spécial est saisit, la variable "b" augmente à 1 et une erreur se déclanchera
                if (Char.IsDigit(a[i]))  
                    b += 1;
            }

            if (libelleTin.Text == "")
            {   // erreur si le champ de saisie est vide
                MessageBox.Show("Veuillez entrer un nouveau type d'individu ", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error); 
            }

            else if (b != 0)
            {   //erreur un charactère spécial à été saisit
                MessageBox.Show("Carractères invalides", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                gst = new GstBDD();
                gst.AjouterTypeIndividu(libelleTin.Text);//appel de la fonction AjouterTypeIndividu du gstBdd, avec le texte saisit comme paramètre, ajoute un nouveau type d'individu
                this.Close();
                MessageBox.Show("Individu ajouté", "Nouveau type d'individu", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
