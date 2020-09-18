using System;
using System.Windows.Input;
using Windows.UI.Text;

namespace DRXNextGeneration.Views.Commands.Editor
{
    internal class BoldTextCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var selection = (ITextSelection) parameter;

            var charFormatting = selection.CharacterFormat;
            charFormatting.Bold = FormatEffect.Toggle;
            selection.CharacterFormat = charFormatting;
        }
    }
}
