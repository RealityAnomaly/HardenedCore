using Windows.UI.Xaml.Navigation;
using DRXNextGeneration.Common;
using DRXNextGeneration.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DRXNextGeneration.Views.Store
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StoreFlagEditorPage : IViewCommandHandler
    {
        private DrxStoreViewModel _store;

        public StoreFlagEditorPage()
        {
             InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!(e.Parameter is DrxNavigationInfo info)) return;
            _store = info.Store;

            FlagSelector.Store = _store;
            HandleCommand(info.Command);
        }

        public void HandleCommand(object command)
        {
            FlagSelector.HandleCommand(command);
        }
    }
}
