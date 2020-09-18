using System;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DRXNextGeneration.Common;
using DRXNextGeneration.Services;
using DRXNextGeneration.ViewModels;
using Microsoft.AppCenter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage
    {
        public string Version
        {
            get
            {
                var version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        private DrxStoreServiceViewModel _service;
        private DrxAppSettingsViewModel _settings;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!(e.Parameter is DrxNavigationInfo info)) return;
            _service = info.Service;

            _settings = new DrxAppSettingsViewModel(_service);
        }

        private async void LoadHsmState()
        {
            var id = SystemIdentification.GetSystemIdForPublisher();
            DeviceId.Text = id.Source == SystemIdentificationSource.Tpm ? "TPM" : "Other";

            var userService = DrxServiceProvider.GetServiceProvider().GetService<UserService>();

            if (await userService.IsMachineGenuineDit())
            {
                HsmState.Text = "Enabled";
                HsmDisableReason.Text = "N/A";
            }
            else
            {
                HsmState.Text = "Disabled";
                HsmDisableReason.Text = "Non-genuine machine";
            }
        }

        private void SettingsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            LoadHsmState();
        }
    }
}
