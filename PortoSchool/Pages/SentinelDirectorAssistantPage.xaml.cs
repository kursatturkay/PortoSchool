using OfficeOpenXml;
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
        public ObservableCollection<HolidaysDataSet> Holidays;
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
                datePickerStart.Date = DateTime.Parse(Settings.getValueByKey("DATEPICKERSTART", DateTime.Now.ToString()));
                datePickerEnd.Date = DateTime.Parse(Settings.getValueByKey("DATEPICKEREND", DateTime.Now.AddDays(180).ToString()));
            }
            catch { }

            Holidays = HolidayManager.GetHolidays();


            LoadListViewHolidays();
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

        private void SaveListViewHolidays()
        {
            var conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), FileUtils.FullDataPath);
            conn.DropTable<HolidaysDataSet>();
            conn.CreateTable<HolidaysDataSet>();

            foreach (var x in HolidayManager.Holidays)
            {
                HolidaysDataSet h = new HolidaysDataSet { date = x.date };
                conn.Insert(h);
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

                //pass the holiday days
                while (isHoliday(ilkt))
                    ilkt = ilkt.AddDays(1);

                //!TODO: https://stackoverflow.com/questions/5716762/datetime-now-dayofweek-tostring-with-cultureinfo
                bool a1 = (ilkt.ToString("dddd").ToUpper() == "CUMARTESİ") || (ilkt.ToString("dddd").ToUpper() == "SATURDAY");
                bool a2 = (ilkt.ToString("dddd").ToUpper() == "PAZAR") || (ilkt.ToString("dddd").ToUpper() == "SUNDAY");

                //if day is not weekend
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

        private bool isHoliday(DateTime ilkt)
        {
            bool a = HolidayManager.Holidays.Any(x => x.date.ToShortDateString() == ilkt.ToShortDateString());
            return a;
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

        /// <summary>
        /// <example>
        /// await SaveStringToLocalFile("\\PortoSchool\\NobMudYrdList.csv", str, false);
        /// await SaveStringToLocalFile("\\PortoSchool\\NobMudYrdList_bom.csv", str, true);
        /// </example>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        /// <param name="withBOM"></param>
        /// <returns></returns>
        async Task SaveStringToLocalFile(string filename, string content, bool withBOM)
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

        private void buttonExportSentinelDirectorAssistantList_Click(object sender, RoutedEventArgs e)
        {
            var x = DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri;

            string str = "NÖBETÇİ MÜDÜR YRD.,NÖBET TARİHİ\r\n";

            /*foreach (var xe in x)
            {
                str += $"\"{xe.sentinelDirectorAssistant.FullName}\",\"{xe.SentinelDate.ToLongDateString()}\"\r\n";
            }
            */

            string file = FileUtils.SharedDirectory + @"\SentineDirectorAssistantsCal.xlsx";

            if (File.Exists(file)) File.Delete(file);
            FileInfo newFile = new FileInfo(file);

            // ok, we can run the real code of the sample now
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //// uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                //// get handle to the existing worksheet
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("CALENDAR OF SENTINEL DIRECTOR ASSISTANTS");
                //var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");   //This one is language dependent
                //namedStyle.Style.Font.UnderLine = true;
                //namedStyle.Style.Font.Color.SetColor(Color.Blue);


                if (worksheet != null)
                {

                    List<string[]> headerrow = new List<string[]> {
                        new string[]{ "SENTINEL DIRECTOR ASSITANT","CALENDAR"}
                    };

                    //determine the header range (e.g.A1:E1) 
                    string headerrange = "A:" + Char.ConvertFromUtf32(headerrow[0].Length + 64) + "1";

                    worksheet.Cells[headerrange].LoadFromArrays(headerrow);

                    int i = 0;
                    foreach (var xe in x)
                    {
                        i++;
                        worksheet.Cells[$"A{i}"].Value = xe.sentinelDirectorAssistant.FullName;
                        worksheet.Cells[$"B{i}"].Value = xe.SentinelDate.ToLongDateString();
                        worksheet.Cells[$"B{i}"].Style.Numberformat.Format = "dd - MM - yyyy HH: mm";
                    }
                    //worksheet.Column(1).AutoFit();

                    //worksheet.Cells["A1"].Value = "AdventureWorks Inc.";

                    //    const int startRow = 5;
                    //    int row = startRow;
                    //    //Create Headers and format them 

                    //    using (ExcelRange r = worksheet.Cells["A1:G1"])
                    //    {
                    //        r.Merge = true;
                    //        r.Style.Font.SetFromFont(new Font("Britannic Bold", 22, FontStyle.Italic));
                    //        r.Style.Font.Color.SetColor(Color.White);
                    //        r.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.CenterContinuous;
                    //        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    //        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(23, 55, 93));
                    //    }
                    //    worksheet.Cells["A2"].Value = "Year-End Sales Report";
                    //    using (ExcelRange r = worksheet.Cells["A2:G2"])
                    //    {
                    //        r.Merge = true;
                    //        r.Style.Font.SetFromFont(new Font("Britannic Bold", 18, FontStyle.Italic));
                    //        r.Style.Font.Color.SetColor(Color.Black);
                    //        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                    //        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //        r.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    //    }

                    //    worksheet.Cells["A4"].Value = "Name";
                    //    worksheet.Cells["B4"].Value = "Job Title";
                    //    worksheet.Cells["C4"].Value = "Region";
                    //    worksheet.Cells["D4"].Value = "Monthly Quota";
                    //    worksheet.Cells["E4"].Value = "Quota YTD";
                    //    worksheet.Cells["F4"].Value = "Sales YTD";
                    //    worksheet.Cells["G4"].Value = "Quota %";
                    //    worksheet.Cells["A4:G4"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    worksheet.Cells["A4:G4"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    //    worksheet.Cells["A4:G4"].Style.Font.Bold = true;


                    //    // lets connect to the AdventureWorks sample database for some data
                    //    using (SqlConnection sqlConn = new SqlConnection(connectionString))
                    //    {
                    //        sqlConn.Open();
                    //        using (SqlCommand sqlCmd = new SqlCommand("select LastName + ', ' + FirstName AS [Name], EmailAddress, JobTitle, CountryRegionName, ISNULL(SalesQuota,0) AS SalesQuota, ISNULL(SalesQuota,0)*12 AS YearlyQuota, SalesYTD from Sales.vSalesPerson ORDER BY SalesYTD desc", sqlConn))
                    //        {
                    //            using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                    //            {
                    //                // get the data and fill rows 5 onwards
                    //                while (sqlReader.Read())
                    //                {
                    //                    int col = 1;
                    //                    // our query has the columns in the right order, so simply
                    //                    // iterate through the columns
                    //                    for (int i = 0; i < sqlReader.FieldCount; i++)
                    //                    {
                    //                        // use the email address as a hyperlink for column 1
                    //                        if (sqlReader.GetName(i) == "EmailAddress")
                    //                        {
                    //                            // insert the email address as a hyperlink for the name
                    //                            string hyperlink = "mailto:" + sqlReader.GetValue(i).ToString();
                    //                            worksheet.Cells[row, 1].Hyperlink = new Uri(hyperlink, UriKind.Absolute);
                    //                        }
                    //                        else
                    //                        {
                    //                            // do not bother filling cell with blank data (also useful if we have a formula in a cell)
                    //                            if (sqlReader.GetValue(i) != null)
                    //                                worksheet.Cells[row, col].Value = sqlReader.GetValue(i);
                    //                            col++;
                    //                        }
                    //                    }
                    //                    row++;
                    //                }
                    //                sqlReader.Close();

                    //                worksheet.Cells[startRow, 1, row - 1, 1].StyleName = "HyperLink";
                    //                worksheet.Cells[startRow, 4, row - 1, 6].Style.Numberformat.Format = "[$$-409]#,##0";
                    //                worksheet.Cells[startRow, 7, row - 1, 7].Style.Numberformat.Format = "0%";

                    //                worksheet.Cells[startRow, 7, row - 1, 7].FormulaR1C1 = "=IF(RC[-2]=0,0,RC[-1]/RC[-2])";

                    //                //Set column width
                    //                worksheet.Column(1).Width = 25;
                    //                worksheet.Column(2).Width = 28;
                    //                worksheet.Column(3).Width = 18;
                    //                worksheet.Column(4).Width = 12;
                    //                worksheet.Column(5).Width = 10;
                    //                worksheet.Column(6).Width = 10;
                    //                worksheet.Column(7).Width = 12;
                    //            }
                    //        }
                    //        sqlConn.Close();
                    //    }

                    //    // lets set the header text 
                    //    worksheet.HeaderFooter.OddHeader.CenteredText = "AdventureWorks Inc. Sales Report";
                    //    // add the page number to the footer plus the total number of pages
                    //    worksheet.HeaderFooter.OddFooter.RightAlignedText =
                    //        string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                    //    // add the sheet name to the footer
                    //    worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                    //    // add the file path to the footer
                    //    worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
                    //}
                    //// we had better add some document properties to the spreadsheet 

                    //// set some core property values
                    //xlPackage.Workbook.Properties.Title = "Sample 3";
                    //xlPackage.Workbook.Properties.Author = "John Tunnicliffe";
                    //xlPackage.Workbook.Properties.Subject = "ExcelPackage Samples";
                    //xlPackage.Workbook.Properties.Keywords = "Office Open XML";
                    //xlPackage.Workbook.Properties.Category = "ExcelPackage Samples";
                    //xlPackage.Workbook.Properties.Comments = "This sample demonstrates how to create an Excel 2007 file from scratch using the Packaging API and Office Open XML";

                    //// set some extended property values
                    //xlPackage.Workbook.Properties.Company = "AdventureWorks Inc.";
                    //xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.codeplex.com/MSFTDBProdSamples");

                    //// set some custom property values
                    //xlPackage.Workbook.Properties.SetCustomPropertyValue("Checked by", "John Tunnicliffe");
                    //xlPackage.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1147");
                    //xlPackage.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "ExcelPackage");

                    //// save the new spreadsheet
                    xlPackage.Save();
                }
            }
        } 
        private void btnAddHoliday_Click(object sender, RoutedEventArgs e)
        {
            HolidaysDataSet h = new HolidaysDataSet { date = datePickerHoliday.Date.DateTime };
            HolidayManager.Holidays.Add(h);
            SaveListViewHolidays();
            LoadListViewHolidays();
            /*

            SentinelDirectorAssistant a = new SentinelDirectorAssistant { FullName = textBoxMdYrdAdSoyad.Text.Trim() };
            SentinelDirectorAssistantManager.SentinelDirectorAssistantList.Add(a);

            Listele();
            SavelistViewNobMudYrd();
            textBoxMdYrdAdSoyad.Text = string.Empty;
            */
        }

        private void LoadListViewHolidays()
        {
            listViewHolidays.ItemsSource = null;
            listViewHolidays.ItemsSource = HolidayManager.Holidays;
        }

        private void btnRemoveHoliday_Click(object sender, RoutedEventArgs e)
        {
            HolidaysDataSet row = (HolidaysDataSet)listViewHolidays.SelectedItem;

            if (row == null) return;

            HolidayManager.Holidays.Remove(row);
            LoadListViewHolidays();
            SaveListViewHolidays();
        }
    }
}
