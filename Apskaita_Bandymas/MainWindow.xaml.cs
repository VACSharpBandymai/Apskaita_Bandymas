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
using System.Windows.Controls.Primitives;

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
        bool arIssaugotas = false;
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
                else if (getcbx == "Visos Prekes")
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
        String[] Failui;
        private void RowEdit(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (Gridas.SelectedItem != null) // jeigu pasirinkta eilute egzistuoja
            {
                (sender as DataGrid).RowEditEnding -= RowEdit;// nereikia zinot
                (sender as DataGrid).CommitEdit();// nereikia zinot // suveiks tik tada jeigu viska busiu uzpildes
                (sender as DataGrid).Items.Refresh();
                if (e.EditAction == DataGridEditAction.Commit)  //kai nori uzbaigti reiksmiu keitima
                {
                    DataRowView drv = (DataRowView)Gridas.SelectedItem; //reiksmiu gavimas // drv kaip stulpeli galiu rasyti!!
                    try //viskas vyksta try bloke, ir jeigu ivyksta kokia klaida, pvz dalyba is nulio, tia jinai leidzia testi darba programos nenulauziant.
                    {
                        if (!BarKodas.Contains(int.Parse(drv[5].ToString()))) // ar nesikartoja vienodos prekes pagal barkodo
                        {
                            Imone = Imone.Concat(new String[] { drv[0].ToString() }).ToArray(); // imone lygu imone pridetas kazkoks elementas //concat prideda
                            Daiktas = Daiktas.Concat(new String[] { drv[1].ToString() }).ToArray(); //prie masyvo prideda kita masyva taciau concat senojo masyvo nekeicia, uz tai rasomas masyvas lygu masyvas concat.
                            Kiekis = Kiekis.Concat(new int[] { int.Parse(drv[2].ToString()) }).ToArray();
                            VienetoPav = VienetoPav.Concat(new String[] { drv[3].ToString() }).ToArray();
                            Kaina = Kaina.Concat(new double[] { double.Parse(drv[4].ToString()) }).ToArray();
                            BarKodas = BarKodas.Concat(new int[] { int.Parse(drv[5].ToString()) }).ToArray();
                            Suma = Suma.Concat(new double[] { (double.Parse(drv[2].ToString()) * double.Parse(drv[4].ToString())) }).ToArray();
                            Failui = new string [Imone.Length];
                            for(int i = 0; i < Failui.Length; i++) {
                                Failui[i] = Imone[i] + "|" + Daiktas[i] + "|" + Kiekis[i] + "|" + VienetoPav[i] + "|" + Kaina[i].ToString().Replace(',', '.') + "|" + BarKodas[i];
                            }
                            saugot.IsEnabled = true;
                        }
                        //else (e.Row.Item as DataRowView).Row[6] = " ";
                        (e.Row.Item as DataRowView).Row[6] = Suma[e.Row.GetIndex()].ToString(); // prideda paskaiciuota suma i ta eilute.

                        // Va cia ir galima rasyt i faila
                       // File.WriteAllLines("TextFile1.txt", Failui);

                    }
                    catch { }
                }
                (sender as DataGrid).RowEditEnding += RowEdit;// nereikia zinot
            }
            else return;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            arIssaugotas = true;
            File.WriteAllLines("TextFile1.txt", Failui);
            saugot.IsEnabled = false;
            System.Windows.Forms.Application.Restart();

            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (arIssaugotas == false && saugot.IsEnabled == true)
            {
                MessageBoxResult Rez = MessageBox.Show("Ar norite issaugoti duomenis?", "Pranesimas", MessageBoxButton.YesNoCancel, MessageBoxImage.Information, MessageBoxResult.Cancel);
                if (Rez == MessageBoxResult.Yes)
                {
                    File.WriteAllLines("TextFile1.txt", Failui);
                    e.Cancel = false;
                }
                else if (Rez == MessageBoxResult.Cancel)
                {
                    e.Cancel = true; //tesiam programos darba, neuzdarom
                }
            }
        }

        private void PapildytiPriemimus_Click(object sender, RoutedEventArgs e)
        {
            Imone = Imone.Concat(new String[] { ImoneBox.Text }).ToArray(); // imone lygu imone pridetas kazkoks elementas //concat prideda
            Daiktas = Daiktas.Concat(new String[] { PrekeBox.Text }).ToArray(); //prie masyvo prideda kita masyva taciau concat senojo masyvo nekeicia, uz tai rasomas masyvas lygu masyvas concat.
            Kiekis = Kiekis.Concat(new int[] { int.Parse(KiekisBox.Text) }).ToArray();
            VienetoPav = VienetoPav.Concat(new String[] { VienotoPavBox.Text }).ToArray();
            Kaina = Kaina.Concat(new double[] { double.Parse((KainaBox.Text)) }).ToArray();
            BarKodas = BarKodas.Concat(new int[] { int.Parse(BarKodasBox.Text) }).ToArray();
            Suma = Suma.Concat(new double[] { (double.Parse(KiekisBox.Text) * double.Parse(KainaBox.Text)) }).ToArray();
            Failui = new string[Imone.Length];
            for (int i = 0; i < Failui.Length; i++)
            {
                Failui[i] = Imone[i] + "|" + Daiktas[i] + "|" + Kiekis[i] + "|" + VienetoPav[i] + "|" + Kaina[i].ToString().Replace(',', '.') + "|" + BarKodas[i];
            }
        }
    }
}