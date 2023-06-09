﻿using FinancesAccounting.Models.DataBase;
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
    public class AddIncomeCategoryViewModel : BindableBase
    {
        AddIncomeCategoryWindow _addWindow;
        public AddIncomeCategoryViewModel(AddIncomeCategoryWindow addWindow)
        {
            _addWindow = addWindow;

            var dbcontext = new AppDbContext();

            var incomeCategories = dbcontext.IncomeCategories;
            IncomeCategories = new ObservableCollection<IncomeCategory>(incomeCategories);
        }

        private IncomeCategory _incomeCategory;

        public IncomeCategory IncomeCategory
        {
            get => _incomeCategory;
            set 
            { 
                _incomeCategory = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<IncomeCategory> _incomeCategories;
        public ObservableCollection<IncomeCategory> IncomeCategories 
        {
            get => _incomeCategories;
            set
            { 
                _incomeCategories = value;
                RaisePropertyChanged();
            } 
        }

        private IncomeCategory _selectedIncomeCategory;

        public IncomeCategory SelectedIncomeCategory
        {
            get => _selectedIncomeCategory;
            set
            {
                _selectedIncomeCategory = value;
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
            IncomeCategory = new IncomeCategory();
            IncomeCategory.Id = Guid.Empty.Equals(IncomeCategory.Id) ? Guid.NewGuid() : IncomeCategory.Id;
            IncomeCategory.Name = NewName;

            var dbContext = new AppDbContext();
            dbContext.IncomeCategories.Add(IncomeCategory);
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
                    dbContext.IncomeCategories.Remove(SelectedIncomeCategory);
                    dbContext.SaveChanges();
                }
                IncomeCategories.Remove(SelectedIncomeCategory);
                SelectedIncomeCategory = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteCommand_CanExecute()
        {
            return SelectedIncomeCategory != null;
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
