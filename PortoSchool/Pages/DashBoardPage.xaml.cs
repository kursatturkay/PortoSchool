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

        static private DispatcherTimer rtc_initialize_Timer;


        private static bool rtc_initiated = false;
        public ObservableCollection<SliderDuration> _sliderdurationdata = new ObservableCollection<SliderDuration> {
            new SliderDuration{row=5},
            new SliderDuration{row=10},
            new SliderDuration{row=20},
            new SliderDuration{row=30},
            new SliderDuration{row=60},
            new SliderDuration{row=120}
        };

        public static int _slideduration { get; set; }
        public DashboardPage()
        {
            InitializeComponent();
            Current = this;

            //if (!rtc_initiated)
            //{
            //    try
            //    {
            //        rtc_initiated = true;
            //        InitRTC();

            //        if (rtc_initialize_Timer == null)
            //        {
            //            rtc_initialize_Timer = new DispatcherTimer()
            //            {
            //                Interval = TimeSpan.FromSeconds(10)
            //            };

            //            rtc_initialize_Timer.Tick += Rtc_initialize_Timer_Tick;
            //            rtc_initialize_Timer.Start();
            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        DebugUtils.WriteLine($"{ex.Message} @ public DashboardPage()");
            //    }
            //}
            textBlockThisDevice.Text = $"{NetworkUtils.GetDeviceName()}";

            setWorkingLocalPathTextBox();
            InitializeUpdater();
            comboboxSlideDuration.ItemsSource = _sliderdurationdata;
        }

        private void Rtc_initialize_Timer_Tick(object sender, object e)
        {
            rtc_initialize_Timer.Stop();

            try
            {
                UpdateCurrentDateAndCurrentTimefromRTC();
                SetSystemDatetimeFromCurrentDateAndCurrentTime();
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine($"{ex.Message} @ Rtc_initialize_Timer_Tick");
            }
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
            string tail = Settings.LocalDataFolder;
            string drv = tail.Substring(0, 1);
            tail= tail.Replace($"{drv}:\\", $"{drv}$\\");
            CultureInfo ci = new CultureInfo("en-US");
     
            string shareddir = $"\\\\{NetworkUtils.GetDeviceName().ToLower(ci)}\\{tail}";
            App.WorkingPath = Settings.LocalDataFolder;
            textBlockDatabasePath.Text = shareddir;
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
            textBoxHostAddress.Text = NetworkUtils.LocalIp;
            FillListBoxLog();

            _slideduration = Convert.ToInt32((Settings.getValueByKey("SLIDER_DURATION", "5")));
            comboboxSlideDuration.SelectedValue = _sliderdurationdata.FirstOrDefault(x => x.row == _slideduration);
            App.Current.IsIdleChanged += onIsIdleChanged;

            int _lang = Convert.ToInt32((Settings.getValueByKey("LANGUAGE","0")));
            comboboxLanguage.SelectedIndex = _lang;

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

        private void buttonAssistantDirector_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SentinelAssistantDirectorPage));
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
        private void textBoxHostAddress_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                NetworkUtils.LocalIp = textBoxHostAddress.Text.Trim();
            }
        }

        private void buttonSettingsPage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }


        private void comboboxSlideDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SliderDuration sel = (SliderDuration)comboboxSlideDuration.SelectedItem;
            Settings.setValueByKey("SLIDER_DURATION", sel.row.ToString());
            _slideduration = sel.row;
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

        private void comboboxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //int _lang = Convert.ToInt32((Settings.getValueByKey("LANGUAGE", "0")));
            Settings.setValueByKey("LANGUAGE", comboboxLanguage.SelectedIndex.ToString());
        }

        private bool Reload(object param = null)
        {
            var type = Frame.CurrentSourcePageType;

            try
            {
                return Frame.Navigate(type, param);
            }
            finally
            {
                Frame.BackStack.Remove(Frame.BackStack.Last());
            }

        }

        private async void buttonLanguage_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem l = (ComboBoxItem)comboboxLanguage.SelectedItem;

            CultureInfo culture = new CultureInfo(l.Content.ToString());
            ApplicationLanguages.PrimaryLanguageOverride =
               culture.Name;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await Task.Delay(1000);
            Reload();
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
    }
}
