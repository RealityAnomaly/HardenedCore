using System;
using System.Windows.Input;
using Windows.UI.Text;

namespace DRXNextGeneration.Views.Commands.Editor
{
    internal class UnderlineTextCommand : ICommand
    {
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            var selection = (ITextSelection) parameter;

            var charFormatting = selection.CharacterFormat;
            charFormatting.Underline = charFormatting.Underline == UnderlineType.None ? UnderlineType.Single : UnderlineType.None;
            selection.CharacterFormat = charFormatting;
        }
    }
}
