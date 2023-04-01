using FinancesAccountingApp.Models.DataBase.Entities;
using FinancesAccountingApp.ViewModels;
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

namespace FinancesAccountingApp.Views
{
    /// <summary>
    /// Логика взаимодействия для AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        public AddIncomeWindow(Income income, Wallet wallet)
        {
            InitializeComponent();
            DataContext = new AddIncomeViewModel(this, income, wallet);
        }
    }
}
