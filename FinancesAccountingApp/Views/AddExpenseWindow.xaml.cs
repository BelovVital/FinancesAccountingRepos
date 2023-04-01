using FinancesAccounting.ViewModels;
using FinancesAccountingApp.Models.DataBase.Entities;
using System;
﻿using System;
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

namespace FinancesAccountingApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AddExpenseWindow.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window
    {
        public AddExpenseWindow(Expense expense, Wallet wallet)
        {
            InitializeComponent();
            DataContext = new AddExpenseViewModel(this, expense, wallet);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
