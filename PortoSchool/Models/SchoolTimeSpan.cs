using PortoSchool.Libs;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace PortoSchool.Models
{
    public class SchoolTimeSpan
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Title { get; set; }
        public string Day { get; set; }
        public TimeSpan TimeStart { get; set; }
        public TimeSpan TimeEnd { get; set; }
    }

    public static class SchoolTimeSpanManager
    {
        private static readonly ObservableCollection<SchoolTimeSpan> schoolTimeSpans = new ObservableCollection<SchoolTimeSpan>();

        public static IEnumerable<SchoolTimeSpan> schoolTimeSpansByWeekDay(string day)
        {
            var ret = schoolTimeSpans.Where(x => x.Day == day);
            return ret;
        }

        public static bool isSchoolTimeSpansEmpty()
        {
            return (schoolTimeSpans.Count == 0)?true:false;
        }

        public struct CourseTableRowIdxAndRemainingTime
        {
            public int idx;
            public TimeSpan RemainingTime;
        }

        public static CourseTableRowIdxAndRemainingTime IndexByTime(TimeSpan ts, IEnumerable<SchoolTimeSpan> tsToday)
        {
            CourseTableRowIdxAndRemainingTime res;
            res.idx = -1;
            res.RemainingTime = new TimeSpan(0,0,0);

            foreach (var x in tsToday)
            {
                res.idx++;
                bool a = (ts > x.TimeStart);
                bool b = (ts < x.TimeEnd);

                if (a && b)
                {
                    res.RemainingTime = x.TimeEnd - ts;
                    return res;
                }
            }
            return res;
        }
        public static async void ImportDataFromCouseTableXlsx()
        {
            var COURSETABLE_xlsx = LocalizationUtils.ResourceValueByKey("COURSETABLE/xlsx");

            schoolTimeSpans.Clear();
            if (!File.Exists(Path.Combine(App.WorkingPath, COURSETABLE_xlsx)))
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/" + COURSETABLE_xlsx));
                StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Settings.LocalDataFolder);
                await file.CopyAsync(storageFolder, COURSETABLE_xlsx);
            }

            ExcelReader a = ExcelReader.SharedReader();
            StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath, COURSETABLE_xlsx));

            List<SheetData> sheetdatas = await a.ParseSpreadSheetFile(f);

            int hours, mins;
            int mod4 = 0;
            SchoolTimeSpan sts = new SchoolTimeSpan(); ;

            var xx = sheetdatas[0];

            var data = xx.data;
            var datacount = data.Count;

            //foreach (var dd in data)
            for (int j = 0; j < datacount; j = j + 1)
            {
                var dd = data[j];
                int ddcount = dd.Count;

                for (int i = 0; i < ddcount; i = i + 1)
                {
                    KeyValuePair<string, string> ddc = dd.ElementAt(i);

                    var splt = ddc.Key.Split('-');

                    if (splt.Length == 2)
                    {
                        if (splt[1] == "TITLE")
                        {
                            sts.Day = splt[0];
                            sts.Title = ddc.Value;
                        }

                        if ((splt[1] == "T1") && (!string.IsNullOrEmpty(ddc.Value)))
                        {
                            hours = int.Parse(ddc.Value.Split(":")[0]);
                            mins = int.Parse(ddc.Value.Split(":")[1]);
                            sts.TimeStart = new TimeSpan(hours, mins, 0);
                        }

                        if ((splt[1] == "T2") && (!string.IsNullOrEmpty(ddc.Value)))
                        {
                            hours = int.Parse(ddc.Value.Split(":")[0]);
                            mins = int.Parse(ddc.Value.Split(":")[1]);
                            sts.TimeEnd = new TimeSpan(hours, mins, 0);
                        }
                    }//end of if..

                    //listView1.Items.Add($"{ddc.Key},{ddc.Value}");
                    if ((mod4 == 3) && (!string.IsNullOrEmpty(ddc.Value)))
                    {
                        schoolTimeSpans.Add(sts);
                        sts = new SchoolTimeSpan();
                    }

                    mod4 = ((mod4 + 1) % 4);
                }//end of for..
            }

            //File.Delete(Path.Combine(App.WorkingPath, "COURSETABLE_OLD.xlsx"));
            //File.Move(Path.Combine(App.WorkingPath, "COURSETABLE.xlsx"), Path.Combine(App.WorkingPath, "COURSETABLE_OLD.xlsx"));
        }
    }
}
