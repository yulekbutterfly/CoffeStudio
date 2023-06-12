using CoffeStudio.ClassHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace CoffeStudio.Pages
{
    /// <summary>
    /// Логика взаимодействия для PaymentPage.xaml
    /// </summary>
    public partial class PaymentPage : Page
    {
        private int typeofPay;

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerOK = new DispatcherTimer();

        bool randomValue;
        public PaymentPage()
        {
            InitializeComponent();
            lvOrder.ItemsSource = GlobalVariables.preOrderList;
            if(GlobalVariables.preOrderList.Count!=0)
            {
                decimal sum = 0;
                foreach(GlobalVariables.preOrder preOrder in GlobalVariables.preOrderList)
                {
                    sum += preOrder.sum;
                }
                tbPrice.Text = sum.ToString()+" рублей";
            }
            CountSale();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += ResultTimer;
            timerOK.Interval = TimeSpan.FromSeconds(5);
            timerOK.Tick += ResultTimerOK;

            PayOKBtn.IsEnabled = false;

            GlobalVariables.UsedEmployee = AppData.Context.Employee.Where(i=>i.IDEmployee==1).FirstOrDefault();
        }

        private void SaleBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Visible;
            PhoneBrd.Visibility = Visibility.Visible;
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) || e.Text.Contains(" "))
            {
                e.Handled = true; // Запретить ввод символов, отличных от цифр
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Collapsed;
            PhoneBrd.Visibility = Visibility.Collapsed;
            tbPhone.Text = "";
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            var authUser = ClassHelper.AppData.Context.Client.ToList().
                Where(i => i.Phone == tbPhone.Text).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(tbPhone.Text))
            {
                
            }
            else if (authUser == null)
            {
                PhoneBrd.Visibility=Visibility.Collapsed;
                RegisterBrd.Visibility= Visibility.Visible;
                tbRegisterPhone.Text = tbPhone.Text;
                tbPhone.Text="";
            }
            else
            {
                GlobalVariables.GlobalClient = authUser;
                PhoneBrd.Visibility = Visibility.Collapsed;
                BonusesBrd.Visibility = Visibility.Visible;
                btnUseBonuses.IsEnabled = false;
                tbClientBonuses.Text = authUser.Bonus.ToString();
                tbClientBirthday.Text = authUser.Birthday.ToString();
                tbClientName.Text = authUser.FirstName;
                tbClientPhone.Text = authUser.Phone;
            }
        }

        private void textBoxes_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (int.TryParse(e.Text, out int i))
            {
                e.Handled = true;
            }
            
        }

        private void CancelRegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            tbRegisterPhone.Text = "";
            tbRegistrationName.Text = "";
            dpBirthday.Text = "Выбор даты";
            RegisterBrd.Visibility = Visibility.Collapsed;
            PlugBrd.Visibility = Visibility.Collapsed;
        }

        private void OKRegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (dpBirthday.SelectedDate == null)
            {
                NotificationBrd.Visibility = Visibility.Visible;
                PlugBrdNotice.Visibility = Visibility.Visible;
                tbNotification.Text = "Укажите дату рождения!";
            }
            else
            if (tbRegisterPhone.Text == null)
            {
                NotificationBrd.Visibility = Visibility.Visible;
                PlugBrdNotice.Visibility = Visibility.Visible;
                tbNotification.Text = "Укажите номер телефона!";
            }
            else
            if (tbRegistrationName.Text == null)
            {
                NotificationBrd.Visibility = Visibility.Visible;
                PlugBrdNotice.Visibility = Visibility.Visible;
                tbNotification.Text = "Укажите имя!";
            }
            else
            {
                EF.Client newClient = new EF.Client();
                newClient.Phone = tbRegisterPhone.Text;
                newClient.FirstName = tbRegistrationName.Text;
                newClient.Birthday = Convert.ToDateTime(dpBirthday.SelectedDate);
                ClassHelper.AppData.Context.Client.Add(newClient);
                ClassHelper.AppData.Context.SaveChanges();

                GlobalVariables.GlobalClient=newClient;

                BonusesBrd.Visibility = Visibility.Visible;
                btnUseBonuses.IsEnabled = false;
                RegisterBrd.Visibility = Visibility.Collapsed;
                tbRegisterPhone.Text = "";
                tbRegistrationName.Text = "";
                dpBirthday.Text = "Введите дату";

                tbClientBonuses.Text = "0";
                tbClientBirthday.Text = newClient.Birthday.ToString();
                tbClientName.Text = newClient.FirstName;
                tbClientPhone.Text = newClient.Phone;
            }
        }

        private void CloseNoticeBtn_Click(object sender, RoutedEventArgs e)
        {
            NotificationBrd.Visibility = Visibility.Collapsed;
            PlugBrdNotice.Visibility = Visibility.Collapsed;
        }

        private void CancelBonusesBtn_Click(object sender, RoutedEventArgs e)
        {
            BonusesBrd.Visibility = Visibility.Collapsed;
            PlugBrd.Visibility = Visibility.Collapsed;
            GlobalVariables.GlobalClient = null;
            tbClientBonuses.Text = "0";
            tbClientBirthday.Text = "Дата рождения?";
            tbClientName.Text = "ИМЯ?";
            tbClientPhone.Text = "88888888888";
        }

        private void btnUseBonuses_Click(object sender, RoutedEventArgs e)
        {
            if (tbClientBonuses.Text!="0")
            {
                BonusesBrd.Visibility = Visibility.Collapsed;
                PlugBrd.Visibility = Visibility.Collapsed;
                tbClientBonuses.Text = "0";
                tbClientBirthday.Text = "Дата рождения?";
                tbClientName.Text = "ИМЯ?";
                tbClientPhone.Text = "88888888888";
                GlobalVariables.useBonuses = 1;
                GlobalVariables.amountOfBonuses =0-Convert.ToInt32(tbUseBonuses.Text);
                CountSale();
            }
            else
            {
                NotificationBrd.Visibility = Visibility.Visible;
                PlugBrdNotice.Visibility = Visibility.Visible;
                tbNotification.Text = "Бонусов нет!";

            }

            
        }

        private void btnGetBonuses_Click(object sender, RoutedEventArgs e)
        {
            BonusesBrd.Visibility = Visibility.Collapsed;
            PlugBrd.Visibility = Visibility.Collapsed;
            tbClientBonuses.Text = "0";
            tbClientBirthday.Text = "Дата рождения?";
            tbClientName.Text = "ИМЯ?";
            tbClientPhone.Text = "88888888888";
            GlobalVariables.amountOfBonuses = Convert.ToInt32(tbPrice.Text.Replace(" рублей", ""))/100;
            GlobalVariables.useBonuses = 0;
        }

        private void CountSale()
        {
            if (GlobalVariables.useBonuses != -1)
            {
                if (GlobalVariables.useBonuses == 0)
                {
                    tbNumberOfSale.Text = "Бонусы не используются";
                    tbTotalPrice.Text = tbPrice.Text;
                }
                else
                {
                    SaleBtn.Visibility = Visibility.Collapsed;
                    tbNumberOfSale.Visibility=Visibility.Visible;
                    tbNumberOfSale.Text = "Скидка: " + GlobalVariables.GlobalClient.Bonus.ToString() + " рублей";
                    tbTotalPrice.Text = (Convert.ToDecimal(tbPrice.Text.Replace(" рублей", "")) - GlobalVariables.GlobalClient.Bonus).ToString() + " рублей";
                }
            }
            else
            {
                tbTotalPrice.Text = tbPrice.Text;
            }
        }

        private void CashPayBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Visible;
            PayBrd.Visibility = Visibility.Visible;
            InputedBrd.Visibility = Visibility.Visible;
            RemainCashBrd.Visibility = Visibility.Visible;
            tbTypeOfPay.Text = "Оплата наличными";
            CashToPaytb.Text = tbTotalPrice.Text.Replace(" рублей", "");
            RemainCashTb.Text = "0";
            RemainCashTitleTb.Text = "Сдача";
            typeofPay = 1;
        }

        private void CardPayBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Visible;
            PayBrd.Visibility = Visibility.Visible;
            CashToPaytb.Text = tbTotalPrice.Text.Replace(" рублей", "");
            InputedBrd.Visibility = Visibility.Collapsed;
            RemainCashBrd.Visibility = Visibility.Collapsed;
            tbTypeOfPay.Text = "Оплата картой";
            typeofPay = 2;
        }

        private void ComplexPaymentBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Visible;
            PayBrd.Visibility = Visibility.Visible;
            InputedBrd.Visibility = Visibility.Visible;
            RemainCashBrd.Visibility = Visibility.Visible;
            CashToPaytb.Text = tbTotalPrice.Text.Replace(" рублей", "");
            RemainCashTitleTb.Text = "Картой";
            typeofPay = 3;
        }

        private void tbInserted_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tbInserted.Text)==false && Convert.ToDecimal(tbInserted.Text) - Convert.ToDecimal(CashToPaytb.Text)>=0 && RemainCashTitleTb.Text=="Сдача")
            {
                RemainCashTb.Text = (Convert.ToDecimal(tbInserted.Text) - Convert.ToDecimal(CashToPaytb.Text)).ToString();
                PayOKBtn.IsEnabled = true;
            }
            else if (string.IsNullOrWhiteSpace(tbInserted.Text) == false && Convert.ToDecimal(CashToPaytb.Text) - Convert.ToDecimal(tbInserted.Text) >= 0 && RemainCashTitleTb.Text == "Картой")
            {
                RemainCashTb.Text = (Convert.ToDecimal(CashToPaytb.Text) - Convert.ToDecimal(tbInserted.Text)).ToString();
                PayOKBtn.IsEnabled = true;
            }
            else
            {
                RemainCashTb.Text = null;
                PayOKBtn.IsEnabled = false;
            }
        }

        private void PayOKBtn_Click(object sender, RoutedEventArgs e)
        {
            if (typeofPay == 1){
                if(Convert.ToDecimal(RemainCashTb.Text)>=0)
                {
                    ExecuteOrder();
                }
                else
                {
                    NotificationBrd.Visibility = Visibility.Visible;
                    PlugBrdNotice.Visibility = Visibility.Visible;
                    tbNotification.Text = "Внесена недостаточная сумма!";
                }
            }
            if(typeofPay == 2) {
                InputedBrd.Visibility = Visibility.Collapsed;
                RemainCashBrd.Visibility = Visibility.Collapsed;
                PayOKBtn.Visibility = Visibility.Collapsed;
                CancelPayBtn.Visibility = Visibility.Collapsed;
                EmulateCardPay();
            }
            if(typeofPay==3) {
                if (Convert.ToDecimal(RemainCashTb.Text) >= 0)
                {
                    InputedBrd.Visibility= Visibility.Collapsed;
                    RemainCashBrd.Visibility= Visibility.Collapsed;
                    PayOKBtn.Visibility= Visibility.Collapsed;
                    CancelPayBtn.Visibility= Visibility.Collapsed;
                    CashToPaytb.Text=RemainCashTb.Text.Replace("-", "");
                    tbTypeOfPay.Text = "Оплата картой";
                    EmulateCardPay();
                }
                else
                {
                    NotificationBrd.Visibility = Visibility.Visible;
                    PlugBrdNotice.Visibility = Visibility.Visible;
                    tbNotification.Text = "Внесена недостаточная сумма!";
                }
            }
        }


        private void CancelPayBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Collapsed;
            PayBrd.Visibility = Visibility.Collapsed;
            tbInserted.Text = null;
            RemainCashTb.Text = null;
            PayOKBtn.IsEnabled = false;
        }

        void EmulateCardPay()
        {
            Random random = new Random();
            randomValue = random.Next(2) == 0;
            timer.Start();
        }

        public void ResultTimer(object sender, EventArgs e)
        {
            timer.Stop();

            if (randomValue)
            {
                imgResult.Visibility = Visibility.Visible;
                imgResult.Source = new BitmapImage(new Uri("/Images/PayOK.png", UriKind.Relative));
                timerOK.Start();
            }
            else
            {
                imgResult.Visibility = Visibility.Visible;
                imgResult.Source = new BitmapImage(new Uri("/Images/PayBad.png", UriKind.Relative));
                EmulateCardPay();
            }
          
        }
        public void ResultTimerOK(object sender, EventArgs e)
        {
            timerOK.Stop();

            GlobalVariables.preOrderList=null;
            lvOrder.ItemsSource = GlobalVariables.preOrderList;
            tbPrice.Text = "0 рублей";
            tbTotalPrice.Text = "0 рублей";
            tbNumberOfSale.Visibility = Visibility.Collapsed;
            SaleBtn.Visibility = Visibility.Visible;
            PayBrd.Visibility = Visibility.Collapsed;
            ExecuteOrder();
        }

        void ExecuteOrder()
        {
            if(GlobalVariables.GlobalClient != null) {
                EF.Order order = new EF.Order();
                order.IDEmployee = GlobalVariables.UsedEmployee.IDEmployee;
                order.IDClient = GlobalVariables.GlobalClient.IDClient;
                order.DateTime = DateTime.Now;
                order.Discount = (100-Convert.ToDecimal(tbTotalPrice.Text.Replace(" рублей", "")) /(Convert.ToDecimal(tbPrice.Text.Replace(" рублей", "")) /100));
                order.IsDeleted = false;
                order.IsItPostponet = false;
                AppData.Context.Order.Add(order);
                AppData.Context.SaveChanges();
                foreach (GlobalVariables.preOrder Row in GlobalVariables.preOrderList)
                {
                    EF.OrderDish orderDish = new EF.OrderDish();
                    orderDish.IDOrder = order.IDOrder;
                    orderDish.IDDish = Row.IDDish;
                    orderDish.Quntity = Row.qty;
                    orderDish.TotalPrice = Row.Cost * Row.qty;
                    AppData.Context.OrderDish.Add(orderDish);
                }
                GlobalVariables.GlobalClient.Bonus = GlobalVariables.GlobalClient.Bonus + GlobalVariables.amountOfBonuses;
                AppData.Context.SaveChanges();

                GlobalVariables.preOrderList = null;
                GlobalVariables.GlobalClient = null;
                GlobalVariables.useBonuses = -1;
                GlobalVariables.amountOfBonuses = 0;

                lvOrder.ItemsSource = GlobalVariables.preOrderList;
                PayBrd.Visibility= Visibility.Collapsed;
                RemainCashTb.Text = null;
                tbPrice.Text = "0 рублей";
                tbTotalPrice.Text="0 рублей";
                SaleBtn.Visibility = Visibility.Visible;
                PlugBrd.Visibility = Visibility.Collapsed;
            }
            else
            {
                GlobalVariables.GlobalClient = AppData.Context.Client.Where(i => i.IDClient == 5).FirstOrDefault();
                GlobalVariables.amountOfBonuses = Convert.ToInt32(Math.Round(Convert.ToDecimal(tbPrice.Text.Replace(" рублей", "")) / 100));
                EF.Order order = new EF.Order();
                order.IDEmployee = GlobalVariables.UsedEmployee.IDEmployee;
                order.IDClient = GlobalVariables.GlobalClient.IDClient;
                order.DateTime = DateTime.Now;
                order.Discount = (100 - Convert.ToDecimal(tbTotalPrice.Text.Replace(" рублей", "")) / (Convert.ToDecimal(tbPrice.Text.Replace(" рублей", "")) / 100));
                order.IsDeleted = false;
                order.IsItPostponet = false;
                AppData.Context.Order.Add(order);
                AppData.Context.SaveChanges();
                foreach (GlobalVariables.preOrder Row in GlobalVariables.preOrderList)
                {
                    EF.OrderDish orderDish = new EF.OrderDish();
                    orderDish.IDOrder = order.IDOrder;
                    orderDish.IDDish = Row.IDDish;
                    orderDish.Quntity = Row.qty;
                    orderDish.TotalPrice = Row.Cost * Row.qty;
                    AppData.Context.OrderDish.Add(orderDish);
                }               
                GlobalVariables.GlobalClient.Bonus = GlobalVariables.GlobalClient.Bonus + GlobalVariables.amountOfBonuses;
                AppData.Context.SaveChanges();

                GlobalVariables.preOrderList = null;
                GlobalVariables.GlobalClient = null;
                GlobalVariables.useBonuses = -1;
                GlobalVariables.amountOfBonuses = 0;

                lvOrder.ItemsSource = GlobalVariables.preOrderList;
                PayBrd.Visibility = Visibility.Collapsed;
                RemainCashTb.Text = null;
                tbPrice.Text = "0 рублей";
                tbTotalPrice.Text = "0 рублей";
                SaleBtn.Visibility = Visibility.Visible;
                PlugBrd.Visibility = Visibility.Collapsed;
            }
            
        }
        private void tbUseBonuses_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Запретить ввод пробела
            }
        }

        private void tbUseBonuses_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbUseBonuses.Text) == false && Convert.ToDecimal(tbClientBonuses.Text)-Convert.ToDecimal(tbUseBonuses.Text) >= 0)
            {
                btnUseBonuses.IsEnabled = true;
            }
            else
            {
                btnUseBonuses.IsEnabled = false;
            }
        }
    }
}
