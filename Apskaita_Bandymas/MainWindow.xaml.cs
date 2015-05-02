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

            string[] Eil;
            string[] Imone = new string[6];
            string[] Daiktas = new string[6];
            int[] Kiekis = new int[6];
            string[] VienetoPav = new string[6];
            int[] Kaina = new int[6];
            int[] BarKodas = new int[6];

            Eil = File.ReadAllLines("TextFile1.txt");

            for (int i = 0; i < Eil.Length; i++)
            {
                String Eilute = Eil[i];
                String[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);

                Imone[i] = Dalys[0];
                Daiktas[i] = Dalys[1];
                Kiekis[i] = int.Parse(Dalys[2]);
                VienetoPav[i] = Dalys[3];
                Kaina[i] = int.Parse(Dalys[4]);
                BarKodas[i] = int.Parse(Dalys[5]);
            }


            
        }

        private object StringSplitOptions(string p)
        {
            throw new NotImplementedException();
        }
        public class Duomenys
        {
            public string Vardas { get; set; }
            public string Pavarde { get; set; }
        }
        private void Pradedam(object sender, EventArgs e)
       {
            Duomenys[] I = new Duomenys[6];
            for (int i = 0; i < 6; i++)
            {
                I[i] = new Duomenys { Vardas = i.ToString(), Pavarde = i.ToString() };
            }
            Gridas.ItemsSource = I;
        }
    }
}
