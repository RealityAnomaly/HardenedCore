using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DRXNextGeneration.ViewModels;

namespace DRXNextGeneration.Views.Dialogs
{
    public sealed partial class DocumentSelectorFlyout
    {
        public delegate void DocumentSelectedDelegate(object sender, DrxDocumentViewModel document);
        public event DocumentSelectedDelegate DocumentSelected;

        public DocumentSelectorFlyout()
        {
            InitializeComponent();
        }

        public void SetDocuments(IEnumerable<DrxDocumentViewModel> documents)
        {
            DocumentView.ItemsSource = documents;
        }

        private void DocumentView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = e.AddedItems.First();
            DocumentSelected?.Invoke(this, (DrxDocumentViewModel) item);
        }
    }
}
