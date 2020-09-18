using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DRXNextGeneration.Services;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views
{
    /// <inheritdoc cref="Page" />
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplash : IViewCommandHandler
    {
        private Rect _splashImageRect;
        private Frame _rootFrame;
        private bool _dismissed;
        private readonly bool _loadState;
        private readonly SplashScreen _splash;
        private ILogger _logger;

        private object _command;

        public ExtendedSplash(SplashScreen splashScreen, bool loadState)
        {
            InitializeComponent();
            _splash = splashScreen;
            _loadState = loadState;
            Window.Current.SizeChanged += CurrentOnSizeChanged;

            if (_splash != null)
            {
                _splash.Dismissed += SplashOnDismissed;
                _splashImageRect = _splash.ImageLocation;
                PositionImage();
                PositionLoader();
            }

            _rootFrame = new Frame();
        }

        public void HandleCommand(object command)
        {
            // Hold the command until the main page is ready.
            _command = command;
        }

        private void CurrentOnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (_splash == null)
                return;
            _splashImageRect = _splash.ImageLocation;
            PositionImage();
            PositionLoader();
        }

        private async void SplashOnDismissed(SplashScreen sender, object args)
        {
            _dismissed = true;
            // setup actions go here

            await Task.Run(SetupApplication);
        }

        private async Task SetupApplication()
        {
            // load
            await SetLoadingStatus(10, "Instantiating services.");
            var services = DrxServiceProvider.GetServiceProvider();
            _logger = services.GetService<ILoggerFactory>().CreateLogger("Splash Screen");

            await SetLoadingStatus(20, "Instantiating app providers.");

            try
            {
                const string secret = "8e68d797-a057-472e-838c-d65348f801d4";
                AppCenter.Start(secret, typeof(Analytics));
                AppCenter.Start(secret, typeof(Distribute));
                AppCenter.Start(secret, typeof(Crashes));
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Exception when initialising App Center.");
            }

            await SetLoadingStatus(30, "Loading document stores.");

            // Init the file store
            var storeService = services.GetService<StoreService>();
            await storeService.Initialise();

            // Get the names - this is used for voice commands
            // TODO: this should be set more dynamically elsewhere
            var defaultStore = storeService.GetDefaultStore();
            if (defaultStore != null)
            {
                var names = defaultStore.GetDocuments().Select(s => s.ToString());

                // Load voice commands
                await SetLoadingStatus(40, "Loading voice commands.");
                await InstallVoiceCommands(names);
            }
            
            // Everything done
            await SetLoadingStatus(100, "Load complete.");

            if (_loadState)
                await RestoreApplicationState();

            // dismiss
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var page = new MainPage();
                _rootFrame = new Frame { Content = page };
                Window.Current.Content = _rootFrame;

                page.HandleCommand(_command);
            });
        }

        private async Task RestoreApplicationState()
        {
            // do stuff to restore state
        }

        private static async Task InstallVoiceCommands(IEnumerable<string> documentNames)
        {
            try
            {
                var assets = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var file = await assets.GetFileAsync("DrxEditorCommands.xml");

                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(file);

                // Update phrase list
                var set = VoiceCommandDefinitionManager.InstalledCommandDefinitions["DrxEditorCommandSet_en-gb"];
                await set.SetPhraseListAsync("name", documentNames);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to install voice commands: {e.Message}");
            }
        }

        private async Task SetLoadingStatus(int progress, string message = null)
        {
            _logger?.LogInformation($"Splash status: \"{message}\"");

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LoadingBar.Value = progress;

                if (!string.IsNullOrWhiteSpace(message))
                    LoadingStatus.Text = message;
            });
        }

        private void PositionImage()
        {
            ExtendedSplashImage.SetValue(Canvas.LeftProperty, _splashImageRect.X);
            ExtendedSplashImage.SetValue(Canvas.TopProperty, _splashImageRect.Y);
            ExtendedSplashImage.Height = _splashImageRect.Height;
            ExtendedSplashImage.Width = _splashImageRect.Width;
        }

        private void PositionLoader()
        {
            ExtendedSplashProgress.SetValue(Canvas.LeftProperty, _splashImageRect.X + (_splashImageRect.Width * 0.5) - (ExtendedSplashProgress.Width * 0.5));
            ExtendedSplashProgress.SetValue(Canvas.TopProperty, (_splashImageRect.Y + _splashImageRect.Height + _splashImageRect.Height * 0.1 - 60));
        }
    }
}
