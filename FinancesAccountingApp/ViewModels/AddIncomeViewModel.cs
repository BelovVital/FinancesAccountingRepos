using FinancesAccounting.Models.DataBase;
using FinancesAccountingApp.Models.DataBase.Entities;
using FinancesAccountingApp.Views;
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
    internal class AddIncomeViewModel : BindableBase
    {
        AddIncomeWindow _addIncomeWindow;
        public AddIncomeViewModel(AddIncomeWindow addIncomeWindow, Income income)
        {
            _addIncomeWindow = addIncomeWindow;
            Income = income;

            var dbcontext = new AppDbContext();

            var currency = dbcontext.Currencies;
            var _currencies = new ObservableCollection<Currency>(currency);
            Currencies = new ObservableCollection<string>(_currencies.Select(x => x.Name));

            var expenseCategories = dbcontext.ExpenseCategories;
            var _categories = new ObservableCollection<ExpenseCategory>(expenseCategories);
            Categories = new ObservableCollection<string>(expenseCategories.Select(x => x.Name));

            var expenseSource = dbcontext.ExpenseSources;
            var _source = new ObservableCollection<ExpenseSource>(expenseSource);
            Source = new ObservableCollection<string>(expenseSource.Select(x => x.Name));

            Dates = DateTime.Now;
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

        private string _summa;
        public string Summa
        {
            get => _summa;
            set
            {
                _summa = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }


        public ObservableCollection<string> Currencies { get; set; }
        private string _currency;
        public string Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                RaisePropertyChanged();
            }
        }
        private string _selectedcurrency;
        public string SelectedCurrency
        {
            get => _selectedcurrency;
            set
            {
                _selectedcurrency = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }


        public ObservableCollection<string> Categories { get; set; }
        private string _expenseCategory;
        public string ExpenseCategory
        {
            get => _expenseCategory;
            set
            {
                _expenseCategory = value;
                RaisePropertyChanged();
            }
        }
        private string _selectedExpenseCategory;
        public string SelectedCategories
        {
            get => _selectedExpenseCategory;
            set
            {
                _selectedExpenseCategory = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<string> Source { get; set; }
        private string _expenseSource;
        public string ExpenseSource
        {
            get => _expenseSource;
            set
            {
                _expenseSource = value;
                RaisePropertyChanged();
            }
        }
        private string _selectedExpenseSource;
        public string SelectedSource
        {
            get => _selectedExpenseSource;
            set
            {
                _selectedExpenseSource = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }


        public Guid currentWallet;

        private DateTime _date;
        public DateTime Dates
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }


        private DelegateCommand _saveCommand;
        private DelegateCommand _cancelCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);
        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(CancelCommand_Execute);


        private void SaveCommand_Execute()
        {
            Income.Summa = double.Parse(Summa);
            Income.Currency = SelectedCurrency;
            Income.Category = SelectedCategories;
            Income.Source = SelectedSource;
            Income.Date = Dates;
            Income.WalletId = 
            Income.Id = Guid.Empty.Equals(Income.Id) ? Guid.NewGuid() : Income.Id;
            _addIncomeWindow.DialogResult = true;
            _addIncomeWindow.Close();
        }

        public bool SaveCommand_CanExecute()
        {
            return
                !string.IsNullOrWhiteSpace(Summa) &&
                !string.IsNullOrWhiteSpace(Currency) &&
                !string.IsNullOrWhiteSpace(ExpenseCategory) &&
                !string.IsNullOrWhiteSpace(ExpenseSource) &&
                SelectedCurrency != null &&
                SelectedCategories != null &&
                SelectedSource != null &&
                Dates != default;
        }

        private void CancelCommand_Execute()
        {
            _addIncomeWindow.DialogResult = false;
            _addIncomeWindow.Close();
        }
    }
}
