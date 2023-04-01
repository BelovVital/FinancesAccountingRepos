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
    class AddExpenseCategoryViewModel : BindableBase
    {
        AddExpenseCategoryWindow _addWindow;
        public AddExpenseCategoryViewModel(AddExpenseCategoryWindow addWindow)
        {
            _addWindow = addWindow;

            var dbcontext = new AppDbContext();

            var expenseCategories = dbcontext.ExpenseCategories;
            ExpenseCategories = new ObservableCollection<ExpenseCategory>(expenseCategories);
        }

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

        private ObservableCollection<ExpenseCategory> _expenseCategories { get; set; }
        public ObservableCollection<ExpenseCategory> ExpenseCategories 
        {
            get => _expenseCategories;
            set
            { 
                _expenseCategories = value;
                RaisePropertyChanged();
            } 
        }

        private ExpenseCategory _selectedExpenseCategory;

        public ExpenseCategory SelectedExpenseCategory
        {
            get => _selectedExpenseCategory;
            set
            {
                _selectedExpenseCategory = value;
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
            ExpenseCategory = new ExpenseCategory();
            ExpenseCategory.Id = Guid.Empty.Equals(ExpenseCategory.Id) ? Guid.NewGuid() : ExpenseCategory.Id;
            ExpenseCategory.Name = NewName;

            var dbContext = new AppDbContext();
            dbContext.ExpenseCategories.Add( ExpenseCategory );
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
                    dbContext.ExpenseCategories.Remove(SelectedExpenseCategory);
                    dbContext.SaveChanges();
                }
                ExpenseCategories.Remove(SelectedExpenseCategory);
                SelectedExpenseCategory = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteCommand_CanExecute()
        {
            return SelectedExpenseCategory != null;
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
