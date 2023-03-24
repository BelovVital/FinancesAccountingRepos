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
        private readonly Income _income;
        public AddIncomeViewModel(AddIncomeWindow addIncomeWindow)
        {
            _addIncomeWindow = addIncomeWindow;

            var dbcontext = new AppDbContext();

            var currency = dbcontext.Currencies;
            Currencies = new ObservableCollection<Currency>(currency);

            var expenseCategories = dbcontext.ExpenseCategories;
            Categories = new ObservableCollection<ExpenseCategory>(expenseCategories);

            var expenseSource = dbcontext.ExpenseSources;
            Source = new ObservableCollection<ExpenseSource>(expenseSource);
        }


        private double _summa;
        private double Summa
        {
            get => _summa;
            set
            {
                _summa = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<Currency> Currencies { get; set; }
        private Currency _currency;
        public Currency Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                RaisePropertyChanged();
            }
        }
        private Currency _selectedcurrency;
        private Currency SelectedCurrency
        {
            get => _selectedcurrency;
            set
            {
                _selectedcurrency = value;
                RaisePropertyChanged();
            }
        }


        public ObservableCollection<ExpenseCategory> Categories { get; set; }
        private ExpenseCategory _expenseCategory;
        public ExpenseCategory ExpenseCategory
        {
            get => _expenseCategory;
            set
            {
                _expenseCategory = value;
                RaisePropertyChanged();
            }
        }
        private ExpenseCategory _selectedExpenseCategory;
        private ExpenseCategory SelectedCategory
        {
            get => _selectedExpenseCategory;
            set
            {
                _selectedExpenseCategory = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<ExpenseSource> Source { get; set; }
        private ExpenseSource _expenseSource;
        public ExpenseSource ExpenseSource
        {
            get => _expenseSource;
            set
            {
                _expenseSource = value;
                RaisePropertyChanged();
            }
        }
        private ExpenseSource _selectedExpenseSource;
        private ExpenseSource SelectedSource
        {
            get => _selectedExpenseSource;
            set
            {
                _selectedExpenseSource = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? _date;
        public DateTime? Dates
        {
            get => _date;
            set
            {
                _date = value;
                RaisePropertyChanged();
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
            _income.Summa = Summa;
            _income.Currency = Currency.Name;
            _income.Category = ExpenseCategory.Name;
            _income.Source = ExpenseSource.Name;
            _income.Date = Dates.Value;
            _addIncomeWindow.DialogResult = true;
            _addIncomeWindow.Close();
        }

        public bool SaveCommand_CanExecute()
        {
            return
                Summa != default &&
                Currency != default &&
                ExpenseCategory != default &&
                ExpenseSource != default &&
                Dates != default;
        }

        private void CancelCommand_Execute()
        {
            _addIncomeWindow.DialogResult = false;
            _addIncomeWindow.Close();
        }
    }
}
