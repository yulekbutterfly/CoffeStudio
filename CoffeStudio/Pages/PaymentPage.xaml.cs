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
        }

        private void SaleBtn_Click(object sender, RoutedEventArgs e)
        {
            PlugBrd.Visibility = Visibility.Visible;
            PhoneBrd.Visibility = Visibility.Visible;
        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
            if (e.Text == " ")
            {
                e.Handled = true; // Запретить ввод пробелов
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
            tbTypeOfPay.Text = "Оплата наличными";
            CashToPaytb.Text = tbTotalPrice.Text.Replace(" рублей", "");
            RemainCashTb.Text = "0";
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
            CashToPaytb.Text = tbTotalPrice.Text.Replace(" рублей", "");
            RemainCashTitleTb.Text = "Картой";
            typeofPay = 3;
        }

        private void tbInserted_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbInserted.Text!="")
            {
                RemainCashTb.Text = (Convert.ToDecimal(tbInserted.Text) - Convert.ToDecimal(CashToPaytb.Text)).ToString();
            }   
        }

        private void PayOKBtn_Click(object sender, RoutedEventArgs e)
        {

            if (typeofPay == 1){
                if(Convert.ToDecimal(RemainCashTb.Text)>=0)
                {

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
        }

        void EmulateCardPay()
        {
            Random random = new Random();
            randomValue = random.Next(2) == 0;
            timer.Start();
        }

        public void ResultTimer(object sender, EventArgs e)
        {
            // Остановить таймер
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
            // Остановить таймер
            timerOK.Stop();

            GlobalVariables.preOrderList=null;
            lvOrder.ItemsSource = GlobalVariables.preOrderList;
            tbPrice.Text = "0 рублей";
            tbTotalPrice.Text = "0 рублей";
            tbNumberOfSale.Visibility = Visibility.Collapsed;
            SaleBtn.Visibility = Visibility.Visible;
            PayBrd.Visibility = Visibility.Collapsed;
        }
    }
}
