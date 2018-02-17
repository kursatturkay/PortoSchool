using PortoSchool.Libs;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortoSchool.Models
{
    public class BulletinFile
    {
        public string FileNameOnly { get; set; }

    }
    public static class BulletinFileManager
    {

        public static readonly ObservableCollection<BulletinFile> BulletinFiles = new
            ObservableCollection<BulletinFile>();

        public static int BulletinFileCount
        {
            get
            {
                return BulletinFiles.Count;
            }
        }


        public static ObservableCollection<BulletinFile> GetBulletinFilesFromStorePath()
        {

            string path = App.WorkingPath; //Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "PortoSchool");

            try
            {
                var a = Directory.GetFiles(path, "*.*").Where(x => x.EndsWith(".rtf"));

                BulletinFiles.Clear();
                foreach (var x in a)
                {
                    //Uri uri_ = new Uri(
                    var fno = Path.Combine("ms-appdata:///local/PortoSchool", Path.GetFileName(x));

                    BulletinFiles.Add(new BulletinFile { FileNameOnly = fno });
                }
            }
            catch (Exception ex)
            {
                App.ErrorLog.Add(ex.Message);
            }
            return BulletinFiles;
        }
    }
    /*
    public class Bulletin
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string BulletinTitle { get; set; }
        public string Content { get; set; }
    }

    public static class BullettinManager
    {
        private static readonly ObservableCollection<Bulletin> Bulletins = new ObservableCollection<Bulletin>();


        public static ObservableCollection<Bulletin> GetBulletins()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);
            Bulletins.Clear();
            List<Bulletin> BullettinList = conn.Table<Bulletin>().ToList();
            BullettinList.ForEach(x => { Bulletins.Add(x); });
            return Bulletins;
        }
    }
    */


}
