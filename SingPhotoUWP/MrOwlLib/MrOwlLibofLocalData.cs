using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingPhotoUWP.MrOwlLib
{
    public class MrOwlLibofLocalData
    {
        private static MrOwlLibofLocalData _localdata = new MrOwlLibofLocalData();

        private Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        private Windows.Storage.ApplicationDataContainer container;

        //public MrOwlLibofLocalData()
        //{
        //    this.localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        //}
        
        public static void CreateContainer(string containerkey)
        {
            if (!_localdata.localSettings.Containers.ContainsKey(containerkey))
            {
                _localdata.container = _localdata.localSettings.CreateContainer(containerkey, Windows.Storage.ApplicationDataCreateDisposition.Always);
            }
        }

        public static bool SaveDatatoContainer(object data, string containerkey, string valuekey)
        {
            if (_localdata.localSettings.Containers.ContainsKey(containerkey))
            {
                _localdata.localSettings.Containers[containerkey].Values[valuekey] = data;
                return true;
            }
            return false;
        }

        public static bool ReadDatafromContainer(ref object data,string containerkey, string valuekey)
        {
            bool hasContainer = _localdata.localSettings.Containers.ContainsKey(containerkey);
            bool hasSetting = false;
            if (hasContainer)
            {
                hasSetting = _localdata.localSettings.Containers[containerkey].Values.ContainsKey(valuekey);
                data = _localdata.localSettings.Containers[containerkey].Values[valuekey];
                return hasSetting;
            }
            return hasContainer;
        }
    }
}
