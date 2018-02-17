using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Mp3Player
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MediaElement PlayMusic = new MediaElement();

        public MainPage()
        {
            this.InitializeComponent();

           
            PlayMusic.AudioCategory = Windows.UI.Xaml.Media.AudioCategory.Media;

            PlayNowAsync();


        }

        public async void PlayNowAsync()
        {
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("mp3s");
            StorageFile sf = await folder.GetFileAsync("file1.mp3");
            PlayMusic.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);
            PlayMusic.Play();
        }
    }
}
/*
 StorageFolder Folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                Folder = await Folder.GetFolderAsync("MyFolder");
                StorageFile sf = await Folder.GetFileAsync("MyFile.mp3");
                PlayMusic.SetSource(await sf.OpenAsync(FileAccessMode.Read), sf.ContentType);
                PlayMusic.Play();
     */
