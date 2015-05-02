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
            string[] Imone;
            string[] Daiktas;
            int[] Kiekis;
            string[] VienetoPav;
            int[] Kaina;
            int[] BarKodas;

            Eil = File.ReadAllLines("TextFile1.txt");

            for (int i = 0; i < Eil.Length; i++)
            {
                String Eilute = Eil[i];
                string[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);
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
