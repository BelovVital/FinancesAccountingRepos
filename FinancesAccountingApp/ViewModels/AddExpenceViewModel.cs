using CarSharingApp.ViewModels.BaseClasses;
using CarSharingApp.Views.Interfaces;
using FinancesAccounting.Models.DataBase;
using FinancesAccountingApp.Models.DataBase;
using FinancesAccountingApp.Models.DataBase.Entities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancesAccounting.ViewModels
{
    internal class AddExpenceViewModel : AddEditViewModelBase
    {
        //private readonly Currency _currency;


        public IReadOnlyCollection<Currency> Currencies { get; }
        public AddExpenceViewModel(IAddEditWindow addEditWindow) 
            : base(addEditWindow)
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
                SummaCommand.RaiseCanExecuteChanged();
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
                SaveCommand.RaiseCanExecuteChanged();
            }
        }


        private bool CanSumma => !string.IsNullOrWhiteSpace(Summa);

        private DelegateCommand _summaCommand;
        private DelegateCommand SummaCommand =>
            _summaCommand ??= new DelegateCommand(SummaCommand_Execute, SummaCommand_CanExecute);


        private bool SummaCommand_CanExecute()
        {
            throw new NotImplementedException();
        }
        private void SummaCommand_Execute()
        {
            using (var dbContext = new AppDbContext())
            {
                Income? income = null;
                try
                {
                    income = dbContext.Incomes.FirstOrDefault(i => i.Summa == double.Parse(Summa));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
