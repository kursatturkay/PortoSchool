using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        RTC_DS3231 _realTimeClock;
        DateTimeOffset _currentDate;
        TimeSpan _currentTime;

        DateTime _reportedTime;
        DateTime _systemDateTime;

        void SetSystemTime()
        {
            var currentTime = CurrentDate + CurrentTime;
            SystemTime sysTime = new SystemTime((currentTime.ToUniversalTime().DateTime));
            Win32.SetSystemTime(ref sysTime);
        }
        public MainPage()
        {
            this.InitializeComponent();
            InitRTC();
           // button_Click(null,null);
        }


        public void InitRTC()
        {
            _realTimeClock = new RTC_DS3231(() =>
            {
                DateTime? realTime = _realTimeClock.ReadTime();
                DateTimeOffset rtcTime = new DateTimeOffset(_realTimeClock.ReadTime().Value, TimeSpan.Zero);
                CurrentDate = rtcTime.ToLocalTime();
                CurrentTime = rtcTime.ToLocalTime().TimeOfDay;
                SetSystemTime();

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (oo, ee) =>
                {
                    SetSystemTime();
                    ReportedTime = DateTime.Now;
                    SystemDateTime = _realTimeClock.ReadTime().Value;

                };
                timer.Start();
            });
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

        public void SetRtcTime()
        {
            SystemTime sysTime;
            DateTimeOffset dto = CurrentDate.Date + CurrentTime;
            var newTime = dto.ToUniversalTime().DateTime;
            sysTime = new SystemTime(newTime);
            Win32.SetSystemTime(ref sysTime);
            _realTimeClock.WriteTime(newTime);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? realTime = _realTimeClock.ReadTime();
            DateTimeOffset rtcTime = new DateTimeOffset(_realTimeClock.ReadTime().Value, TimeSpan.Zero);
            CurrentDate = rtcTime.ToLocalTime();
            CurrentTime = rtcTime.ToLocalTime().TimeOfDay;

            timePicker1.Time = CurrentTime;
            datePicker1.Date = CurrentDate;
            textBlock1.Text = $"{CurrentTime}";
        }

        private void buttonSetTRCTime_Click(object sender, RoutedEventArgs e)
        {
            SystemTime sysTime;
            DateTimeOffset dto = datePicker1.Date+ timePicker1.Time;
            var newTime = dto.ToUniversalTime().DateTime;
            sysTime = new SystemTime(newTime);
            Win32.SetSystemTime(ref sysTime);
            _realTimeClock.WriteTime(newTime);
        }

        private void buttonSetSystemDatetime_Click(object sender, RoutedEventArgs e)
        {
            DateTimeOffset dto = datePicker1.Date+ timePicker1.Time;
            Windows.System.DateTimeSettings.SetSystemDateTime(dto);
        }
    }
}
