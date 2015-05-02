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
        public MainWindow()
        {
            InitializeComponent();

            string[] Eil = File.ReadAllLines("TextFile1.txt");
            string[] Imone = new string[Eil.Length];
            string[] Daiktas = new string[Eil.Length];
            int[] Kiekis = new int[Eil.Length];
            string[] VienetoPav = new string[Eil.Length];
            double[] Kaina = new double[Eil.Length];
            int[] BarKodas = new int[Eil.Length];


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
            }


            
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
        }
    }
}
