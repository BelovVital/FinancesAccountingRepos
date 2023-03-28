using FinancesAccounting.Models.DataBase;
using FinancesAccountingApp.Models.DataBase.Entities;
using FinancesAccountingApp.Views;
using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancesAccountingApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            var dbContext = new AppDbContext();

            var wallet = dbContext.Wallets;
            Wallets = new ObservableCollection<Wallet>();

            var expenses = dbContext.Expenses;
            Expenses = new ObservableCollection<Expense>(expenses);

            var incomes = dbContext.Incomes;
            Incomes = new ObservableCollection<Income>(incomes);
        }

        private Wallet _selectedwallet;

        public Wallet SelectedWallet
        {
            get => _selectedwallet;
            set
            {
                _selectedwallet = value;
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
        public ObservableCollection<Expense> Expenses { get; set; }
        public ObservableCollection<Income> Incomes { get; set; }

        public bool HasCanEditOrRemoveExpense => SelectedExpense != null;
        public bool HasCanEditOrRemoveIncome => SelectedIncome != null;

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                RaisePropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                RaisePropertyChanged();
            }
        }

        private DelegateCommand _addExpenseCommand;
        public DelegateCommand AddExpenseCommand =>
                    _addExpenseCommand ??= new DelegateCommand(AddCommand_Execute);

        public void AddCommand_Execute()
        {
            var expense = new Expense();
            var addWindow = new AddExpenseWindow(expense, SelectedWallet.Id);
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
            var addWindow = new AddIncomeWindow(income, SelectedWallet.Id);
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
        public bool DeleteWalletCommand_CanExecute()
        {
            return SelectedWallet != null;
        }
    }
}
