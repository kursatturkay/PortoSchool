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
    public class SentryLocationDataset
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string SentryLocation { get; set; }
    }

    public static class NobetAlanManager
    {
        private static readonly ObservableCollection<SentryLocationDataset> NobetAlanlari = new ObservableCollection<SentryLocationDataset>();
        private static readonly ObservableCollection<SentinelsDataset> nobetAlanVeNobOgrList = new ObservableCollection<SentinelsDataset>();

        public static ObservableCollection<SentryLocationDataset> GetNobetAlanlari()
        {
            //var dir_defaultdoc = FileUtils.CreateDefaultFolder();
            //var path = Path.Combine(dir_defaultdoc.Result, "settings.sqlite");
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath );


            NobetAlanlari.Clear();
            List<SentryLocationDataset> NobetAlanlariList = conn.Table<SentryLocationDataset>().ToList();
            NobetAlanlariList.ForEach(x=> { NobetAlanlari.Add(x); });
            return NobetAlanlari;
        }
    }
}
