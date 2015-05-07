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

        String[][] Masyvas;
        //prie uzsakymo nauji masyvai
        string[] UzsakymoEil;
        string[] Pirkejas;
        int[] UzsakytasKiekis;
        double[] UzsakymoSuma;
        int[] UzsakymoBarKodas;


        public MainWindow()
        {
            InitializeComponent();
            Eil = File.ReadAllLines("TextFile1.txt");
            Masyvas = new string[Eil.Length][];
            for (int i = 0; i < Eil.Length; i++) Masyvas[i] = new string[7];
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
                for (int j = 0; j < 6; j++) Masyvas[i][j] = Dalys[j];
                Imone[i] = Dalys[0];
                Daiktas[i] = Dalys[1];
                Kiekis[i] = int.Parse(Dalys[2]);
                VienetoPav[i] = Dalys[3];
                Kaina[i] = double.Parse(Dalys[4], System.Globalization.CultureInfo.InvariantCulture);
                BarKodas[i] = int.Parse(Dalys[5]);
                Suma[i] = Kiekis[i] * Kaina[i];
                Masyvas[i][6] = Suma[i].ToString();
            }
            PildytiLentele(Gridas, new string[] { "Imones Pavadinimas", "Prekes", "Kiekis", "Vienetas", "Kaina Eur", "BarKodas", "Suma Eur" }, Masyvas);
            PildytiComboBoxus(Cbx, Imone, "Visos Prekes");
            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < Eil.Length; i++)
            {
                Sumukas += Suma[i];
                Kiekiukas += Kiekis[i];
            }
            PildytiLentele(GridasAts, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" }, 
                new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });

            UzsakymuSkaitymas();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string getcbx = Cbx.SelectedItem.ToString();
            PildytiLentele(Gridas, new string[] { "Imones Pavadinimas", "Prekes", "Kiekis", "Vienetas", "Kaina Eur", "BarKodas", "Suma Eur" }, Masyvas, Cbx);
            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < Eil.Length; i++)
            {
                if (getcbx == Imone[i])
                {
                    Sumukas += Suma[i];
                    Kiekiukas += Kiekis[i];
                }
                else if (getcbx == "Visos Prekes")
                {
                    Sumukas += Suma[i];
                    Kiekiukas += Kiekis[i];
                }
            }
            PildytiLentele(GridasAts, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                             new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
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
                        // if (!BarKodas.Contains(int.Parse(drv[5].ToString()))) // ar nesikartoja vienodos prekes pagal barkodo
                        {
                            Imone[Gridas.Items.IndexOf(Gridas.CurrentItem)] = drv.Row[0].ToString();
                            Daiktas[Gridas.Items.IndexOf(Gridas.CurrentItem)] = drv[1].ToString();
                            Kiekis[Gridas.Items.IndexOf(Gridas.CurrentItem)] = int.Parse(drv[2].ToString());
                            VienetoPav[Gridas.Items.IndexOf(Gridas.CurrentItem)] = drv[3].ToString();
                            Kaina[Gridas.Items.IndexOf(Gridas.CurrentItem)] = double.Parse(drv[4].ToString());
                            BarKodas[Gridas.Items.IndexOf(Gridas.CurrentItem)] = int.Parse(drv[5].ToString());
                            Suma[Gridas.Items.IndexOf(Gridas.CurrentItem)] = double.Parse(drv[4].ToString()) * double.Parse(drv[2].ToString());
                            Failui = new string[Imone.Length];
                            for (int i = 0; i < Failui.Length; i++)
                                Failui[i] = Imone[i] + "|" + Daiktas[i] + "|" + Kiekis[i] + "|" + VienetoPav[i] + "|" + Kaina[i].ToString().Replace(',', '.') + "|" + BarKodas[i];
                            File.WriteAllLines("TextFile1.txt", Failui);
                            (e.Row.Item as DataRowView).Row[6] = Suma[Gridas.Items.IndexOf(Gridas.CurrentItem)].ToString(); // prideda paskaiciuota suma i ta eilute.
                            
                            double Kiekiukas = 0, Sumukas = 0;
                            for (int i = 0; i < Eil.Length; i++)
                            {
                                Sumukas += Suma[i];
                                Kiekiukas += Kiekis[i];
                            }
                            PildytiLentele(GridasAts, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                 new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
                        }
                    }
                    catch { }
                }
                (sender as DataGrid).RowEditEnding += RowEdit;// nereikia zinot
            }
            else return;
        }

        /*    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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
            } */

        private void PapildytiPriemimus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Imone = Imone.Concat(new String[] { ImoneBox.Text }).ToArray(); // imone lygu imone pridetas kazkoks elementas //concat prideda
                Daiktas = Daiktas.Concat(new String[] { PrekeBox.Text }).ToArray(); //prie masyvo prideda kita masyva taciau concat senojo masyvo nekeicia, uz tai rasomas masyvas lygu masyvas concat.
                Kiekis = Kiekis.Concat(new int[] { int.Parse(KiekisBox.Text) }).ToArray();
                VienetoPav = VienetoPav.Concat(new String[] { VienotoPavBox.Text }).ToArray();
                Kaina = Kaina.Concat(new double[] { double.Parse((KainaBox.Text)) }).ToArray();
                BarKodas = BarKodas.Concat(new int[] { int.Parse(BarKodasBox.Text) }).ToArray();
                Suma = Suma.Concat(new double[] { (double.Parse(KiekisBox.Text) * double.Parse(KainaBox.Text)) }).ToArray();
                Failui = new string[Imone.Length];
                for (int i = 0; i < Imone.Length; i++)
                {
                    Masyvas[i][0] = Imone[i];
                    Masyvas[i][1] = Daiktas[i];
                    Masyvas[i][2] = Kiekis[i].ToString();
                    Masyvas[i][3] = VienetoPav[i];
                    Masyvas[i][4] = Kaina[i].ToString().Replace(',', '.');
                    Masyvas[i][5] = BarKodas[i].ToString();
                    Masyvas[i][6] = Suma[i].ToString().Replace(',', '.');
                }
                PildytiLentele(Gridas, new string[] { "Imones Pavadinimas", "Prekes", "Kiekis", "Vienetas", "Kaina Eur", "BarKodas", "Suma Eur" }, Masyvas);

                for (int i = 0; i < Failui.Length; i++)
                    Failui[i] = Imone[i] + "|" + Daiktas[i] + "|" + Kiekis[i] + "|" + VienetoPav[i] + "|" + Kaina[i].ToString().Replace(',', '.') + "|" + BarKodas[i];
                File.WriteAllLines("TextFile1.txt", Failui);
                double Kiekiukas = 0, Sumukas = 0;
                for (int i = 0; i < Eil.Length; i++)
                {
                    Sumukas += Suma[i];
                    Kiekiukas += Kiekis[i];
                }
                PildytiLentele(GridasAts, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                    new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });

                ImoneBox.Text = "";
                PrekeBox.Text = "";
                KiekisBox.Text = "";
                VienotoPavBox.Text = "";
                KainaBox.Text = "";
                BarKodasBox.Text = "";
            }
            catch
            {
                MessageBox.Show("Neteisingi duomenys!");
            }
        }

        private void Papildytipardavimus_Click(object sender, RoutedEventArgs e)
        {
            ImoneBox1.Text = "";
            PrekeBox1.Text = "";
            KiekisBox1.Text = "";
            VienotoPavBox1.Text = "";
            KainaBox1.Text = "";
            BarKodasBox1.Text = "";
        }
        void UzsakymuSkaitymas()
        {
            DataTable L = new DataTable();

            L.Columns.Add("Pirkejas");
            L.Columns.Add("BarKodas");
            L.Columns.Add("Daiktai");
            L.Columns.Add("Kiekis");
            L.Columns.Add("Kaina");
            L.Columns.Add("Suma Eur");

            UzsakymoEil = File.ReadAllLines("Uzsakymai.txt");
            Pirkejas = new string[UzsakymoEil.Length];
            UzsakytasKiekis = new int[UzsakymoEil.Length];
            UzsakymoBarKodas = new int[UzsakymoEil.Length];
            UzsakymoSuma = new double[UzsakymoEil.Length];
            bool arpridetas = false;
            //Skaitymo funkcija
            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                String Eilute = UzsakymoEil[i];
                String[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);

                Pirkejas[i] = Dalys[0];
                UzsakymoBarKodas[i] = int.Parse(Dalys[1]);
                Daiktas[i] = Dalys[2];
                UzsakytasKiekis[i] = int.Parse(Dalys[3]);          
            }

            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                for (int j = 0; j < Eil.Length; j++)
                {

                    if (BarKodas[j] == UzsakymoBarKodas[i])
                    {
                        UzsakymoSuma[i] = Kaina[j] * UzsakytasKiekis[i];
                        L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[i], UzsakytasKiekis[i], Kaina[j].ToString().Replace(',', '.'), UzsakymoSuma[i].ToString().Replace(',', '.'));
                        arpridetas = true;
                    }
                }
                if (!arpridetas) L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[i], UzsakytasKiekis[i]);
            }

            PildytiComboBoxus(Cbx1, Pirkejas, "Visi Pirkejai");

            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                //Sumukas += UzsakymoSuma[i];
                Kiekiukas += UzsakytasKiekis[i];
            }
            PildytiLentele(GridasAts1, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
            GridasUzsakymo.DataContext = L.DefaultView;
        }

        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataTable L = new DataTable();

            L.Columns.Add("Pirkejas");
            L.Columns.Add("BarKodas");
            L.Columns.Add("Daiktai");
            L.Columns.Add("Kiekis");
            L.Columns.Add("Kaina");
            L.Columns.Add("Suma Eur");

            string getcbx = Cbx1.SelectedItem.ToString();
            // MessageBox.Show(getcbx);

            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                if (getcbx == Pirkejas[i])
                {
                    L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[i], UzsakytasKiekis[i]);
                }
                else if (getcbx == "Visi Pirkejai")
                    L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[i], UzsakytasKiekis[i]);
            }

            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                if (getcbx == Pirkejas[i])
                {
                   // Sumukas += Suma[i];
                    Kiekiukas += UzsakytasKiekis[i];
                    
                }
                else if (getcbx == "Visi Pirkejai")
                {
                    //Sumukas += Suma[i];
                    Kiekiukas += UzsakytasKiekis[i];
                   
                }
            }
            PildytiLentele(GridasAts1, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
            GridasUzsakymo.DataContext = L.DefaultView;
        }
        void PildytiLentele(DataGrid Lentele, string[] Stulpeliai, string[][] Eilutes, ComboBox Pasirink = null)
        {
            DataTable Lenta = new DataTable();
            foreach (string Stulpelis in Stulpeliai) Lenta.Columns.Add(Stulpelis);
            if (Pasirink == null)
                foreach (string[] Eilute in Eilutes) Lenta.Rows.Add(Eilute);
            else
            {
                foreach (string[] Eilute in Eilutes)
                {
                    if (Pasirink.SelectedItem.ToString() == Eilute[0]) Lenta.Rows.Add(Eilute);
                    if (Pasirink.SelectedItem.ToString() == "Visos Prekes") Lenta.Rows.Add(Eilute);
                }
            }
            Lentele.DataContext = Lenta.DefaultView;
        }
        void PildytiComboBoxus(ComboBox Boxas, string[] Pasirinkimai, string Visi)
        {
            foreach (string Pasirinkimas in Pasirinkimai)
                for (int i = 0; i < Eil.Length; i++)
                    if (!Boxas.Items.Contains(Pasirinkimas)) //jeigu kazkoks elementas neegzistuoja tame combobox'e tai vadinasi kazka ten daryt.
                        Boxas.Items.Add(Pasirinkimas);
            Boxas.Items.Add(Visi);
            Boxas.SelectedItem = Visi;
        }
    }
}