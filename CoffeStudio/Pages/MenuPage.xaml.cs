using CoffeStudio.EF;
using System;
using System.Collections;
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
using CoffeStudio.ClassHelper;

namespace CoffeStudio.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        EF.Dish dish = new EF.Dish();

        public MenuPage()
        {
            InitializeComponent();
            lvMenu.ItemsSource = ClassHelper.AppData.Context.Dish.Where(i => i.IDCategory == 1).ToList();
        }

        private void HotsBtn_Click(object sender, RoutedEventArgs e)
        {
            HotsBtn.BorderBrush = new SolidColorBrush(Colors.Black);
            DrinksBtn.BorderBrush = null;
            SaladsBtn.BorderBrush = null;
            DessertsBtn.BorderBrush = null;
            lvMenu.ItemsSource = ClassHelper.AppData.Context.Dish.Where(i => i.IDCategory == 2).ToList();
        }

        private void DrinksBtn_Click(object sender, RoutedEventArgs e)
        {
            HotsBtn.BorderBrush = null;
            DrinksBtn.BorderBrush = new SolidColorBrush(Colors.Black);
            SaladsBtn.BorderBrush = null;
            DessertsBtn.BorderBrush = null;
            lvMenu.ItemsSource = ClassHelper.AppData.Context.Dish.Where(i => i.IDCategory == 1).ToList();
        }

        private void SaladsBtn_Click(object sender, RoutedEventArgs e)
        {
            HotsBtn.BorderBrush = null;
            DrinksBtn.BorderBrush = null;
            SaladsBtn.BorderBrush = new SolidColorBrush(Colors.Black);
            DessertsBtn.BorderBrush = null;
            lvMenu.ItemsSource = ClassHelper.AppData.Context.Dish.Where(i => i.IDCategory == 3).ToList();
        }

        private void DessertsBtn_Click(object sender, RoutedEventArgs e)
        {
            HotsBtn.BorderBrush = null;
            DrinksBtn.BorderBrush = null;
            SaladsBtn.BorderBrush = null;
            DessertsBtn.BorderBrush = new SolidColorBrush(Colors.Black);
            lvMenu.ItemsSource = ClassHelper.AppData.Context.Dish.Where(i => i.IDCategory == 4).ToList();
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            HotsBtn.BorderBrush = null;
            DrinksBtn.BorderBrush = null;
            SaladsBtn.BorderBrush = null;
            DessertsBtn.BorderBrush = null;
            Filter();
        }

        private void Filter()
        {
            List<EF.Dish> listDish = new List<EF.Dish>();
            listDish = ClassHelper.AppData.Context.Dish.Where(i => i.IsDeleted == false).ToList();

            listDish = listDish.
                Where(i => i.DishTitle.ToLower().Contains(SearchTb.Text.ToLower())
                || i.IDDish.ToString().Contains(SearchTb.Text.ToLower())).ToList();

            lvMenu.ItemsSource = listDish;
        }

        private void lvMenu_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            dish = lvMenu.SelectedItem as EF.Dish;
            PlugBrd.Visibility= Visibility.Visible;
            QTYBrd.Visibility = Visibility.Visible;
            TbMaxQTY.Text ="(Максимум: "+dish.Remains.ToString()+")";
            
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            dish = null;
            PlugBrd.Visibility = Visibility.Collapsed;
            QTYBrd.Visibility = Visibility.Collapsed;
            tbQTY.Text = "1";
        }

        private void MinusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tbQTY.Text !="1")
            {
                tbQTY.Text = (Convert.ToInt32(tbQTY.Text) - 1).ToString();
            }
        }

        private void PlusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(tbQTY.Text) < dish.Remains)
            {
                tbQTY.Text = (Convert.ToInt32(tbQTY.Text) + 1).ToString();
            }
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            dish = lvMenu.SelectedItem as EF.Dish;
            GlobalVariables.preOrder preOrder = new GlobalVariables.preOrder()
            {
                IDDish = dish.IDDish,
                DishTitle = dish.DishTitle,
                Cost = dish.Cost,
                qty = Convert.ToInt32(tbQTY.Text)
            };
            GlobalVariables.preOrderList.Add(preOrder);
            dish.Remains = dish.Remains - Convert.ToInt32(tbQTY.Text);
            ClassHelper.AppData.Context.SaveChanges();
            NavigationService.Navigate(new PaymentPage());
        }
    }

}
