using SingPhotoUWP.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
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
    public sealed partial class DisplayPage : Page
    {
        private ShowInfo ViewModel { get; set; }
        private ImageSource imageSource;
        private MediaPlayer mediaPlayer = new MediaPlayer();
        public DisplayPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //这个e.Parameter是获取传递过来的参数，其实大家应该再次之前判断这个参数是否为null的，我偷懒了
            if(e != null)
            {
                ViewModel = (ShowInfo)e.Parameter;
                //this.imageSource = await ImageHelper.ConvertBytesToBitmapImage(ViewModel.imge_source);
                var s = Convert.ToBase64String(ViewModel.imge_source);
                imageSource = await ImageHelper.SaveToImageSource(ViewModel.imge_source);
                this.Image_Thumbnail.Source = this.imageSource;
            }
        }

        private void Circle_Boy_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.musicsource_boy != null)
            {
                if (this.ViewModel.musicsource_boy.Length > 0)
                {
                    this.mediaPlayer.Dispose();
                    this.mediaPlayer = new MediaPlayer();
                    mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(this.ViewModel.musicsource_boy));
                    mediaPlayer.Play();
                }
            }
        }

        private void Circle_Girl_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.musicsource_girl != null)
            {
                if (this.ViewModel.musicsource_girl.Length > 0)
                {
                    this.mediaPlayer.Dispose();
                    this.mediaPlayer = new MediaPlayer();
                    mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(this.ViewModel.musicsource_girl));
                    mediaPlayer.Play();
                }
            }
        }
    }
}
