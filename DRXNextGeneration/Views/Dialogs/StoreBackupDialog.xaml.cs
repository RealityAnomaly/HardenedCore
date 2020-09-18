using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DRXNextGeneration.ViewModels;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views.Dialogs
{
    public sealed partial class StoreBackupDialog
    {
        private readonly DrxStoreViewModel _store;
        public StoreBackupDialog(DrxStoreViewModel store)
        {
            InitializeComponent();
            _store = store;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var exportDecrypted = NoCryptoRadioButton.IsChecked == true;
            if (!await _store.BackupWithUiAsync(exportDecrypted))
                args.Cancel = true;
        }
    }
}
