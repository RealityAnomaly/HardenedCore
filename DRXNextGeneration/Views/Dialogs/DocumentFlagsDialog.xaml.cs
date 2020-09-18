using Windows.UI.Xaml.Controls;
using DRXLibrary.Models.Drx;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views.Dialogs
{
    public sealed partial class DocumentFlagsDialog : ContentDialog
    {
        public DrxDocument Document { get; set; }
        public DocumentFlagsDialog(DrxDocument document)
        {
            InitializeComponent();
            Document = document;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
