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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PortoSchool.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SentinelKioskFramePage : Page
    {
        public ObservableCollection<Sentinels> nobOgrList;

        public DispatcherTimer clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        public DispatcherTimer HalfDayTimer = new DispatcherTimer { Interval = TimeSpan.FromHours(12) };

        public SentinelKioskFramePage()
        {
            this.InitializeComponent();
            ImageSource imgsrc = new BitmapImage(new Uri("ms-appdata:///local/PortoSchool/Logo.png", UriKind.RelativeOrAbsolute));
            image1.Source = imgsrc;
           
        }

        public string getCurrentMudYrd()
        {
            var ret = string.Empty;

            var nob = AssistantDirectorSentinelDayManager.MdYrdNöbetGünleri.FirstOrDefault(x => x.SentinelDate.Date == DateTime.Today.Date);
            ret = nob?.sentinelAssistantDirector.FullName ?? string.Empty;
            return ret;
        }

        private string getNow()
        {
            string ret = string.Empty;
            ret = DateTime.Now.ToString("hh:mm:ss");
            return ret;
        }


        private void ClockTimer_Tick(object sender, object e)
        {
            textBlockClock.Text = getNow();
            textBlockToday.Text = getTodayUpper("tr-TR");
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

            textBlockAssistantDirector.Text = getCurrentMudYrd();
            nobOgrList = SentinelsManager.getNobOgrList(getWeekDayUpper("tr-TR"));

            listViewNobetAlanlariVeNobOgr.ItemsSource = null;
            listViewNobetAlanlariVeNobOgr.ItemsSource = nobOgrList;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            clockTimer.Stop();
            clockTimer.Tick -= ClockTimer_Tick;

            HalfDayTimer.Stop();
            HalfDayTimer.Tick -= HalfDayTimer_Tick;

            base.OnNavigatedFrom(e);
        }
        private void HalfDayTimer_Tick(object sender, object e)
        {
            textBlockAssistantDirector.Text = getCurrentMudYrd();
            nobOgrList = SentinelsManager.getNobOgrList(getWeekDayUpper("tr-TR"));

            listViewNobetAlanlariVeNobOgr.ItemsSource = null;
            listViewNobetAlanlariVeNobOgr.ItemsSource = nobOgrList;
        }

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
