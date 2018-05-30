using SingPhotoUWP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SingPhotoUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoHistory : Page
    {
        private List<ShowInfo> HistorViewModel { get; set; }
        public InfoHistory()
        {
            this.InitializeComponent();
           
            var gridView = this.FindName("GridView") as ItemsControl;
            LoadHistory();
            gridView.ItemsSource = HistorViewModel.Select((p, i) => new{ Item = p, Index = i + 1});
        }

        public async void LoadHistory()
        {
            HistorViewModel = await StorageHelper<List<ShowInfo>>.LoadData(App.HistoryFileName);
        }
    }
}
