using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using CoreLibrary.Common.CoreLibrary.Common;
using DRXLibrary.Models.Drx;
using DRXLibrary.Models.Drx.Store;
using DRXNextGeneration.Models;
using Microsoft.AppCenter.Crashes;

namespace DRXNextGeneration.ViewModels
{
    public class DrxStoreViewModel : BindableBase
    {
        public readonly DrxStore Model;
        private readonly DrxStoreServiceViewModel _service;

        public string Name
        {
            get => Model.Name;
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }

        public bool CryptoProviderPresent => Model.Key != null;
        public ObservableCollection<DrxDocumentViewModel> Documents { get; } = new ObservableCollection<DrxDocumentViewModel>();
        public ObservableCollection<DrxFlagViewModel> FlagDefinitions { get; } = new ObservableCollection<DrxFlagViewModel>();

        public DrxStoreViewModel(DrxStore store, DrxStoreServiceViewModel service)
        {
            Model = store;
            _service = service;
            RefreshModel();
        }

        public async Task DeleteAsync() => await _service.DeleteStoreAsync(this);
        public async Task SaveAsync() => await _service.SaveCacheAsync(); // TODO

        /// <summary>
        /// Displays the file picker to back up this store.
        /// </summary>
        public async Task<bool> BackupWithUiAsync(bool exportDecrypted)
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder,
                SuggestedFileName = DateTime.Now.ToShortDateString(),
                DefaultFileExtension = ".bdrx",
                CommitButtonText = "Backup"
            };

            picker.FileTypeChoices.Add("DRX Store Backup", new List<string> { ".bdrx" });

            var file = await picker.PickSaveFileAsync();
            if (file == null)
                return false;

            try
            {
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    stream.SetLength(0); // Make sure we clear the file so there are no bytes left at the end
                    await Model.BackupStoreAsync(stream, exportDecrypted);
                }
                    
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Displays the file picker to restore this store.
        /// </summary>
        public async Task<bool> RestoreWithUiAsync()
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder,
                CommitButtonText = "Restore",
            };

            picker.FileTypeFilter.Add(".bdrx");
            var file = await picker.PickSingleFileAsync();
            if (file == null)
                return false;

            try
            {
                using (var stream = await file.OpenStreamForReadAsync())
                    await Model.RestoreStoreAsync(stream);

                return true;
            }
            catch (Exception)
            {
                throw; // TODO: test
                return false;
            }
        }

        public void CommitEncryption(string providerName, object init)
        {
            // We wait until commit time to set encryption
            // because there might be some protectors that
            // needed initialisation before setting the key
            if (providerName == "None")
            {
                Model.DisableEncryption();
                return;
            }

            Model.EnableEncryption(providerName, init);
        }

        public void RefreshModel()
        {
            Documents.Clear();

            // TODO: THIS SHOULDN'T BE HERE!
            if (Model.FlagDefinitions == null)
                Model.FlagDefinitions = new List<DrxFlag>();

            // Sort by date.
            // TODO: in the future this should be configurable somehow
            var documentCollection = from drxDocument in Model.GetDocuments()
                orderby drxDocument.Header.TimeStamp descending 
                select drxDocument;
            foreach (var document in documentCollection)
                Documents.Add(new DrxDocumentViewModel(document, this));

            // Flags are sorted ascending by name.
            var flagCollection = from flag in Model.FlagDefinitions
                orderby flag.Tag 
                select flag;
            foreach (var flag in flagCollection)
                FlagDefinitions.Add(new DrxFlagViewModel(flag));
        }

        public override string ToString() => Model.ToString();
    }
}
