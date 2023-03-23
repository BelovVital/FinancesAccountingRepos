using FinancesAccounting.Models.DataBase;
using FinancesAccountingApp.Models.DataBase;
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
using System.Windows;

namespace FinancesAccounting.ViewModels
{
    internal class AddExpenseViewModel : BindableBase
    {
        AddExpenseWindow _addExpenseWindow;
        private readonly Expense _expense;
        public AddExpenseViewModel(AddExpenseWindow addExpenseWindow) 
        {
            _addExpenseWindow = addExpenseWindow;
            
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


        public ObservableCollection<ExpenseSource> Source { get; set; }
        private ExpenseSource _expenseSource;
        public ExpenseSource ExpenseSource
        {
            get=> _expenseSource;
            set
            {
                _expenseSource = value;
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
            _expense.Summa = Summa;
            _expense.Currency = Currency.Name;
            _expense.Category = ExpenseCategory.Name;
            _expense.Source = ExpenseSource.Name;
            _expense.Date = Dates.Value;
            _addExpenseWindow.DialogResult = true;
            _addExpenseWindow.Close();
        }

        public bool SaveCommand_CanExecute()
        {
            return true;
        }

        private void CancelCommand_Execute()
        {
            _addExpenseWindow.DialogResult = false;
            _addExpenseWindow.Close();
        }
    }
}
