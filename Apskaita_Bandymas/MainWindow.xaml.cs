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
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           /* Duomenys[] I = new Duomenys[6];
            for (int i = 0; i < 6; i++)
            {
                I[i] = new Duomenys { Vardas = i.ToString(), Pavarde = "Pavarde" };
                Gridas.ItemsSource = I;
            }
            */
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
                I[i] = new Duomenys { Vardas = i.ToString(), Pavarde = "Pavarde" };
            }
            Gridas.ItemsSource = I;

        }
    }
}
