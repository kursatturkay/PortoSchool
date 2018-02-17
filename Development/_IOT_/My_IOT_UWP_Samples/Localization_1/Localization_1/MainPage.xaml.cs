using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Localization_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            //this.NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ResourceLoader loader = new ResourceLoader();
            MessageDialog dialog = new MessageDialog(
                loader.GetString("EntryAddedMessageContent"),
                loader.GetString("EntryAddedMessageTitle"));
            dialog.Commands.Add(new UICommand(
                loader.GetString("OK")));

            await dialog.ShowAsync();

        }

        private async void button_Click_1(object sender, RoutedEventArgs e)
        {
            CultureInfo culture = new CultureInfo("tr-TR");
            ApplicationLanguages.PrimaryLanguageOverride =
               culture.Name;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;


            //Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            // Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();

            await Task.Delay(100);
            Reload();
           
            //Frame.Navigate(this.GetType());

            // Frame.GoBack();
            //Frame.Navigate(this.GetType());
        }

        private async void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo culture = new CultureInfo("en-US");
            ApplicationLanguages.PrimaryLanguageOverride =
               culture.Name;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            //Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().Reset();
            //Windows.ApplicationModel.Resources.Core.ResourceContext.GetForViewIndependentUse().Reset();

            //await Task.Delay(100);
            //Frame.Navigate(this.GetType());
            await Task.Delay(100);
            Reload();
            // Frame.GoBack();
            //Frame.Navigate(this.GetType());
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
    }
}
