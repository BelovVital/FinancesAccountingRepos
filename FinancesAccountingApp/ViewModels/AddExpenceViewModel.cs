using FinancesAccounting.Models.DataBase;
using FinancesAccountingApp.Models.DataBase;
using FinancesAccountingApp.Models.DataBase.Entities;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancesAccounting.ViewModels
{
    internal class AddExpenceViewModel : BindableBase
    {
        //private readonly Currency _currency;


        public IReadOnlyCollection<Currency> Currencies { get; }
        public AddExpenceViewModel() 
        {
            var dbcontext = new AppDbContext();
            Currencies = dbcontext.Currencies.ToList();
            //Доделать SelectedCurrency, Добавить регионы
            SelectedCurrency = (Currency)Currencies;
        }
        private string _summa;
        private string Summa
        {
            get => _summa;
            set
            {
                _summa = value;
                RaisePropertyChanged();
            }
        }


        private Currency _selectedcurrency;
        private Currency SelectedCurrency
        {
            get => _selectedcurrency;
            set
            {
                _selectedcurrency = value;
                RaisePropertyChanged();
            }
        }

        protected override void SaveEntityOperation()
        {
            throw new NotImplementedException();
        }

        protected override bool SaveCommand_CanExecute()
        {
            throw new NotImplementedException();
        }
    }
}
