using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using DRXNextGeneration.Services;
using Microsoft.AppCenter.Analytics;

namespace DRXNextGeneration.ViewModels
{
    public class DrxStoreServiceViewModel
    {
        public readonly StoreService Model;
        public readonly ObservableCollection<DrxStoreViewModel> Stores
            = new ObservableCollection<DrxStoreViewModel>();
        public DrxStoreViewModel DefaultStore;

        public DrxStoreServiceViewModel(StoreService service)
        {
            Model = service;
        }

        public void PopulateStoreList()
        {
            // Assign our stores
            Stores.Clear();
            foreach (var store in Model.GetStores())
                Stores.Add(new DrxStoreViewModel(store, this));

            // Assign the default store
            var def = Model.GetDefaultStore();
            if (def != null)
                DefaultStore = Stores.FirstOrDefault(s => s.Model.Id == def.Id);
        }

        public async Task AddStoreAsync(DrxStoreViewModel store)
        {
            Analytics.TrackEvent("Store created.");

            Stores.Add(store);
            await Task.Run(async () =>
            {
                await Model.AddStoreAsync(store.Model);
                await Model.SaveStoreCache();
            }).ConfigureAwait(false);
        }

        public async Task DeleteStoreAsync(DrxStoreViewModel store)
        {
            Analytics.TrackEvent("Store deleted.");

            Stores.Remove(store);
            await Task.Run(async () =>
            {
                // Delete it from the cache
                await Model.DeleteStoreAsync(store.Model);
                await Model.SaveStoreCache();
            }).ConfigureAwait(false);
        }

        public async Task SaveCacheAsync()
        {
            await Task.Run(async () => { await Model.SaveStoreCache(); }).ConfigureAwait(false);
        }
    }
}
