﻿using CoffeStudio.ClassHelper;
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
        public BillPage()
        {
            InitializeComponent();

            lvSeller.ItemsSource = AppData.Context.Order.ToList();
        }
    }
}
