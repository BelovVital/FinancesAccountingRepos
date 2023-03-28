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

namespace FinancesAccountingApp.ViewModels
{
    public class AddWalletViewModel : BindableBase
    {
        public AddWalletViewModel(AddWalletWindow addWalletWindow, Wallet wallet)
        {
            _addWalletWindow = addWalletWindow;

            var dbcontext = new AppDbContext();

            var currency = dbcontext.Currencies;
            var _currencies = new ObservableCollection<Currency>(currency);
            Currencies = new ObservableCollection<string>(_currencies.Select(x => x.Name));
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

        private string _Name;
        public string Name
        {
            get => _Name;
            set 
            { 
                _Name = value; 
                RaisePropertyChanged();
            }
        }

        private string _Summa;
        public string Summa
        {
            get => _Summa;
            set
            {
                _Summa = value;
                RaisePropertyChanged();
            }
        }

        private string _selectedCurrency;
        public string SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<string> Currencies { get; set; }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);

        private void SaveCommand_Execute()
        {
            Wallet.Name = Name;
            Wallet.Summa = double.Parse(Summa);
            Wallet.Currency = SelectedCurrency;
            Wallet.Date = DateTime.Now;
            Wallet.Id = Guid.Empty.Equals(Wallet.Id) ? Guid.NewGuid() : Wallet.Id;
            _addWalletWindow.DialogResult = true;
            _addWalletWindow.Close();
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
            _addWalletWindow.DialogResult = false;
            _addWalletWindow.Close();
        }
    }
}
