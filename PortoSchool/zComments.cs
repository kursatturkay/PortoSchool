/*
 xlsreader errors.
 if key is doubled or non exists nullexception may occur.
 if empty sheet also generates nullexception error
 aralarda bir header (key) boş ise nullexpcetion
 */
/*

var doc = Windows.Storage.UserDataPaths.GetDefault().LocalAppData;
path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");

conn.Insert<Settings>(new Settings(){ Key="Hola",Value="HolaBabe"},Settings);
Settings rec = new Settings() {Key="Hola",Value="HolaBaby" };

var dir_defaultdoc = CreateDefaultFolder();

--------------------------------------------------------------------------------------------------------------
  <ComboBox x:Name="comboBoxNobetGunu" HorizontalAlignment="Left" Margin="20,140,0,0" VerticalAlignment="Top" Width="360" ItemsSource="{x:Bind nobGunList}" Height="35" >
                <ComboBox.ItemTemplate>
                    <DataTemplate x:DataType="local:SentinelsPage">
                        <TextBlock Text="{x:Bind nobGunList }"/>
                    </DataTemplate>
                </ComboBox.ItemTemplate>
            </ComboBox>
 
--------------------------------------------------------------------------------------------------------------
  public class ImageConverter : IValueConverter
   {

       public object Convert(object value, Type targetType, object parameter, string language)
       {
           return new BitmapImage(new Uri(value.ToString()));
       }

       public object ConvertBack(object value, Type targetType, object parameter, string language)
       {
           throw new NotImplementedException();
       }
   }

--------------------------------------------------------------------------------------------------------------

 listViewFiles.ItemsSource = PicFileManager.GetPicFilesFromStorePath();

           
           ObservableCollection<Uri> a=  PicFileManager.GetPicFilesFromStorePath();

           foreach (var x in a)
           {

               ImageBrush brush1 = new ImageBrush();
               brush1.ImageSource = new BitmapImage(x);

           }
           
//FlipView flip1 = newFlipView();
//ImageBrush brush1 = new ImageBrush();
//brush1.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/image1.png"));
//ImageBrush brush2 = new ImageBrush();
//brush2.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/image2.png"));
//ImageBrush brush3 = new ImageBrush();
//brush3.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/image3.png"));


//string path = System.IO.Path.GetDirectoryName(FileUtils.FullDataPath);

bmImage.UriSource = new Uri(new Uri(
Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" +
Windows.Storage.ApplicationData.Current.LocalFolder.Name),
"favicon.scale-100.ico");


--------------------------------------------------------------------------------------------------------------

<Button x:Name="buttonOpenLocalFolder" Grid.Column="1" Grid.Row="3" HorizontalAlignment="Stretch" VerticalAlignment="Stretch" Content="YÜKLEME DİZİNİNİ AÇ" Click="buttonOpenLocalFolder_Click"/>

async void DefaultLaunch(string uri)
{
    // Launch the URI
    var success = await Windows.System.Launcher.LaunchUriAsync(new Uri("http://www.google.com"));

    if (success)
    {
        // URI launched
    }
    else
    {
        // URI launch failed
    }
}

--------------------------------------------------------------------------------------------------------------
 private void nobetAlanPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                CoreVirtualKeyStates menuShiftStatetate = Window.Current.CoreWindow.GetKeyState(VirtualKey.Shift);

                if (menuShiftStatetate==CoreVirtualKeyStates.Down)
                { 
                    e.Handled= true;
                ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(0.5f));
                }
            }
        }
--------------------------------------------------------------------------------------------------------------

 private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var p = e.GetCurrentPoint((UIElement)sender);
                if (p.Properties.IsRightButtonPressed)
                {
                    Frame.Navigate(typeof(LoginPage));
                }
            }
        }
 --------------------------------------------------------------------------------------------------------------
   //var query = conn.Table<Settings>().Where(v => v.Key.StartsWith(key));
            //var q = conn.Table<Settings>().Where(x => x.Equals(key));
            // var query = conn.Table<Settings>();

             var tableQuery = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Entry';"
             bool tableExists = conn.ExecuteScalar<int>( tableQuery ) == 1; 
            --------------------------------------------------------------------------------------------------------------
  */