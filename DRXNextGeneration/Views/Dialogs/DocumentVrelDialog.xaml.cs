using Windows.UI.Xaml.Controls;
using DRXNextGeneration.ViewModels;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views.Dialogs
{
    public sealed partial class DocumentVrelDialog
    {
        public readonly DrxDocumentViewModel Document;
        public DocumentVrelDialog(DrxDocumentViewModel document)
        {
            InitializeComponent();
            Document = document;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var vrel = Document.Model.Header.Vrel;
            vrel.Vividity = VividityControl.Value;
            vrel.Remembrance = RemembranceControl.Value;
            vrel.Emotion = EmotionControl.Value;
            vrel.Length = LengthControl.Value;
        }
    }
}
