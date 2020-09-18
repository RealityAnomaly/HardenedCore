using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using CoreLibrary.Common.CoreLibrary.Common;
using DRXLibrary.Models.Drx;
using DRXNextGeneration.Common.Extensions;
using Microsoft.Toolkit.Uwp.Helpers;

namespace DRXNextGeneration.ViewModels
{
    public class DrxFlagViewModel : BindableBase
    {
        public readonly DrxFlag Model;

        // Notifying properties
        public string Tag
        {
            get => Model.Tag;
            set { Model.Tag = value; OnPropertyChanged(); }
        }
        public string Name
        {
            get => Model.Name;
            set { Model.Name = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get => Model.Description;
            set { Model.Description = value; OnPropertyChanged(); }
        }
        public Color Colour
        {
            get => ColourExtensions.FromDrxColour(Model.Colour);
            set { Model.Colour = value.ToDrxColour(); OnPropertyChanged(); }
        }
        public DrxSecurityLevel SecurityLevel
        {
            get => Model.SecurityLevel;
            set { Model.SecurityLevel = value; OnPropertyChanged(); }
        }

        public DrxFlagViewModel(DrxFlag flag)
        {
            Model = flag;
        }

        public override string ToString() => Model.ToString();
    }
}
