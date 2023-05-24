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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool IsSessionOpened=false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenShiftBtn_Click(object sender, RoutedEventArgs e)
        {
            IsSessionOpened = true;
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseShiftBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SellingBtn_Click(object sender, RoutedEventArgs e)
        {
            if(IsSessionOpened)
            {
                CashBoxWindow cashBoxWindow = new CashBoxWindow();
                this.Hide();
                cashBoxWindow.ShowDialog();
                this.ShowDialog();
            }
            else
            {
                PlugBrd.Visibility = Visibility.Visible;
                NotificationMainMenuBrd.Visibility = Visibility.Visible;
            }
            
        }

        private void CloseNoticeBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Collapsed;
            NotificationMainMenuBrd.Visibility = Visibility.Collapsed;
        }
    }
}
