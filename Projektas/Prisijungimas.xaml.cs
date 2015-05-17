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
using System.Windows.Shapes;

namespace Apskaita
{
    /// <summary>
    /// Interaction logic for Prisijungimas.xaml
    /// </summary>
    public partial class Prisijungimas : Window
    {
        public Prisijungimas()
        {
            InitializeComponent();
            this.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/numbers_sized.jpg")));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Vartotojas.Text == "admin" && pass.Password == "admin")
            {
                MainWindow win2 = new MainWindow();
                win2.Show();
                Close();
            }
            else MessageBox.Show("Neteisingas prisijugimas!");
        }
    }
}
