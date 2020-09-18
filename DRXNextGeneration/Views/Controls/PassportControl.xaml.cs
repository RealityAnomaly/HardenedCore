using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DRXNextGeneration.Views.Controls
{
    public sealed partial class PassportControl
    {
        public bool KeyCredentialAvailable;

        public PassportControl()
        {
            InitializeComponent();
        }

        public async Task Initialise()
        {
            PassportStatus.Text = "Checking Microsoft Passport state...";
            PassportLoader.IsActive = true;

            await Task.Run(async () =>
            {
                KeyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PassportLoader.IsActive = false;
                    if (!KeyCredentialAvailable)
                    {
                        ContainerPanel.Background = new SolidColorBrush(Color.FromArgb(255, 50, 170, 207));
                        PassportStatus.Text = "Microsoft Passport is not set up!\nGo to Windows Settings to enable it.";
                        return;
                    }

                    PassportStatus.Text = "Microsoft Passport is ready to use.";
                });
            }).ConfigureAwait(false);
        }
    }
}
