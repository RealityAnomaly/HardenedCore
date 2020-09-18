using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using DRXLibrary.Models.Drx;
using DRXNextGeneration.Common;
using DRXNextGeneration.Common.Extensions;
using DRXNextGeneration.Common.Groupings;
using DRXNextGeneration.ViewModels;
using DRXNextGeneration.Views.Dialogs;
using Microsoft.AppCenter.Analytics;
using static DRXNextGeneration.Common.DrxCommand;

namespace DRXNextGeneration.Views.Store
{
    public sealed partial class StoreDocumentsPage : IViewCommandHandler
    {
        private DrxStoreViewModel _store;

        public StoreDocumentsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!(e.Parameter is DrxNavigationInfo info)) return;
            _store = info.Store;

            // avoid crashes
            if (_store == null) return;

            // Setter updates UI so run on main thread
            UpdateDocuments();

            // Handle the navigation command
            HandleCommand(info.Command);
        }

        public void HandleCommand(object command)
        {
            if (!(command is DrxCommand drxCommand)) return;
            switch (drxCommand.Subject)
            {
                case DrxCommandSubject.StoreDocuments:
                    switch (drxCommand.Command)
                    {
                        case "new":
                            AddButton_OnClick(this, new RoutedEventArgs());
                            break;
                    }
                    break;
                case DrxCommandSubject.Document:
                    // Resolve the document here
                    DrxDocumentViewModel document = null;
                    if (drxCommand.Parameters.ContainsKey("contextId"))
                    {
                        document = _store.Documents.FirstOrDefault(s =>
                            s.Model.Id == (Guid)drxCommand.Parameters["contextId"]);
                    }
                    else if (drxCommand.Parameters.ContainsKey("documentName"))
                    {
                        document = _store.Documents.FirstOrDefault(s =>
                            s.Title == (string)drxCommand.Parameters["documentName"]);
                    }

                    // Resolve failed
                    if (document == null) break;

                    // Use this document now
                    SetSelectedItems(new List<DrxDocumentViewModel> { document });

                    switch (drxCommand.Command)
                    {
                        case "open":
                            // We've already opened it by selecting it
                            break;
                        case "delete":
                            //DeleteConfirmation_Click(this, new RoutedEventArgs());
                            break;
                        case "properties":
                            // TODO, must convert to ICommand
                            break;
                    }
                    break;
            }
        }

        private async void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_store == null) return;

            // Create the blank document
            var document = new DrxDocumentViewModel(new DrxDocument {Store = _store.Model, Header = {BodyType = DrxBodyType.Rtf}}, _store);
            var result = await new DocumentPropertiesDialog(document, true).ShowAsync();

            // Ensure the user didn't click Cancel and the title is set.
            if (result != ContentDialogResult.Primary ||
                string.IsNullOrWhiteSpace(document.Model.Header.Title))
                return;

            // Add it to the view model and persist
            _store.Documents.Add(document);
            await document.Model.SaveAsync();
            UpdateDocuments();

            Analytics.TrackEvent("Document(s) added.");
        }

        private async void DeleteConfirmation_Click(object sender, RoutedEventArgs e)
        {
            if (!MasterViewList.SelectedItems.Any()) return;
            Analytics.TrackEvent("Document delete(s) confirmed.");

            var items = new List<object>(MasterViewList.SelectedItems);
            foreach (var item in items)
                _store.Documents.Remove((DrxDocumentViewModel)item);
            DeleteFlyout.Hide();

            await Task.Run(async () =>
            {
                foreach (var item in items)
                    await ((DrxDocumentViewModel)item).Model.DeleteAsync();
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    ClearSelectedItems();
                    UpdateDocuments();
                });
            }).ConfigureAwait(false);
        }

        private void MasterViewList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Set the current calendar items
            DrxCalendar.SetSelectedDocuments(MasterViewList.SelectedItems);
        }

        private async void MasterViewList_OnDragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            try
            {
                foreach (var item in e.Items)
                {
                    var document = (DrxDocumentViewModel)item;
                    if (document.Encrypted)
                    {
                        // TODO: don't handle encrypted documents yet
                        e.Cancel = true;
                        return;
                    }

                    // Ensure the body's loaded before we do this
                    await document.Model.LoadBodyAsync();
                    e.Data.SetExportedDrx(document.Model, document.Store.Model);
                    e.Data.RequestedOperation = DataPackageOperation.Copy;
                }
            }
            catch (Exception)
            {
                e.Cancel = true;
            }
        }

        private void DrxCalendar_OnDocumentSelected(object sender, IList<DrxDocumentViewModel> documents)
        {
            // Remove the event handler here so we don't go into a loop.
            MasterViewList.SelectionChanged -= MasterViewList_OnSelectionChanged;
            SetSelectedItems(documents);

            // Re-register the event handler
            MasterViewList.SelectionChanged += MasterViewList_OnSelectionChanged;
        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) =>
            UpdateDocuments(args.QueryText);

        private void UpdateGroupedDocuments(IEnumerable<DrxDocumentViewModel> enumerable)
        {
            DrxDocumentCvs.Source = enumerable.GroupBy(d => new DateTime(d.Model.Header.TimeStamp.Year, d.Model.Header.TimeStamp.Month, 1), (key, list) => new DrxDocumentTimeGrouping(key, list));
        }

        /// <summary>
        /// Helper method to clear selected items depending on selection mode,
        /// to avoid a catastrophic failure
        /// </summary>
        private void ClearSelectedItems()
        {
            switch (MasterViewList.SelectionMode)
            {
                case ListViewSelectionMode.Multiple:
                case ListViewSelectionMode.Extended:
                    MasterViewList.SelectedItems.Clear();
                    break;
                case ListViewSelectionMode.Single:
                    MasterViewList.SelectedItem = null;
                    break;
            }
        }
        private void SetSelectedItems(IEnumerable<DrxDocumentViewModel> documents)
        {
            ClearSelectedItems();
            switch (MasterViewList.SelectionMode)
            {
                case ListViewSelectionMode.Multiple:
                case ListViewSelectionMode.Extended:
                    foreach (var document in documents)
                        MasterViewList.SelectedItems.Add(document);
                    break;
                case ListViewSelectionMode.Single:
                    MasterViewList.SelectedItem = documents.FirstOrDefault();
                    break;
            }
        }
        private void UpdateDocuments() => UpdateGroupedDocuments(_store.Documents);
        private void UpdateDocuments(string searchName) => UpdateGroupedDocuments(_store.Documents.Where(d => d.Title.Contains(searchName, StringComparison.InvariantCultureIgnoreCase)));

        private void ToggleSelection_Click(object sender, RoutedEventArgs e)
        {
            var button = (ToggleButton) sender;
            MasterViewList.SelectionMode = button.IsChecked == true 
                ? ListViewSelectionMode.Multiple 
                : ListViewSelectionMode.Single;
            DrxCalendar.SelectionMode = button.IsChecked == true
                ? CalendarViewSelectionMode.Multiple
                : CalendarViewSelectionMode.Single;
        }

        
    }
}
