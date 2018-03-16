using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PortoSchool.Pages
{
    public sealed partial class DashboardPage : Page
    {
        private static bool initializeUpdaterExecuted = false;
        private static readonly DispatcherTimer updatetimer=new DispatcherTimer { Interval=TimeSpan.FromSeconds(2)};

        public void InitializeUpdater()
        {
            if (initializeUpdaterExecuted) return;

            initializeUpdaterExecuted = true;
            UpdateButtonsAvailablity(false);
            updatetimer.Tick += Updatetimer_Tick;
            updatetimer.Start();
        }

        private void Updatetimer_Tick(object sender, object e)
        {
            updatetimer.Stop();
            CheckUpdateXlsFilesAsync();
            UpdateButtonsAvailablity(true);

        }

        private async Task UpdateNobOgrIfXLSExistsAsync()
        {
            if (!File.Exists(Path.Combine(App.WorkingPath, "OGRETMEN.xlsx")))
                return;


            ExcelReader a = ExcelReader.SharedReader();
            List<string> ÖğretmenAdıSoyadıList = new List<string>();
            List<string> NöbetGünüList = new List<string>();
            List<string> NöbetAlanList = new List<string>();

            StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath, "OGRETMEN.xlsx"));

            List<SheetData> x = await a.ParseSpreadSheetFile(f);

            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            conn.DropTable<Sentinels>();
            conn.CreateTable<Sentinels>();

           
            foreach (var xx in x)
            {
                var data = xx.data;

                foreach (var dd in data)
                {
                    foreach (var ddc in dd)
                    {
                        switch (ddc.Key)
                        {
                            case "ÖĞRETMEN ADI SOYADI": ÖğretmenAdıSoyadıList.Add(ddc.Value.ToUpper()); break;
                            case "NÖBET GÜNÜ": NöbetGünüList.Add(ddc.Value.ToUpper()); break;
                            case "NÖBET YERİ": NöbetAlanList.Add(ddc.Value.ToUpper()); break;
                        }
                        //listView1.Items.Add($"{ddc.Key},{ddc.Value}");
                    }
                }
            }

            for (int i = 0; i < ÖğretmenAdıSoyadıList.Count; i++)
            {
                Sentinels row = new Sentinels
                {
                    OgretmenAdiSoyadi = ÖğretmenAdıSoyadıList[i],
                   NobetGunu= NöbetGünüList[i],
                   NobetAlan= NöbetAlanList[i]
                };
                conn.Insert(row);
            }

            File.Delete(Path.Combine(App.WorkingPath, "OGRETMEN_OLD.xlsx"));
            File.Move(Path.Combine(App.WorkingPath, "OGRETMEN.xlsx"), Path.Combine(App.WorkingPath, "OGRETMEN_OLD.xlsx"));
        }

        private async Task UpdateNobAlanIfXLSExistsAsync()
        {
            if (!File.Exists(Path.Combine(App.WorkingPath, "NOBETALAN.xlsx")))
                return;


            ExcelReader a = ExcelReader.SharedReader();
           
            List<string> NöbetAlanList = new List<string>();

            StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath, "NOBETALAN.xlsx"));

            List<SheetData> x = await a.ParseSpreadSheetFile(f);

            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            conn.DropTable<NobetAlan>();
            conn.CreateTable<NobetAlan>();

            foreach (var xx in x)
            {
                var data = xx.data;

                foreach (var dd in data)
                {
                    foreach (var ddc in dd)
                    {
                        switch (ddc.Key)
                        {
                            case "NÖBET YERİ": NöbetAlanList.Add(ddc.Value.ToUpper()); break;
                        }
                        //listView1.Items.Add($"{ddc.Key},{ddc.Value}");
                    }
                }
            }

            for (int i = 0; i < NöbetAlanList.Count; i++)
            {
                NobetAlan row = new NobetAlan
                {
                    NobetYeri = NöbetAlanList[i],
                };
                conn.Insert(row);
            }

            File.Delete(Path.Combine(App.WorkingPath, "NOBETALAN_OLD.xlsx"));
            File.Move(Path.Combine(App.WorkingPath, "NOBETALAN.xlsx"), Path.Combine(App.WorkingPath, "NOBETALAN_OLD.xlsx"));

        }
        private async void CheckUpdateXlsFilesAsync()
        {
            await UpdateNobAlanIfXLSExistsAsync();
            await UpdateNobOgrIfXLSExistsAsync();
            
        }

        private void UpdateButtonsAvailablity(bool avail)
        {
            this.buttonPresentationPage.IsEnabled = avail;
            this.buttonSentinelsPage.IsEnabled = avail;
            this.buttonPresentationPage.IsEnabled = avail;
            this.buttonSettingsPage.IsEnabled = avail;
            this.buttonCloseApp.IsEnabled = avail;
            this.buttonSentryLocationsPage.IsEnabled = avail;
            this.buttonDirectorAssistant.IsEnabled = avail;
            this.buttonBulletinPage.IsEnabled = avail;
        }
    }
}
