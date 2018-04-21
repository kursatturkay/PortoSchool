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
        public static string SharedDirectory = String.Empty;
        static public async Task<string> CreateDefaultFolder()
        {
            StorageFolder newfolder;
            if (FullDataPath == string.Empty)
            {
                newfolder = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("PortoSchool", CreationCollisionOption.OpenIfExists);
                FullDataPath = Path.Combine(newfolder.Path, "settings.sqlite");
                SharedDirectory = newfolder.Path;
            }

            return FullDataPath;
        }

        // The following code snippet shows how to asynchronously check if a file or folder exists in the specific folder. 
        // Usage: 
        //if(await IfStorageItemExist(folder, itemName)) 
        //{ 
        //    // file/folder exists. 
        //} 
        //else 
        //{ 
        //    // file/folder does not exist. 
        //} 
        public static async Task<bool> IfStorageItemExist(StorageFolder folder, string itemName)
        {
            try
            {
                IStorageItem item = await folder.TryGetItemAsync(itemName);
                return (item != null);
            }
            catch (Exception ex)
            {
                // Should never get here 
                return false;
            }
        }
        
    }
}
