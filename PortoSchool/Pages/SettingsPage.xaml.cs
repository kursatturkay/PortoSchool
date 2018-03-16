using PortoSchool.Libs;
using PortoSchool.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Core;
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
    public sealed partial class SettingsPage : Page
    {
        public static int _slideduration {
            get {
                return Convert.ToInt32((Settings.getValueByKey("SLIDER_DURATION", "5")));
            }
            set {
                Settings.setValueByKey("SLIDER_DURATION",value.ToString());
            }
        }

        public static int _coverslideduration
        {
            get
            {
                return Convert.ToInt32((Settings.getValueByKey("COVERSLIDER_DURATION", "5")));
            }
            set
            {
                Settings.setValueByKey("COVERSLIDER_DURATION", value.ToString());
            }
        }
        

        public ObservableCollection<SliderDuration> _sliderdurationdata = new ObservableCollection<SliderDuration> {
            new SliderDuration{row=5},
            new SliderDuration{row=10},
            new SliderDuration{row=20},
            new SliderDuration{row=30},
            new SliderDuration{row=60},
            new SliderDuration{row=120}
        };

        public SettingsPage()
        {
            this.InitializeComponent();

            textBlockThisDevice.Text = $"{NetworkUtils.GetDeviceName()}";
            comboboxSlideDuration.ItemsSource = _sliderdurationdata;
            comboboxCoverSlideDuration.ItemsSource = _sliderdurationdata;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            int _lang = Convert.ToInt32((Settings.getValueByKey("LANGUAGE", "0")));
            comboboxLanguage.SelectedIndex = _lang;

            textBoxSchoolName.Text = Settings.getValueByKey("SCHOOLNAME","");

            string tail = Settings.LocalDataFolder;
            string drv = tail.Substring(0, 1);
            tail = tail.Replace($"{drv}:\\", $"{drv}$\\");
            CultureInfo ci = new CultureInfo("en-US");

            string shareddir = $"\\\\{NetworkUtils.GetDeviceName().ToLower(ci)}\\{tail}";
            textBlockDatabasePath.Text = shareddir;

            textBoxHostAddress.Text = NetworkUtils.LocalIp;

            comboboxSlideDuration.SelectedValue = _sliderdurationdata.FirstOrDefault(x => x.row == _slideduration);

            comboboxCoverSlideDuration.SelectedValue = _sliderdurationdata.FirstOrDefault(x => x.row == _coverslideduration);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void textBoxSchoolName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                Settings.setValueByKey("SCHOOLNAME",textBoxSchoolName.Text.Trim());
            }
        }

        private bool Reload(object param = null)
        {
            var type = Frame.CurrentSourcePageType;

            try
            {
                return Frame.Navigate(type, param);
            }
            finally
            {
                Frame.BackStack.Remove(Frame.BackStack.Last());
            }
        }

        private void textBoxHostAddress_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                NetworkUtils.LocalIp = textBoxHostAddress.Text.Trim();
            }
        }

        private void comboboxSlideDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SliderDuration sel = (SliderDuration)comboboxSlideDuration.SelectedItem;
            Settings.setValueByKey("SLIDER_DURATION", sel.row.ToString());
            _slideduration = sel.row;
        }

        private async void btnChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            //int _lang = Convert.ToInt32((Settings.getValueByKey("LANGUAGE", "0")));
            Settings.setValueByKey("LANGUAGE", comboboxLanguage.SelectedIndex.ToString());

            ComboBoxItem l = (ComboBoxItem)comboboxLanguage.SelectedItem;

            CultureInfo culture = new CultureInfo(l.Content.ToString());
            ApplicationLanguages.PrimaryLanguageOverride =
               culture.Name;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            await Task.Delay(1000);
            Reload();
        }

        private void comboboxCoverSlideDuration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SliderDuration sel = (SliderDuration)comboboxCoverSlideDuration.SelectedItem;
            //Settings.setValueByKey("COVERSLIDER_DURATION", sel.row.ToString());
            _coverslideduration = sel.row;
        }

        private async void btnCopyCouseTableToSharedFolder_Click(object sender, RoutedEventArgs e)
        {

            var res = ResourceLoader.GetForCurrentView();

            //If you want to access a string such as DeleteBlock.Text you cannot put a period. Instead, put a /
            // like this var deleteText = res.GetString("DeleteBlock/Text"); instead of DeleteBlock.Text
            var COURSETABLE_xlsx = res.GetString("COURSETABLE/xlsx");
            //var confirmYes = res.GetString("ConfirmYes");

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/" + COURSETABLE_xlsx));
            StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(Settings.LocalDataFolder);
            await file.CopyAsync(storageFolder, COURSETABLE_xlsx);
        }
    }
}
