using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingPhotoUWP.Model
{
    public class SongsURLs
    {
        public string 男子汉 { get; set; }
        public string 萌妹子 { get; set; }
    }

    public class ImageCVAnalysisResult
    {
        public IList<object> Faces { get; set; }
        public string Relation { get; set; }
        public bool Adult { get; set; }
        public bool IsRacy { get; set; }
        public string DominantColor { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public IList<object> Emotions { get; set; }
        public IList<string> ImageThemes { get; set; }
        public IList<string> ImageTags { get; set; }
        public string AnalysisTime { get; set; }
        public object AnalysisStatus { get; set; }
        public object AnalysisLog { get; set; }
    }

    public class SingPhotoResult
    {
        public bool IsSucceed { get; set; }
        public bool IsAdult { get; set; }
        public bool IsRacy { get; set; }
        public string Singer { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public SongsURLs SongsURLs { get; set; }
        public IList<string> ImageTags { get; set; }
        public IList<string> Lyrics { get; set; }
        public ImageCVAnalysisResult Image_CV_Analysis_Result { get; set; }
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public SingPhotoResult()
        {

        }
    }

    public class SingPhotoResultHelper
    {
        public static SingPhotoResult GetSingPhotoResultFromJsonText(string jsontext)
        {
            SingPhotoResult result = JsonSerializerClass.DataContractJasonSerializer<SingPhotoResult>(jsontext);
            return result;
        }
    }
}
