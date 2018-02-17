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
    public class SentinelAssistantDirectorDataset
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

    public class SentinelAssistantDirector
    {
        public string FullName { get; set; }
        public List<bool> UnwantedDays = new List<bool> { false, false, false, false, false };
    }
    public class SentinelAssistantDirectorManager
    {
        private readonly static ObservableCollection<SentinelAssistantDirector> SentinelAssistantDirectorList_ = new ObservableCollection<SentinelAssistantDirector>();

        public static void Clear()
        {
            SentinelAssistantDirectorList_.Clear();
        }

        public static ObservableCollection<SentinelAssistantDirector> SentinelAssistantDirectorList
        {
            get
            {
                if (SentinelAssistantDirectorList_.Count == 0)
                {
                    var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

                    //listViewNobMudYrd.Items.Clear();

                    var SentinelAssistantDirectorDataset_ = conn.Table<SentinelAssistantDirectorDataset>().ToList();

                    SentinelAssistantDirectorList_.Clear();
                    foreach (var x in SentinelAssistantDirectorDataset_)
                    {
                        SentinelAssistantDirector mdyrd = new SentinelAssistantDirector { FullName = x.FullName };

                        mdyrd.UnwantedDays[0] = x.NotMonday;
                        mdyrd.UnwantedDays[1] = x.NotTuesday;
                        mdyrd.UnwantedDays[2] = x.NotWednesday;
                        mdyrd.UnwantedDays[3] = x.NotThursday;
                        mdyrd.UnwantedDays[4] = x.NotFriday;

                        SentinelAssistantDirectorList_.Add(mdyrd);
                    }
                }
                return SentinelAssistantDirectorList_;
            }
        }



        public int TotalSentinelDays(SentinelAssistantDirector SentinelAssistantDirector)
        {
            int sonuç = 0;

            foreach (AssistantDirectorSentinelDay x in AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri)
            {
                if (x.sentinelAssistantDirector.FullName == SentinelAssistantDirector.FullName)
                    sonuç++;
            }
            return sonuç;
        }

        //class method
        public static SentinelAssistantDirector AdaGöreBul(string pAdSoyad)
        {
            SentinelAssistantDirector sonuç = null;

            foreach (SentinelAssistantDirector x in SentinelAssistantDirectorManager.SentinelAssistantDirectorList)
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
