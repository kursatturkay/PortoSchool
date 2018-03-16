using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PortoSchool.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SentinelDirectorAssistantPage : Page
    {
        public SentinelDirectorAssistantPage()
        {
            this.InitializeComponent();
            AlternateColorConverter.reset();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                datePickerStart.Date = DateTime.Parse(Settings.getValueByKey("DATEPICKERSTART",DateTime.Now.ToString()));
                datePickerEnd.Date = DateTime.Parse(Settings.getValueByKey("DATEPICKEREND", DateTime.Now.AddDays(180).ToString()));
            }
            catch { }

            LoadlistViewNobMudYrd();
            Load_listViewNobMudYrdCalendar();
            App.Current.IsIdleChanged += onIsIdleChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Current.IsIdleChanged -= onIsIdleChanged;
        }
        private void onIsIdleChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine("SentinelDirectorAssistantPage::onIsIdleChanged");
            switch (App.Current.isIdle)
            {
                case true:
                    //if (frame.CanGoBack)
                    Frame.Navigate(typeof(KioskPage));
                    break;
                    //case true:
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DashboardPage));
        }

        private void buttonMdYrdEkle_Click(object sender, RoutedEventArgs e)
        {
            SentinelDirectorAssistant a = new SentinelDirectorAssistant { FullName = textBoxMdYrdAdSoyad.Text.Trim() };
            SentinelDirectorAssistantManager.SentinelDirectorAssistantList.Add(a);

            Listele();
            SavelistViewNobMudYrd();
            textBoxMdYrdAdSoyad.Text = string.Empty;
        }

        private void SavelistViewNobMudYrd()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            conn.DropTable<SentinelDirectorAssistantDataset>();
            conn.CreateTable<SentinelDirectorAssistantDataset>();

            foreach (var x in SentinelDirectorAssistantManager.SentinelDirectorAssistantList)
            {
                SentinelDirectorAssistantDataset row = new SentinelDirectorAssistantDataset
                {
                    FullName = x.FullName,
                    NotMonday = x.UnwantedDays[0],
                    NotTuesday = x.UnwantedDays[1],
                    NotWednesday = x.UnwantedDays[2],
                    NotThursday = x.UnwantedDays[3],
                    NotFriday = x.UnwantedDays[4]
                };
                conn.Insert(row);
            }
        }

        private void LoadlistViewNobMudYrd()
        {
            // SentinelDirectorAssistantManager.SentinelDirectorAssistantList
            //var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            //listViewNobMudYrd.Items.Clear();

            //var SentinelDirectorAssistantDataset_ = conn.Table<SentinelDirectorAssistantDataset>().ToList();

            SentinelDirectorAssistantManager.Clear();
            //foreach (var x in SentinelDirectorAssistantDataset_)
            //{
            //    SentinelDirectorAssistant mdyrd = new SentinelDirectorAssistant { FullName = x.FullName };

            //    mdyrd.UnwantedDays[0] = x.NotMonday;
            //    mdyrd.UnwantedDays[1] = x.NotTuesday;
            //    mdyrd.UnwantedDays[2] = x.NotWednesday;
            //    mdyrd.UnwantedDays[3] = x.NotThursday;
            //    mdyrd.UnwantedDays[4] = x.NotFriday;

            //    SentinelDirectorAssistantManager.SentinelDirectorAssistantList.Add(mdyrd);
            //}

            Listele();
        }

        private void Load_listViewNobMudYrdCalendar()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath, false);

            listViewNobMudYrdCalendar.Items.Clear();

            try
            {
                //var DirectorAssistantSentinelDayDataset_ = conn.Table<DirectorAssistantSentinelDayDataset>().ToList();

                //DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri.Clear();
                //foreach (var x in DirectorAssistantSentinelDayDataset_)
                //{
                //    DirectorAssistantSentinelDay DirectorAssistantSentinelDay = new DirectorAssistantSentinelDay();
                //    DirectorAssistantSentinelDay.SentinelDate = x.SentinelDate;
                //    DirectorAssistantSentinelDay.nobetci = SentinelDirectorAssistantManager.AdaGöreBul(x.FullName);
                //    DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri.Add(DirectorAssistantSentinelDay);
                //}
            }
            catch
            {
                return;
            }
            GünleriListele();
        }

        private void Save_listViewNobMudYrdCalendar()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath, false);

            conn.DropTable<DirectorAssistantSentinelDayDataset>();
            conn.CreateTable<DirectorAssistantSentinelDayDataset>();

            foreach (var x in DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri)
            {
                DirectorAssistantSentinelDayDataset row = new DirectorAssistantSentinelDayDataset
                {
                    FullName = x.sentinelDirectorAssistant.FullName,
                    SentinelDate = x.SentinelDate

                };
                conn.Insert(row);
            }

        }
        public void Listele()
        {
            listViewNobMudYrd.ItemsSource = null;
            listViewNobMudYrd.ItemsSource = SentinelDirectorAssistantManager.SentinelDirectorAssistantList;

        }


        private void checkBoxIstenmeyenGunler_Checkeds(object sender, RoutedEventArgs e)
        {
            string a = (listViewNobMudYrd.SelectedItem as SentinelDirectorAssistant).FullName;//(((sender as CheckBox).Content) as TextBlock).Text;
            SentinelDirectorAssistant seçilinöb = SentinelDirectorAssistantManager.AdaGöreBul(a);

            int x = Convert.ToInt32((sender as CheckBox).Tag);
            bool ischecked = (sender as CheckBox).IsChecked ?? false;

            seçilinöb.UnwantedDays[x] = ischecked;

            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            string FullName = (listViewNobMudYrd.SelectedItem as SentinelDirectorAssistant).FullName;
            var row = conn.Query<SentinelDirectorAssistantDataset>("select * from SentinelDirectorAssistantDataset where FullName=?", FullName).FirstOrDefault();
            if (row != null)
            {
                if (x == 0) row.NotMonday = ischecked;
                if (x == 1) row.NotTuesday = ischecked;
                if (x == 2) row.NotWednesday = ischecked;
                if (x == 3) row.NotThursday = ischecked;
                if (x == 4) row.NotFriday = ischecked;
            }
            conn.Update(row);
        }

        private void UpdateCheckBoxAvailablity(bool avail)
        {
            checkBoxPtesi.IsEnabled = avail;
            checkBoxSalı.IsEnabled = avail;
            checkBoxÇarşamba.IsEnabled = avail;
            checkBoxPerşembe.IsEnabled = avail;
            checkBoxCuma.IsEnabled = avail;
        }

        private void listViewNobMudYrd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SentinelDirectorAssistant sel = (SentinelDirectorAssistant)listViewNobMudYrd.SelectedItem;
            UpdateCheckBoxAvailablity((sel != null));

            int selidx = (sel == null) ? -1 : SentinelDirectorAssistantManager.SentinelDirectorAssistantList.IndexOf(sel);

            buttonMoveUp.IsEnabled = (sel != null) && (selidx != 0);
            buttonMoveDown.IsEnabled = (sel != null) && (selidx != SentinelDirectorAssistantManager.SentinelDirectorAssistantList.Count - 1);


            if (sel == null) return;

            checkBoxPtesi.IsChecked = sel.UnwantedDays[0];
            checkBoxSalı.IsChecked = sel.UnwantedDays[1];
            checkBoxÇarşamba.IsChecked = sel.UnwantedDays[2];
            checkBoxPerşembe.IsChecked = sel.UnwantedDays[3];
            checkBoxCuma.IsChecked = sel.UnwantedDays[4];
            textBoxMdYrdAdSoyad.Text = sel.FullName;


        }

        private void buttonCalculate_Click(object sender, RoutedEventArgs e)
        {
            DirectorAssistantSentinelDayManager.Clear();

            DateTime ilkt, sont;
            ilkt = datePickerStart.Date.DateTime;
            sont = datePickerEnd.Date.DateTime;

            while (ilkt <= sont)
            {

                bool a1 = (ilkt.ToString("dddd").ToUpper() == "CUMARTESİ") || (ilkt.ToString("dddd").ToUpper() == "SATURDAY");
                bool a2 = (ilkt.ToString("dddd").ToUpper() == "PAZAR") || (ilkt.ToString("dddd").ToUpper() == "SUNDAY");

                if (!(a1 || a2))
                {
                    DirectorAssistantSentinelDay a = new DirectorAssistantSentinelDay();
                    a.SentinelDate = ilkt;
                    DirectorAssistantSentinelDayManager.Add(a);
                }

                ilkt = ilkt.AddDays(1);
            }

            int i = 0;
            int t = SentinelDirectorAssistantManager.SentinelDirectorAssistantList.Count;

            foreach (DirectorAssistantSentinelDay x in DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri)
            {
                x.sentinelDirectorAssistant = SentinelDirectorAssistantManager.SentinelDirectorAssistantList[i];
                i = (i + 1) % t;
            }

            Save_listViewNobMudYrdCalendar();
            GünleriListele();
            Listele();
        }

        public void GünleriListele()
        {
            listViewNobMudYrdCalendar.ItemsSource = null;
            listViewNobMudYrdCalendar.ItemsSource = DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri;
            textBlockCakismaSayisi.Text = $"{DirectorAssistantSentinelDayManager.collisionCount}";
        }

        private void datePickerTar_DateChangeds(object sender, DatePickerValueChangedEventArgs e)
        {
            switch ((sender as DatePicker).Tag)
            {
                case "1": Settings.setValueByKey("DATEPICKERSTART", Convert.ToString(datePickerStart.Date.DateTime)); break;
                case "2": Settings.setValueByKey("DATEPICKEREND", Convert.ToString(datePickerEnd.Date.DateTime)); break;
            }
        }

        private void buttonMdYrdSil_Click(object sender, RoutedEventArgs e)
        {
            SentinelDirectorAssistant SentinelDirectorAssistant = (SentinelDirectorAssistant)listViewNobMudYrd.SelectedItem;
            if (SentinelDirectorAssistant == null) return;

            SentinelDirectorAssistantManager.SentinelDirectorAssistantList.Remove(SentinelDirectorAssistant);

            Listele();
            SavelistViewNobMudYrd();
            textBoxMdYrdAdSoyad.Text = string.Empty;
        }

        private void buttonMdYrdGuncelle_Click(object sender, RoutedEventArgs e)
        {
            SentinelDirectorAssistant SentinelDirectorAssistant = (SentinelDirectorAssistant)listViewNobMudYrd.SelectedItem;
            if (SentinelDirectorAssistant == null) return;

            var r = SentinelDirectorAssistantManager.SentinelDirectorAssistantList.FirstOrDefault(x => x.FullName == SentinelDirectorAssistant.FullName);
            r.FullName = textBoxMdYrdAdSoyad.Text.Trim();

            //Listele();
            SavelistViewNobMudYrd();
            Listele();
        }

        private void buttonMoveDown_Click(object sender, RoutedEventArgs e)
        {
            var sel = (SentinelDirectorAssistant)listViewNobMudYrd.SelectedItem;
            if (sel == null) return;

            int selidx = SentinelDirectorAssistantManager.SentinelDirectorAssistantList.IndexOf(sel);
            if (selidx == -1) return;

            var down = SentinelDirectorAssistantManager.SentinelDirectorAssistantList[selidx + 1];
            if (down == null) return;

            SentinelDirectorAssistant tmp = sel;
            SentinelDirectorAssistantManager.SentinelDirectorAssistantList[selidx] = down;
            SentinelDirectorAssistantManager.SentinelDirectorAssistantList[selidx + 1] = tmp;

            Listele();
            SavelistViewNobMudYrd();
        }

        private void buttonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            var sel = (SentinelDirectorAssistant)listViewNobMudYrd.SelectedItem;
            if (sel == null) return;

            int selidx = SentinelDirectorAssistantManager.SentinelDirectorAssistantList.IndexOf(sel);
            if (selidx == -1) return;

            var up = SentinelDirectorAssistantManager.SentinelDirectorAssistantList[selidx - 1];
            if (up == null) return;

            SentinelDirectorAssistant tmp = sel;
            SentinelDirectorAssistantManager.SentinelDirectorAssistantList[selidx] = up;
            SentinelDirectorAssistantManager.SentinelDirectorAssistantList[selidx - 1] = tmp;

            Listele();
            SavelistViewNobMudYrd();
        }

        private void buttonMoveUp_NobMudYrdCalendar_Click(object sender, RoutedEventArgs e)
        {
            var sel = (DirectorAssistantSentinelDay)listViewNobMudYrdCalendar.SelectedItem;
            if (sel == null) return;

            int selidx = DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri.IndexOf(sel);
            if (selidx == -1) return;

            var up = DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri[selidx - 1];
            if (up == null) return;

            DateTime tmpdate = sel.SentinelDate;
            sel.SentinelDate = up.SentinelDate;
            up.SentinelDate = tmpdate;

            DirectorAssistantSentinelDay tmp = sel;
            DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri[selidx] = up;
            DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri[selidx - 1] = tmp;

            GünleriListele();
            Save_listViewNobMudYrdCalendar();
        }

        private void buttonMoveDown_NobMudYrdCalendar_Click(object sender, RoutedEventArgs e)
        {
            var sel = (DirectorAssistantSentinelDay)listViewNobMudYrdCalendar.SelectedItem;
            if (sel == null) return;

            int selidx = DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri.IndexOf(sel);
            if (selidx == -1) return;

            var down = DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri[selidx + 1];
            if (down == null) return;

            DateTime tmpdate = sel.SentinelDate;
            sel.SentinelDate = down.SentinelDate;
            down.SentinelDate = tmpdate;

            DirectorAssistantSentinelDay tmp = sel;
            DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri[selidx] = down;
            DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri[selidx + 1] = tmp;

            GünleriListele();
            Save_listViewNobMudYrdCalendar();
        }

        private void listViewNobMudYrdCalendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DirectorAssistantSentinelDay sel = (DirectorAssistantSentinelDay)listViewNobMudYrdCalendar.SelectedItem;

            int selidx = (sel == null) ? -1 : DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri.IndexOf(sel);

            buttonMoveUp_NobMudYrdCalendar.IsEnabled = (sel != null) && (selidx != 0);
            buttonMoveDown_NobMudYrdCalendar.IsEnabled = (sel != null) && (selidx != DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri.Count - 1);

            //  if (sel == null) return;
        }

        async Task SaveStringToLocalFile(string filename, string content,bool withBOM)
        {
            // saves the string 'content' to a file 'filename' in the app's local storage folder

            // BOM not present - create the new byte array  
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(content.ToCharArray());
            byte[] outBuffer;

            if (withBOM)
            {
                outBuffer = new byte[fileBytes.Length + 3];

                // add the BOM
                outBuffer[0] = 0xEF;
                outBuffer[1] = (byte)0xBB;
                outBuffer[2] = (byte)0xBF;
                Array.Copy(fileBytes, 0, outBuffer, 3, fileBytes.Length);
            }
            else
            {
                outBuffer = new byte[fileBytes.Length];
                Array.Copy(fileBytes, 0, outBuffer, 0, fileBytes.Length);
            }
            // create a file with the given filename in the local folder; replace any existing file with the same name
            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            // write the char array created from the content string into the file
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                stream.Write(outBuffer, 0, outBuffer.Length);
            }


        }

        private async void buttonExportSentinelDirectorAssistantList_Click(object sender, RoutedEventArgs e)
        {
            var x=DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri;
            //StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri(bf.FileNameOnly));
            //StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath, "NobMudYrdList.csv"));

            string  str="NÖBETÇİ MÜDÜR YRD.,NÖBET TARİHİ\r\n";
            foreach (var xe in x)
            {
                //string sYear = xe.SentinelDate.Year.ToString();
                //string sMonth = xe.SentinelDate.Month.ToString().PadLeft(2, '0');
                //string sDay = xe.SentinelDate.Day.ToString().PadLeft(2, '0');
               // string caseTime = $"{sDay}-{sMonth}-{sYear}";
                str += $"\"{xe.sentinelDirectorAssistant.FullName}\",\"{xe.SentinelDate.ToLongDateString()}\"\r\n";
            }

            await SaveStringToLocalFile("\\PortoSchool\\NobMudYrdList.csv", str,false);
            await SaveStringToLocalFile("\\PortoSchool\\NobMudYrdList_bom.csv", str, true);
        }
    }
}
