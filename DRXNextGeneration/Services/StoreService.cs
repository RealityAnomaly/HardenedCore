using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using DRXLibrary.Models.Drx.Store;
using DRXNextGeneration.Common.Extensions;
using MessagePack;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace DRXNextGeneration.Services
{
    /// <summary>
    /// Provides an interface to the document store subsystem.
    /// </summary>
    public class StoreService
    {
        private DrxStoreCache _storesCache;
        private StorageFolder _storesFolder;
        private readonly ILogger _logger;
        private const string FileName = "stores.pack";

        public StoreService(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger(typeof(StoreService));
            _logger.LogInformation("Store service logging initialised.");
        }

        public IEnumerable<DrxStore> Stores => _storesCache.Stores;

        public IEnumerable<DrxStore> GetStores()
        {
            return _storesCache.Stores;
        }

        public DrxStore GetStore(Guid id)
        {
            return _storesCache.Stores.FirstOrDefault(s => s.Id == id);
        }

        public DrxStore GetDefaultStore()
        {
            var roamingSettings = ApplicationData.Current.RoamingSettings;
            return roamingSettings.Values.ContainsKey("DefaultStore")
                ? Stores.FirstOrDefault(s => s.Id == (Guid)roamingSettings.Values["DefaultStore"])
                : null;
        }

        public async Task Initialise()
        {
            _logger.LogInformation("Initialising stores.");

            // Load all stores
            _storesFolder = await ApplicationData.Current.RoamingFolder.CreateFolderAsync("stores", CreationCollisionOption.OpenIfExists);
            await LoadStoreCache();

            foreach (var store in _storesCache.Stores)
            {
                store.RegisterLocalBacker();
                await store.LoadAsync();
                await store.LoadHeadersAsync();

                _logger.LogInformation($"Store {store} loaded.");
            }

            // If no stores exist, create the default local store
            if (!_storesCache.Stores.Any())
            {
                var store = new DrxStore {Name = "Default Local Store"};
                store.RegisterLocalBacker();
                await AddStoreAsync(store);
                await SaveStoreCache();

                _logger.LogInformation($"Default store {store} autocreated.");
            }
        }

        private async Task LoadStoreCache()
        {
            _logger.LogInformation("Loading store cache.");

            // Create the stores file if it does not exist
            if (await _storesFolder.FileExistsAsync(FileName))
            {
                var storesFile = await _storesFolder.GetFileAsync(FileName);
                using (var stream = await storesFile.OpenStreamForReadAsync())
                    _storesCache = await MessagePackSerializer.DeserializeAsync<DrxStoreCache>(stream);
            }
            // Compatability for old version
            else if (await _storesFolder.FileExistsAsync("stores.json"))
            {
                var storesFile = await _storesFolder.GetFileAsync("stores.json");
                _storesCache = JsonConvert.DeserializeObject<DrxStoreCache>(await FileIO.ReadTextAsync(storesFile));
            }
            else
            {
                _storesCache = new DrxStoreCache();
            }
        }

        public async Task SaveStoreCache()
        {
            _logger.LogInformation("Saving store cache.");

            var storesFile = await _storesFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            using (var stream = await storesFile.OpenStreamForWriteAsync())
                await MessagePackSerializer.SerializeAsync(stream, _storesCache);
        }

        /// <summary>
        /// Initialises the specified store and adds it to the cache.
        /// </summary>
        public async Task AddStoreAsync(DrxStore store)
        {
            _logger.LogInformation($"Adding store {store} to cache.");

            await store.LoadAsync();
            _storesCache.Stores.Add(store);
        }

        public async Task DeleteStoreAsync(DrxStore store)
        {
            _logger.LogInformation($"Removing store {store} from cache.");

            await store.DeleteAsync();
            _storesCache.Stores.Remove(store);
        }
    }
}
