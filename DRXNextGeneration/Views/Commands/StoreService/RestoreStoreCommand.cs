using System;
using System.Windows.Input;
using DRXLibrary.Models.Drx.Store;
using DRXNextGeneration.Common.Extensions;
using DRXNextGeneration.ViewModels;

namespace DRXNextGeneration.Views.Commands.StoreService
{
    internal class RestoreStoreCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            var service = (DrxStoreServiceViewModel)parameter;
            var store = new DrxStoreViewModel(new DrxStore(), service);
            store.Model.RegisterLocalBacker();

            if (!await store.RestoreWithUiAsync())
                return;

            await service.AddStoreAsync(store);
        }
    }
}
