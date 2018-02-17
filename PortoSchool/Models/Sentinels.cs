using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortoSchool.Models
{
    [Flags] public enum DayEnum { PAZARTESİ = 0, SALI = 1, ÇARŞAMBA = 2, PERŞEMBE = 3, CUMA = 4 };


    public class Sentinels
    {

        [PrimaryKey, AutoIncrement]
        public int id { get; set; }

        public string OgretmenAdiSoyadi { get; set; }

        [Indexed]
        public string NobetAlan { get; set; }

        public string NobetGunu { get; set; }

        public DayEnum DayNO { get; set; }

    }

    public static class SentinelsManager
    {
        private static readonly ObservableCollection<Sentinels> NobOgrList = new ObservableCollection<Sentinels>();
      
        public static ObservableCollection<Sentinels> getNobOgrList(String filter=null)
        {
            string path = Settings.FullPathSQLite;
            //\\192.168.18.182\C$\Data\Users\DefaultAccount\AppData\Local\Packages\06fb6d66-31b3-4beb-893c-2e0d9fe465f1_3asabdzxmrwg6\LocalState\Project1\settings.sqlite
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            NobOgrList.Clear();

            if (filter == null)
            {
                //List<Sentinels> rows = conn.Table<Sentinels>().ToList();
                //var rows = conn.Query<Sentinels>("select *,(case NobetGunu WHEN 'PAZARTESİ' then 0 WHEN 'SALI' then 1 WHEN 'ÇARŞAMBA' then 2 WHEN 'PERŞEMBE' then 3 WHEN 'CUMA' then 4 END) as workdayno from Sentinels order by workdayno").ToList();

                var Sentinels = conn.Query<Sentinels>("select * from Sentinels");

                var rows = (from e in Sentinels
                            select new Sentinels()
                            {
                                id = e.id,
                                OgretmenAdiSoyadi = e.OgretmenAdiSoyadi,
                                NobetAlan = e.NobetAlan,
                                NobetGunu = e.NobetGunu,
                                 DayNO = (DayEnum)Enum.Parse(typeof(DayEnum), e.NobetGunu)
                            }).OrderBy(e => e.DayNO);

                // rows.ForEach(x => { NobOgrList.Add(x); });

                foreach (var x in rows)
                {
                    NobOgrList.Add(x);
                }
            }

            else
            {
                string flt = $"%{filter}%";
                var Sentinels = conn.Query<Sentinels>("select * from Sentinels where NOBETALAN LIKE ? OR OGRETMENADISOYADI LIKE ? OR  NOBETGUNU LIKE ?", flt, flt, flt).ToList();
                var rows = (from e in Sentinels
                            select new Sentinels()
                            {
                                id = e.id,
                                OgretmenAdiSoyadi = e.OgretmenAdiSoyadi,
                                NobetAlan = e.NobetAlan,
                                NobetGunu = e.NobetGunu,
                                DayNO = (DayEnum)Enum.Parse(typeof(DayEnum), e.NobetGunu)
                            }).OrderBy(e => e.DayNO);

                // rows.ForEach(x => { NobOgrList.Add(x); });

                foreach (var x in rows)
                {
                    NobOgrList.Add(x);
                }
            }
            //var rows = conn.Query<NobetAlan>("select * from NobetAlan where Id=?", x.id).FirstOrDefault();

            return NobOgrList;
        }

        

    }
}
