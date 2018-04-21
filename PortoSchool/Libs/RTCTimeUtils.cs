using PortoSchool.Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PortoSchool.Libs
{
    public static class RTCTimeUtils
    {
        public static RTC_DS3231 _realTimeClock;
        public static DateTimeOffset _currentDate;

        public static TimeSpan _currentTime;

        public static DateTime _reportedTime;
        public static DateTime _systemDateTime;

        public static void SetSystemTime()
        {
            var currentTime = CurrentDate + CurrentTime;
            SystemTime sysTime = new SystemTime((currentTime.ToUniversalTime().DateTime));
            Win32.SetSystemTime(ref sysTime);
        }

        public static void InitRTC()
        {
            if (_realTimeClock != null) return;

            _realTimeClock = new RTC_DS3231(() =>
            {
                DateTime? realTime = _realTimeClock.ReadTime();
                DateTimeOffset rtcTime = new DateTimeOffset(_realTimeClock.ReadTime().Value, TimeSpan.Zero);
                CurrentDate = rtcTime.ToLocalTime();
                CurrentTime = rtcTime.ToLocalTime().TimeOfDay;
                SetSystemTime();

                //DispatcherTimer timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromSeconds(1);
                //timer.Tick += (oo, ee) =>
               // {
               //     SetSystemTime();
               //     ReportedTime = DateTime.Now;
               //     SystemDateTime = _realTimeClock.ReadTime().Value;
               // };
               // timer.Start();
            });
        }
        public static DateTimeOffset CurrentDate
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

        public static TimeSpan CurrentTime
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

        public static DateTime ReportedTime
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


        public static DateTime SystemDateTime
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

        public static void SetRtcTime()
        {
            SystemTime sysTime;
            DateTimeOffset dto = CurrentDate.Date + CurrentTime;
            var newTime = dto.ToUniversalTime().DateTime;
            sysTime = new SystemTime(newTime);
            Win32.SetSystemTime(ref sysTime);
            _realTimeClock.WriteTime(newTime);
        }

        /*
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
                    DateTimeOffset dto = datePicker1.Date + timePicker1.Time;
                    var newTime = dto.ToUniversalTime().DateTime;
                    sysTime = new SystemTime(newTime);
                    Win32.SetSystemTime(ref sysTime);
                    _realTimeClock.WriteTime(newTime);
                }

                private void buttonSetSystemDatetime_Click(object sender, RoutedEventArgs e)
                {
                    DateTimeOffset dto = datePicker1.Date + timePicker1.Time;
                    Windows.System.DateTimeSettings.SetSystemDateTime(dto);
                }
                */
    }
}
