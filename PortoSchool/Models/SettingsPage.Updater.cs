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
        private static readonly DispatcherTimer updatetimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };

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

            string res_SentinelsFN = LocalizationUtils.ResourceValueByKey("SENTINELS_xlsx");//determine whether file is SENTINELS.xlsx or NOBETCILER.xlsx etc.
            if (!File.Exists(Path.Combine(App.WorkingPath, $"{res_SentinelsFN}.xlsx")))
                return;

            string res_SentinelFullNameTitle = LocalizationUtils.ResourceValueByKey("SENTINELS_xlsx_SENTINELFULLNAME_TITLE");
            string res_SentryDateTitle = LocalizationUtils.ResourceValueByKey("SENTINELS_xlsx_SENTRYDATE_TITLE");
            string res_SentryLocationTitle = LocalizationUtils.ResourceValueByKey("SENTINELS_xlsx_SENTRYLOCATION_TITLE");

            ExcelReader a = ExcelReader.SharedReader();
            List<string> ÖğretmenAdıSoyadıList = new List<string>();
            List<string> NöbetGünüList = new List<string>();
            List<string> NöbetAlanList = new List<string>();

            StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath, $"{res_SentinelsFN}.xlsx"));

            List<SheetData> x = await a.ParseSpreadSheetFile(f);

            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            conn.DropTable<SentinelsDataset>();
            conn.CreateTable<SentinelsDataset>();

            foreach (var xx in x)
            {
                var data = xx.data;

                foreach (var dd in data)
                {
                    foreach (var ddc in dd)
                    {

                        if (ddc.Key == res_SentinelFullNameTitle) ÖğretmenAdıSoyadıList.Add(ddc.Value.ToUpper());
                        else
                        if (ddc.Key == res_SentryDateTitle) NöbetGünüList.Add(ddc.Value.ToUpper());
                        else
                        if (ddc.Key == res_SentryLocationTitle) NöbetAlanList.Add(ddc.Value.ToUpper());

                    }
                }
            }

            for (int i = 0; i < ÖğretmenAdıSoyadıList.Count; i++)
            {
                SentinelsDataset row = new SentinelsDataset
                {
                    SentinelFullName = ÖğretmenAdıSoyadıList[i],
                    SentryDate = NöbetGünüList[i],
                    SentryLocation = NöbetAlanList[i]
                };
                conn.Insert(row);
            }

            File.Delete(Path.Combine(App.WorkingPath, $"{res_SentinelsFN}_OLD.xlsx"));
            File.Move(Path.Combine(App.WorkingPath, $"{res_SentinelsFN}.xlsx"), Path.Combine(App.WorkingPath, $"{res_SentinelsFN}_OLD.xlsx"));
        }

        private async Task UpdateNobAlanIfXLSExistsAsync()
        {
            var res_SentryLocationFN= LocalizationUtils.ResourceValueByKey("SENTRYLOCATION_xlsx");
            var res_SENTRYLOCATION_xlsx_SENTRYLOCATION_TITLE=LocalizationUtils.ResourceValueByKey("SENTRYLOCATION_xlsx_SENTRYLOCATION_TITLE");
            if (!File.Exists(Path.Combine(App.WorkingPath, $"{res_SentryLocationFN}.xlsx")))
                return;


            ExcelReader a = ExcelReader.SharedReader();

            List<string> NöbetAlanList = new List<string>();

            StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath, $"{res_SentryLocationFN}.xlsx"));

            List<SheetData> x = await a.ParseSpreadSheetFile(f);

            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            conn.DropTable<SentryLocationDataset>();
            conn.CreateTable<SentryLocationDataset>();

            foreach (var xx in x)
            {
                var data = xx.data;

                foreach (var dd in data)
                {
                    foreach (var ddc in dd)
                    {
                        if(ddc.Key== res_SENTRYLOCATION_xlsx_SENTRYLOCATION_TITLE)
                            NöbetAlanList.Add(ddc.Value.ToUpper());
                    }
                }
            }

            for (int i = 0; i < NöbetAlanList.Count; i++)
            {
                SentryLocationDataset row = new SentryLocationDataset
                {
                    SentryLocation = NöbetAlanList[i],
                };
                conn.Insert(row);
            }

            File.Delete(Path.Combine(App.WorkingPath, $"{res_SentryLocationFN}_OLD.xlsx"));
            File.Move(Path.Combine(App.WorkingPath, $"{res_SentryLocationFN}.xlsx"), Path.Combine(App.WorkingPath, $"{res_SentryLocationFN}_OLD.xlsx"));

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
            this.buttonSettingsPage.IsEnabled = avail;
            this.buttonCloseApp.IsEnabled = avail;
            this.buttonSentryLocationsPage.IsEnabled = avail;
            this.buttonDirectorAssistant.IsEnabled = avail;
            this.buttonBulletinPage.IsEnabled = avail;
        }
    }
}
