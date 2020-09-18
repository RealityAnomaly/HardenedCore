using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CoreLibrary.Models.Crypto;
using CoreLibrary.Models.Crypto.Providers;
using DRXNextGeneration.ViewModels;
using System.Threading.Tasks;
using Windows.UI.Core;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views.Dialogs
{
    public sealed partial class StorePropertiesDialog
    {
        public readonly ObservableCollection<string> ProviderCollection = new ObservableCollection<string>();
        private readonly bool _creating;
        public DrxStoreViewModel Store;

        // Certificate selector properties
        public ObservableCollection<X509Certificate2> Certificates { get; set; } =
            new ObservableCollection<X509Certificate2>();

        /// <summary>
        /// Name of the key protector to use, as defined in <see cref="ProviderCollection"/>.
        /// </summary>
        public string CryptoProvider { get; set; }

        public StorePropertiesDialog(DrxStoreViewModel store, bool creating = false)
        {
            InitializeComponent();
            Store = store;

            _creating = creating;

            // Add all the providers to the available list, and set the provider
            foreach (var provider in CryptoProviders.UiProviders)
                ProviderCollection.Add(((CryptoProviderAttribute)provider.GetCustomAttribute(typeof(CryptoProviderAttribute))).Name);
            LoadCryptoProvider();

            LoadCryptoUi();
            if (!creating) return;
            Title = "New Store";
            PrimaryButtonText = "Create";
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCryptoUi();
        }

        private void LoadCryptoProvider()
        {
            // Load current crypto provider
            if (!(Store.Model.Key is CryptoKey protectedKey))
            {
                CryptoProvider = "None";
                return;
            }

            CryptoProvider = protectedKey.PrimaryProtector.ProtectorName;
        }

        private async void LoadCryptoUi()
        {
            switch (CryptoProvider)
            {
                case "Certificate":
                    CertificateSelector.Visibility = Visibility.Collapsed;

                    await Task.Run(async () =>
                    {
                        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                        store.Open(OpenFlags.ReadOnly);

                        await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Certificates.Clear();
                            foreach (var certificate in store.Certificates)
                                Certificates.Add(certificate);

                            // Auto-assign the existing certificate if present
                            if (Store.Model.Key is CryptoKey protectedKey &&
                                protectedKey.PrimaryProtector.Provider is CertificateCryptoProvider provider &&
                                provider.Certificate != null)
                                CertificateSelector.SelectedItem = provider.Certificate;

                            CertificateSelector.Visibility = Visibility.Visible;
                        });
                    }).ConfigureAwait(false);
                    break;
                default:
                    CertificateSelector.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void CommitEncryption(ContentDialogButtonClickEventArgs args)
        {
            try
            {
                object parameters = null;

                // Special provider handling for certificate crypto
                if (CryptoProvider == "Certificate")
                    parameters = CertificateSelector.SelectedItem;

                // Commit the model to activate the crypto provider
                Store.CommitEncryption(CryptoProvider, parameters);
            }
            catch (Exception)
            {
                args.Cancel = true;
            }
        }

        private void StorePropertiesDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            CommitEncryption(args);
        }
    }
}
