using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Common.CoreLibrary.Common;
using DRXLibrary.Models.Drx;

namespace DRXNextGeneration.ViewModels
{
    public class DrxDocumentViewModel : BindableBase
    {
        public readonly DrxDocument Model;
        public readonly DrxStoreViewModel Store;
        private bool _unsaved;

        // Notifying properties
        public string Title
        {
            get => Model.Header.Title;
            set { Model.Header.Title = value; OnPropertyChanged(nameof(Title)); }
        }
        public DrxSecurityLevel SecurityLevel
        {
            get => Model.Header.SecurityLevel;
            set
            {
                Model.Header.SecurityLevel = value;
                OnPropertyChanged();
            }
        }

        // Non-notifying
        public DateTimeOffset TimeStamp
        {
            get => Model.Header.TimeStamp;
            set => Model.Header.TimeStamp = value;
        }
        public bool Encrypted
        {
            get => Model.Header.Encrypted;
            set => Model.Header.Encrypted = value;
        }
        public bool Unsaved
        {
            get => _unsaved;
            set => SetProperty(ref _unsaved, value);
        }

        public DrxDocumentViewModel(DrxDocument document, DrxStoreViewModel store)
        {
            Model = document;
            Store = store;
        }

        public bool IsHighSecurity() => SecurityLevel >= DrxSecurityLevel.StormVault;
        public override string ToString() => Model.ToString();
    }
}
