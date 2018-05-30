using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;

namespace SingPhotoUWP.Model
{
    public static class StorageHelper<T>
    {
        public static async Task SaveData(string filename, T data)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream raStream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (IOutputStream outStream = raStream.GetOutputStreamAt(0))
                    {
                        
                        var json = JsonConvert.SerializeObject(data);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(string));
                        serializer.WriteObject(outStream.AsStreamForWrite(), json);
                        var t =JsonConvert.DeserializeObject<List<ShowInfo>>(json);
                        await outStream.FlushAsync();
                    }
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public static async System.Threading.Tasks.Task<List<ShowInfo>> LoadData(string filename)
        {
            try
            {
                if (ApplicationData.Current.LocalFolder.TryGetItemAsync(filename) != null)
                {
                    StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filename);
                    using (IInputStream inStream = await file.OpenSequentialReadAsync())
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(string));
                        var json = (string)serializer.ReadObject(inStream.AsStreamForRead());
                        var data = JsonConvert.DeserializeObject<List<ShowInfo>>(json);
                        return data;
                    }
                }
                else
                {
                    return new List<ShowInfo>();
                }
                
                //var buffer = await Windows.Storage.FileIO.ReadBufferAsync(sampleFile);
                //using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
                //{
                //    string text = dataReader.ReadString(buffer.Length);
                //}
               
            }
            catch (FileNotFoundException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public static async Task<string> WriteToFile(SoftwareBitmap softwareBitmap)
        //{
        //    string fileName = Path.GetRandomFileName();

        //    if (softwareBitmap != null)
        //    {
        //        // save image file to cache
        //        StorageFile file = await _localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
        //        using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
        //        {
        //            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
        //            encoder.SetSoftwareBitmap(softwareBitmap);
        //            await encoder.FlushAsync();
        //        }
        //    }
        //    return fileName;
        //}

        //public static async Task<SoftwareBitmap> ReadFromFile(string filename)
        //{
        //    StorageFile file = await _localFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
        //    //var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri( filename));
        //    using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
        //    {
        //        // Create the decoder from the stream
        //        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
        //        // Get the SoftwareBitmap representation of the file
        //        SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
        //        return softwareBitmap;
        //    }
        //}
    }   
}
