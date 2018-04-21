using PortoSchool.Libs;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortoSchool.Models
{
    public class HolidaysDataSet
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public DateTime date { get; set; }
    }

    public static class HolidayManager
    {
        public static readonly ObservableCollection<HolidaysDataSet> Holidays = new ObservableCollection<HolidaysDataSet>();

        //public static ObservableCollection<HolidayManager> GetHolidays()
        //{
        //    throw new Exception();
        //}

        public static ObservableCollection<HolidaysDataSet> GetHolidays()
        {
            //var dir_defaultdoc = FileUtils.CreateDefaultFolder();
            //var path = Path.Combine(dir_defaultdoc.Result, "settings.sqlite");
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);


            Holidays.Clear();
            List<HolidaysDataSet> HolidayList = conn.Table<HolidaysDataSet>().ToList();
            HolidayList.ForEach(x => { Holidays.Add(x); });
            return Holidays;
        }
    }


}
