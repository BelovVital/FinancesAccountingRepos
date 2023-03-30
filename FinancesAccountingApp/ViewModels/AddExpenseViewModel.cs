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
using System.Windows;

namespace FinancesAccounting.ViewModels
{
    public class AddExpenseViewModel : BindableBase
    {
        AddExpenseWindow _addExpenseWindow; 
        public AddExpenseViewModel(AddExpenseWindow addExpenseWindow, Expense expense, Guid walletId) 
        {
            _addExpenseWindow = addExpenseWindow;
            Expense = expense;
            _walletId= walletId;
            
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

        Guid _walletId;

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

        private string _selectedExpenseCategories;
        public string SelectedCategories
        {
            get => _selectedExpenseCategories;
            set
            {
                _selectedExpenseCategories = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<string> Source { get; set; }
        private string _expenseSource;
        public string ExpenseSource
        {
            get=> _expenseSource;
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
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);

        public void SaveCommand_Execute()
        {
            Expense.Summa = double.Parse(Summa);
            Expense.Currency = SelectedCurrency;
            Expense.Category = SelectedCategories;
            Expense.Source = SelectedSource;
            Expense.Date = Dates;
            Expense.WalletId = _walletId;
            Expense.Id = Guid.Empty.Equals(Expense.Id)? Guid.NewGuid() : Expense.Id;
            _addExpenseWindow.DialogResult = true;
            _addExpenseWindow.Close();
        }

        public bool SaveCommand_CanExecute()
        {
            return !string.IsNullOrWhiteSpace(Summa);
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(CancelCommand_Execute);

        private void CancelCommand_Execute()
        {
            _addExpenseWindow.DialogResult = false;
            _addExpenseWindow.Close();
        }


        private DelegateCommand _addCurrencyCommand;
        public DelegateCommand AddCurrencyCommand =>
             _addCurrencyCommand ??= new DelegateCommand(AddCurrencyCommand_Execute);

        private void AddCurrencyCommand_Execute()
        {
            var addWindow = new AddCurrencyWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        var currency = dbContext.Currencies;
                        var _currencies = new ObservableCollection<Currency>(currency);
                        Currencies = new ObservableCollection<string>(_currencies.Select(x => x.Name));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private DelegateCommand _addCategoryCommand;
        public DelegateCommand AddCategoryCommand =>
             _addCategoryCommand ??= new DelegateCommand(AddCategoryCommand_Execute);

        private void AddCategoryCommand_Execute()
        {
            var addWindow = new AddExpenseCategoryWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        var expenseCategories = dbContext.ExpenseCategories;
                        var _categories = new ObservableCollection<ExpenseCategory>(expenseCategories);
                        Categories = new ObservableCollection<string>(expenseCategories.Select(x => x.Name));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private DelegateCommand _addSourceCommand;
        public DelegateCommand AddSourceCommand =>
             _addSourceCommand ??= new DelegateCommand(AddSourceCommand_Execute);

        private void AddSourceCommand_Execute()
        {
            var addWindow = new AddExpenseSourceWindow();
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        var expenseSource = dbContext.ExpenseSources;
                        var _source = new ObservableCollection<ExpenseSource>(expenseSource);
                        Source = new ObservableCollection<string>(expenseSource.Select(x => x.Name));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
