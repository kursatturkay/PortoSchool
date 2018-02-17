using PortoSchool.Libs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
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
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            this.InitializeComponent();
        }

        private static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;
        }
        private async void button1_Click(object sender, RoutedEventArgs e)
        {

            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(App.WorkingPath);
            var opts = new QueryOptions();
            opts.FileTypeFilter.Add(".jpg");
            opts.FolderDepth = FolderDepth.Deep;
            StorageFileQueryResult query = folder.CreateFileQueryWithOptions(opts);
            IReadOnlyList<StorageFile> fileList = await query.GetFilesAsync();

            foreach (var x in fileList)
            {
             
                listView1.Items.Add(x.DisplayName);
            }

            //image.Source = fileList.FirstOrDefault().Path.Substring();
            BitmapImage img = new BitmapImage();
            img = await LoadImage(fileList.FirstOrDefault());

            image1.Source = img;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void buttonReadXLS_Click(object sender, RoutedEventArgs e)
        {
            ExcelReader a = ExcelReader.SharedReader();
            List<string> ÖğretmenAdıSoyadı=new List<string>();
            List<string> NöbetGünü = new List<string>();
            List<string> NöbetYeri = new List<string>();

            StorageFile f = await StorageFile.GetFileFromPathAsync(Path.Combine(App.WorkingPath,"Book1.xlsx"));

            List<SheetData> x= await a.ParseSpreadSheetFile(f);

            foreach (var xx in x)
            {
                var data = xx.data;

                foreach (var dd in data)
                {

                    foreach (var ddc in dd)
                    {
                        switch (ddc.Key)
                        {
                            case "ÖĞRETMEN ADI SOYADI": ÖğretmenAdıSoyadı.Add(ddc.Value); break;
                            case "NÖBET GÜNÜ": NöbetGünü.Add(ddc.Value); break;
                            case "NÖBET YERİ": NöbetYeri.Add(ddc.Value); break;
                        }
                        //listView1.Items.Add($"{ddc.Key},{ddc.Value}");
                    }
                }

                for (int i = 0; i<ÖğretmenAdıSoyadı.Count; i++)
                listView1.Items.Add($"{ÖğretmenAdıSoyadı[i]} {NöbetGünü[i]} {NöbetYeri[i]}");
            }
            // TODO : sdssss

            /*x.ForEach(c=> {
                var g=c.data;
                var t=g.FirstOrDefault();
                
                foreach(var tt in t)
                listView1.Items.Add($"{tt.Key},{tt.Value}");
            });*/
        }
    }
}
