using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DRXNextGeneration.ViewModels;
using DRXNextGeneration.Views.Dialogs;

namespace DRXNextGeneration.Views.Commands.Document
{
    internal class EditDocumentCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public async void Execute(object parameter)
        {
            var document = (DrxDocumentViewModel)parameter;
            var dialog = new DocumentPropertiesDialog(document);
            await dialog.ShowAsync();
            await document.Model.SaveAsync();
        }
    }
}
