using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SingPhotoUWP.Model;
using Windows.Media.Playback;
using Windows.Media.Core;
using Windows.UI.Popups;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.ViewManagement;
using Windows.UI;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace SingPhotoUWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private StorageFile imgFile;

        private async void Btn_StartUpload_Click(object sender, RoutedEventArgs e)
        {

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
                using (var content = new MultipartFormDataContent())
                {
                    var formDatas = this.GetFormDataByteArrayContent();
                    if (this.imgFile != null)
                    {
                        var files = await GetByteArrayContents();
                        Action<List<ByteArrayContent>> act_formdata = (dataContents) =>
                        {
                            foreach (var byteArrayContent in dataContents)
                            {
                                content.Add(byteArrayContent, "code");
                            }
                        };
                        Action<List<ByteArrayContent>> act_file = (dataContents) =>
                        {
                            for (int i = 0; i < dataContents.Count; i++)
                            {
                                content.Add(dataContents[i], "imageFile", imgFile.Path);
                            }
                        };
                        act_formdata(formDatas);
                        act_file(files);
                        try
                        {
                            LoadingControl.IsLoading = true;
                            var result = await client.PostAsync("http://singingphoto.com/songbot/requestsong", content);
                            string jsontext = result.Content.ReadAsStringAsync().Result;
                            if (jsontext.Length > 0)
                            {
                                SingPhotoResult DataSource = SingPhotoResultHelper.GetSingPhotoResultFromJsonText(jsontext);
                                if (DataSource.IsSucceed)
                                {
                                    
                                    string lyrics = string.Empty;
                                    foreach (var line in DataSource.Lyrics)
                                    {
                                        string[] temp = line.Split('#');
                                        if (temp.Length == 2)
                                        {
                                            lyrics += temp[1] + '\r';
                                        }
                                    }
                                    //byte[] tmp = ImageHelper.ToBase64(this.Image_Thumbnail).Result;
                                    //var bitmap = new RenderTargetBitmap();
                                    //await bitmap.RenderAsync(this.Image_Thumbnail);
                                    //var pixels = await bitmap.GetPixelsAsync();
                                    //byte[] bytes = pixels.ToArray();
                                    
                                    var bytes = await ImageHelper.SaveToBytesAsync(this.Image_Thumbnail.Source);
                                    var s = Convert.ToBase64String(bytes);
                                    ShowInfo info = new ShowInfo(lyrics, DataSource.SongsURLs.男子汉, DataSource.SongsURLs.萌妹子, bytes);
                                    //MrOwlLib.MrOwlLibofLocalData.SaveDatatoContainer(info, App.AppContainerKey, App.AppHistoryInfoKey);
                                    if (!App.InfoHistory.Contains(info))
                                    {
                                        App.InfoHistory.Add(info);
                                        await StorageHelper<List<ShowInfo>>.SaveData(App.HistoryFileName, App.InfoHistory);
                                    }
                                    Frame root = Window.Current.Content as Frame;
                                    //这里参数自动装箱
                                    root.Navigate(typeof(DisplayPage), info);

                                }
                                else
                                {
                                    MessageDialog msgdlg = new MessageDialog(DataSource.Message, "请求失败");
                                    await msgdlg.ShowAsync();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageDialog msgdlg = new MessageDialog(ex.Message, "请求失败");
                            await msgdlg.ShowAsync();
                        }
                        LoadingControl.IsLoading = false;
                    }
                    else
                    {
                        MessageDialog msgdlg = new MessageDialog("请选择图片~");
                        await msgdlg.ShowAsync();
                    }

                }
            }
        }

        private async void ChoosePhoto()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileOpenPicker.FileTypeFilter.Add(".jpg");
            fileOpenPicker.FileTypeFilter.Add(".png");
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            StorageFile imgFile_tmp = await fileOpenPicker.PickSingleFileAsync();
            if (imgFile_tmp == null)
            {
                return;
            }
            else
            {
                //SoftwareBitmap softwareBitmap;
                //this.imgFile = imgFile_tmp;
                //using (IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.Read))
                //{
                //    // Create the decoder from the stream
                //    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                //    // Get the SoftwareBitmap representation of the file
                //    softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                //}
                //if (softwareBitmap.BitmapPixelFormat != BitmapPixelFormat.Bgra8 || softwareBitmap.BitmapAlphaMode == BitmapAlphaMode.Straight)
                //{
                //    softwareBitmap = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                //}
                //var source = new SoftwareBitmapSource();
                //await source.SetBitmapAsync(softwareBitmap);
                //// Set the source of the Image control
                //Image_Thumbnail.Source = source;
                this.imgFile = imgFile_tmp;
                //var urisource = new Uri(imgFile_tmp.Path,UriKind.Absolute);
                //BitmapImage bitmapImage = new BitmapImage(urisource);
                //Image_Thumbnail.Source = bitmapImage;
                using (IRandomAccessStream stream = await imgFile.OpenAsync(FileAccessMode.Read))
                {
                    var srcImage = new WriteableBitmap(50,50);
                    await srcImage.SetSourceAsync(stream);
                    Image_Thumbnail.Source = srcImage;
                }
            }
        } 
       
        private async Task<List<ByteArrayContent>> GetFileByteArrayContent(HashSet<string> files)
        {
            List<ByteArrayContent> list = new List<ByteArrayContent>();           
            foreach (var file in files)
            {
                await Task.Run(() => {
                    if(file.Length > 0)
                    {
                        try
                        {
                            var fileContent = new ByteArrayContent(File.ReadAllBytes(file));
                            ContentDispositionHeaderValue dispositionHeader = new ContentDispositionHeaderValue("file");
                            dispositionHeader.DispositionType = "file";
                            dispositionHeader.Name = "imageFile";
                            dispositionHeader.FileName = Path.GetFileName(file);
                            fileContent.Headers.ContentDisposition = dispositionHeader;
                            list.Add(fileContent);
                        }
                        catch(Exception ex)
                        {
                            MessageDialog msgdlg = new MessageDialog("请求失败", ex.Message);
                            msgdlg.ShowAsync();
                        }
                    }
                });
            }
            return list;
        }
        private List<ByteArrayContent> GetFormDataByteArrayContent()
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("code", "5a96a50f-4a47-4760");
            List<ByteArrayContent> list = new List<ByteArrayContent>();
            foreach (var key in collection.AllKeys)
            {
                var dataContent = new ByteArrayContent(Encoding.UTF8.GetBytes(collection[key]));
                dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = key
                };
                list.Add(dataContent);
            }
            return list;
        }
        //private HashSet<string> GetHashSet(string path)
        //{
        //    HashSet<string> hash = new HashSet<string>();
        //    hash.Add(path);
        //    return hash;
        //}
        private async Task<List<ByteArrayContent>> GetByteArrayContents()
        {
            List<ByteArrayContent> files = new List<ByteArrayContent>();
            if (imgFile != null)
            {
                try
                {
                    var buffer = await FileIO.ReadBufferAsync(imgFile);
                    byte[] content = new byte[buffer.Length];
                    // Use a dataReader object to read from the buffer
                    using (DataReader dataReader = DataReader.FromBuffer(buffer))
                    {
                        dataReader.ReadBytes(content);
                        // Perform additional tasks
                    }
                    var fileContent = new ByteArrayContent(content);
                    ContentDispositionHeaderValue dispositionHeader = new ContentDispositionHeaderValue("file");
                    dispositionHeader.DispositionType = "file";
                    dispositionHeader.Name = "imageFile";
                    dispositionHeader.FileName = imgFile.Path;
                    fileContent.Headers.ContentDisposition = dispositionHeader;
                    files.Add(fileContent);
                }
                catch (Exception ex)
                {
                    MessageDialog msgdlg = new MessageDialog("GetByteArrayContents", ex.Message);
                    await msgdlg.ShowAsync();
                }
            }
            return files;
        }
        private void Btn_ChoosePhoto_Click(object sender, RoutedEventArgs e)
        {
            ChoosePhoto();
        }

        private void Btn_History_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            //这里参数自动装箱
            root.Navigate(typeof(InfoHistory));
        }
    }
}
