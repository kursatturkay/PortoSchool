using PortoSchool.Models;
using PortoSchool.Libs;
using PortoSchool.Pages;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PortoSchool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       string path;
        SQLiteConnection conn;

        private DispatcherTimer timer;//for directly navigate ongoin Page... test purposes only

        public MainPage()
        {
            this.InitializeComponent();
         
            var dir_defaultdoc=FileUtils.CreateDefaultFolder();//initially shall be invoked  for once.
            path = FileUtils.FullDataPath;// Path.Combine(dir_defaultdoc.Result,"settings.sqlite");
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(),path);
            conn.CreateTable<Settings>();
            conn.CreateTable<Sentinels>();
            conn.CreateTable<NobetAlan>();
            conn.CreateTable<SentinelAssistantDirectorDataset>();
            conn.CreateTable<AssistantDirectorSentinelDayDataset>();
            //conn.CreateTable<Bulletin>();
        }

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //timer = new DispatcherTimer() {Interval=new TimeSpan(0,0,1)};
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick +=TimerTick;
            App.Current.IsIdleChanged += onIsIdleChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Current.IsIdleChanged -= onIsIdleChanged;
        }

        private void onIsIdleChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine("Current_IsIdleChanged");
            switch (App.Current.isIdle)
            {
                case true:
                    Frame.Navigate(typeof(KioskPage));
                    break;
                    //case true:
            }
        }

        void TimerTick(object sender,object e)
        {
            timer.Stop();
            Frame.Navigate(typeof(DashboardPage));
        }
    }
}
