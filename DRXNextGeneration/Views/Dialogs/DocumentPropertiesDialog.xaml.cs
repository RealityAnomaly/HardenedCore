using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using CoreLibrary.Utilities;
using DRXLibrary.Models.Drx;
using DRXNextGeneration.Common.Converters;
using DRXNextGeneration.Services;
using DRXNextGeneration.Utilities;
using DRXNextGeneration.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DRXNextGeneration.Views.Dialogs
{
    /// <inheritdoc cref="ContentDialog" />
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DocumentPropertiesDialog
    {
        public readonly DrxDocumentViewModel Document;
        public DocumentPropertiesDialog(DrxDocumentViewModel document, bool creating = false)
        {
            InitializeComponent();
            Document = document;

            if (!creating) return;
            Title = "New Document";
            PrimaryButtonText = "Create";
        }

        private async void DocumentPropertiesDialog_OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            var securityLevel = (DrxSecurityLevel)((IValueConverter)Resources["IntToEnumConverter"]).ConvertBack(SecurityLevelComboBox.SelectedIndex,
                typeof(DrxSecurityLevel), null, string.Empty);

            var userService = DrxServiceProvider.GetServiceProvider().GetService<UserService>();
            
            var notAuthorised = !await userService.IsMachineGenuineDit() && securityLevel >= DrxSecurityLevel.StormVault;
            var notEncrypted = securityLevel >= DrxSecurityLevel.Secret && !Document.Encrypted;

            // Don't allow enabling HSM unless we're genuine DIT
            if (notAuthorised || notEncrypted)
            {
                if (notAuthorised)
                {
                    ErrorBlock.Text = "Not authorised to use this security level.";
                }
                else if (notEncrypted)
                {
                    ErrorBlock.Text = "Document must be encrypted to elevate to this level.";
                }

                args.Cancel = true;
                ErrorBlock.Visibility = Visibility.Visible;
                deferral.Complete();
                return;
            }

            Document.Title = TitleTextBox.Text;
            Document.TimeStamp = DatePicker.Date.GetValueOrDefault(DateTimeOffset.UtcNow);
            Document.SecurityLevel = securityLevel;

            if (Document.IsHighSecurity())
                Document.Encrypted = true;

            deferral.Complete();
        }
    }
}
