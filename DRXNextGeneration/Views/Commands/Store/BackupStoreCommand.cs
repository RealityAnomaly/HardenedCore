using System;
using System.Windows.Input;
using DRXNextGeneration.ViewModels;
using DRXNextGeneration.Views.Dialogs;

namespace DRXNextGeneration.Views.Commands.Store
{
    internal class BackupStoreCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            var store = (DrxStoreViewModel) parameter;
            var dialog = new StoreBackupDialog(store);
            await dialog.ShowAsync();
        }
    }
}
