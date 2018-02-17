using PortoSchool.Libs;
using PortoSchool.Models;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
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
    public sealed partial class SentryLocationsPage : Page
    {
        public SentryLocationsPage()
        {
            this.InitializeComponent();
            AlternateColorConverter.reset();
        }

        public ObservableCollection<NobetAlan> NobetAlanlari;

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Current.IsIdleChanged -= onIsIdleChanged;
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NobetAlanlari = NobetAlanManager.GetNobetAlanlari();
            App.Current.IsIdleChanged += onIsIdleChanged;
        }

        private void onIsIdleChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine("SentryLocationsPage::onIsIdleChanged");
            switch (App.Current.isIdle)
            {
                case true:
                    Frame.Navigate(typeof(KioskPage));
                    break;
            }
        }

        public void PostDataAsync()
        {
            // string path = await FileUtils.CreateDefaultFolder();//await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("PortoSchool", CreationCollisionOption.OpenIfExists);
            //path = Path.Combine(path, "settings.sqlite");
            SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), FileUtils.FullDataPath);

            conn.Insert(new Models.NobetAlan() { NobetYeri = textBoxNobetYeri.Text });
            listViewNobetAlanlari.ItemsSource = NobetAlanManager.GetNobetAlanlari();
        }

        private void buttonNobetAlaniEkle_Click(object sender, RoutedEventArgs e)
        {
            PostDataAsync();
        }

        private void listViewNobetAlanlari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = (NobetAlan)listViewNobetAlanlari.SelectedItem;

            if (x != null)
                textBoxNobetYeri.Text = x.NobetYeri;
        }

        private void buttonNobetAlaniSil_Click(object sender, RoutedEventArgs e)
        {
            var x = (NobetAlan)listViewNobetAlanlari.SelectedItem;

            if (x != null)
            {
                string path = FileUtils.FullDataPath;//await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("PortoSchool", CreationCollisionOption.OpenIfExists);
                //path = Path.Combine(path, "settings.sqlite");

                SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
                conn.Delete<NobetAlan>(x.id);
                listViewNobetAlanlari.ItemsSource = NobetAlanManager.GetNobetAlanlari();
            }
        }

        private void buttonNobetAlaniGuncelle_Click(object sender, RoutedEventArgs e)
        {
            var x = (NobetAlan)listViewNobetAlanlari.SelectedItem;
            int idx = listViewNobetAlanlari.SelectedIndex;

            if (x != null)
            {
                string path = FileUtils.FullDataPath;//await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("PortoSchool", CreationCollisionOption.OpenIfExists);
                //path = Path.Combine(path, "settings.sqlite");

                SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);
                var row = conn.Query<NobetAlan>("select * from NobetAlan where Id=?", x.id).FirstOrDefault();

                if (row != null)
                {
                    row.NobetYeri = textBoxNobetYeri.Text.Trim();

                    conn.RunInTransaction(() =>
                    {
                        conn.Update(row);
                    });

                    listViewNobetAlanlari.ItemsSource = NobetAlanManager.GetNobetAlanlari();
                    listViewNobetAlanlari.SelectedIndex = idx;
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(0.5f));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }
    }
}
