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
    //class DirectorAssistantSentinelDay

    public class DirectorAssistantSentinelDayDataset
    {
        [AutoIncrement, PrimaryKey]
        public int id { get; set; }
        public string FullName { get; set; }
        public DateTime SentinelDate { get; set; }
    }
    public class DirectorAssistantSentinelDay
    {
        public SentinelDirectorAssistant sentinelDirectorAssistant { get; set; }
        public DateTime SentinelDate { get; set; }

        public bool IsUnwantedDay
        {
            get
            {
                if (sentinelDirectorAssistant == null) return false;

                bool sonuç = false;
                string strdate = SentinelDate.ToString("dddd").ToUpper();

                bool a1 = ((sentinelDirectorAssistant.UnwantedDays[0] == true) && ((strdate == "PAZARTESİ") || (strdate == "MONDAY")));
                bool a2 = ((sentinelDirectorAssistant.UnwantedDays[1] == true) && ((strdate == "SALI") || (strdate == "TUESDAY")));
                bool a3 = ((sentinelDirectorAssistant.UnwantedDays[2] == true) && ((strdate == "ÇARŞAMBA") || (strdate == "WEDNESDAY")));
                bool a4 = ((sentinelDirectorAssistant.UnwantedDays[3] == true) && ((strdate == "PERŞEMBE") || (strdate == "TUESDAY")));
                bool a5 = ((sentinelDirectorAssistant.UnwantedDays[4] == true) && ((strdate == "CUMA") || (strdate == "FRIDAY")));

                if (a1 || a2 || a3 || a4 || a5)
                    sonuç = true;
                return sonuç;
            }
        }
    }
    public static class DirectorAssistantSentinelDayManager
    {
        private static readonly ObservableCollection<DirectorAssistantSentinelDay> DirectorAssistantSentinelDays_ = new ObservableCollection<DirectorAssistantSentinelDay>();

        public static void Clear()
        {
            DirectorAssistantSentinelDays_.Clear();
        }

        public static void Add(DirectorAssistantSentinelDay DirectorAssistantSentinelDay)
        {
            DirectorAssistantSentinelDays_.Add(DirectorAssistantSentinelDay);
        }
        public static ObservableCollection<DirectorAssistantSentinelDay> MdYrdNöbetGünleri
        {
            get
            {
                if (DirectorAssistantSentinelDays_.Count == 0)
                {
                    var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath, false);
                    //listViewNobMudYrdCalendar.Items.Clear();
                    try
                    {
                        var DirectorAssistantSentinelDayDataset_ = conn.Table<DirectorAssistantSentinelDayDataset>().ToList();

                        //DirectorAssistantSentinelDays_.Clear();
                        foreach (var x in DirectorAssistantSentinelDayDataset_)
                        {
                            DirectorAssistantSentinelDay DirectorAssistantSentinelDay = new DirectorAssistantSentinelDay();
                            DirectorAssistantSentinelDay.SentinelDate = x.SentinelDate;
                            DirectorAssistantSentinelDay.sentinelDirectorAssistant = SentinelDirectorAssistantManager.AdaGöreBul(x.FullName);
                            DirectorAssistantSentinelDays_.Add(DirectorAssistantSentinelDay);
                        }
                    }
                    catch { }

                }
                return DirectorAssistantSentinelDays_;
            }
        }

        public static int collisionCount
        {
            get
            {
                int sonuç = 0;

                foreach (DirectorAssistantSentinelDay x in MdYrdNöbetGünleri)
                {
                    if (x.IsUnwantedDay)
                        sonuç++;
                }

                return sonuç;
            }
        }

    }

}

