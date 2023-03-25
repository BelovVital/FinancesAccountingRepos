using FinancesAccountingApp.Models.DataBase.Entities;
using FinancesAccountingApp.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancesAccountingApp.ViewModels
{
    public class ChartWindowViewModel : BindableBase
    {
        public ChartWindowViewModel(
            ChartWindow charWindow, 
            ObservableCollection<Income> incomes,
            ObservableCollection<Expense> expensies,
            Wallet wallet)
        {
            _chartWindow = charWindow;
            Expensies = expensies;
            Incomes = incomes;
            Wallet = wallet;

            Expensies.OrderBy(x => x.Date);
            Incomes.OrderBy(x => x.Date);

            var dataX = new List<DateTime>();
            var dataY = new List<double>();

            foreach (var expense in Expensies)
            {
                dataX.Add(expense.Date);
                dataY.Add(expense.Summa);
            }

            var firstDay = dataX.OrderBy(x => x).First();
            double[] xd = dataX.OrderBy(x => x).Select(x => x.ToOADate()).ToArray();
            double[] yd = dataY.ToArray();

            _chartWindow.Chart.Plot.AddScatter(xd, yd);
            _chartWindow.Chart.Plot.XAxis.DateTimeFormat(true);

            _chartWindow.Chart.Refresh();

        }

        ChartWindow _chartWindow;

        private Wallet _wallet;

        public Wallet Wallet
        {
            get => _wallet;
            set
            {
                _wallet = value;
                RaisePropertyChanged();
            }
        }

        private Expense _expense;
        public Expense Expense
        {
            get => _expense;
            set
            {
                _expense = value;
                RaisePropertyChanged();
            }
        }

        private void RaiseCanExecuteChanged()
        {
            throw new NotImplementedException();
        }

        private Income _selectedIncome;
        public Income SelectedIncome
        {
            get => _selectedIncome;
            set
            {
                _selectedIncome = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Expense> Expensies { get; set; }
        public ObservableCollection<Income> Incomes { get; set; }


    }
}
