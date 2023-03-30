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

        public ObservableCollection<ExpenseCategory> ExpenseCategories { get; set; }

        private ExpenseCategory _selectedExpenseCategory;

        public ExpenseCategory SelectedExpenseCategory
        {
            get => _selectedExpenseCategory;
            set
            {
                _selectedExpenseCategory = value;
                RaisePropertyChanged();
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


        private DelegateCommand _deleteExpenseCommand;

        public DelegateCommand DeleteExpenseCommand =>
                            _deleteExpenseCommand ??= new DelegateCommand(DeleteExpenseCommand_Execute, DeleteExpenseCommand_CanExecute);

        public void DeleteExpenseCommand_Execute()
        {
            try
            {
                using (var dbContext = new AppDbContext())
                {
                    DbSet<ExpenseCategory> dbSet = dbContext.Set<ExpenseCategory>();
                    dbSet.Remove(SelectedExpenseCategory);
                    dbContext.SaveChanges();
                }
                ObservableCollection<ExpenseCategory> itemsCollection = ExpenseCategories;
                itemsCollection.Remove(SelectedExpenseCategory);
                itemsCollection = null;
                SelectedExpenseCategory = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteExpenseCommand_CanExecute()
        {
            return SelectedExpenseCategory != null;
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
             _cancelCommand ??= new DelegateCommand(CancelCommand_Execute);

        private void CancelCommand_Execute()
        {
            _addWindow.DialogResult = false;
            _addWindow.Close();
        }
    }
}
