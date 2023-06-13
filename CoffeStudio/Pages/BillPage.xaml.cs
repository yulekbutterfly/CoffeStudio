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
using System.Windows.Shapes;

namespace CoffeStudio.Pages
{
    /// <summary>
    /// Логика взаимодействия для BillPage.xaml
    /// </summary>
    public partial class BillPage : Page
    {
        EF.Order bill = null;
        public BillPage()
        {
            InitializeComponent();

            Filter();
        }

        private void lvSeller_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var Selectedbill = lvSeller.SelectedItem as EF.Order;
            PlugBrd.Visibility=Visibility.Visible;
            DeleteBillBrd.Visibility = Visibility.Visible;
            var dishList = AppData.Context.OrderDish
                .Where(i=> i.IDOrder== Selectedbill.IDOrder)
                .Select(dishOrder => dishOrder.Dish.DishTitle) // Получаем название блюда
                .ToList();
            string dishNames = string.Join(", ", dishList);
            BillDateTb.Text = Selectedbill.formattedDateTime;
            ContainsTb.Text=dishNames;
            SumTb.Text = Selectedbill.TotalPrice.ToString();
            bill = Selectedbill;
            
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            if (bill is EF.Order)
            {

                bill.IsDeleted = true;

                ClassHelper.AppData.Context.SaveChanges();

                Filter();
                CancelDeletion();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelDeletion();
        }

        private void Filter()
        {
            List<EF.Order> billList = AppData.Context.Order.ToList();

            billList = ClassHelper.AppData.Context.Order.Where(i => i.IsDeleted == false).ToList();

            lvSeller.ItemsSource = billList;
        }

        private void CancelDeletion()
        {
            PlugBrd.Visibility = Visibility.Collapsed;
            DeleteBillBrd.Visibility = Visibility.Collapsed;
            bill = null;
        }
    }
}
