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
    class AddExpenseSourceViewModel : BindableBase
    {
        AddExpenseSourceWindow _addWindow;
        public AddExpenseSourceViewModel(AddExpenseSourceWindow addWindow)
        {
            _addWindow = addWindow;

            var dbcontext = new AppDbContext();

            var expenseSources = dbcontext.ExpenseSources;
            ExpenseSources = new ObservableCollection<ExpenseSource>(expenseSources);
        }

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

        private ObservableCollection<ExpenseSource> _expenseSources;
        public ObservableCollection<ExpenseSource> ExpenseSources 
        {
            get => _expenseSources;
            set
            { 
                _expenseSources = value;
                RaisePropertyChanged();
            } 
        }

        private ExpenseSource _selectedExpenseSource;

        public ExpenseSource SelectedExpenseSource
        {
            get => _selectedExpenseSource;
            set
            {
                _selectedExpenseSource = value;
                RaisePropertyChanged();
                DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private string _newName;

        public string NewName
        {
            get => _newName;
            set
            {
                _newName = value;
                RaisePropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);

        private void SaveCommand_Execute()
        {
            ExpenseSource = new ExpenseSource();
            ExpenseSource.Id = Guid.Empty.Equals(ExpenseSource.Id) ? Guid.NewGuid() : ExpenseSource.Id;
            ExpenseSource.Name = NewName;

            var dbContext = new AppDbContext();
            dbContext.ExpenseSources.Add(ExpenseSource);
            dbContext.SaveChanges();

            _addWindow.DialogResult = true;
            _addWindow.Close();
        }

        public bool SaveCommand_CanExecute()
        {
            return !string.IsNullOrWhiteSpace(NewName);
        }


        private DelegateCommand _deleteCommand;

        public DelegateCommand DeleteCommand =>
                            _deleteCommand ??= new DelegateCommand(DeleteCommand_Execute, DeleteCommand_CanExecute);

        public void DeleteCommand_Execute()
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    dbContext.ExpenseSources.Remove(SelectedExpenseSource);
                    dbContext.SaveChanges();
                }
                ExpenseSources.Remove(SelectedExpenseSource);
                SelectedExpenseSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteCommand_CanExecute()
        {
            return SelectedExpenseSource != null;
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
             _cancelCommand ??= new DelegateCommand(CancelCommand_Execute);

        private void CancelCommand_Execute()
        {
            _addWindow.DialogResult = true;
            _addWindow.Close();
        }
    }
}
