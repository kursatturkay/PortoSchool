using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortoSchool.Models
{
    public class Settings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }

        public static string getValueByKey(string key_, string defaultvalue_)
        {
            string res = defaultvalue_;

            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FullPathSQLite);

            var row = conn.Table<Settings>().Where(x => x.Key == key_).FirstOrDefault();

            if (row == null)
            {
                Settings row_ = new Settings { Key = key_, Value = defaultvalue_, DefaultValue = defaultvalue_ };
                conn.Insert(row_);
            }
            else
            {
                res = row.Value;
            }

            //res = (q == null) ? res : q.Value;
            return res;

        }

        public static bool setValueByKey(string key_, string value_)
        {
            bool res = false;
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FullPathSQLite);

            try
            {
                var row = conn.Query<Settings>("select * from Settings where Key=?", key_).FirstOrDefault();

                if (row != null)
                {
                    row.Value = value_;
                    conn.RunInTransaction(() =>
                    {
                        conn.Update(row);
                    });

                    res = true;
                }
                else
                {
                    Settings row_ = new Settings { Key = key_, Value = value_, DefaultValue = value_ };

                    conn.Insert(row_);
                    res = true;
                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            return res;
        }

        public static string FullAppPathLocalFolder
        {
            get
            {
                var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path);
                return path;
            }
        }

        public static string FullPathSQLite
        {
            get
            {
                //var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,"PortoSchool" , "settings.sqlite");
                var path = Path.Combine(App.WorkingPath, "settings.sqlite");

                return path;
            }
        }

        public static string LocalDataFolder
        {
            get
            {
                var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "PortoSchool");
                return path;
            }
        }

        //\\192.168.18.182\c$\Data\Users\DefaultAccount\AppData\Local
        public static string NetworkDataFolder(string hostip)
        {
            var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "PortoSchool");

            string[] parts = path.Split(new string[] { @"\AppData\Local\" }, StringSplitOptions.RemoveEmptyEntries);
            path = $@"\\{hostip}\C$\Data\Users\DefaultAccount\AppData\Local\{parts[1]}";
            return path;
        }
    }


}
