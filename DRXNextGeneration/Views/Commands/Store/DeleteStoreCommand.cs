using System;
using System.Windows.Input;
using DRXNextGeneration.ViewModels;

namespace DRXNextGeneration.Views.Commands.Store
{
    internal class DeleteStoreCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            var store = (DrxStoreViewModel) parameter;
            // no longer used as there is no default local store
            //if (store.Model is DrxStore local && local.Default) return;
            await store.DeleteAsync();
        }
    }
}
