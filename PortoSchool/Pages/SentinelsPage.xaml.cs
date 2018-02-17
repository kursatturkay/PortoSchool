using PortoSchool.Libs;
using PortoSchool.Models;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
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
    public sealed partial class SentinelsPage : Page
    {
        public ObservableCollection<Sentinels> nobOgrList;
        public ObservableCollection<NobetAlan> nobAlanList;

        public ObservableCollection<string> nobGunList;
        public SentinelsPage()
        {
            this.InitializeComponent();
            AlternateColorConverter.reset();
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

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Current.IsIdleChanged -= onIsIdleChanged;
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            nobOgrList = SentinelsManager.getNobOgrList();
            nobAlanList = NobetAlanManager.GetNobetAlanlari();

            nobGunList = new ObservableCollection<string> { "PAZARTESİ", "SALI", "ÇARŞAMBA", "PERŞEMBE", "CUMA" };

            //NobetAlanlari = NobetAlanManager.GetNobetAlanlari();
            App.Current.IsIdleChanged += onIsIdleChanged;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }

        private void listViewNobOgr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = (Sentinels)listViewNobetAlanlariVeNobOgr.SelectedItem;

            if (x != null)
            {
                textBoxNobOgrAdSoyad.Text = x.OgretmenAdiSoyadi;

                //comboBoxNobetAlan.SelectedValuePath = (x.NobetAlan==null)?string.Empty:x.NobetAlan;
                //var v1 = nobAlanList.FirstOrDefault(xx => xx.NobetYeri == x.NobetAlan);
                //comboBoxNobetAlan.SelectedIndex =comboBoxNobetAlan.Items.IndexOf(x.NobetAlan);// SelectedValuePath = x.NobetAlan??string.Empty;
                comboBoxNobetAlan.SelectedValue = nobAlanList.FirstOrDefault(xx => xx.NobetYeri == x.NobetAlan);
                comboBoxNobetGunu.SelectedValue = nobGunList.FirstOrDefault(xx => xx == x.NobetGunu);
            }

        }

        public void InsertData()
        {

            // string path = await FileUtils.CreateDefaultFolder();//await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFolderAsync("PortoSchool", CreationCollisionOption.OpenIfExists);
            //path = Path.Combine(path, "settings.sqlite");

            
           // bool a1 = comboBoxNobetAlan.SelectedItem == null;
           // bool a3=comboBoxNobetGunu.sele
            //if (a1) return;

            bool a2 = string.IsNullOrWhiteSpace(textBoxNobOgrAdSoyad.Text.Trim());
            if (a2) return;

            SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), FileUtils.FullDataPath);

            NobetAlan nobalan = new NobetAlan {
                NobetYeri =(comboBoxNobetAlan.SelectedItem as NobetAlan).NobetYeri??""};

            string nobetGunu = (string)comboBoxNobetGunu.SelectedItem ?? "";
            conn.Insert(new Models.Sentinels() { NobetAlan= nobalan.NobetYeri ,OgretmenAdiSoyadi=textBoxNobOgrAdSoyad.Text,NobetGunu= nobetGunu });

            //listViewNobetAlanlari.ItemsSource = NobetAlanManager.GetNobetAlanlari();
        }

        private void buttonSentinelsPageEkle_Click(object sender, RoutedEventArgs e)
        {
            InsertData();
            listViewNobetAlanlariVeNobOgr.ItemsSource = SentinelsManager.getNobOgrList();
        }

        private void buttonSentinelsPageSil_Click(object sender, RoutedEventArgs e)
        {
            var x = (Sentinels)listViewNobetAlanlariVeNobOgr.SelectedItem;

            if (x != null)
            {
                SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), FileUtils.FullDataPath);
                conn.Delete<Sentinels>(x.id);
                listViewNobetAlanlariVeNobOgr.ItemsSource = SentinelsManager.getNobOgrList();
            }
        }

        private void buttonSentinelsPageGuncelle_Click(object sender, RoutedEventArgs e)
        {
            var x = (Sentinels)listViewNobetAlanlariVeNobOgr.SelectedItem;

            if (x != null)
            {
                SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(),FileUtils.FullDataPath);
                var row = conn.Query<Sentinels>("select * from Sentinels where Id=?", x.id).FirstOrDefault();
                if (row != null)
                {
                    row.NobetAlan = (comboBoxNobetAlan.SelectedItem as NobetAlan).NobetYeri;
                    row.OgretmenAdiSoyadi = textBoxNobOgrAdSoyad.Text.Trim();
                    row.NobetGunu = (comboBoxNobetGunu.SelectedItem as string);
                    conn.RunInTransaction(() => {
                        conn.Update(row);
                    });
                    listViewNobetAlanlariVeNobOgr.ItemsSource = SentinelsManager.getNobOgrList();
                }
            }
        }

        private void buttonAra_Click(object sender, RoutedEventArgs e)
        {
            listViewNobetAlanlariVeNobOgr.ItemsSource = SentinelsManager.getNobOgrList(textBoxAra.Text.Trim());
        }

        private void textBoxAra_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                buttonAra_Click(sender,e);
            }
        }
    }
}
