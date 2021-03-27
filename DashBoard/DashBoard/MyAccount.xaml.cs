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
    /// Interaction logic for MyAccount.xaml
    /// </summary>
    public partial class MyAccount : Window
    {
        public MyAccount()
        {
            InitializeComponent();
        }

        private void fn4(object sender, RoutedEventArgs e)
        {
            this.Hide();
            // MyAccount accountWin = new MyAccount();
            // accountWin.Show();
        }

        private void fn3(object sender, RoutedEventArgs e)
        {
            this.Hide();
            // MyAccount accountWin = new MyAccount();
            // accountWin.Show();

        }
    }
}
