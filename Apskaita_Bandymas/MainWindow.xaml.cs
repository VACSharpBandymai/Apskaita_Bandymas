using System;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Apskaita_Bandymas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] Eil;
        string[] Imone;
        string[] Daiktas;
        int[] Kiekis;
        string[] VienetoPav;
        double[] Kaina;
        int[] BarKodas;
        public MainWindow()
        {
            InitializeComponent();
            DataTable L = new DataTable();
            L.Columns.Add("Imones Pavadinimas");
            L.Columns.Add("Preke");
            // Va šitaip
            
            Eil = File.ReadAllLines("TextFile1.txt");
            Imone = new string[Eil.Length];
            Daiktas = new string[Eil.Length];
            Kiekis = new int[Eil.Length];
            VienetoPav = new string[Eil.Length];
            Kaina = new double[Eil.Length];
            BarKodas = new int[Eil.Length];


            for (int i = 0; i < Eil.Length; i++)
            {
                String Eilute = Eil[i];
                String[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);

                Imone[i] = Dalys[0];
                Daiktas[i] = Dalys[1];
                Kiekis[i] = int.Parse(Dalys[2]);
                VienetoPav[i] = Dalys[3];
                Kaina[i] = double.Parse(Dalys[4], System.Globalization.CultureInfo.InvariantCulture);
                BarKodas[i] = int.Parse(Dalys[5]);
                L.Rows.Add(Imone[i], Daiktas[i]);
            }

            Gridas.DataContext = L.DefaultView;                     




            
        }

        private object StringSplitOptions(string p)
        {
            throw new NotImplementedException();
        }
        public class Duomenys
        {
            public string Imones_Pavadinimas { get; set; }
            public string Daiktas { get; set; }
            public string Kiekis { get; set; }
            public string VienetoPav { get; set; }
            public string KainaEUR { get; set; }
            public string BarKodas { get; set; }

        }

        private void Pradedam(object sender, EventArgs e)
       {

            Duomenys[] I = new Duomenys[Eil.Lenght];
            for (int i = 0; i < Eil.Lenght; i++)
            {
                I[i] = new Duomenys { Imones_Pavadinimas = "bum"};
            }
            Gridas.ItemsSource = I;

            Gridas.DataContext = L.DefaultView;                     


        }
    }
}
