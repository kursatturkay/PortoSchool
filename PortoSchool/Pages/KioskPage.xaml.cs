using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PortoSchool.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KioskPage : Page
    {
        DispatcherTimer _shifttimer;

        public KioskPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _shifttimer.Stop();
            App.Current.IsIdleChanged -= onIsIdleChanged;
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //var pics = PicFileManager.GetPicFilesFromStorePath();

            App.Current.IsIdleChanged += onIsIdleChanged;

            frame2.Navigate(typeof(PresentationPage), false);
            frame1.Navigate(typeof(SentinelKioskFramePage), false);
            frame3.Navigate(typeof(BulletinPage), false);
            _shifttimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(SettingsPage._coverslideduration) };
            _shifttimer.Tick += ChangeSlider;
            _shifttimer.Start();
           
        }

        private void ChangeSlider(object sender, object e)
        {
            var totalItems = flipViewKiosk.Items.Count;
            if (totalItems == 0) return;

            var _totpics = PicFileManager.PicFileCount;
            var _totbulletins = BulletinFileManager.BulletinFileCount;

            var newItemIndex = (flipViewKiosk.SelectedIndex + 1) % totalItems;

            ///0:SentinelKioskFramePage
            ///1:PresentationPage
            ///2:BulletinPage

            switch (newItemIndex)
            {
                case 0:
                    _shifttimer.Interval = TimeSpan.FromSeconds(SettingsPage._coverslideduration);
                    BulletinPage.Current._shifttimer.Stop();
                    PresentationPage.Current._shifttimer.Stop();
                    break;
                case 1:
                    _shifttimer.Interval = TimeSpan.FromSeconds(_totpics * SettingsPage._slideduration);
                    BulletinPage.Current._shifttimer.Stop();
                    PresentationPage.Current._shifttimer.Start();
                    break;
                case 2:
                    _shifttimer.Interval = TimeSpan.FromSeconds((_totbulletins+1) * SettingsPage._slideduration);
                    BulletinPage.Current._shifttimer.Start();
                    PresentationPage.Current._shifttimer.Stop();
                    break;
                default:
                    break;
            }
            flipViewKiosk.SelectedIndex = newItemIndex;
            DebugUtils.WriteLine($"flipViewKiosk.SelectedIndex:{flipViewKiosk.SelectedIndex}");
        }

        private void onIsIdleChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine("onIsIdleChanged");
            switch (App.Current.isIdle)
            {
                case false:
                    Frame.Navigate(typeof(DashboardPage));
                    break;
            }
        }

        private void frame1_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
