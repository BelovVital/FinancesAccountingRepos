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
    class AddIncomeSourseViewModel : BindableBase
    {
        AddIncomeSourceWindow _addWindow;
        public AddIncomeSourseViewModel(AddIncomeSourceWindow addWindow)
        {
            _addWindow = addWindow;

            var dbcontext = new AppDbContext();

            var incomeSources = dbcontext.IncomeSources;
            IncomeSources = new ObservableCollection<IncomeSource>(incomeSources);
        }

        private IncomeSource _incomeSource;

        public IncomeSource IncomeSource
        {
            get => _incomeSource;
            set
            {
                _incomeSource = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<IncomeSource> _incomeSources;
        public ObservableCollection<IncomeSource> IncomeSources 
        {
            get => _incomeSources;
            set
            { 
                _incomeSources = value; 
                RaisePropertyChanged();
            } 
        }

        private IncomeSource _selectedIncomeSource;

        public IncomeSource SelectedIncomeSource
        {
            get => _selectedIncomeSource;
            set
            {
                _selectedIncomeSource = value;
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
            IncomeSource = new IncomeSource();
            IncomeSource.Id = Guid.Empty.Equals(IncomeSource.Id) ? Guid.NewGuid() : IncomeSource.Id;
            IncomeSource.Name = NewName;

            var dbContext = new AppDbContext();
            dbContext.IncomeSources.Add(IncomeSource);
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
                    dbContext.IncomeSources.Remove(SelectedIncomeSource);
                    dbContext.SaveChanges();
                }
                IncomeSources.Remove(SelectedIncomeSource);
                SelectedIncomeSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool DeleteCommand_CanExecute()
        {
            return SelectedIncomeSource != null;
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
