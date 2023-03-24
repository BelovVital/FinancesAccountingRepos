using CarSharingApp.Views.Interfaces;
using Prism.Commands;
using Prism.Mvvm;

namespace CarSharingApp.ViewModels.BaseClasses
{
    public abstract class AddEditViewModelBase : BindableBase
    {
        private readonly IAddEditWindow _addEditWindow;

        public AddEditViewModelBase(IAddEditWindow addEditWindow)
        {
            _addEditWindow = addEditWindow;
        }

        private DelegateCommand _saveCommand;

        private DelegateCommand _cancelCommand;

        public DelegateCommand SaveCommand =>
            _saveCommand ??= new DelegateCommand(SaveCommand_Execute, SaveCommand_CanExecute);

        public DelegateCommand CancelCommand =>
            _cancelCommand ??= new DelegateCommand(CancelCommand_Execute);

        private void SaveCommand_Execute()
        {
            SaveEntityOperation();
            _addEditWindow.DialogResult = true;
            _addEditWindow.Close();
        }

        protected abstract void SaveEntityOperation();
        protected abstract bool SaveCommand_CanExecute();

        private void CancelCommand_Execute()
        {
            _addEditWindow.DialogResult = false;
            _addEditWindow.Close();
        }
    }
}