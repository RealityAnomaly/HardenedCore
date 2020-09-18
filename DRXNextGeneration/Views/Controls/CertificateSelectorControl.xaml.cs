using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DRXNextGeneration.Views.Controls
{
    public sealed partial class CertificateSelectorControl
    {
        public ObservableCollection<X509Certificate2> ItemsSource { get; set; }
        public X509Certificate2 SelectedItem { get; set; }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<X509Certificate2>), typeof(CertificateSelectorControl), null);
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(X509Certificate), typeof(CertificateSelectorControl), null);

        public CertificateSelectorControl()
        {
            InitializeComponent();
        }
    }
}
