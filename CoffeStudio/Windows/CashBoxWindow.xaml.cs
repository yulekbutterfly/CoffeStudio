using CoffeStudio.Pages;
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

namespace CoffeStudio
{
    /// <summary>
    /// Логика взаимодействия для CashBoxWindow.xaml
    /// </summary>
    public partial class CashBoxWindow : Window
    {
        public CashBoxWindow()
        {
            InitializeComponent();
            frPages.Content = new PaymentPage();
        }

        private void PaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            frPages.Content = new PaymentPage();
        }

        private void SellerBtn_Click(object sender, RoutedEventArgs e)
        {
            frPages.Content = new SellerPage();
        }

        private void BillsBtn_Click(object sender, RoutedEventArgs e)
        {
            frPages.Content = new BillPage();
        }

        private void MenuBtn_Click(object sender, RoutedEventArgs e)
        {
            frPages.Content = new MenuPage();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
