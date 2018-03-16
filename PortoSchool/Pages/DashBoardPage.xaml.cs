using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.System;
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
    /// 



    public sealed partial class DashboardPage : Page
    {
        public static DashboardPage Current;
        DateTimeOffset _currentDate;
        TimeSpan _currentTime;

        DateTime _reportedTime;
        DateTime _systemDateTime;

     

       
        public DashboardPage()
        {
            InitializeComponent();
            Current = this;

            setWorkingLocalPathTextBox();
            InitializeUpdater();

            if(SchoolTimeSpanManager.isSchoolTimeSpansEmpty())
            SchoolTimeSpanManager.ImportDataFromCouseTableXlsx();
        }


        private void SetSystemDatetimeFromCurrentDateAndCurrentTime()
        {
            DateTimeOffset dto = CurrentDate.Date + CurrentTime;
            Windows.System.DateTimeSettings.SetSystemDateTime(dto);
        }

        private void UpdateCurrentDateAndCurrentTimefromRTC()
        {
           // DateTime? realTime = _realTimeClock.ReadTime();
           // DateTimeOffset rtcTime = new DateTimeOffset(_realTimeClock.ReadTime().Value, TimeSpan.Zero);
           // CurrentDate = rtcTime.ToLocalTime();
           // CurrentTime = rtcTime.ToLocalTime().TimeOfDay;

           // timePicker1.Time = CurrentTime;
           // datePicker1.Date = CurrentDate;
            //textBlock1.Text = $"{CurrentTime}";
        }

        public void FillListBoxLog()
        {
            listBoxLog.Items.Clear();
            App.ErrorLog.ForEach(x => { listBoxLog.Items.Add(x); });
        }
        private void setWorkingLocalPathTextBox()
        {
            //string res = (!toggleSwitchLocatorHost.IsOn) ? Settings.LocalDataFolder : Settings.NetworkDataFolder(textBoxHostAddress.Text);
            //\\minwinpc\c$\Data\Users\DefaultAccount\AppData\Local\Packages\06fb6d66-51b3-4beb-893c-7e099fe465f1_3asabdzxmrwg6\LocalState\PortoSchool
            App.WorkingPath = Settings.LocalDataFolder;
        }

        #region Events Section
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Current.IsIdleChanged -= onIsIdleChanged;
            base.OnNavigatedFrom(e);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //textBlockDatabasePath.Text = Settings.FullPathSQLite;
            
            FillListBoxLog();

            App.Current.IsIdleChanged += onIsIdleChanged;

           

            textBlockHeader.Text = Settings.getValueByKey("SCHOOLNAME", "");
        }

        private void onIsIdleChanged(object sender, EventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine($"IsIdle: {App.Current.IsIdle}");
            DebugUtils.WriteLine($"IsIdle: {App.Current.isIdle}");

            switch (App.Current.isIdle)
            {
                case false:; break;
                case true:
                    this.Frame.Navigate(typeof(KioskPage));
                    ; break;
            }
            //textBox.Visibility = App.Current.IsIdle ? Visibility.Collapsed : Visibility.Visible;// = App.Current.IsIdle;
        }

        private void buttonSentryLocationsPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SentryLocationsPage));
        }

        private void buttonCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void buttonSentinelsPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SentinelsPage));
        }

        private void buttonDirectorAssistant_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SentinelDirectorAssistantPage));
        }

        private void buttonPresentationPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PresentationPage), true);
        }

        private void buttonOpenLocalFolder_Click(object sender, RoutedEventArgs e)
        {
            Uri newuri = new Uri(
                Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "PortoSchool"));

            //DefaultLaunch();
        }


        private void buttonSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }




        void SetSystemTime()
        {
            var currentTime = CurrentDate + CurrentTime;
            SystemTime sysTime = new SystemTime((currentTime.ToUniversalTime().DateTime));
            Win32.SetSystemTime(ref sysTime);

        }
      
        public DateTimeOffset CurrentDate
        {
            get { return _currentDate - _currentDate.TimeOfDay; }
            set
            {
                if (_currentDate != value)
                {
                    _currentDate = value;
                }
            }
        }

        public TimeSpan CurrentTime
        {
            get { return _currentTime; }
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                }
            }
        }

        public DateTime ReportedTime
        {
            get { return _reportedTime; }
            set
            {
                if (_reportedTime != value)
                {
                    _reportedTime = value;
                }
            }
        }


        public DateTime SystemDateTime
        {
            get { return _systemDateTime; }
            set
            {
                if (_systemDateTime != value)
                {
                    _systemDateTime = value;
                }
            }
        }     

        private void button_Click(object sender, RoutedEventArgs e)
        {
           // DateTime? realTime = _realTimeClock.ReadTime();
          //  DateTimeOffset rtcTime = new DateTimeOffset(_realTimeClock.ReadTime().Value, TimeSpan.Zero);
           // CurrentDate = rtcTime.ToLocalTime();
           // CurrentTime = rtcTime.ToLocalTime().TimeOfDay;

            //timePicker1.Time = CurrentTime;
            //datePicker1.Date = CurrentDate;
            //textBlock1.Text = $"{CurrentTime}";
        }

        private void buttonSetTRCTime_Click(object sender, RoutedEventArgs e)
        {
            SystemTime sysTime;
            DateTimeOffset dto = datePicker1.Date + timePicker1.Time;
            var newTime = dto.ToUniversalTime().DateTime;
            sysTime = new SystemTime(newTime);
            Win32.SetSystemTime(ref sysTime);

          // _realTimeClock.WriteTime(newTime);
        }

        private void buttonSetSystemDatetime_Click(object sender, RoutedEventArgs e)
        {
            DateTimeOffset dto = CurrentDate + CurrentTime;
            Windows.System.DateTimeSettings.SetSystemDateTime(dto);
        }
        #endregion Events Section

        private void buttonBulletinPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BulletinPage), true);
        }


        private void datePicker1_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DateTimeSettings.SetSystemDateTime(e.NewDate.UtcDateTime);
        }

        private void timePicker1_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            var currentDate = DateTime.Now.ToUniversalTime();

            var newDateTime = new DateTime(currentDate.Year,
                                           currentDate.Month,
                                           currentDate.Day,
                                           e.NewTime.Hours,
                                           e.NewTime.Minutes,
                                           e.NewTime.Seconds);

            DateTimeSettings.SetSystemDateTime(newDateTime);
        }

        private void buttonCourseTablePage_Click(object sender, RoutedEventArgs e)
        {
            //Frame.Navigate(typeof(CourseTablePage));
        }
    }
}
