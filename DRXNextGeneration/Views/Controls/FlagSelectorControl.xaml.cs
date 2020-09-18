using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using DRXLibrary.Models.Drx;
using DRXNextGeneration.Annotations;
using DRXNextGeneration.Common;
using DRXNextGeneration.Services;
using DRXNextGeneration.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using static DRXNextGeneration.Common.DrxCommand;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DRXNextGeneration.Views.Controls
{
    public sealed partial class FlagSelectorControl : INotifyPropertyChanged, IViewCommandHandler
    {
        public DrxStoreViewModel Store
        {
            get => (DrxStoreViewModel) GetValue(StoreProperty);
            set { SetValue(StoreProperty, value); OnPropertyChanged(); }
        }
        public static readonly DependencyProperty StoreProperty =
            DependencyProperty.Register(nameof(Store), typeof(DrxStoreViewModel), typeof(FlagSelectorControl), null);

        public DrxDocumentViewModel Document
        {
            get => (DrxDocumentViewModel) GetValue(DocumentProperty);
            set
            {
                SetValue(DocumentProperty, value);
                OnPropertyChanged();

                if (value == null || SelectorMode != FlagSelectorMode.Selection) return;
                // Unregister event handler to avoid running self
                SelectorGrid.SelectionChanged -= SelectorGrid_OnSelectionChanged;
                SelectorGrid.SelectedItems.Clear();
                foreach (var flagId in value.Model.Header.Flags)
                {
                    var flag = Store.FlagDefinitions.FirstOrDefault(f => f.Model.Id == flagId);
                    if (flag != null)
                    {
                        SelectorGrid.SelectedItems.Add(flag);
                    }
                    //else
                    //{
                        // TODO: show alert and remove the flag, as it no longer exists
                    //}
                }

                // Re-register handler
                SelectorGrid.SelectionChanged += SelectorGrid_OnSelectionChanged;

                // Update the security level (greys out unselectable flags)
                Document.PropertyChanged += DocumentOnPropertyChanged;
                UpdateSecurityLevel(Document.SecurityLevel);
            }
        }

        private void DocumentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Document.SecurityLevel)) UpdateSecurityLevel(Document.SecurityLevel);
        }

        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register(nameof(Document), typeof(DrxDocumentViewModel), typeof(FlagSelectorControl), null);

        public FlagSelectorMode SelectorMode
        {
            get => (FlagSelectorMode) GetValue(SelectorProperty);
            set
            {
                SetValue(SelectorProperty, value);
                OnPropertyChanged();

                switch (value)
                {
                    case FlagSelectorMode.Edit:
                        SelectorGrid.SelectionMode = ListViewSelectionMode.Single;
                        EditCommandBar.Visibility = Visibility.Visible;
                        break;
                    case FlagSelectorMode.Selection:
                        SelectorGrid.SelectionMode = ListViewSelectionMode.Multiple;
                        EditCommandBar.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }
        public static readonly DependencyProperty SelectorProperty =
            DependencyProperty.Register(nameof(SelectorProperty), typeof(FlagSelectorMode), typeof(FlagSelectorControl), null);

        private readonly StoreService _storeService;

        public DrxFlagViewModel EditingFlag
        {
            get => _editingFlag;
            set { _editingFlag = value; OnPropertyChanged(); }
        }
        private DrxFlagViewModel _editingFlag;

        public FlagSelectorControl()
        {
            InitializeComponent();
            _storeService = DrxServiceProvider.GetServiceProvider().GetService<StoreService>();
        }

        private void SelectorGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Document == null) return;
            foreach (var item in e.AddedItems)
                Document.Model.Header.Flags.Add(((DrxFlagViewModel)item).Model.Id);
            foreach (var item in e.RemovedItems)
                Document.Model.Header.Flags.Remove(((DrxFlagViewModel)item).Model.Id);
            if (e.AddedItems.Any() || e.RemovedItems.Any())
                Document.Unsaved = true;
        }

        // Methods below are edit mode only, NOT selection mode
        private void AddButton_OnClick(object sender, RoutedEventArgs e) => ShowEditFlyout(true, AddButton);
        private async void DeleteConfirmation_Click(object sender, RoutedEventArgs e)
        {
            // Deletion is only allowed in edit mode
            if (!(SelectorGrid.SelectedItem is DrxFlagViewModel item)) return;
            Store.FlagDefinitions.Remove(item);
            Store.Model.FlagDefinitions.Remove(item.Model);

            await _storeService.SaveStoreCache();
            DeleteFlyout.Hide();
        }
        private void Edit_Click(object sender, RoutedEventArgs e) => ShowEditFlyout(false, EditButton);
        private async void EditConfirmation_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(FlagTagTextBox.Text) ||
                string.IsNullOrWhiteSpace(FlagNameTextBox.Text)) return;
            EditingFlag.Tag = FlagTagTextBox.Text;
            EditingFlag.Name = FlagNameTextBox.Text;
            EditingFlag.Description = FlagDescriptionTextBox.Text;
            EditingFlag.SecurityLevel = (DrxSecurityLevel)((IValueConverter)Resources["IntToEnumConverter"]).Convert(FlagSecurityLevelSelector.SelectedIndex,
                typeof(DrxSecurityLevel), null, string.Empty);

            // Add it to the store (in case it's a new flag)
            if (!Store.FlagDefinitions.Contains(EditingFlag))
            {
                Store.FlagDefinitions.Add(EditingFlag);
                Store.Model.FlagDefinitions.Add(EditingFlag.Model);
                // TODO: should we just be refreshing the model instead?
            }
                
            EditingFlag = null;
            await _storeService.SaveStoreCache();
            EditFlyout.Hide();
        }
        private void InitEditFlyout(bool creating)
        {
            if (creating)
            {
                EditingFlag = new DrxFlagViewModel(new DrxFlag());
            }
            else
            {
                EditingFlag = (DrxFlagViewModel)SelectorGrid.SelectedItem;
                if (EditingFlag == null) return;
            }

            FlagTagTextBox.Text = EditingFlag.Tag;
            FlagNameTextBox.Text = EditingFlag.Name;
            FlagDescriptionTextBox.Text = string.IsNullOrWhiteSpace(EditingFlag.Description) ? string.Empty : EditingFlag.Description;
            FlagSecurityLevelSelector.SelectedIndex = (int)((IValueConverter)Resources["IntToEnumConverter"]).Convert(EditingFlag.SecurityLevel,
                typeof(int), null, string.Empty);
        }
        private void ShowEditFlyout(bool creating, FrameworkElement target)
        {
            EditFlyout.ShowAt(target);
            InitEditFlyout(creating);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateSecurityLevel(DrxSecurityLevel documentSecurityLevel)
        {
            /**
            foreach (var item in SelectorGrid.Items)
            {
                var item2 = SelectorGrid.ContainerFromItem(item) as GridViewItem;
                item2.
            }*/
        }

        public enum FlagSelectorMode
        {
            Edit,
            Selection
        }

        public void HandleCommand(object command)
        {
            // Wait for full load to handle command(s)
            if (!IsLoaded)
            {
                Loaded += delegate { HandleCommand(command); };
                return;
            }

            if (!(command is DrxCommand drxCommand)) return;
            if (SelectorMode != FlagSelectorMode.Edit) return; // TODO: Other modes not supported yet
            switch (drxCommand.Subject)
            {
                case DrxCommandSubject.StoreFlags:
                    switch (drxCommand.Command)
                    {
                        case "new":
                            AddButton_OnClick(this, new RoutedEventArgs());
                            break;
                    }
                    break;
                case DrxCommandSubject.Flag:
                    // Flag gets resolved here
                    DrxFlagViewModel flag = null;
                    if (drxCommand.Parameters.ContainsKey("contextId"))
                    {
                        flag = Store.FlagDefinitions.FirstOrDefault(s =>
                            s.Model.Id == (Guid)drxCommand.Parameters["contextId"]);
                    }
                    else if (drxCommand.Parameters.ContainsKey("flagName"))
                    {
                        flag = Store.FlagDefinitions.FirstOrDefault(s =>
                            s.Tag == (string)drxCommand.Parameters["flagName"]);
                    }
                    else if (drxCommand.Parameters.ContainsKey("flagDescription"))
                    {
                        flag = Store.FlagDefinitions.FirstOrDefault(s =>
                            s.Name == (string)drxCommand.Parameters["flagDescription"]);
                    }

                    // Failed to resolve flag
                    if (flag == null) return;

                    // Set our current flag
                    SelectorGrid.SelectedItem = flag;
                    switch (drxCommand.Command)
                    {
                        case "open":
                            // Already open
                            break;
                        case "delete":
                            DeleteFlyout.ShowAt(DeleteButton);
                            break;
                        case "properties":
                            Edit_Click(this, new RoutedEventArgs());
                            break;
                    }
                    break;
            }
        }
    }
}
