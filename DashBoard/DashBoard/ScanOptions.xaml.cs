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

namespace DashBoard
{
    /// <summary>
    /// Interaction logic for ScanOptions.xaml
    /// </summary>
    public partial class ScanOptions : Window
    {
        public ScanOptions()
        {
            InitializeComponent();
        }

        private void btnClose(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }


        private void back_Btn_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }


        private void BtnCompleteScan(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mWin2 = new MainWindow();
            mWin2.Show();

            /*try
            {
                await Escribe();
                await Lee();
                btn_log_extend.Visibility = Visibility.Visible;
            }

            catch (Exception)
            {
                
            }*/
            
        }
    }
}
