using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SingPhotoUWP.Model
{
    public class ShowInfo
    {
        public string lyrics { get; set; }
        public string musicsource_boy { get; set; }
        public string musicsource_girl { get; set; }
        public byte[] imge_source { get; set; }

        public ShowInfo()
        {

        }

        public ShowInfo(string lyrics, string musicsource_boy, string musicsource_girl, ImageSource imge_source)
        {
            this.lyrics = lyrics;
            this.musicsource_boy = musicsource_boy;
            this.musicsource_girl = musicsource_girl;
            this.imge_source = ImageHelper.SaveToBytesAsync(imge_source).Result;
        }

        public ShowInfo(string lyrics, string musicsource_boy, string musicsource_girl, byte[] imge_source)
        {
            this.lyrics = lyrics;
            this.musicsource_boy = musicsource_boy;
            this.musicsource_girl = musicsource_girl;
            this.imge_source = imge_source;
        }

    }

    public static class ImageHelper
    {
        public static async Task<byte[]> ToBase64(ImageSource source)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var img = (BitmapImage)source;
                WriteableBitmap btmMap = new WriteableBitmap(img.PixelWidth, img.PixelHeight);
                
                var bytes = btmMap.PixelBuffer.ToArray();
                //bt.SaveJpeg(btmMap, ms, img.PixelWidth, img.PixelHeight, 0, 100);
                //img = null;

                return bytes;
            }
            //var himage = (BitmapImage)source;
            //RandomAccessStreamReference random = RandomAccessStreamReference.CreateFromUri(himage.UriSource);

            //byte[] bytes = null;
            //using (IRandomAccessStream stream = await random.OpenReadAsync())
            //{
            //    //Create a decoder for the image
            //    var decoder = await BitmapDecoder.CreateAsync(stream);
            //    Windows.Graphics.Imaging.PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
            //    bytes = pixelData.DetachPixelData();
            //}

            ////var himage = (BitmapImage)source;
            ////RandomAccessStreamReference random = RandomAccessStreamReference.CreateFromUri(himage.UriSource);

            ////byte[] bytes = null;
            ////using (IRandomAccessStream stream = await random.OpenReadAsync())
            ////{
            ////    Create a decoder for the image
            ////    var decoder = await BitmapDecoder.CreateAsync(stream);
            ////    Windows.Graphics.Imaging.PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
            ////    bytes = pixelData.DetachPixelData();
            ////}
            //return bytes;
        }

        public async static Task<BitmapImage> ConvertBytesToBitmapImage(byte[] bytes)
        {
            try
            {
                if (bytes == null || bytes.Length == 0) return null;

                var stream = new MemoryStream(bytes);
                var randomAccessStream = new InMemoryRandomAccessStream();
                using (var outputStream = randomAccessStream.GetOutputStreamAt(0))
                {
                    var dw = new DataWriter(outputStream);
                    var task = new Task(() => dw.WriteBytes(stream.ToArray()));
                    task.Start();
                    await task;
                    await dw.StoreAsync();
                    await outputStream.FlushAsync();

                    var bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(randomAccessStream);
                    return bitmapImage;
                }
            }
            catch (Exception exception)
            {
               //Debug.WriteLine("[Error] Convert bytes to BitmapImage failed,exception:{0}", exception);
                return null;
            }
        }

        public static async Task<byte[]> SaveToBytesAsync(ImageSource imageSource)
        {
            byte[] imageBuffer;
            //var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var localFolder = ApplicationData.Current.TemporaryFolder;
            var file = await localFolder.CreateFileAsync("temp.jpg", CreationCollisionOption.GenerateUniqueName);
            using (var ras = await file.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None))
            {
                WriteableBitmap bitmap = imageSource as WriteableBitmap;
                var stream = bitmap.PixelBuffer.AsStream();
                byte[] buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, ras);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96.0, 96.0, buffer);
                await encoder.FlushAsync();

                var imageStream = ras.AsStream();
                imageStream.Seek(0, SeekOrigin.Begin);
                imageBuffer = new byte[imageStream.Length];
                var re = await imageStream.ReadAsync(imageBuffer, 0, imageBuffer.Length);

            }
            await file.DeleteAsync(StorageDeleteOption.Default);
            return imageBuffer;
        }

        //public static async Task<byte[]> SaveToBytesAsync(ImageSource imageSource)
        //{
        //    BitmapImage source = imageSource as BitmapImage;

        //    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(source.UriSource);
        //    using (var inputStream = await file.OpenSequentialReadAsync())
        //    {
        //        var readStream = inputStream.AsStreamForRead();

        //        var byteArray = new byte[readStream.Length];
        //        await readStream.ReadAsync(byteArray, 0, byteArray.Length);
        //        return byteArray;
        //    }
        //}

        //public static byte[] SavetoBytes(ImageSource imageSource)
        //{
        //    WriteableBitmap source = imageSource as WriteableBitmap;
        //    var byteArray = source.PixelBuffer;
        //    return byteArray.ToArray();
        //}

        //public static byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
        //{
        //    byte[] bytes = null;
        //    var bitmapSource = imageSource as BitmapSource;

        //    if (bitmapSource != null)
        //    {
        //        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

        //        using (var stream = new MemoryStream())
        //        {
        //            encoder.Save(stream);
        //            bytes = stream.ToArray();
        //        }
        //    }

        //    return bytes;
        //}
        //public static Byte[] ImageToByte(WriteableBitmap imageSource)
        //{
        //    Stream stream = imageSource.PixelBuffer.AsStream();
        //    Byte[] buffer = null;
        //    if (stream != null && stream.Length > 0)
        //    {
        //        using (BinaryReader br = new BinaryReader(stream))
        //        {
        //            buffer = br.ReadBytes((Int32)stream.Length);
        //        }
        //    }

        //    return buffer;
        //}

        public static async Task<ImageSource> SaveToImageSource(byte[] imageBuffer)
        {
            ImageSource imageSource = null;
            try
            {
                using (MemoryStream stream = new MemoryStream(imageBuffer))
                {
                    var ras = stream.AsRandomAccessStream();
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(BitmapDecoder.JpegDecoderId, ras);
                    var provider = await decoder.GetPixelDataAsync();
                    byte[] buffer = provider.DetachPixelData();
                    WriteableBitmap bitmap = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                    await bitmap.PixelBuffer.AsStream().WriteAsync(buffer, 0, buffer.Length);
                    imageSource = bitmap;
                }
                //using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                //{
                //    using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                //    {
                //        writer.WriteBytes(imageBuffer);
                //        await writer.StoreAsync();
                //    }
                //    BitmapImage image = new BitmapImage();
                //    await image.SetSourceAsync(stream);
                //    return image;
                //}
            }
            catch (Exception ex)
            {

            }
            return imageSource;
        }
        //public static async Task<ImageSource> SaveToImageSource(this byte[] imageBuffer)
        //{
        //    ImageSource imageSource = null;
        //    using (MemoryStream stream = new MemoryStream(imageBuffer))
        //    {

        //    }
        //    return imageSource;
        //}
        public static async Task<ImageSource> FromBase64(byte[] bytes)
        {
            try
            {
                //// read stream
                //var image = bytes.AsBuffer().AsStream().AsRandomAccessStream();

                //// decode image
                //var decoder = await BitmapDecoder.CreateAsync(image);
                //image.Seek(0);

                //// create bitmap
                //var output = new WriteableBitmap((int)decoder.PixelHeight, (int)decoder.PixelWidth);
                //await output.SetSourceAsync(image);
                //return output;


                //       var softwareBitmap = new SoftwareBitmap(
                //BitmapPixelFormat.Bgra8,
                //(int)bitmapDecoder.PixelWidth,
                //(int)bitmapDecoder.PixelHeight,
                //BitmapAlphaMode.Premultiplied);

                //       softwareBitmap.CopyFromBuffer(bits.AsBuffer());
                BitmapImage image = new BitmapImage();
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    var s = Convert.ToBase64String(bytes);
                    await stream.WriteAsync(bytes.AsBuffer());
                    stream.Seek(0);
                    await image.SetSourceAsync(stream);
                }
                return image;

            }
            catch (Exception ex)
            {

            }
            return new WriteableBitmap(0,0);
        }
    }

}
