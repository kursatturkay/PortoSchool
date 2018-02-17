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
    public class NobetAlan
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string NobetYeri { get; set; }
    }

    public static class NobetAlanManager
    {
        private static readonly ObservableCollection<NobetAlan> NobetAlanlari = new ObservableCollection<NobetAlan>();
        private static readonly ObservableCollection<Sentinels> nobetAlanVeNobOgrList = new ObservableCollection<Sentinels>();

       /* public static ObservableCollection<NobetAlanVeNobOgr> GetNobetAlanVeNobOgr()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            // var row = conn.Query<Sentinels>("select * from Sentinels where Id=?", x.id).FirstOrDefault();
            conn.Query<NobetAlanVeNobOgr>("" )
        }*/
        public static ObservableCollection<NobetAlan> GetNobetAlanlari()
        {
            //var dir_defaultdoc = FileUtils.CreateDefaultFolder();
            //var path = Path.Combine(dir_defaultdoc.Result, "settings.sqlite");
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath );


            NobetAlanlari.Clear();
            List<NobetAlan> NobetAlanlariList = conn.Table<NobetAlan>().ToList();
            NobetAlanlariList.ForEach(x=> { NobetAlanlari.Add(x); });
            return NobetAlanlari;
        }
    }
}
