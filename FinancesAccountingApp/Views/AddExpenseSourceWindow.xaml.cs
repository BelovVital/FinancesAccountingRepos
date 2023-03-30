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
    /// Логика взаимодействия для AddExpenseSourceWindow.xaml
    /// </summary>
    public partial class AddExpenseSourceWindow : Window
    {
        public AddExpenseSourceWindow()
        {
            InitializeComponent();
            DataContext = new AddExpenseSourceViewModel(this);
        }
    }
}
