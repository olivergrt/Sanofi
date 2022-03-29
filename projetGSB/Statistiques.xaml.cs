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
using LiveCharts;
using LiveCharts.Wpf;
using Bibliothèque;
using GstBdd;

namespace projetGSB
{
    /// <summary>
    /// Logique d'interaction pour Statistiques.xaml
    /// </summary>
    public partial class Statistiques : Window
    {
        public Statistiques(GstBDD gst)
        {
            InitializeComponent();
        }
        GstBDD gst;

        private void Window_Loaded_Statistiques(object sender, RoutedEventArgs e)
        {
            gst = new GstBDD();
            cboActions.ItemsSource = gst.GetAllMedicaments();

            // Graphique 1 Classic 
            ColumnSeries cs = new ColumnSeries();
            cs.Fill = Brushes.Aquamarine;
            ChartValues<int> line = new ChartValues<int>();
            Dictionary<string, int> lesDatas = new Dictionary<string, int>();
            lesDatas = gst.GetDatasGraph1();

            foreach (string cle in lesDatas.Keys)
            {
                line.Add(lesDatas[cle]);
            }
            cs.Values = line;
            /*graph_NbMedParFamille.Series.Clear();*/
            /*graph_NbMedParFamille.AxisX.Clear();*/
            Axis axe = new Axis();
            axe.Labels = lesDatas.Keys.ToList();
            graph_NbMedParFamille.AxisX.Add(axe);
            graph_NbMedParFamille.Series.Add(cs);
            cs.Title = "Prix échantillon médicament";
            cs.DataLabels = true;
            graph_NbMedParFamille.LegendLocation = LegendLocation.Top;


            // Graphique 2 Camembert
            PieSeries ps;
            ChartValues<int> line2;

            Dictionary<string, int> lesDatas2 = new Dictionary<string, int>();

            lesDatas2 = gst.GetDatasGraph2();

            foreach (string cle in lesDatas2.Keys)
            {
                ps = new PieSeries();
                line2 = new ChartValues<int>();
                line2.Add(lesDatas2[cle]);
                ps.Values = line2;
                ps.Title = cle;
                ps.DataLabels = true;
                graphCamembert_NbMedParFamille.Series.Add(ps);
            }
            graphCamembert_NbMedParFamille.LegendLocation = LegendLocation.Bottom;
        }

        // Graphique 3
        private void cboActions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColumnSeries cs = new ColumnSeries();
            cs.Fill = Brushes.Red;
            ChartValues<double> line3 = new ChartValues<double>();

            Dictionary<string, double> lesDatas3 = new Dictionary<string, double>();

            lesDatas3 = gst.GetDatasGraph3((cboActions.SelectedItem as Medicament).DepotLegalMed);
            foreach (string cle in lesDatas3.Keys)
            {
                line3.Add(lesDatas3[cle]);
            }
            cs.Values = line3;

            graph_MedPerturbateurPrix.Series.Clear();
            graph_MedPerturbateurPrix.AxisX.Clear();
            Axis axe = new Axis();
            axe.Labels = lesDatas3.Keys.ToList();
            graph_MedPerturbateurPrix.AxisX.Add(axe);
            graph_MedPerturbateurPrix.Series.Add(cs);
            cs.Title = "Prix par médicaments non perturbateurs";
            cs.DataLabels = true;

            graph_MedPerturbateurPrix.LegendLocation = LegendLocation.Top;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
