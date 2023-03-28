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
            Wallet = wallet;

            var dbcontext = new AppDbContext();

            var currency = dbcontext.Currencies;
            var _currencies = new ObservableCollection<Currency>(currency);
            Currencies = new ObservableCollection<string>(_currencies.Select(x => x.Name));
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
        private ObservableCollection<string> _currencies;
        public ObservableCollection<string> Currencies 
        { 
            get => _currencies;
            set
            {
                _currencies = value;
                RaisePropertyChanged();
            }
        }

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);

        public void SaveCommand_Execute()
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
    }
}
