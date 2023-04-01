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
        }

        private Wallet _selectedWallet;
        public Wallet SelectedWallet
        {
            get => _selectedWallet;
            set
            {
                _selectedWallet = value;
                RaisePropertyChanged();
                if (SelectedWallet != null)
                    ResetIncomesExpensesInWallet();
                else
                {
                    Expenses.Clear();
                    Incomes.Clear();
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

        public Wallet CurrentWallet {get; set;}

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

        private ObservableCollection<Wallet> _wallets;
        public ObservableCollection<Wallet> Wallets
        {
            get => _wallets;
            set
            { 
                _wallets = value;
                RaisePropertyChanged();
            } 
        }
        


        private ObservableCollection<Expense> _expenses;
        public ObservableCollection<Expense> Expenses 
        {
            get => _expenses;
            set
            { 
                _expenses = value;
                RaisePropertyChanged();
            } 
        }

        private ObservableCollection<Income> _incomes;
        public ObservableCollection<Income> Incomes 
        {
            get => _incomes;
            set
            { 
                _incomes = value;   
                RaisePropertyChanged();
            } 
        }


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
                    _addExpenseCommand ??= new DelegateCommand(AddExpenseCommand_Execute);

        public void AddExpenseCommand_Execute()
        {
            var expense = new Expense();
            var addWindow = new AddExpenseWindow(expense, SelectedWallet);
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        SelectedWallet.Summa -= expense.Summa;
                        dbContext.Wallets.Update(SelectedWallet);

                        dbContext.Expenses.Add(expense);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Expenses.Add(expense);
                SelectedExpense = null;
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
                    SelectedWallet.Summa += SelectedExpense.Summa;
                    dbContext.Wallets.Update(SelectedWallet);

                    dbContext.Expenses.Remove(SelectedExpense);
                    dbContext.SaveChanges();
                }
                Expenses.Remove(SelectedExpense);
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
            var addWindow = new AddIncomeWindow(income, SelectedWallet);
            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    using (var dbContext = new AppDbContext())
                    {
                        SelectedWallet.Summa += income.Summa;
                        dbContext.Wallets.Update(SelectedWallet);

                        dbContext.Incomes.Add(income);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Incomes.Add(income);
                SelectedIncome = null;
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
                    SelectedWallet.Summa -= SelectedIncome.Summa;
                    dbContext.Wallets.Update(SelectedWallet);

                    dbContext.Incomes.Remove(SelectedIncome);
                    dbContext.SaveChanges();
                }
                Incomes.Remove(SelectedIncome);
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
                SelectedWallet = wallet;
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
                    dbContext.Wallets.Remove(SelectedWallet);
                    dbContext.SaveChanges();
                }
                Wallets.Remove(SelectedWallet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            SelectedWallet ??= Wallets.FirstOrDefault();
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
            var addWindow = new ChartWindow(Incomes, Expenses, SelectedWallet);
            addWindow.ShowDialog();
        }


        private DelegateCommand _exitCommand;

        public DelegateCommand ExitCommand =>
                            _exitCommand ??= new DelegateCommand(ExitCommand_Execute);

        private void ExitCommand_Execute()
        {
            Environment.Exit(0);
        }

        public void ResetIncomesExpenses()
        {
            var dbContext = new AppDbContext();

            var expenses = dbContext.Expenses.Where(x => x.WalletId == SelectedWallet.Id
                && x.Date >= StartDate && x.Date <= EndDate);
            Expenses = new ObservableCollection<Expense>(expenses);
            //RaisePropertyChanged(nameof(Expenses));

            var incomes = dbContext.Incomes.Where(x => x.WalletId == SelectedWallet.Id
                && x.Date >= StartDate && x.Date <= EndDate);
            Incomes = new ObservableCollection<Income>(incomes);
            //RaisePropertyChanged(nameof(Incomes));
        }

        public void ResetIncomesExpensesInWallet()
        {
            var dbContext = new AppDbContext();

            var expenses = dbContext.Expenses.Where(x => x.WalletId == SelectedWallet.Id);
            Expenses = new ObservableCollection<Expense>(expenses);
            //RaisePropertyChanged(nameof(Expenses));

            var incomes = dbContext.Incomes.Where(x => x.WalletId == SelectedWallet.Id);
            Incomes = new ObservableCollection<Income>(incomes);
            //RaisePropertyChanged(nameof(Incomes));

            if (Expenses.Any())
            {
                StartDate = Expenses.Min(x => x.Date);
                EndDate = Expenses.Max(x => x.Date);
            }
            else 
            {
                StartDate = DateTime.Now;
                EndDate = DateTime.Now;
            }
        }

        

    }
}
