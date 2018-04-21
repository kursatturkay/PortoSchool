using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using System.IO;
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
        FileSystemWatcher _watcher = new FileSystemWatcher();

        public KioskPage()
        {
            this.InitializeComponent();
            LoadFileSystemWatcher();
        }

        public void LoadFileSystemWatcher()
        {
            //FileSystemWatcher watcher = new FileSystemWatcher();
            _watcher.Path = FileUtils.SharedDirectory;

            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Only watch text files.
            _watcher.Filter = "*.txt";

            // Add event handlers.
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.Created += new FileSystemEventHandler(OnChanged);
            _watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            //Windows.Storage.StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(FileUtils.SharedDirectory);

            //Windows.Storage.StorageFile sampleFile =
            //    await storageFolder.GetFileAsync("SETTINGS.txt");
            //sampleFile.DateCreated
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            DebugUtils.WriteLine("OnChanged");
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
            frame1.Navigate(typeof(CoverPage), false);
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

            ///0:CoverPage
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
