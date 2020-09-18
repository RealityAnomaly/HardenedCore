using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Protection;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DRXLibrary.Models.Drx;
using DRXNextGeneration.Annotations;
using DRXNextGeneration.Common.Extensions;
using DRXNextGeneration.Utilities;
using DRXNextGeneration.ViewModels;
using DRXNextGeneration.Views.Dialogs;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views.Controls
{
    /// <inheritdoc cref="UserControl" />
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrxEditorControl : INotifyPropertyChanged
    {
        // HSM
        private HdcpSession _hdcpSession;
        private bool _hsmActivated;
        public bool HsmActivated
        {
            get => _hsmActivated;
            private set
            {
                _hsmActivated = value;
                OnPropertyChanged();
            }
        }

        // Document view models
        private DrxDocumentViewModel _document;
        public DrxDocumentViewModel Document
        {
            get => _document;
            set
            {
                _document = value;
                LoadDocument();
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register(nameof(Document), typeof(DrxDocumentViewModel), typeof(DrxEditorControl), null);

        public DrxEditorControl()
        {
            InitializeComponent();
        }

        #region Interaction Logic
        private void Editor_OnTextChanged(object sender, RoutedEventArgs e) => Document.Unsaved = true;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.Document.SaveToDrx(Document.Model);

            Task.Run(async () =>
            {
                await Document.Model.SaveAsync();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Document.Unsaved = false; });
            });
        }
        private async void VrelButton_Click(object sender, RoutedEventArgs e)
        {
            await new DocumentVrelDialog(Document).ShowAsync();
        }
        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            await new DocumentPropertiesDialog(Document).ShowAsync();
            if (Document.IsHighSecurity())
            {
                await BeginHsmSession();
            }
            else
            {
                await EndHsmSession();
            }
        }
        private void Protect_Click(object sender, RoutedEventArgs e) => Document.Unsaved = true;
        private void Share_Click(object sender, RoutedEventArgs e) => DataTransferManager.ShowShareUI();
        #endregion
        #region HSM Logic
        private void _hdcpSession_ProtectionChanged(HdcpSession sender, object args)
        {
            // Unload the document if HDCP gets disabled
            if (!sender.IsEffectiveProtectionAtLeast(HdcpProtection.On))
                Document = null;
        }

        private async Task<bool> BeginHsmSession()
        {
            // Session is already active
            if (HsmActivated)
                return true;

            _hdcpSession = new HdcpSession();
            var result = await _hdcpSession.SetDesiredMinProtectionAsync(HdcpProtection.On);

            // Display failure status message if enabling protection failed
            switch (result)
            {
                case HdcpSetProtectionResult.Success:
                    break;
                case HdcpSetProtectionResult.TimedOut:
                    await DisplayLoadError("HDCP enablement timed out.");
                    return false;
                case HdcpSetProtectionResult.NotSupported:
                    await DisplayLoadError("HDCP is not supported.");
                    return false;
                default:
                    await DisplayLoadError("HDCP unknown failure.");
                    return false;
            }

            // Register for events to close the document if the protection state changes
            _hdcpSession.ProtectionChanged += _hdcpSession_ProtectionChanged;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // Disable screen capture since we're going into a secure pipeline
                ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = false;

                // Show the high-sec UI
                HsmActivated = true;
            });

            return true;
        }

        private async Task EndHsmSession()
        {
            if (!HsmActivated)
                return;

            // Dispose the HDCP session, if it exists
            _hdcpSession?.SetDesiredMinProtectionAsync(HdcpProtection.Off);
            _hdcpSession?.Dispose();
            _hdcpSession = null;

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                HsmActivated = false;
                ApplicationView.GetForCurrentView().IsScreenCaptureEnabled = true;
            });
        }
        #endregion

        private async void LoadDocument()
        {
            // Dispose the HDCP session if it already exists
            await EndHsmSession();

            FlagSelector.Store = Document?.Store;
            FlagSelector.Document = Document;

            if (Document == null)
            {
                // Document is being unloaded
                // TODO: add "do you want to save" NOT here
                NoDocumentBlock.Visibility = Visibility.Visible;
                Editor.Visibility = Visibility.Collapsed;
                LoadingBlock.Visibility = Visibility.Collapsed;
                CommandBar.Visibility = Visibility.Collapsed;
                ControlsBar.Visibility = Visibility.Collapsed;

                return;
            }

            Editor.Visibility = Visibility.Collapsed;
            NoDocumentBlock.Visibility = Visibility.Collapsed;
            LoadingBlock.Visibility = Visibility.Visible;
            CommandBar.Visibility = Visibility.Collapsed;
            ControlsBar.Visibility = Visibility.Collapsed;

            await Task.Run(async () =>
            {
                if (Document.Model.PlainTextBody == null)
                    await Document.Model.LoadBodyAsync();

                // Return if we're a secure document and can't init HDCP
                if (Document.SecurityLevel >= DrxSecurityLevel.StormVault && !await BeginHsmSession())
                    return;

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    try
                    {
                        // Load the document body
                        Editor.Document.LoadFromDrx(Document.Model);
                    }
                    catch (Exception e)
                    {
                        LoadingBlock.Text = e.GetType().Name == "WindowsCryptographicException" ? $"Decryption error: {e.Message}" : $"Error loading document: {e.Message}";
                        return;
                    }

                    LoadingBlock.Visibility = Visibility.Collapsed;
                    Editor.Visibility = Visibility.Visible;
                    CommandBar.Visibility = Visibility.Visible;
                    ControlsBar.Visibility = Visibility.Visible;
                    Editor.Focus(FocusState.Pointer);
                });
            }).ConfigureAwait(false);
        }

        private async Task DisplayLoadError(string error)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () => { LoadingBlock.Text = $"Error loading document: {error}"; });
        }

        private void DrxEditorControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
        }

        private async void DrxEditorControl_OnUnloaded(object sender, RoutedEventArgs e)
        {
            await EndHsmSession();
        }

        /// <summary>
        /// This method provides export/share functional
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;

            if (Document == null)
            {
                request.FailWithDisplayText("No document is currently open.");
                return;
            }

            request.Data.SetExportedDrx(Document.Model, Document.Store.Model);
        }

        private void DrxEditorControl_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Automatically ensures bars get hidden when needed.
            ControlsBar.Visibility = CommandBar.ActualWidth + ControlsBar.ActualWidth > ActualWidth ? Visibility.Collapsed : Visibility.Visible;
        }

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
