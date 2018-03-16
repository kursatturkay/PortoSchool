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
    public class SentinelDirectorAssistantDataset
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string FullName { get; set; }
        public bool NotMonday { get; set; }
        public bool NotTuesday { get; set; }
        public bool NotWednesday { get; set; }
        public bool NotThursday { get; set; }
        public bool NotFriday { get; set; }
    }

    public class SentinelDirectorAssistant
    {
        public string FullName { get; set; }
        public List<bool> UnwantedDays = new List<bool> { false, false, false, false, false };
    }
    public class SentinelDirectorAssistantManager
    {
        private readonly static ObservableCollection<SentinelDirectorAssistant> SentinelDirectorAssistantList_ = new ObservableCollection<SentinelDirectorAssistant>();

        public static void Clear()
        {
            SentinelDirectorAssistantList_.Clear();
        }

        public static ObservableCollection<SentinelDirectorAssistant> SentinelDirectorAssistantList
        {
            get
            {
                if (SentinelDirectorAssistantList_.Count == 0)
                {
                    var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

                    //listViewNobMudYrd.Items.Clear();

                    var SentinelDirectorAssistantDataset_ = conn.Table<SentinelDirectorAssistantDataset>().ToList();

                    SentinelDirectorAssistantList_.Clear();
                    foreach (var x in SentinelDirectorAssistantDataset_)
                    {
                        SentinelDirectorAssistant mdyrd = new SentinelDirectorAssistant { FullName = x.FullName };

                        mdyrd.UnwantedDays[0] = x.NotMonday;
                        mdyrd.UnwantedDays[1] = x.NotTuesday;
                        mdyrd.UnwantedDays[2] = x.NotWednesday;
                        mdyrd.UnwantedDays[3] = x.NotThursday;
                        mdyrd.UnwantedDays[4] = x.NotFriday;

                        SentinelDirectorAssistantList_.Add(mdyrd);
                    }
                }
                return SentinelDirectorAssistantList_;
            }
        }



        public int TotalSentinelDays(SentinelDirectorAssistant SentinelDirectorAssistant)
        {
            int sonuç = 0;

            foreach (DirectorAssistantSentinelDay x in DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri)
            {
                if (x.sentinelDirectorAssistant.FullName == SentinelDirectorAssistant.FullName)
                    sonuç++;
            }
            return sonuç;
        }

        //class method
        public static SentinelDirectorAssistant AdaGöreBul(string pAdSoyad)
        {
            SentinelDirectorAssistant sonuç = null;

            foreach (SentinelDirectorAssistant x in SentinelDirectorAssistantManager.SentinelDirectorAssistantList)
            {
                if (x.FullName == pAdSoyad.Trim())
                {
                    sonuç = x;
                    break;
                }
            }
            return sonuç;
        }
    }
}
