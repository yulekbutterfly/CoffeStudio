using CoffeStudio.ClassHelper;
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

namespace CoffeStudio.Pages
{
    /// <summary>
    /// Логика взаимодействия для SellerPage.xaml
    /// </summary>
    public partial class SellerPage : Page
    {
        EF.Employee CheckEmployee = new EF.Employee();
        public SellerPage()
        {
            InitializeComponent();
            lvSellers.ItemsSource= ClassHelper.AppData.Context.Employee.ToList();
        }

        private void lvSellers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PassBrd.Visibility = Visibility.Visible;
            PlugBrd.Visibility = Visibility.Visible;
            CheckEmployee = lvSellers.SelectedItem as EF.Employee;
        }

        private void CheckPassBtn_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(PassPb.Password)==false)
            {
                if(CheckEmployee.Password==PassPb.Password)
                {
                    GlobalVariables.UsedEmployee = CheckEmployee;
                    PassBrd.Visibility = Visibility.Collapsed;
                    PlugBrd.Visibility = Visibility.Collapsed;
                    CheckEmployee = null;
                }
                else
                {
                    IncorrectPassTb.Visibility = Visibility.Visible;
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            PassBrd.Visibility = Visibility.Collapsed;
            PlugBrd.Visibility = Visibility.Collapsed;
            CheckEmployee = null;
            PassPb.Password = null;
        }
    }
}
