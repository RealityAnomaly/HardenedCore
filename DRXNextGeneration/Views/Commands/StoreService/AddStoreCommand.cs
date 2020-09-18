using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using DRXLibrary.Models.Drx.Store;
using DRXNextGeneration.Common.Extensions;
using DRXNextGeneration.ViewModels;
using DRXNextGeneration.Views.Dialogs;

namespace DRXNextGeneration.Views.Commands.StoreService
{
    internal class AddStoreCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            var service = (DrxStoreServiceViewModel) parameter;
            var store = new DrxStoreViewModel(new DrxStore(), service);
            store.Model.RegisterLocalBacker();

            var result = await new StorePropertiesDialog(store, true).ShowAsync();
            if (result != ContentDialogResult.Primary ||
                string.IsNullOrWhiteSpace(store.Name))
                return;
            await service.AddStoreAsync(store);
        }
    }
}
