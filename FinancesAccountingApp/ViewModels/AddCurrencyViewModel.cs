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
    class AddCurrencyViewModel : BindableBase
    {
        AddCurrencyWindow _addWindow;
        public AddCurrencyViewModel(AddCurrencyWindow addWindow)
        {
            _addWindow = addWindow;

            var dbcontext = new AppDbContext();

            var currencies = dbcontext.Currencies;
            Currencies = new ObservableCollection<Currency>(currencies);
        }

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

        public ObservableCollection<Currency> Currencies { get; set; }

        private Currency _selectedCurrency;

        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
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

        private string _newScale;
        public string NewScale 
        {
            get => _newScale;
            set
            {
                _newScale = value;
                RaisePropertyChanged();
            }
        }


        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);

        private void SaveCommand_Execute()
        {
            Currency = new Currency();
            Currency.Id = Guid.Empty.Equals(Currency.Id) ? Guid.NewGuid() : Currency.Id;
            Currency.Name = NewName;
            Currency.Scale = NewScale;

            var dbContext = new AppDbContext();
            dbContext.Currencies.Add(Currency);
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
                    DbSet<Currency> dbSet = dbContext.Set<Currency>();
                    dbSet.Remove(SelectedCurrency);
                    dbContext.SaveChanges();
                }
                ObservableCollection<Currency> itemsCollection = Currencies;
                itemsCollection.Remove(SelectedCurrency);
                itemsCollection = null;
                SelectedCurrency = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteExpenseCommand_CanExecute()
        {
            return SelectedCurrency != null;
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
