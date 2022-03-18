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
using Bibliothèque; 

namespace projetGSB
{
    /// <summary>
    /// Logique d'interaction pour AjouterMed.xaml
    /// </summary>
    public partial class AjouterMed : Window
    {
        public AjouterMed(GstBDD gst)
        {
            InitializeComponent();
        }
        GstBDD gst;

        //initialise la page est mette dans le combobox le nom des famille
        private void Window_Loaded_Ajout_Med(object sender, RoutedEventArgs e)
        {
            gst = new GstBDD();
            cboFamille.ItemsSource = gst.GetAllFamilles();
        }

        //permet de rajouté un medicament en verifianbt tout les ellement qu'il a besoin 
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //verifier si une famille est selectione 
            if (cboFamille.SelectedItem == null)
            {
                MessageBox.Show("Veuillez choisir une famille.", "Erreur de choix", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //verifier si un nom est ecrit 
                if (nomMed.Text == "")
                {
                    MessageBox.Show("Veuillez écrire un nom médicament.", "Erreur de choix", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    //verifier si un prix est mis 
                    if (prixMed.Text != "")
                    {
                        int result;
                        if (int.TryParse(prixMed.Text, out result))
                        {
                            //verifier si il y a une description au medicament 
                            if (composition.Text == "")
                            {
                                MessageBox.Show("Veuillez écrire une composition.", "Erreur de choix", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {
                                //verifier si les effect son renseigner 
                                if (effet.Text == "")
                                {
                                    MessageBox.Show("Veuillez écrire un effect.", "Erreur de choix", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else
                                {
                                    //verifier les si les contre indication son renseigner 
                                    if (contreindic.Text == "")
                                    {
                                        MessageBox.Show("Veuillez écrire une contre indication.", "Erreur de choix", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        //rajouter chaque ellement a AjoutMed
                                        string nom = nomMed.Text;
                                        int famCode = (cboFamille.SelectedItem as Famille).CodeFamille;
                                        //string prixOK = prixMed.Text.Replace(',', '.');
                                        decimal prix = Convert.ToDecimal(prixMed.Text);
                                        string comp = composition.Text;
                                        string effet_med = effet.Text;
                                        string contre = contreindic.Text;

                                        gst.AjoutMed(nom, famCode, prix, comp, effet_med, contre);
                                        MessageBox.Show("Le médicament a bien été créé.");
                                        this.Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Veuillez écrire un prix valide.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Veuillez écrire un prix médicament.", "Erreur de saisie", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
