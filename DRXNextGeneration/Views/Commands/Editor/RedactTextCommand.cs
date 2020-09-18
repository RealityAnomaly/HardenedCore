using System;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Text;

namespace DRXNextGeneration.Views.Commands.Editor
{
    internal class RedactTextCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var selection = (ITextSelection) parameter;

            var charFormatting = selection.CharacterFormat;
            charFormatting.BackgroundColor = charFormatting.BackgroundColor == Colors.Red ? Colors.Transparent : Colors.Red;
        }
    }
}
