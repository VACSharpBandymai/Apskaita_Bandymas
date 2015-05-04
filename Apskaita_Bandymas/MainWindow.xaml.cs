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
        double[] Suma;

        public MainWindow()
        {
            InitializeComponent();

            DataTable L = new DataTable();

            L.Columns.Add("Imones Pavadinimas");
            L.Columns.Add("Prekes");
            L.Columns.Add("Kiekis");
            L.Columns.Add("Vienetas");
            L.Columns.Add("Kaina Eur");
            L.Columns.Add("BarKodas");
            L.Columns.Add("Suma Eur");

            Eil = File.ReadAllLines("TextFile1.txt");
            Imone = new string[Eil.Length];
            Daiktas = new string[Eil.Length];
            Kiekis = new int[Eil.Length];
            VienetoPav = new string[Eil.Length];
            Kaina = new double[Eil.Length];
            BarKodas = new int[Eil.Length];
            Suma = new double[Eil.Length];

            //Skaitymo funkcija
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
                Suma[i] = Kiekis[i] * Kaina[i];
                L.Rows.Add(Imone[i], Daiktas[i], Kiekis[i], VienetoPav[i], Kaina[i], BarKodas[i], Suma[i]);
            }
            //Comboxo lenteles mygtuku atsiradimas
            for (int i = 0; i < Eil.Length; i++)
            {
                if (!Cbx.Items.Contains(Imone[i])) //jeigu kazkoks elementas neegzistuoja tame combobox'e tai vadinasi kazka ten daryt.
                {
                    Cbx.Items.Add(Imone[i]);
                }
            }
            Cbx.Items.Add("Visos Prekes");

            double Kiekiukas = 0, Imoniusk = 0, Daiktuk = 0, Sumukas = 0;
            for (int i = 0; i < Eil.Length; i++)
            { 
                Sumukas += Suma[i];
                Kiekiukas += Kiekis[i];
                Imoniusk++;
                Daiktuk++;
            }

            DataTable S = new DataTable();
            S.Columns.Add("Imoniu Duomenys");
            S.Columns.Add("Bendras Prekiu Kiekis");
            S.Columns.Add("Bendru Prekiu Suma");

            S.Rows.Add(" ", Kiekiukas, Sumukas);

            Gridas.DataContext = L.DefaultView;
            GridasAts.DataContext = S.DefaultView;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataTable L = new DataTable();

            L.Columns.Add("Imones Pavadinimas");
            L.Columns.Add("Prekes");
            L.Columns.Add("Kiekis");
            L.Columns.Add("Vienetas");
            L.Columns.Add("Kaina Eur");
            L.Columns.Add("BarKodas");
            L.Columns.Add("Suma Eur");

            string getcbx = Cbx.SelectedItem.ToString();
            // MessageBox.Show(getcbx);

            for (int i = 0; i < Eil.Length; i++)
            {
                if (getcbx == Imone[i])
                {
                    L.Rows.Add(Imone[i], Daiktas[i], Kiekis[i], VienetoPav[i], Kaina[i], BarKodas[i], Suma[i]);
                }
                else if(getcbx == "Visos Prekes")
                    L.Rows.Add(Imone[i], Daiktas[i], Kiekis[i], VienetoPav[i], Kaina[i], BarKodas[i], Suma[i]);
            }

            double Kiekiukas = 0, Imoniusk = 0, Daiktuk = 0, Sumukas = 0;
            for (int i = 0; i < Eil.Length; i++)
            {
                if (getcbx == Imone[i])
                {
                        Sumukas += Suma[i];
                        Kiekiukas += Kiekis[i];
                        Imoniusk++;
                        Daiktuk++;
                }
                else if (getcbx == "Visos Prekes")
                {
                    Sumukas += Suma[i];
                    Kiekiukas += Kiekis[i];
                    Imoniusk++;
                    Daiktuk++;
                }
            }

            DataTable S = new DataTable();
            S.Columns.Add("Imoniu Duomenys");
            S.Columns.Add("Bendras Prekiu Kiekis");
            S.Columns.Add("Bendru Prekiu Suma");

            S.Rows.Add(" ", Kiekiukas, Sumukas);
            
            Gridas.DataContext = L.DefaultView;
            GridasAts.DataContext = S.DefaultView;
        }

        private void RowEdit(object sender, DataGridRowEditEndingEventArgs e)
        {
            Imone = new string[Imone.Length + 1];
            Daiktas = new string[Daiktas.Length + 1];
            Kiekis = new int[Kiekis.Length + 1];
            VienetoPav = new string[VienetoPav.Length + 1];
            Kaina = new double[Kaina.Length + 1];
            BarKodas = new int[BarKodas.Length + 1];
            Suma = new double[Suma.Length + 1];

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
                Suma[i] = Kiekis[i] * Kaina[i];
            }
            DataRowView datarow = (DataRowView)Gridas.SelectedItem;
            Imone[Imone.Length - 1] = Gridas.SelectedItems[0].ToString();
            MessageBox.Show(Imone[Imone.Length - 1]);
        }     
    }
}