using PortoSchool.Libs;
using PortoSchool.Models;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Pdf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI.Text;
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
    public sealed partial class BulletinPage : Page
    {
        private static bool _isineditmode = true;
        public static BulletinPage Current;
        public DispatcherTimer _shifttimer;

        private StorageFile storageFile;//using to get pdf files
        private PdfDocument pdfDoc;

        //public ObservableCollection<BitmapImage> PdfPages
        // {
        //    get;
        //     set;
        //} = new ObservableCollection<BitmapImage>();

        async void LoadPdf(PdfDocument pdfDoc, int qualityRatio = 1)
        {
            //PdfPages.Clear();

            //for (uint i = 0; i < pdfDoc.PageCount; i++)
            //{
            BitmapImage image = new BitmapImage();

            var page = pdfDoc.GetPage(0);

            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                var opt = new PdfPageRenderOptions
                {
                    DestinationWidth = (uint)page.Size.Width * (uint)qualityRatio,
                    DestinationHeight = (uint)page.Size.Height * (uint)qualityRatio
                };


                await page.RenderToStreamAsync(stream, opt);
                await image.SetSourceAsync(stream);
            }

            //PdfPages.Add(image);
            ImagePdfPage.Source = image;
            // }
        }

        public bool isInEditMode
        {
            get
            {
                return _isineditmode;
            }
            set
            {
                _isineditmode = value;
                listView1.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;

                ImagePdfPage.Margin = (_isineditmode) ? new Thickness(320, 325, 20, 15) : new Thickness(0, 0, 0, 0);
                gridHeader.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;

                if (value) _shifttimer.Stop();
                else _shifttimer.Start();

                buttonAdd.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;
                buttonRemove.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;
                buttonUpdate.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;
                textBoxBulletinTitle.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public BulletinPage()
        {
            this.InitializeComponent();
            Current = this;
            listView1.ItemsSource = BulletinFileManager.GetBulletinFilesFromStorePath();

            _shifttimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(SettingsPage._slideduration) };
            _shifttimer.Tick += _shifttimer_Tick;
        }

        private void _shifttimer_Tick(object sender, object e)
        {
            var total_ = BulletinFileManager.BulletinFileCount;
            if (total_ == 0) return;

            var newItemIndex = (listView1.SelectedIndex + 1) % total_;
            DebugUtils.WriteLine($"{newItemIndex}");
            listView1.SelectedIndex = newItemIndex;

            if (listView1.SelectedIndex == 1)
                _shifttimer.Interval = TimeSpan.FromSeconds(total_ * SettingsPage._slideduration);
            else
                _shifttimer.Interval = TimeSpan.FromSeconds(SettingsPage._slideduration);
        }

        // public ObservableCollection<Bulletin> Bulletins;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            isInEditMode = (bool)e.Parameter;

            if (isInEditMode)
                App.Current.IsIdleChanged += onIsIdleChanged;
            //  Bulletins = BullettinManager.GetBulletins();

            if (BulletinFileManager.BulletinFileCount > 0)
                listView1.SelectedIndex = 0;
        }


        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            App.Current.IsIdleChanged -= onIsIdleChanged;
            base.OnNavigatingFrom(e);
        }
        private void onIsIdleChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine("BulletinPage::onIsIdleChanged");
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            /*
            string title = textBoxBulletinTitle.Text.Trim();

            if (string.IsNullOrEmpty(title))
            {
                textBoxBulletinTitle.Focus(FocusState.Programmatic);
            }
            else
            {
              

                RichEditBoxBulletinContent.Document.SetText(TextSetOptions.FormatRtf, string.Empty);

                StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Settings.LocalDataFolder);

                StorageFile sf =
                    await storageFolder.CreateFileAsync($"{title}.rtf",
                        CreationCollisionOption.ReplaceExisting);

                CachedFileManager.DeferUpdates(sf);

                IRandomAccessStream stream = await sf.OpenAsync(FileAccessMode.ReadWrite);
                RichEditBoxBulletinContent.Document.SaveToStream(TextGetOptions.FormatRtf, stream);

                FileUpdateStatus fus = await CachedFileManager.CompleteUpdatesAsync(sf);//is 

                stream.Dispose();
                listView1.ItemsSource = null;
                listView1.ItemsSource = BulletinFileManager.GetBulletinFilesFromStorePath();
            }
            */
        }

        private void btnUpdate_ClickAsync(object sender, RoutedEventArgs e)
        {
            /*
            BulletinFile bf = (BulletinFile)listView1.SelectedItem;
            StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri(bf.FileNameOnly));

            CachedFileManager.DeferUpdates(sf);//is it necessarry ? who knows.
            IRandomAccessStream stream = await sf.OpenAsync(FileAccessMode.ReadWrite);

            RichEditBoxBulletinContent.Document.SaveToStream(TextGetOptions.FormatRtf, stream);
            stream.Dispose();
            FileUpdateStatus fus = await CachedFileManager.CompleteUpdatesAsync(sf);//is it necessarry ? who knows.
            */
        }

        private async void listView1_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
        {

            BulletinFile bf = (BulletinFile)listView1.SelectedItem;
            if (bf != null)
            {
                storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(bf.FileNameOnly));
                pdfDoc = await PdfDocument.LoadFromFileAsync(storageFile);

                LoadPdf(pdfDoc, 2);
                //StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri(bf.FileNameOnly));
                //var stream = await sf.OpenAsync(FileAccessMode.Read);

                //RichEditBoxBulletinContent.Document.LoadFromStream(TextSetOptions.FormatRtf, stream);

            }

        }

        private async void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            BulletinFile bf = (BulletinFile)listView1.SelectedItem;

            if (bf != null)
            {
                StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri(bf.FileNameOnly));
                await sf.DeleteAsync();

                listView1.ItemsSource = null;
                listView1.ItemsSource = BulletinFileManager.GetBulletinFilesFromStorePath();
            }
        }
    }
}
