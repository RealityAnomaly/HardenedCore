using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using CoreLibrary.Common.CoreLibrary.Common;

namespace DRXNextGeneration.ViewModels
{
    public class DrxAppSettingsViewModel : BindableBase
    {
        private readonly DrxStoreServiceViewModel _service;
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        private readonly ApplicationDataContainer _roamingSettings = ApplicationData.Current.RoamingSettings;

        public DrxStoreViewModel DefaultStore
        {
            get
            {
                return _roamingSettings.Values.ContainsKey(nameof(DefaultStore)) 
                    ? _service.Stores.FirstOrDefault(s => s.Model.Id == (Guid)_roamingSettings.Values[nameof(DefaultStore)]) 
                    : null;
            }

            set
            {
                if (DefaultStore == value) return;
                _roamingSettings.Values["DefaultStore"] = value.Model.Id;
                OnPropertyChanged();
            }
        }

        public DrxAppSettingsViewModel(DrxStoreServiceViewModel service)
        {
            _service = service;
        }
    }
}
