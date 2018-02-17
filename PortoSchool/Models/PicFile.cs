using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace PortoSchool.Models
{
    public class PicFile
    {
        public Uri Img { get; set; }
    }

    public static class PicFileManager
    {
        private static readonly ObservableCollection<PicFile> PicFiles = new ObservableCollection<PicFile>();

        public static int PicFileCount
        {
            get
            {
                return PicFiles.Count;

            }
        }

        public static ObservableCollection<PicFile> GetPicFilesFromStorePath()
        {

            string path = App.WorkingPath; //Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "PortoSchool");

            try
            {
                var a = Directory.GetFiles(path, "*.*").Where(x => x.EndsWith(".jpg"));

                PicFiles.Clear();
                foreach (var x in a)
                {
                    Uri uri_ = new Uri(
                       Path.Combine("ms-appdata:///local/PortoSchool", Path.GetFileName(x)));

                    PicFiles.Add(new PicFile { Img = uri_ });
                }
            }
            catch (Exception ex)
            {
                App.ErrorLog.Add(ex.Message);
            }
            return PicFiles;
        }
    }
}
