using FinancesAccountingApp.Models.DataBase.Entities;
using FinancesAccountingApp.Views;
using Microsoft.IdentityModel.Tokens;
using Prism.Commands;
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

            double expenseSumm = 0;
            foreach (var expense in Expensies)
            {
                expenseSumm += expense.Summa;
            }
            ExpenseSumm = expenseSumm.ToString();

            double incomeSumm = 0;
            foreach (var income in Incomes)
            {
                incomeSumm += income.Summa;
            }
            IncomeSumm = incomeSumm.ToString();
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

        private Income _income;
        public Income Income
        {
            get => _income;
            set
            {
                _income = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Expense> Expensies { get; set; }
        public ObservableCollection<Income> Incomes { get; set; }


        private string _expenseSumm;
        public string ExpenseSumm
        {
            get => _expenseSumm;
            set 
            {
                _expenseSumm = value;
                RaisePropertyChanged();
            }
        }

        private string _incomeSumm;
        public string IncomeSumm
        {
            get => _incomeSumm;
            set
            {
                _incomeSumm = value;
                RaisePropertyChanged();
            }
        }

        public string Summa
        {
            get => Wallet.Summa.ToString();
        }

        private DelegateCommand _showExpenseCommand;
        public DelegateCommand ShowExpenseCommand =>
            _showExpenseCommand ??= new DelegateCommand(ShowExpenseCommand_Execute, ShowExpenseCommand_CanExecute);

        private bool ShowExpenseCommand_CanExecute()
        {
            return !Expensies.IsNullOrEmpty();
        }

        private void ShowExpenseCommand_Execute()
        {
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

        private DelegateCommand _showIncomeCommand;
        public DelegateCommand ShowIncomeCommand =>
            _showIncomeCommand ??= new DelegateCommand(ShowIncomeCommand_Execute, ShowIncomeCommand_CanExecute);

        private bool ShowIncomeCommand_CanExecute()
        {
            return !Incomes.IsNullOrEmpty();
        }

        private void ShowIncomeCommand_Execute()
        {
            var dataX = new List<DateTime>();
            var dataY = new List<double>();

            foreach (var income in Incomes)
            {
                dataX.Add(income.Date);
                dataY.Add(income.Summa);
            }

            var firstDay = dataX.OrderBy(x => x).First();
            double[] xd = dataX.OrderBy(x => x).Select(x => x.ToOADate()).ToArray();
            double[] yd = dataY.ToArray();

            _chartWindow.Chart.Plot.AddScatter(xd, yd);
            _chartWindow.Chart.Plot.XAxis.DateTimeFormat(true);

            _chartWindow.Chart.Refresh();
        }
    }
}
