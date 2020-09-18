using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using DRXNextGeneration.Common;
using DRXNextGeneration.Services;
using DRXNextGeneration.ViewModels;
using DRXNextGeneration.Views.Store;
using Microsoft.Extensions.DependencyInjection;
using static DRXNextGeneration.Common.DrxCommand;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DRXNextGeneration.Views
{
    /// <inheritdoc cref="Page" />
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : IViewCommandHandler
    {
        private readonly DrxStoreServiceViewModel _storeService;

        private readonly IList<(string Tag, Type Type)> _pages = new List<(string tag, Type type)>
        {
            ("documents", typeof(StoreDocumentsPage)),
            ("auditor", typeof(StoreAuditorPage)),
            ("flags", typeof(StoreFlagEditorPage)),
            ("preferences", typeof(StorePreferencesPage))
        };

        public MainPage()
        {
            InitializeComponent();
            var service = DrxServiceProvider.GetServiceProvider().GetService<StoreService>();
            _storeService = new DrxStoreServiceViewModel(service);
            _storeService.PopulateStoreList();
        }

        public void HandleCommand(object command)
        {
            // We might not be fully loaded, so if not,
            // register a delegate to run the command when we are.
            if (!IsLoaded)
            {
                Loaded += delegate { HandleCommand(command); };
                return;
            }

            if (!(command is DrxCommand drxCommand)) return;
            switch (drxCommand.Subject)
            {
                case DrxCommandSubject.StoreService:
                    switch (drxCommand.Command)
                    {
                        case "new":
                            ((ICommand)Resources["AddStoreCommand"]).Execute(_storeService);
                            break;
                        case "restore":
                            ((ICommand)Resources["RestoreStoreCommand"]).Execute(_storeService);
                            break;
                    }
                    break;
                case DrxCommandSubject.Store:
                case DrxCommandSubject.StoreDocuments:
                case DrxCommandSubject.StoreFlags:
                case DrxCommandSubject.Document:
                case DrxCommandSubject.Flag:
                    HandleStoreCtxCommand(drxCommand);
                    break;
            }
        }

        private void HandleStoreCtxCommand(DrxCommand command)
        {
            DrxStoreViewModel store;
            if (command.Parameters.ContainsKey("storeId"))
            {
                store = _storeService.Stores.FirstOrDefault(s =>
                    s.Model.Id == (Guid) command.Parameters["storeId"]);
            }
            else if (command.Parameters.ContainsKey("storeName"))
            {
                store = _storeService.Stores.FirstOrDefault(s =>
                    s.Model.Name == (string) command.Parameters["storeName"]);
            }
            else
            {
                store = _storeService.DefaultStore;
            }

            if (store == null) return;

            StoreList.SelectedItem = _storeService.DefaultStore;

            switch (command.Subject)
            {
                case DrxCommandSubject.Store:
                    switch (command.Command)
                    {
                        case "open":
                            // Already opened by selecting it
                            break;
                        case "backup":
                            ((ICommand)Resources["BackupStoreCommand"]).Execute(store);
                            break;
                        case "delete":
                            ((ICommand)Resources["DeleteStoreCommand"]).Execute(store);
                            break;
                        case "auditor":
                            DoPageTransition("auditor", new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft }, false, command);
                            break;
                        case "properties":
                            //((ICommand)Resources["EditStoreCommand"]).Execute(store);
                            DoPageTransition("preferences", new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft }, false, command);
                            break;
                    }
                    break;
                case DrxCommandSubject.StoreDocuments:
                case DrxCommandSubject.Document:
                    DoPageTransition("documents", new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft }, false, command);
                    break;
                case DrxCommandSubject.StoreFlags:
                case DrxCommandSubject.Flag:
                    DoPageTransition("flags", new SlideNavigationTransitionInfo { Effect = SlideNavigationTransitionEffect.FromLeft }, false, command);
                    break;
                default:
                    return;
            }
        }

        private void ContentFrame_OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Could not navigate to page {e.SourcePageType.FullName}");
        }

        private void On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return;

            if (NavigationView.IsPaneOpen &&
                (NavigationView.DisplayMode == NavigationViewDisplayMode.Compact ||
                 NavigationView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return;

            ContentFrame.GoBack();
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavigationView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                NavigationView.SelectedItem = (NavigationViewItem) NavigationView.SettingsItem;
                NavigationView.Header = "Settings";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                var item = _pages.FirstOrDefault(p => p.Type == e.SourcePageType);
                NavigationView.SelectedItem = NavigationView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));
                NavigationView.Header = ((NavigationViewItem) NavigationView.SelectedItem)?.Content?.ToString();
            }
        }

        private void On_BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private void NavigationView_OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        private void NavigationView_OnLoaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigated += On_Navigated;
            if (StoreList.Items?.Count > 0)
                StoreList.SelectedItem = StoreList.Items?[0];

            // Add keyboard accelerators
            var goBack = new KeyboardAccelerator {Key = VirtualKey.GoBack};
            goBack.Invoked += On_BackInvoked;
            KeyboardAccelerators.Add(goBack);

            var altLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left,
                Modifiers = VirtualKeyModifiers.Menu
            };

            altLeft.Invoked += On_BackInvoked;
            KeyboardAccelerators.Add(altLeft);
        }

        private void NavigationView_OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                NavigationView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavigationView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo) =>
            DoPageTransition(navItemTag, transitionInfo);

        private void DoPageTransition(string navItemTag, NavigationTransitionInfo transitionInfo,
            bool forceTransition = false, object command = null)
        {
            var page = navItemTag == "settings" ? typeof(SettingsPage) : _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag)).Type;

            var preNavPageType = ContentFrame.CurrentSourcePageType;
            if (!(page is null) && (preNavPageType != page || forceTransition))
            {
                var info = new DrxNavigationInfo
                {
                    Store = (DrxStoreViewModel) StoreList.SelectedItem,
                    Service = _storeService,
                    Command = command
                };

                ContentFrame.Navigate(page, info, transitionInfo);
            }
            else if (ContentFrame.Content is IViewCommandHandler handler)
            {
                // If our page is already open and we aren't forcing transition,
                // use the handler to handle the command. This saves us from
                // the unnecessary creation and destruction of pages.
                handler.HandleCommand(command);
            }
        }

        private void NavigationView_OnPaneOpening(NavigationView sender, object args)
        {
            StoreSelectorGrid.Visibility = Visibility.Visible;
        }

        private void NavigationView_OnPaneClosing(NavigationView sender, object args)
        {
            StoreSelectorGrid.Visibility = Visibility.Collapsed;
        }

        private void StoreList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NavigationView.SelectedItem is NavigationViewItem viewItem)
            {
                if (!(viewItem.Tag is string str) || string.IsNullOrWhiteSpace(str) || str == "settings")
                    return;
                var transition = new SlideNavigationTransitionInfo();
                var added = e.AddedItems.FirstOrDefault();
                var removed = e.RemovedItems.FirstOrDefault();
                if (added != null && removed != null)
                    transition.Effect = StoreList.Items?.IndexOf(added) < StoreList.Items?.IndexOf(removed) ? SlideNavigationTransitionEffect.FromLeft : SlideNavigationTransitionEffect.FromRight;

                DoPageTransition(str, transition, true);
            }
        }

        private void DeleteConfirmation_Click(object sender, RoutedEventArgs e) => DeleteFlyout.Hide();
    }
}
