using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DRXNextGeneration.ViewModels;
using DRXNextGeneration.Common.Converters;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DRXNextGeneration.Views.Controls
{
    public sealed partial class DrxCalendarSelectorControl : INotifyPropertyChanged
    {
        public DrxStoreViewModel Store { get; set; }
        public static readonly DependencyProperty StoreProperty =
            DependencyProperty.Register(nameof(Store), typeof(DrxStoreViewModel), typeof(DrxCalendarSelectorControl), null);

        public CalendarViewSelectionMode SelectionMode
        {
            get => (CalendarViewSelectionMode) GetValue(SelectionModeProperty);
            set => SetDependencyValue(SelectionModeProperty, value);
        }
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register(nameof(SelectionMode), typeof(CalendarViewSelectionMode), typeof(DrxCalendarSelectorControl), null);

        public delegate void DocumentSelectionChangedDelegate(object sender, IList<DrxDocumentViewModel> document);
        public event DocumentSelectionChangedDelegate DocumentSelectionChanged;

        public DrxCalendarSelectorControl()
        {
            InitializeComponent();
        }

        public void SetSelectedDocuments(IList<object> documents)
        {
            // Unregister event to avoid firing twice
            Calendar.SelectedDatesChanged -= Calendar_OnSelectedDatesChanged;
            Calendar.SelectedDates.Clear();

            foreach (var document in documents)
            {
                var day = ((DrxDocumentViewModel)document).Model.Header.TimeStamp.Date;
                Calendar.SelectedDates.Add(day);
            }
            
            // Only set the currently displayed date if we have
            // one document, to avoid confusing users
            if (documents.Count == 1)
                Calendar.SetDisplayDate(((DrxDocumentViewModel)documents.First()).Model.Header.TimeStamp.Date);

            // Re-register event
            Calendar.SelectedDatesChanged += Calendar_OnSelectedDatesChanged;
        }

        private void DrxCalendar_OnCalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            if (args.Item == null || Store == null)
                return;

            switch (args.Phase)
            {
                case 0:
                    // register for first phase callback
                    args.RegisterUpdateCallback(DrxCalendar_OnCalendarViewDayItemChanging);
                    return;
                case 1:
                    // run this
                    var day = args.Item.Date;
                    var documents = Store.Documents.Where(d => d.Model.Header.TimeStamp.Date == day.Date).ToList();

                    // TODO: this breaks adding documents. Disabled until we can figure out a way to refresh the control when adding or removing documents.
                    /**
                    if (!documents.Any())
                    {
                        args.Item.IsBlackout = true;
                        return;
                    }*/

                    var colours = documents.Select(document => GuiConverterMappings.SecurityLevelToColourMappings[document.SecurityLevel]);
                    args.Item.SetDensityColors(colours);
                    break;
                default:
                    return;
            }
        }

        private void Calendar_OnSelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (Store == null) return;
            var documents = sender.SelectedDates.SelectMany(date => Store.Documents.Where(d => d.Model.Header.TimeStamp.Date == date.Date).ToList()).ToList();
            DocumentSelectionChanged?.Invoke(this, documents);
        }

        public void ForceUpdate() => Calendar.UpdateLayout();

        public event PropertyChangedEventHandler PropertyChanged;
        private void SetDependencyValue(DependencyProperty property, object value,
            [CallerMemberName] string propertyName = null)
        {
            SetValue(property, value);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
