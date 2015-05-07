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
        
        //prie uzsakymo nauji masyvai
        string[] UzsakymoEil;
        string[] Pirkejas;
        int[] UzsakytasKiekis;
        double[] UzsakymoSuma;
        int[] UzsakymoBarKodas;

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
            Cbx.SelectedItem = "Visos Prekes";

            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < Eil.Length; i++)
            {
                Sumukas += Suma[i];
                Kiekiukas += Kiekis[i];
            }

            DataTable S = new DataTable();
            S.Columns.Add("Imoniu Duomenys");
            S.Columns.Add("Bendras Prekiu Kiekis");
            S.Columns.Add("Bendru Prekiu Suma");

            S.Rows.Add(" ", Kiekiukas, Sumukas);

            Gridas.DataContext = L.DefaultView;
            GridasAts.DataContext = S.DefaultView;
            UzsakymuSkaitymas();
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
                        // if (!BarKodas.Contains(int.Parse(drv[5].ToString()))) // ar nesikartoja vienodos prekes pagal barkodo
                        {
                            /*                            Imone = Imone.Concat(new String[] { drv[0].ToString() }).ToArray(); // imone lygu imone pridetas kazkoks elementas //concat prideda
                                                        Daiktas = Daiktas.Concat(new String[] { drv[1].ToString() }).ToArray(); //prie masyvo prideda kita masyva taciau concat senojo masyvo nekeicia, uz tai rasomas masyvas lygu masyvas concat.
                                                        Kiekis = Kiekis.Concat(new int[] { int.Parse(drv[2].ToString()) }).ToArray();
                                                        VienetoPav = VienetoPav.Concat(new String[] { drv[3].ToString() }).ToArray();
                                                        Kaina = Kaina.Concat(new double[] { double.Parse(drv[4].ToString()) }).ToArray();
                                                        BarKodas = BarKodas.Concat(new int[] { int.Parse(drv[5].ToString()) }).ToArray();
                                                        Suma = Suma.Concat(new double[] { (double.Parse(drv[2].ToString()) * double.Parse(drv[4].ToString())) }).ToArray();
                             */

                            Imone[Gridas.Items.IndexOf(Gridas.CurrentItem)] = drv.Row[0].ToString();
                            Daiktas[Gridas.Items.IndexOf(Gridas.CurrentItem)] = drv[1].ToString();
                            Kiekis[Gridas.Items.IndexOf(Gridas.CurrentItem)] = int.Parse(drv[2].ToString());
                            VienetoPav[Gridas.Items.IndexOf(Gridas.CurrentItem)] = drv[3].ToString();
                            Kaina[Gridas.Items.IndexOf(Gridas.CurrentItem)] = double.Parse(drv[4].ToString());
                            BarKodas[Gridas.Items.IndexOf(Gridas.CurrentItem)] = int.Parse(drv[5].ToString());
                            Suma[Gridas.Items.IndexOf(Gridas.CurrentItem)] = double.Parse(drv[4].ToString()) * double.Parse(drv[2].ToString());
                            Failui = new string[Imone.Length];
                            for (int i = 0; i < Failui.Length; i++)
                            {
                                Failui[i] = Imone[i] + "|" + Daiktas[i] + "|" + Kiekis[i] + "|" + VienetoPav[i] + "|" + Kaina[i].ToString().Replace(',', '.') + "|" + BarKodas[i];
                            }
                            File.WriteAllLines("TextFile1.txt", Failui);
                            (e.Row.Item as DataRowView).Row[6] = Suma[Gridas.Items.IndexOf(Gridas.CurrentItem)].ToString(); // prideda paskaiciuota suma i ta eilute.
                            
                            double Kiekiukas = 0, Sumukas = 0;
                            for (int i = 0; i < Eil.Length; i++)
                            {
                                Sumukas += Suma[i];
                                Kiekiukas += Kiekis[i];
                            }

                            DataTable S = new DataTable();
                            S.Columns.Add("Imoniu Duomenys");
                            S.Columns.Add("Bendras Prekiu Kiekis");
                            S.Columns.Add("Bendru Prekiu Suma");

                            S.Rows.Add(" ", Kiekiukas, Sumukas);
                            GridasAts.DataContext = S.DefaultView;
                        }
                        //else (e.Row.Item as DataRowView).Row[6] = " ";

                        // Va cia ir galima rasyt i faila
                        // File.WriteAllLines("TextFile1.txt", Failui);

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

            Imone = Imone.Concat(new String[] { ImoneBox.Text }).ToArray(); // imone lygu imone pridetas kazkoks elementas //concat prideda
            Daiktas = Daiktas.Concat(new String[] { PrekeBox.Text }).ToArray(); //prie masyvo prideda kita masyva taciau concat senojo masyvo nekeicia, uz tai rasomas masyvas lygu masyvas concat.
            Kiekis = Kiekis.Concat(new int[] { int.Parse(KiekisBox.Text) }).ToArray();
            VienetoPav = VienetoPav.Concat(new String[] { VienotoPavBox.Text }).ToArray();
            Kaina = Kaina.Concat(new double[] { double.Parse((KainaBox.Text)) }).ToArray();
            BarKodas = BarKodas.Concat(new int[] { int.Parse(BarKodasBox.Text) }).ToArray();
            Suma = Suma.Concat(new double[] { (double.Parse(KiekisBox.Text) * double.Parse(KainaBox.Text)) }).ToArray();
            Failui = new string[Imone.Length];
            DataTable R = new DataTable();
            R.Columns.Add("Imones Pavadinimas");
            R.Columns.Add("Prekes");
            R.Columns.Add("Kiekis");
            R.Columns.Add("Vienetas");
            R.Columns.Add("Kaina Eur");
            R.Columns.Add("BarKodas");
            R.Columns.Add("Suma Eur");


            for (int i = 0; i < Failui.Length; i++)
            {
                Failui[i] = Imone[i] + "|" + Daiktas[i] + "|" + Kiekis[i] + "|" + VienetoPav[i] + "|" + Kaina[i].ToString().Replace(',', '.') + "|" + BarKodas[i];
                R.Rows.Add(Imone[i], Daiktas[i], Kiekis[i], VienetoPav[i], Kaina[i], BarKodas[i], Suma[i]);
            }
            File.WriteAllLines("TextFile1.txt", Failui);
            Gridas.DataContext = R.DefaultView;
            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < Eil.Length; i++)
            {
                Sumukas += Suma[i];
                Kiekiukas += Kiekis[i];
            }

            DataTable S = new DataTable();
            S.Columns.Add("Imoniu Duomenys");
            S.Columns.Add("Bendras Prekiu Kiekis");
            S.Columns.Add("Bendru Prekiu Suma");

            S.Rows.Add(" ", Kiekiukas, Sumukas);
            GridasAts.DataContext = S.DefaultView;

            ImoneBox.Text = "";
            PrekeBox.Text = "";
            KiekisBox.Text = "";
            VienotoPavBox.Text = "";
            KainaBox.Text = "";
            BarKodasBox.Text = "";
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


            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                if (!Cbx1.Items.Contains(Imone[i])) //jeigu kazkoks elementas neegzistuoja tame combobox'e tai vadinasi kazka ten daryt.
                {
                    Cbx1.Items.Add(Pirkejas[i]);
                }
            }
            Cbx1.Items.Add("Visi Pirkejai");
            Cbx1.SelectedItem = "Visi Pirkejai";

            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                //Sumukas += UzsakymoSuma[i];
                Kiekiukas += UzsakytasKiekis[i];
            }

            DataTable S = new DataTable();
            S.Columns.Add("Imoniu Duomenys");
            S.Columns.Add("Bendras Prekiu Kiekis");
            S.Columns.Add("Bendru Prekiu Suma");

            S.Rows.Add(" ", Kiekiukas, Sumukas);

            GridasUzsakymo.DataContext = L.DefaultView;

            GridasAts1.DataContext = S.DefaultView;
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

            DataTable S = new DataTable();
            S.Columns.Add("Imoniu Duomenys");
            S.Columns.Add("Bendras Prekiu Kiekis");
            S.Columns.Add("Bendru Prekiu Suma");

            S.Rows.Add(" ", Kiekiukas, Sumukas);

            GridasUzsakymo.DataContext = L.DefaultView;
            GridasAts1.DataContext = S.DefaultView;
        }
    }
}