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
    public sealed partial class SentinelAssistantDirectorPage : Page
    {
        public SentinelAssistantDirectorPage()
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
            DebugUtils.WriteLine("SentinelAssistantDirectorPage::onIsIdleChanged");
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
            SentinelAssistantDirector a = new SentinelAssistantDirector { FullName = textBoxMdYrdAdSoyad.Text.Trim() };
            SentinelAssistantDirectorManager.SentinelAssistantDirectorList.Add(a);

            Listele();
            SavelistViewNobMudYrd();
            textBoxMdYrdAdSoyad.Text = string.Empty;
        }

        private void SavelistViewNobMudYrd()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            conn.DropTable<SentinelAssistantDirectorDataset>();
            conn.CreateTable<SentinelAssistantDirectorDataset>();

            foreach (var x in SentinelAssistantDirectorManager.SentinelAssistantDirectorList)
            {
                SentinelAssistantDirectorDataset row = new SentinelAssistantDirectorDataset
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
            // SentinelAssistantDirectorManager.SentinelAssistantDirectorList
            //var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            //listViewNobMudYrd.Items.Clear();

            //var SentinelAssistantDirectorDataset_ = conn.Table<SentinelAssistantDirectorDataset>().ToList();

            SentinelAssistantDirectorManager.Clear();
            //foreach (var x in SentinelAssistantDirectorDataset_)
            //{
            //    SentinelAssistantDirector mdyrd = new SentinelAssistantDirector { FullName = x.FullName };

            //    mdyrd.UnwantedDays[0] = x.NotMonday;
            //    mdyrd.UnwantedDays[1] = x.NotTuesday;
            //    mdyrd.UnwantedDays[2] = x.NotWednesday;
            //    mdyrd.UnwantedDays[3] = x.NotThursday;
            //    mdyrd.UnwantedDays[4] = x.NotFriday;

            //    SentinelAssistantDirectorManager.SentinelAssistantDirectorList.Add(mdyrd);
            //}

            Listele();
        }

        private void Load_listViewNobMudYrdCalendar()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath, false);

            listViewNobMudYrdCalendar.Items.Clear();

            try
            {
                //var AssistantDirectorSentinelDayDataset_ = conn.Table<AssistantDirectorSentinelDayDataset>().ToList();

                //AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri.Clear();
                //foreach (var x in AssistantDirectorSentinelDayDataset_)
                //{
                //    AssistantDirectorSentinelDay AssistantDirectorSentinelDay = new AssistantDirectorSentinelDay();
                //    AssistantDirectorSentinelDay.SentinelDate = x.SentinelDate;
                //    AssistantDirectorSentinelDay.nobetci = SentinelAssistantDirectorManager.AdaGöreBul(x.FullName);
                //    AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri.Add(AssistantDirectorSentinelDay);
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

            conn.DropTable<AssistantDirectorSentinelDayDataset>();
            conn.CreateTable<AssistantDirectorSentinelDayDataset>();

            foreach (var x in AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri)
            {
                AssistantDirectorSentinelDayDataset row = new AssistantDirectorSentinelDayDataset
                {
                    FullName = x.sentinelAssistantDirector.FullName,
                    SentinelDate = x.SentinelDate

                };
                conn.Insert(row);
            }

        }
        public void Listele()
        {
            listViewNobMudYrd.ItemsSource = null;
            listViewNobMudYrd.ItemsSource = SentinelAssistantDirectorManager.SentinelAssistantDirectorList;

        }


        private void checkBoxIstenmeyenGunler_Checkeds(object sender, RoutedEventArgs e)
        {
            string a = (listViewNobMudYrd.SelectedItem as SentinelAssistantDirector).FullName;//(((sender as CheckBox).Content) as TextBlock).Text;
            SentinelAssistantDirector seçilinöb = SentinelAssistantDirectorManager.AdaGöreBul(a);

            int x = Convert.ToInt32((sender as CheckBox).Tag);
            bool ischecked = (sender as CheckBox).IsChecked ?? false;

            seçilinöb.UnwantedDays[x] = ischecked;

            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);

            string FullName = (listViewNobMudYrd.SelectedItem as SentinelAssistantDirector).FullName;
            var row = conn.Query<SentinelAssistantDirectorDataset>("select * from SentinelAssistantDirectorDataset where FullName=?", FullName).FirstOrDefault();
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
            SentinelAssistantDirector sel = (SentinelAssistantDirector)listViewNobMudYrd.SelectedItem;
            UpdateCheckBoxAvailablity((sel != null));

            int selidx = (sel == null) ? -1 : SentinelAssistantDirectorManager.SentinelAssistantDirectorList.IndexOf(sel);

            buttonMoveUp.IsEnabled = (sel != null) && (selidx != 0);
            buttonMoveDown.IsEnabled = (sel != null) && (selidx != SentinelAssistantDirectorManager.SentinelAssistantDirectorList.Count - 1);


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
            AssistantDirectorSentinelDayManager.Clear();

            DateTime ilkt, sont;
            ilkt = datePickerStart.Date.DateTime;
            sont = datePickerEnd.Date.DateTime;

            while (ilkt <= sont)
            {

                bool a1 = (ilkt.ToString("dddd").ToUpper() == "CUMARTESİ") || (ilkt.ToString("dddd").ToUpper() == "SATURDAY");
                bool a2 = (ilkt.ToString("dddd").ToUpper() == "PAZAR") || (ilkt.ToString("dddd").ToUpper() == "SUNDAY");

                if (!(a1 || a2))
                {
                    AssistantDirectorSentinelDay a = new AssistantDirectorSentinelDay();
                    a.SentinelDate = ilkt;
                    AssistantDirectorSentinelDayManager.Add(a);
                }

                ilkt = ilkt.AddDays(1);
            }

            int i = 0;
            int t = SentinelAssistantDirectorManager.SentinelAssistantDirectorList.Count;

            foreach (AssistantDirectorSentinelDay x in AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri)
            {
                x.sentinelAssistantDirector = SentinelAssistantDirectorManager.SentinelAssistantDirectorList[i];
                i = (i + 1) % t;
            }

            Save_listViewNobMudYrdCalendar();
            GünleriListele();
            Listele();
        }

        public void GünleriListele()
        {
            listViewNobMudYrdCalendar.ItemsSource = null;
            listViewNobMudYrdCalendar.ItemsSource = AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri;
            textBlockCakismaSayisi.Text = $"{AssistantDirectorSentinelDayManager.collisionCount}";
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
            SentinelAssistantDirector SentinelAssistantDirector = (SentinelAssistantDirector)listViewNobMudYrd.SelectedItem;
            if (SentinelAssistantDirector == null) return;

            SentinelAssistantDirectorManager.SentinelAssistantDirectorList.Remove(SentinelAssistantDirector);

            Listele();
            SavelistViewNobMudYrd();
            textBoxMdYrdAdSoyad.Text = string.Empty;
        }

        private void buttonMdYrdGuncelle_Click(object sender, RoutedEventArgs e)
        {
            SentinelAssistantDirector SentinelAssistantDirector = (SentinelAssistantDirector)listViewNobMudYrd.SelectedItem;
            if (SentinelAssistantDirector == null) return;

            var r = SentinelAssistantDirectorManager.SentinelAssistantDirectorList.FirstOrDefault(x => x.FullName == SentinelAssistantDirector.FullName);
            r.FullName = textBoxMdYrdAdSoyad.Text.Trim();

            //Listele();
            SavelistViewNobMudYrd();
            Listele();
        }

        private void buttonMoveDown_Click(object sender, RoutedEventArgs e)
        {
            var sel = (SentinelAssistantDirector)listViewNobMudYrd.SelectedItem;
            if (sel == null) return;

            int selidx = SentinelAssistantDirectorManager.SentinelAssistantDirectorList.IndexOf(sel);
            if (selidx == -1) return;

            var down = SentinelAssistantDirectorManager.SentinelAssistantDirectorList[selidx + 1];
            if (down == null) return;

            SentinelAssistantDirector tmp = sel;
            SentinelAssistantDirectorManager.SentinelAssistantDirectorList[selidx] = down;
            SentinelAssistantDirectorManager.SentinelAssistantDirectorList[selidx + 1] = tmp;

            Listele();
            SavelistViewNobMudYrd();
        }

        private void buttonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            var sel = (SentinelAssistantDirector)listViewNobMudYrd.SelectedItem;
            if (sel == null) return;

            int selidx = SentinelAssistantDirectorManager.SentinelAssistantDirectorList.IndexOf(sel);
            if (selidx == -1) return;

            var up = SentinelAssistantDirectorManager.SentinelAssistantDirectorList[selidx - 1];
            if (up == null) return;

            SentinelAssistantDirector tmp = sel;
            SentinelAssistantDirectorManager.SentinelAssistantDirectorList[selidx] = up;
            SentinelAssistantDirectorManager.SentinelAssistantDirectorList[selidx - 1] = tmp;

            Listele();
            SavelistViewNobMudYrd();
        }

        private void buttonMoveUp_NobMudYrdCalendar_Click(object sender, RoutedEventArgs e)
        {
            var sel = (AssistantDirectorSentinelDay)listViewNobMudYrdCalendar.SelectedItem;
            if (sel == null) return;

            int selidx = AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri.IndexOf(sel);
            if (selidx == -1) return;

            var up = AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri[selidx - 1];
            if (up == null) return;

            DateTime tmpdate = sel.SentinelDate;
            sel.SentinelDate = up.SentinelDate;
            up.SentinelDate = tmpdate;

            AssistantDirectorSentinelDay tmp = sel;
            AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri[selidx] = up;
            AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri[selidx - 1] = tmp;

            GünleriListele();
            Save_listViewNobMudYrdCalendar();
        }

        private void buttonMoveDown_NobMudYrdCalendar_Click(object sender, RoutedEventArgs e)
        {
            var sel = (AssistantDirectorSentinelDay)listViewNobMudYrdCalendar.SelectedItem;
            if (sel == null) return;

            int selidx = AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri.IndexOf(sel);
            if (selidx == -1) return;

            var down = AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri[selidx + 1];
            if (down == null) return;

            DateTime tmpdate = sel.SentinelDate;
            sel.SentinelDate = down.SentinelDate;
            down.SentinelDate = tmpdate;

            AssistantDirectorSentinelDay tmp = sel;
            AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri[selidx] = down;
            AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri[selidx + 1] = tmp;

            GünleriListele();
            Save_listViewNobMudYrdCalendar();
        }

        private void listViewNobMudYrdCalendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AssistantDirectorSentinelDay sel = (AssistantDirectorSentinelDay)listViewNobMudYrdCalendar.SelectedItem;

            int selidx = (sel == null) ? -1 : AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri.IndexOf(sel);

            buttonMoveUp_NobMudYrdCalendar.IsEnabled = (sel != null) && (selidx != 0);
            buttonMoveDown_NobMudYrdCalendar.IsEnabled = (sel != null) && (selidx != AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri.Count - 1);

            //  if (sel == null) return;
        }

        async Task SaveStringToLocalFile(string filename, string content)
        {
            // saves the string 'content' to a file 'filename' in the app's local storage folder

            // BOM not present - create the new byte array  
            


            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(content.ToCharArray());

            byte[] outBuffer = new byte[fileBytes.Length + 3];

            // add the BOM  
            outBuffer[0] = (byte)0xEF;
            outBuffer[1] = (byte)0xBB;
            outBuffer[2] = (byte)0xBF;
            Array.Copy(fileBytes, 0, outBuffer, 3, fileBytes.Length);

            // create a file with the given filename in the local folder; replace any existing file with the same name
            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

            // write the char array created from the content string into the file
            using (var stream = await file.OpenStreamForWriteAsync())
            {
                stream.Write(outBuffer, 0, outBuffer.Length);
            }
        }

        private async void buttonExportSentinelAssistantDirectorList_Click(object sender, RoutedEventArgs e)
        {
            var x=AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri;
            //StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri(bf.FileNameOnly));
            //StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath, "NobMudYrdList.csv"));

            string  str="NÖBETÇİ MÜDÜR YRD.,NÖBET TARİHİ\r\n";
            foreach (var xe in x)
            {
                //string sYear = xe.SentinelDate.Year.ToString();
                //string sMonth = xe.SentinelDate.Month.ToString().PadLeft(2, '0');
                //string sDay = xe.SentinelDate.Day.ToString().PadLeft(2, '0');
               // string caseTime = $"{sDay}-{sMonth}-{sYear}";
                str += $"\"{xe.sentinelAssistantDirector.FullName}\",\"{xe.SentinelDate.ToLongDateString()}\"\r\n";
            }

            await SaveStringToLocalFile("\\PortoSchool\\NobMudYrdList.csv", str);
        }
    }
}
