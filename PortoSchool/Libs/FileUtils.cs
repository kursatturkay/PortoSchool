using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PortoSchool.Libs
{
    public static class FileUtils
    {
        public static string FullDataPath = String.Empty;
        static public async Task<string> CreateDefaultFolder()
        {
            StorageFolder newfolder;
            if (FullDataPath == string.Empty)
            {
                newfolder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("PortoSchool", CreationCollisionOption.OpenIfExists);
                FullDataPath = Path.Combine(newfolder.Path, "settings.sqlite");
            }
            
            return FullDataPath;
        }
    }
}
