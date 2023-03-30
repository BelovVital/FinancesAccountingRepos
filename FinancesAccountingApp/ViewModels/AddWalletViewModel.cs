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

namespace FinancesAccountingApp.ViewModels
{
    public class AddWalletViewModel : BindableBase
    {
        public AddWalletViewModel(AddWalletWindow addWalletWindow, Wallet wallet)
        {
            _addWalletWindow = addWalletWindow;
            Wallet = wallet;

            var dbcontext = new AppDbContext();

            var currency = dbcontext.Currencies;
            Currencies = new ObservableCollection<Currency>(currency);
            SelectedCurrency = Currencies.FirstOrDefault();
        }

        AddWalletWindow _addWalletWindow;

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

        private string _name;
        public string Name
        {
            get => _name;
            set 
            { 
                _name = value; 
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
            }
        }

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


        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);

        public void SaveCommand_Execute()
        {
            Wallet.Name = Name;
            Wallet.Summa = double.Parse(Summa);
            Wallet.Currency = SelectedCurrency.Name;
            Wallet.Date = DateTime.Now;
            Wallet.Id = Guid.Empty.Equals(Wallet.Id) ? Guid.NewGuid() : Wallet.Id;
            _addWalletWindow.DialogResult = true;
            _addWalletWindow.Close();
        }

        public bool SaveCommand_CanExecute()
        {
            return true;
            //return !string.IsNullOrWhiteSpace(Summa) 
            //    && !string.IsNullOrWhiteSpace(Name);
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
             _cancelCommand ??= new DelegateCommand(CancelCommand_Execute);

        private void CancelCommand_Execute()
        {
            _addWalletWindow.DialogResult = false;
            _addWalletWindow.Close();
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
                        Currencies = new ObservableCollection<Currency>(currency);
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
