using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Windows.Media.Animation;
using System.Globalization;

namespace Apskaita
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
        String[][] UzsakymuMasyvas;
        //prie uzsakymo nauji masyvai
        string[] UzsakymoEil;
        string[] Pirkejas;
        int[] UzsakytasKiekis;
        double[] UzsakymoSuma;
        int[] UzsakymoBarKodas;

        public MainWindow()
        {
            InitializeComponent();
            Eil = File.ReadAllLines("Pirkimai.txt");
            Masyvas = new string[Eil.Length][];
            for (int i = 0; i < Eil.Length; i++) Masyvas[i] = new string[7];
            Imone = new string[Eil.Length];
            Daiktas = new string[Eil.Length];
            Kiekis = new int[Eil.Length];
            VienetoPav = new string[Eil.Length];
            Kaina = new double[Eil.Length];
            BarKodas = new int[Eil.Length];
            Suma = new double[Eil.Length];
            // skaitomi duomenys
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
            // keiciam duomenis
            try
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
            catch { }
        }
        String[] Failui;
        String[] UzsakymoFailui;
        private void RowEdit(object sender, DataGridRowEditEndingEventArgs e)
        {
            // lenteles keitimas
            if (Gridas.SelectedItem != null) // jeigu pasirinkta eilute egzistuoja
            {
                (sender as DataGrid).RowEditEnding -= RowEdit;// reikalingas norint teisingai keisti duomenis lenteleje
                (sender as DataGrid).CommitEdit();// nereikia zinot // suveiks tik tada jeigu viska busiu uzpildes
                (sender as DataGrid).Items.Refresh();
                if (e.EditAction == DataGridEditAction.Commit)  //kai nori uzbaigti reiksmiu keitima
                {
                    DataRowView drv = (DataRowView)Gridas.SelectedItem; //reiksmiu gavimas // drv kaip stulpeli galiu rasyti!!
                    try //viskas vyksta try bloke, ir jeigu ivyksta kokia klaida, pvz dalyba is nulio, tia jinai leidzia testi darba programos nenulauziant.
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
                        File.WriteAllLines("Pirkimai.txt", Failui);
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
                    catch { }
                }
                (sender as DataGrid).RowEditEnding += RowEdit;// reikalingas norint teisingai keisti duomenis lenteleje
            }
            else return;
        }
        private void PapildytiPriemimus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ImoneBox.Text) || !String.IsNullOrEmpty(PrekeBox.Text) || !String.IsNullOrEmpty(KiekisBox.Text) || !String.IsNullOrEmpty(VienotoPavBox.Text)
                    || !String.IsNullOrEmpty(KainaBox.Text) || !String.IsNullOrEmpty(BarKodasBox.Text))
                {
                    Imone = Imone.Concat(new String[] { ImoneBox.Text }).ToArray(); // imone lygu imone pridetas kazkoks elementas //concat prideda
                    Daiktas = Daiktas.Concat(new String[] { PrekeBox.Text }).ToArray(); //prie masyvo prideda kita masyva taciau concat senojo masyvo nekeicia, uz tai rasomas masyvas lygu masyvas concat.
                    Kiekis = Kiekis.Concat(new int[] { int.Parse(KiekisBox.Text) }).ToArray();
                    VienetoPav = VienetoPav.Concat(new String[] { VienotoPavBox.Text }).ToArray();
                    Kaina = Kaina.Concat(new double[] { double.Parse((KainaBox.Text)) }).ToArray();
                    BarKodas = BarKodas.Concat(new int[] { int.Parse(BarKodasBox.Text) }).ToArray();
                    Suma = Suma.Concat(new double[] { (double.Parse(KiekisBox.Text) * double.Parse(KainaBox.Text)) }).ToArray();

                    Masyvas = Masyvas.Concat(new string[][] { new string[] { ImoneBox.Text, PrekeBox.Text, KiekisBox.Text, VienotoPavBox.Text, 
                KainaBox.Text, BarKodasBox.Text, (double.Parse(KiekisBox.Text) * double.Parse(KainaBox.Text)).ToString() } }).ToArray();

                    Failui = new string[Imone.Length];
                }
                else throw new Exception("Neįvesti duomenys!");
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
                File.WriteAllLines("Pirkimai.txt", Failui);
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
                REZZ.Content = "";
                ImoneBox.Focus();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void Papildytipardavimus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BarKodas.Contains(int.Parse(BarKodasBox1.Text)))
                {
                    if (!String.IsNullOrEmpty(ImoneBox1.Text) || !String.IsNullOrEmpty(KiekisBox1.Text) || !String.IsNullOrEmpty(BarKodasBox1.Text))
                    {
                        Pirkejas = Pirkejas.Concat(new String[] { ImoneBox1.Text }).ToArray(); // imone lygu imone pridetas kazkoks elementas //concat prideda
                        UzsakytasKiekis = UzsakytasKiekis.Concat(new int[] { int.Parse(KiekisBox1.Text) }).ToArray();
                        UzsakymoBarKodas = UzsakymoBarKodas.Concat(new int[] { int.Parse(BarKodasBox1.Text) }).ToArray();
                        UzsakymoSuma = UzsakymoSuma.Concat(new double[] { 0 }).ToArray();

                        UzsakymuMasyvas = UzsakymuMasyvas.Concat(new string[][] { new string[] { ImoneBox1.Text, KiekisBox1.Text, "",
                        BarKodasBox1.Text, " ", " " } }).ToArray();

                        UzsakymoFailui = new string[Pirkejas.Length];
                    }
                    else throw new Exception("Neįvesti duomenys!");
                    bool ArRasti = true;
                    int kuris = 0;
                    for (int i = 0; i < Pirkejas.Length; i++)
                    {
                        UzsakymuMasyvas[i][0] = Pirkejas[i];
                        UzsakymuMasyvas[i][3] = UzsakytasKiekis[i].ToString();
                        for (int j = 0; j < Kaina.Length; j++)
                            if (UzsakymoBarKodas[i] == BarKodas[j])
                            {
                                UzsakymuMasyvas[i][4] = Kaina[j].ToString().Replace(',', '.');
                                UzsakymoSuma[i] = Kaina[j] * UzsakytasKiekis[i];
                                UzsakymuMasyvas[i][2] = Daiktas[j];
                                UzsakymuMasyvas[i][5] = UzsakymoSuma[i].ToString().Replace(',', '.');
                                ArRasti = true;
                                break;
                            }
                            else
                            {
                                kuris = i;
                                ArRasti = false;
                            }
                        UzsakymuMasyvas[i][1] = UzsakymoBarKodas[i].ToString();
                    }
                    if (!ArRasti)
                    {
                        UzsakymuMasyvas[kuris] = new string[5];
                    }
                    PildytiLentele(GridasUzsakymo, new string[] { "Pirkejas", "Barkodas", "Daiktai", "Kiekis", "Kaina", "Suma Eur" }, UzsakymuMasyvas);
                    PildytiComboBoxus(Cbx1, Pirkejas, "Visi Pirkejai");
                    for (int i = 0; i < UzsakymoFailui.Length; i++)
                        UzsakymoFailui[i] = UzsakymuMasyvas[i][0] + "|" + UzsakymuMasyvas[i][1] + "|" + UzsakymuMasyvas[i][3] + "|" + UzsakymuMasyvas[i][4];
                    File.WriteAllLines("Uzsakymai.txt", UzsakymoFailui);
                    double Kiekiukas = 0, Sumukas = 0;
                    for (int i = 0; i < UzsakymuMasyvas.Length; i++)
                    {
                        Sumukas += UzsakymoSuma[i];
                        Kiekiukas += UzsakytasKiekis[i];
                    }
                    PildytiLentele(GridasAts1, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                        new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
                    ImoneBox1.Text = "";
                    KiekisBox1.Text = "";
                    BarKodasBox1.Text = "";
                    ImoneBox1.Focus();
                }
                else MessageBox.Show("Bar Kodas neegzistuoja!");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            Cbx1.SelectedItem = "Visi Pirkejai";

            REZZ.Content = "";
            Likuciams();
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
            UzsakymuMasyvas = new string[UzsakymoEil.Length][];
            for (int i = 0; i < UzsakymoEil.Length; i++) UzsakymuMasyvas[i] = new string[6];
            Pirkejas = new string[UzsakymoEil.Length];
            UzsakytasKiekis = new int[UzsakymoEil.Length];
            UzsakymoBarKodas = new int[UzsakymoEil.Length];
            UzsakymoSuma = new double[UzsakymoEil.Length];
            bool arpridetas = false;
            //Skaitymas
            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                String Eilute = UzsakymoEil[i];
                String[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);

                Pirkejas[i] = Dalys[0];
                UzsakymoBarKodas[i] = int.Parse(Dalys[1]);
                UzsakytasKiekis[i] = int.Parse(Dalys[2]);
            }

            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                for (int j = 0; j < Eil.Length; j++)
                {

                    if (BarKodas[j] == UzsakymoBarKodas[i])
                    {
                        UzsakymoSuma[i] = (Kaina[j] + ((Kaina[j] * 0.21) + (Kaina[j] * 0.21) * 0.70)) * UzsakytasKiekis[i];
                        L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[j], UzsakytasKiekis[i], string.Format("{0:f2}", (Kaina[j] + ((Kaina[j] * 0.21) + (Kaina[j] * 0.21) * 0.70))).ToString().Replace(',', '.'),string.Format("{0:f2}", UzsakymoSuma[i]).Replace(',', '.'));
                        arpridetas = true;
                        UzsakymuMasyvas[i] = (new string[] { Pirkejas[i], UzsakymoBarKodas[i].ToString(), Daiktas[j],
                        UzsakytasKiekis[i].ToString(), Kaina[j].ToString().Replace(',', '.') , UzsakymoSuma[i].ToString().Replace(',', '.') });
                    }
                }
                if (!arpridetas) L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[i], UzsakytasKiekis[i]);
            }

            PildytiComboBoxus(Cbx1, Pirkejas, "Visi Pirkejai");

            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                Sumukas += UzsakymoSuma[i];
                Kiekiukas += UzsakytasKiekis[i];
            }
            PildytiLentele(GridasAts1, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
            GridasUzsakymo.DataContext = L.DefaultView;
        }

        private void ComboBox1_SelectionChanged(object sender = null, SelectionChangedEventArgs e = null)
        {
            DataTable L = new DataTable();

            L.Columns.Add("Pirkejas");
            L.Columns.Add("BarKodas");
            L.Columns.Add("Daiktai");
            L.Columns.Add("Kiekis");
            L.Columns.Add("Kaina");
            L.Columns.Add("Suma Eur");

            string getcbx = Cbx1.SelectedItem.ToString();

            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                for (int j = 0; j < BarKodas.Length; j++)
                    if (BarKodas[j] == UzsakymoBarKodas[i])
                    {
                        if (getcbx == Pirkejas[i])
                        {
                            L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[j], UzsakytasKiekis[i], string.Format("{0:f2}",(Kaina[j] + ((Kaina[j] * 0.21) + (Kaina[j] * 0.21) * 0.70))), UzsakymoSuma[i]);
                        }
                        else if (getcbx == "Visi Pirkejai")
                            L.Rows.Add(Pirkejas[i], UzsakymoBarKodas[i], Daiktas[j], UzsakytasKiekis[i], string.Format("{0:f2}",(Kaina[j] + ((Kaina[j] * 0.21) + (Kaina[j] * 0.21) * 0.70))), UzsakymoSuma[i]);
                    }
            }

            double Kiekiukas = 0, Sumukas = 0;
            for (int i = 0; i < UzsakymoEil.Length; i++)
            {
                if (getcbx == Pirkejas[i])
                {
                    Sumukas += UzsakymoSuma[i];
                    Kiekiukas += UzsakytasKiekis[i];

                }
                else if (getcbx == "Visi Pirkejai")
                {
                    Sumukas += UzsakymoSuma[i];
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
            if (!Boxas.Items.Contains(Visi)) Boxas.Items.Add(Visi);
            Boxas.SelectedItem = Visi;
        }

        private void APreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (((TextBox)sender).Text.Length < 6)
            {
                // leidzia ivest tik skaicius ir viena kableli
                char c = Convert.ToChar(e.Text);
                if (Char.IsNumber(c)) e.Handled = false;
                else if (Char.IsPunctuation(c) && (sender as TextBox).Text.Count(m => {if(m == c) return true; else return false;}) < 1) { }
                else e.Handled = true;
            }
            else e.Handled = true;
        }
        private void BarKodasBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Tikrina ar ivestas barkodas geras
            try
            {
                if (!string.IsNullOrEmpty(BarKodasBox1.Text))
                {
                    Pasiulymai.Visibility = System.Windows.Visibility.Visible;
                }
                else Pasiulymai.Visibility = System.Windows.Visibility.Collapsed;

                Pasiulymai.Items.Clear();
                for (int i = 0; i < BarKodas.Length; i++)
                {
                    REZZ.Content = "";
                    if (BarKodas[i] == int.Parse(BarKodasBox1.Text))
                    {
                        REZZ.Content = Daiktas[i] + " " + BarKodas[i];
                        Pasiulymai.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    }
                    else REZZ.Content = "Tokio Bar kodo nera!";               
                    if (BarKodas[i].ToString().Contains(BarKodasBox1.Text)) Pasiulymai.Items.Add(BarKodas[i] + "   " + Daiktas[i]); 
                }
            }
            catch { }
        }

        private void Pasirinkimo_indexas(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < BarKodas.Length; i++)
                {

                    if ((BarKodas[i] + "   " + Daiktas[i]) == Pasiulymai.SelectedItem.ToString())

                    if (BarKodas[i] + "   " + Daiktas[i] == Pasiulymai.SelectedItem.ToString())

                        BarKodasBox1.Text = BarKodas[i].ToString();

                }
            }
            catch { }
        }

        private void Likuciams()
        {
            string[][] LikMas = new string[Gridas.Items.Count][];
            for (int i = 0; i < Gridas.Items.Count; i++) LikMas[i] = new string[9]; //kiekvienai eilutei po 8 stulpelius pridedam.
            int KiekiuSumaUzsakyme = 0;
            for (int i = 0; i < Gridas.Items.Count; i++)
            {
                KiekiuSumaUzsakyme = 0;
                LikMas[i][0] = Imone[i];
                LikMas[i][1] = Daiktas[i];
                LikMas[i][2] = Kiekis[i].ToString();
                LikMas[i][6] = Suma[i].ToString();
                LikMas[i][7] = "0";
                LikMas[i][8] = "0";
                for (int j = 0; j < GridasUzsakymo.Items.Count; j++)
                {
                    if (BarKodas[i] == UzsakymoBarKodas[j])
                    {
                        KiekiuSumaUzsakyme += int.Parse((GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[j][3].ToString());
                    }
                }
                for (int j = 0; j < GridasUzsakymo.Items.Count; j++)
                {
                    if (BarKodas[i] == int.Parse((GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[j][1].ToString()))
                    {
                        LikMas[i][2] = (Kiekis[i] - KiekiuSumaUzsakyme).ToString();
                        LikMas[i][6] = (int.Parse(LikMas[i][2].ToString()) * Kaina[i]).ToString();
                        LikMas[i][7] = KiekiuSumaUzsakyme.ToString();
                        LikMas[i][8] = (KiekiuSumaUzsakyme * UzsakymoSuma[j] / UzsakytasKiekis[j]).ToString();
                    }
                }

                LikMas[i][3] = VienetoPav[i];
                LikMas[i][4] = Kaina[i].ToString();
                LikMas[i][5] = BarKodas[i].ToString();

            }
            PildytiLentele(GridasLikutis, new string[] { "Imones Pavadinimas", "Prekes", "Kiekis", "Vienetas", "Kaina Eur", "BarKodas", "Suma Eur", "Pardavimai", "Pelnas Eur" }, LikMas);
            double Kiekiukas = 0, Sumukas = 0, Pelnukas = 0, Pardavimukai = 0;
            for (int i = 0; i < Gridas.Items.Count; i++)
            {
                Sumukas += double.Parse(LikMas[i][6].ToString(), CultureInfo.InvariantCulture);
                Kiekiukas += double.Parse(LikMas[i][2].ToString(), CultureInfo.InvariantCulture);
                Pelnukas += double.Parse(LikMas[i][8].ToString(), CultureInfo.InvariantCulture);
                Pardavimukai += double.Parse(LikMas[i][7].ToString(), CultureInfo.InvariantCulture);
            }
            PildytiLentele(GridasAts3, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma", "Bendri Pardavimai", "Bendras Pelnas" },
                new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString(), Pardavimukai.ToString(), Pelnukas.ToString() } });
        }
        private void Gridas_Loaded(object sender, RoutedEventArgs e)
        {
            Likuciams();
        }

        private void TrintiPriemima_Click(object sender, RoutedEventArgs e)
        {
            if (Gridas.SelectedIndex > -1)
            {
                DataTable L = new DataTable();
                L.Merge((Gridas.ItemsSource as DataView).ToTable());
                string temp = L.Rows[Gridas.SelectedIndex][5].ToString();
                string pav = L.Rows[Gridas.SelectedIndex][0].ToString();
                L.Rows.RemoveAt(Gridas.SelectedIndex);
                Gridas.DataContext = L.DefaultView;
                double Kiekiukas = 0, Sumukas = 0;
                for (int i = 0; i < Gridas.Items.Count; i++)
                {
                    Sumukas += double.Parse((Gridas.ItemsSource as DataView).ToTable().Rows[i][6].ToString());//6
                    Kiekiukas += double.Parse((Gridas.ItemsSource as DataView).ToTable().Rows[i][2].ToString());
                }
                PildytiLentele(GridasAts, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                    new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
                string[] Failui = new string[Gridas.Items.Count];
                for (int i = 0; i < Failui.Length; i++)
                {
                    Failui[i] =
                        (Gridas.ItemsSource as DataView).ToTable().Rows[i][0].ToString() + "|" +
                        (Gridas.ItemsSource as DataView).ToTable().Rows[i][1].ToString() + "|" +
                        (Gridas.ItemsSource as DataView).ToTable().Rows[i][2].ToString() + "|" +
                        (Gridas.ItemsSource as DataView).ToTable().Rows[i][3].ToString() + "|" +
                        (Gridas.ItemsSource as DataView).ToTable().Rows[i][4].ToString() + "|" +
                        (Gridas.ItemsSource as DataView).ToTable().Rows[i][5].ToString();
                }
                File.WriteAllLines("Pirkimai.txt", Failui);

                DataTable U = new DataTable();
                U.Merge((GridasUzsakymo.ItemsSource as DataView).ToTable());
                foreach (DataRow i in U.Rows)
                {
                    if (i[1].ToString().Contains(temp))
                    {
                        U.Rows.Remove(i);
                        break;
                    }
                }
                GridasUzsakymo.DataContext = U.DefaultView;
                string[] UzsakymoFailui = new string[GridasUzsakymo.Items.Count];
                for (int i = 0; i < UzsakymoFailui.Length; i++)
                {
                    UzsakymoFailui[i] =
                        (GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[i][0].ToString() + "|" +
                        (GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[i][1].ToString() + "|" +
                        (GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[i][3].ToString();
                }
                File.WriteAllLines("Uzsakymai.txt", UzsakymoFailui);
                Likuciams();
                Cbx.Items.Remove(pav);
            }
            else MessageBox.Show("Pasirinkite kuri irasa trinti!");
        }

        private void TrintiUzsakyma_Click(object sender, RoutedEventArgs e)
        {
            if (GridasUzsakymo.SelectedIndex > -1)
            {
                DataTable L = new DataTable();
                L.Merge((GridasUzsakymo.ItemsSource as DataView).ToTable());
                string vardas = L.Rows[GridasUzsakymo.SelectedIndex][0].ToString();
                L.Rows.RemoveAt(GridasUzsakymo.SelectedIndex);
                GridasUzsakymo.DataContext = L.DefaultView;
                double Kiekiukas = 0, Sumukas = 0;
                for (int i = 0; i < GridasUzsakymo.Items.Count; i++)
                {
                    Sumukas += UzsakymoSuma[i];
                    Kiekiukas += UzsakytasKiekis[i];
                }
                PildytiLentele(GridasAts1, new string[] { "Imoniu Duomenys", "Bendras Prekiu Kiekis", "Bendru Prekiu Suma" },
                    new string[][] { new string[] { " ", Kiekiukas.ToString(), Sumukas.ToString() } });
                string[] UzsakymoFailui = new string[GridasUzsakymo.Items.Count];
                for (int i = 0; i < UzsakymoFailui.Length; i++)
                {
                    UzsakymoFailui[i] =
                        (GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[i][0].ToString() + "|" +
                        (GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[i][1].ToString() + "|" +
                        (GridasUzsakymo.ItemsSource as DataView).ToTable().Rows[i][3].ToString();
                }
                File.WriteAllLines("Uzsakymai.txt", UzsakymoFailui);
                Likuciams();
                Cbx1.Items.Remove(vardas);
            }
            else MessageBox.Show("Pasirinkite kuri irasa trinti!");
        }
    }
}