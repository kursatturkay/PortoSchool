using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class PresentationPage : Page
    {

        private static bool isineditmode = true;
        public static PresentationPage Current;

        public bool isInEditMode
        {
            get
            {
                return isineditmode;
            }
            set
            {
                isineditmode = value;
                listViewFiles.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;

                //Margin = new Thickness(20, 20, 20, 20);

                flipView1.Margin = (isineditmode) ? new Thickness(20, 70, 420, 20) : new Thickness(0, 0, 0, 0);
                gridHeader.Visibility = (value == false) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public DispatcherTimer _shifttimer;

        public PresentationPage()
        {
            this.InitializeComponent();
            AlternateColorConverter.reset();

            Current = this;
            flipView1.ItemsSource = PicFileManager.GetPicFilesFromStorePath();

            _shifttimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(DashboardPage._slideduration) };
            _shifttimer.Tick += ChangeImage;
            _shifttimer.Start();
        }
        private void ChangeImage(object sender, object o)
        {
            var totalItems = flipView1.Items.Count;

            if (totalItems == 0) return;

            var newItemIndex = (flipView1.SelectedIndex + 1) % totalItems;
            flipView1.SelectedIndex = newItemIndex;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DashboardPage));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (isInEditMode)
                App.Current.IsIdleChanged -= onIsIdleChanged;

            _shifttimer.Stop();
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            isInEditMode =(bool)e.Parameter;
            
            if (isInEditMode)
                App.Current.IsIdleChanged += onIsIdleChanged;
        }

        private void onIsIdleChanged(object sender, EventArgs e)
        {
            DebugUtils.WriteLine("PresentationPage::onIsIdleChanged");
            switch (App.Current.isIdle)
            {
                case true:
                    Frame.Navigate(typeof(KioskPage));
                    break;
            }
        }
    }
}
