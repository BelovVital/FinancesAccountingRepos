using FinancesAccounting.Models.DataBase;
using FinancesAccountingApp.Models.DataBase.Entities;
using FinancesAccountingApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinancesAccountingApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            var dbContext = new AppDbContext();
            var wallets = dbContext.Wallets;
            Wallets = new ObservableCollection<Wallet>(wallets);

            if (!wallets.IsNullOrEmpty<Wallet>())
            {
                CurrentWallet = Wallets.FirstOrDefault();
                WalletNames ??= new ObservableCollection<string>(Wallets.Select(x => x.Name));
                SelectedWallet ??= WalletNames.FirstOrDefault();

                var expenses = dbContext.Expenses.Where(x => x.WalletId == CurrentWallet.Id);
                if (!expenses.IsNullOrEmpty<Expense>())
                    Expenses = new ObservableCollection<Expense>(expenses);
                RaisePropertyChanged(nameof(Expenses));

                var incomes = dbContext.Incomes.Where(x => x.WalletId == CurrentWallet.Id);
                if (!incomes.IsNullOrEmpty<Income>())
                    Incomes = new ObservableCollection<Income>(incomes);
                RaisePropertyChanged(nameof(Incomes));

                if (!expenses.IsNullOrEmpty<Expense>())
                {
                    StartDate = Expenses.Min(x => x.Date);
                    EndDate = Expenses.Max(x => x.Date);
                }
            }
        }

        private string _selectedWallet;
        public string SelectedWallet
        {
            get => _selectedWallet;
            set
            {
                _selectedWallet = value;
                RaisePropertyChanged();
            }
        }

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

        private Wallet _currentWallet;
        public Wallet CurrentWallet 
        {
            get => _currentWallet;
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }

        private Expense _selectedExpense;
        public Expense SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                _selectedExpense = value;
                DeleteExpenseCommand.RaiseCanExecuteChanged();
            }
        }

        private Income _selectedIncome;
        public Income SelectedIncome
        {
            get => _selectedIncome;
            set
            {
                _selectedIncome = value;
                DeleteIncomeCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<Wallet> Wallets { get; set; }

        private ObservableCollection<string> _walletNames;
        public ObservableCollection<string> WalletNames
        {
            get => _walletNames;
            set
            {
                _walletNames = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Expense> Expenses { get; set; }
        public ObservableCollection<Income> Incomes { get; set; }

        public bool HasCanEditOrRemoveExpense => SelectedExpense != null;
        public bool HasCanEditOrRemoveIncome => SelectedIncome != null;

        private DateTime? _startDate;

        public DateTime StartDate
        {
            get => _startDate ?? default;
            set
            {
                _startDate = value;
                RaisePropertyChanged();
                if (_endDate.HasValue && _startDate.HasValue)
                    ResetIncomesExpenses();
            }
        }

        private DateTime? _endDate;

        public DateTime EndDate
        {
            get => _endDate ?? default;
            set
            {
                _endDate = value;
                RaisePropertyChanged();
                if (_endDate.HasValue && _startDate.HasValue)
                    ResetIncomesExpenses();
            }
        }

        private DelegateCommand _addExpenseCommand;
        public DelegateCommand AddExpenseCommand =>
                    _addExpenseCommand ??= new DelegateCommand(AddCommand_Execute);

        public void AddCommand_Execute()
        {
            var expense = new Expense();
            var addWindow = new AddExpenseWindow(expense, CurrentWallet.Id);
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Expenses.Add(expense);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Expenses.Add(expense);
            }
        }

        private DelegateCommand _deleteExpenseCommand;

        public DelegateCommand DeleteExpenseCommand =>
                            _deleteExpenseCommand ??= new DelegateCommand(DeleteExpenseCommand_Execute, DeleteExpenseCommand_CanExecute);

        public void DeleteExpenseCommand_Execute()
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    DbSet<Expense> dbSet = dbContext.Set<Expense>();
                    dbSet.Remove(SelectedExpense);
                    dbContext.SaveChanges();
                }
                ObservableCollection<Expense> itemsCollection = Expenses;
                itemsCollection.Remove(SelectedExpense);
                itemsCollection = null;
                SelectedExpense = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteExpenseCommand_CanExecute()
        {
            return SelectedExpense != null;
        }

        private DelegateCommand _addIncomeCommand;
        public DelegateCommand AddIncomeCommand =>
                    _addIncomeCommand ??= new DelegateCommand(AddIncomeCommand_Execute);

        public void AddIncomeCommand_Execute()
        {
            var income = new Income();
            var addWindow = new AddIncomeWindow(income, CurrentWallet.Id);
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Incomes.Add(income);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Incomes.Add(income);
            }
        }

        private DelegateCommand _deleteIncomeCommand;

        public DelegateCommand DeleteIncomeCommand =>
                            _deleteIncomeCommand ??= new DelegateCommand(DeleteIncomeCommand_Execute, DeleteIncomeCommand_CanExecute);

        protected virtual void DeleteIncomeCommand_Execute()
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    DbSet<Income> dbSet = dbContext.Set<Income>();
                    dbContext.Remove(SelectedIncome);
                    dbContext.SaveChanges();
                }
                ObservableCollection<Income> itemsCollection = Incomes;
                itemsCollection.Remove(SelectedIncome);
                itemsCollection = null;
                SelectedExpense = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteIncomeCommand_CanExecute()
        {
            return SelectedIncome != null;
        }


        private DelegateCommand _addWalletCommand;
        public DelegateCommand AddWalletCommand =>
                    _addWalletCommand ??= new DelegateCommand(AddWalletCommand_Execute);

        public void AddWalletCommand_Execute()
        {
            var wallet = new Wallet();
            var addWindow = new AddWalletWindow(wallet);
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        dbContext.Wallets.Add(wallet);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Wallets.Add(wallet);
            }
        }

        private DelegateCommand _deleteWalletCommand;

        public DelegateCommand DeleteWalletCommand =>
                            _deleteWalletCommand ??= new DelegateCommand(DeleteWalletCommand_Execute, DeleteWalletCommand_CanExecute);

        protected virtual void DeleteWalletCommand_Execute()
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    DbSet<Income> dbSet = dbContext.Set<Income>();
                    dbSet.Remove(SelectedIncome);
                    dbContext.SaveChanges();
                }
                ObservableCollection<Income> itemsCollection = Incomes;
                itemsCollection.Remove(SelectedIncome);
                itemsCollection = null;
                SelectedExpense = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteWalletCommand_CanExecute()
        {
            return SelectedWallet != null;
        }

        private DelegateCommand _accountCommand;
        public DelegateCommand AccountCommand =>
                    _accountCommand ??= new DelegateCommand(AccountCommand_Execute);

        public void AccountCommand_Execute()
        {
            var addWindow = new ChartWindow(Incomes, Expenses, CurrentWallet);
            addWindow.ShowDialog();
        }

        public void ResetIncomesExpenses()
        {
            var dbContext = new AppDbContext();

            var expenses = dbContext.Expenses.Where(x => x.WalletId == CurrentWallet.Id
                && x.Date >= StartDate && x.Date <= EndDate);
            if (!expenses.IsNullOrEmpty())
                Expenses ??= new ObservableCollection<Expense>(expenses);
            RaisePropertyChanged(nameof(Expenses));

            var incomes = dbContext.Incomes.Where(x => x.WalletId == CurrentWallet.Id
                && x.Date >= StartDate && x.Date <= EndDate);
            if (!incomes.IsNullOrEmpty())  
                Incomes ??= new ObservableCollection<Income>(incomes);
            RaisePropertyChanged(nameof(Incomes));
        }

    }
}
