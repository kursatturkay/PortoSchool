using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static PortoSchool.Models.SchoolTimeSpanManager;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PortoSchool.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CoverPage : Page
    {
        public ObservableCollection<SentinelsDataset> nobOgrList;

        public DispatcherTimer clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        public DispatcherTimer HalfDayTimer = new DispatcherTimer { Interval = TimeSpan.FromHours(12) };

        public CoverPage()
        {
            this.InitializeComponent();
            ImageSource imgsrc = new BitmapImage(new Uri("ms-appdata:///local/PortoSchool/Logo.png", UriKind.RelativeOrAbsolute));
            image1.Source = imgsrc;
            LoadCourseTable();
        }

        public void LoadCourseTable()
        {
            //ImportDataFromCouseTableXlsx();
            //strdayofweek maybe (if en-US)MONDAY, TUESDAY.. ,(if tr-TR)PAZARTESİ, SALI... etc.
            string strdayofweek = DateTime.Today.ToString("dddd").ToUpper();

            var x = SchoolTimeSpanManager.schoolTimeSpansByWeekDay(strdayofweek);
            listViewCourseTable.ItemsSource = x;
        }


        public string getCurrentMudYrd()
        {
            var ret = string.Empty;

            var nob = DirectorAssistantSentinelDayManager.MdYrdNöbetGünleri.FirstOrDefault(x => x.SentinelDate.Date == DateTime.Today.Date);
            ret = nob?.sentinelDirectorAssistant.FullName ?? string.Empty;
            return ret;
        }

        private string getNow()
        {
            string ret = string.Empty;
            ret = DateTime.Now.ToString("HH:mm:ss");
            return ret;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var pics = PicFileManager.GetPicFilesFromStorePath();

            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();

            HalfDayTimer.Tick += HalfDayTimer_Tick;
            HalfDayTimer.Start();

            // PresentationPage.Current.isInEditMode = false;

            textBlockDirectorAssistant.Text = getCurrentMudYrd();
            nobOgrList = SentinelsManager.getNobOgrList(getWeekDayUpper(LocalizationUtils.GetDefaultCultureInforName()));

            listViewNobetAlanlariVeNobOgr.ItemsSource = null;
            listViewNobetAlanlariVeNobOgr.ItemsSource = nobOgrList;
            textBlockHeader1.Text = Settings.getValueByKey("SCHOOLNAME1", "");
            textBlockHeader2.Text = Settings.getValueByKey("SCHOOLNAME2", "");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            clockTimer.Stop();
            clockTimer.Tick -= ClockTimer_Tick;

            HalfDayTimer.Stop();
            HalfDayTimer.Tick -= HalfDayTimer_Tick;

            base.OnNavigatedFrom(e);
        }

        #region Timers-------------------------------------------------------------------------------
        private void ClockTimer_Tick(object sender, object e)
        {
            textBlockClock.Text = getNow();
            textBlockToday.Text = getTodayUpper(LocalizationUtils.GetDefaultCultureInforName());

            if (!SchoolTimeSpanManager.isSchoolTimeSpansEmpty())
            {
                string strdayofweek = DateTime.Today.ToString("dddd").ToUpper();
                TimeSpan tsnow = DateTime.Now.TimeOfDay;
                CourseTableRowIdxAndRemainingTime i_and_t;

                i_and_t = SchoolTimeSpanManager.IndexByTime(tsnow, SchoolTimeSpanManager.schoolTimeSpansByWeekDay(strdayofweek));

                try
                {
                    listViewCourseTable.SelectedIndex = i_and_t.idx;
                }
                catch
                {

                }

                TextBlockRemeainingTime.Text = i_and_t.RemainingTime.ToString(@"hh\:mm\:ss");
            }
        }
        private void HalfDayTimer_Tick(object sender, object e)
        {
            DebugUtils.WriteLine("HalfDayTimer_Tick executed");
            textBlockDirectorAssistant.Text = getCurrentMudYrd();
            nobOgrList = SentinelsManager.getNobOgrList(getWeekDayUpper(LocalizationUtils.GetDefaultCultureInforName()));

            listViewNobetAlanlariVeNobOgr.ItemsSource = null;
            listViewNobetAlanlariVeNobOgr.ItemsSource = nobOgrList;
            LoadCourseTable();


        }
        #endregion timers

        private string getWeekDayUpper(string cult)
        {
            string ret = string.Empty;
            CultureInfo cult_ = new CultureInfo(cult);
            ret = DateTime.Today.ToString("dddd", cult_).ToUpper();
            return ret;
        }

        private string getTodayUpper(string cult)
        {
            string ret = string.Empty;
            CultureInfo cult_ = new CultureInfo(cult);
            ret = DateTime.Today.ToString("dd MMMM yyyy dddd", cult_).ToUpper();
            return ret;
        }


    }
}
