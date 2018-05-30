using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SingPhotoUWP.Model
{
    class JsonSerializerClass
    {
        /// <summary>
        /// 将Json字符串实例化为类
        /// </summary>
        /// <typeparam name="T">需要实例化的对象类型</typeparam>
        /// <param name="resText">传入Json字符串</param>
        /// <returns>返回Json的实例化类的对象</returns>
        public static T DataContractJasonSerializer<T>(string resText)
        {
            var ds = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(resText));
            T result = (T)ds.ReadObject(ms);
            ms.Dispose();
            return result;
        }

    }
}
