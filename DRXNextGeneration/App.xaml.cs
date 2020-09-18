using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DRXNextGeneration.Common;
using DRXNextGeneration.Common.Extensions;
using DRXNextGeneration.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.Extensions.Logging;
using static DRXNextGeneration.Common.DrxCommand;
using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;

namespace DRXNextGeneration
{
    /// <inheritdoc cref = "Application"/>
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {
        private static ILogger _crashLogger;

        /// <inheritdoc/>
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

            UnhandledException += (sender, args) =>
            {
                _crashLogger?.LogCritical(args.Exception, "Unhandled exception.");
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                _crashLogger?.LogCritical(args.Exception, "Unhandled exception in task scheduler.");
            };
        }

        private Frame GetOrCreateRootFrame(IActivatedEventArgs args)
        {
            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (Window.Current.Content is Frame rootFrame) return rootFrame;

            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();
            rootFrame.NavigationFailed += OnNavigationFailed;
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                var loadState = (args.PreviousExecutionState == ApplicationExecutionState.Terminated);
                var extendedSplash = new ExtendedSplash(args.SplashScreen, loadState);
                rootFrame.Content = extendedSplash;
                Window.Current.Content = extendedSplash;
            }

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
            return rootFrame;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name = "e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = GetOrCreateRootFrame(e);
            if (e.PrelaunchActivated)
                return;
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);
            DrxCommand command = null;

            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var voiceArgs = (VoiceCommandActivatedEventArgs) args;
                var speechResult = voiceArgs.Result;
                var commandName = speechResult.RulePath[0];
                var textSpoken = speechResult.Text;

                switch (commandName)
                {
                    case "openDrxDocument":
                        var name = speechResult.SemanticInterpretation("name");
                        command = new DrxCommand {Command = "open", Subject = DrxCommandSubject.Document};
                        command.Parameters["documentName"] = name;
                        break;
                    case "newDrxDocument":
                        command = new DrxCommand {Command = "new", Subject = DrxCommandSubject.StoreDocuments};
                        break;
                    default:
                        break;
                }

                if (command != null)
                    Analytics.TrackEvent($"Voice command-based activation registered.", new Dictionary<string, string>
                    {
                        { "command", command.Command },
                        { "subject", command.Subject.ToString() }
                    });
            }
            else if (args.Kind == ActivationKind.Protocol)
            {
                var protoArgs = (ProtocolActivatedEventArgs) args;
                command = DrxUriParser.Parse(protoArgs.Uri);

                Analytics.TrackEvent("Protocol-based activation registered.");
            }

            var rootFrame = GetOrCreateRootFrame(args);
            if (!(rootFrame?.Content is IViewCommandHandler handler)) return;

            // Handle the navigation using the root frame
            handler.HandleCommand(command);
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name = "sender">The Frame which failed navigation</param>
        /// <param name = "e">Details about the navigation failure</param>
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name = "sender">The source of the suspend request.</param>
        /// <param name = "e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        internal static void RegisterCrashLogger(ILogger logger)
        {
            _crashLogger = logger;
        }
    }
}