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
    //class AssistantDirectorSentinelDay

    public class AssistantDirectorSentinelDayDataset
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string FullName { get; set; }
        public DateTime SentinelDate { get; set; }
    }
    public class AssistantDirectorSentinelDay
    {
        public SentinelAssistantDirector sentinelAssistantDirector { get; set; }
        public DateTime SentinelDate { get; set; }

        public bool IsUnwantedDay
        {
            get
            {
                if (sentinelAssistantDirector == null) return false;

                bool sonuç = false;
                string strdate = SentinelDate.ToString("dddd").ToUpper();

                bool a1 = ((sentinelAssistantDirector.UnwantedDays[0] == true) && ((strdate == "PAZARTESİ") || (strdate == "MONDAY")));
                bool a2 = ((sentinelAssistantDirector.UnwantedDays[1] == true) && ((strdate == "SALI") || (strdate == "TUESDAY")));
                bool a3 = ((sentinelAssistantDirector.UnwantedDays[2] == true) && ((strdate == "ÇARŞAMBA") || (strdate == "WEDNESDAY")));
                bool a4 = ((sentinelAssistantDirector.UnwantedDays[3] == true) && ((strdate == "PERŞEMBE") || (strdate == "TUESDAY")));
                bool a5 = ((sentinelAssistantDirector.UnwantedDays[4] == true) && ((strdate == "CUMA") || (strdate == "FRIDAY")));

                if (a1 || a2 || a3 || a4 || a5)
                    sonuç = true;
                return sonuç;
            }
        }
    }
    public static class AssistantDirectorSentinelDayManager
    {
        private static readonly ObservableCollection<AssistantDirectorSentinelDay> AssistantDirectorSentinelDays_ = new ObservableCollection<AssistantDirectorSentinelDay>();

        public static void Clear()
        {
            AssistantDirectorSentinelDays_.Clear();
        }

        public static void Add(AssistantDirectorSentinelDay AssistantDirectorSentinelDay)
        {
            AssistantDirectorSentinelDays_.Add(AssistantDirectorSentinelDay);
        }
        public static ObservableCollection<AssistantDirectorSentinelDay> MdYrdNöbetGünleri
        {
            get
            {
                if (AssistantDirectorSentinelDays_.Count == 0)
                {
                    var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath, false);
                    //listViewNobMudYrdCalendar.Items.Clear();
                    try
                    {
                        var AssistantDirectorSentinelDayDataset_ = conn.Table<AssistantDirectorSentinelDayDataset>().ToList();

                        //AssistantDirectorSentinelDays_.Clear();
                        foreach (var x in AssistantDirectorSentinelDayDataset_)
                        {
                            AssistantDirectorSentinelDay AssistantDirectorSentinelDay = new AssistantDirectorSentinelDay();
                            AssistantDirectorSentinelDay.SentinelDate = x.SentinelDate;
                            AssistantDirectorSentinelDay.sentinelAssistantDirector = SentinelAssistantDirectorManager.AdaGöreBul(x.FullName);
                            AssistantDirectorSentinelDays_.Add(AssistantDirectorSentinelDay);
                        }
                    }
                    catch { }

                }
                return AssistantDirectorSentinelDays_;
            }
        }

        public static int collisionCount
        {
            get
            {
                int sonuç = 0;

                foreach (AssistantDirectorSentinelDay x in MdYrdNöbetGünleri)
                {
                    if (x.IsUnwantedDay)
                        sonuç++;
                }

                return sonuç;
            }
        }

    }

}

