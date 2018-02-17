using PortoSchool.Libs;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;


using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace PortoSchool
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        public static List<string> ErrorLog = new List<string>();
        public static string WorkingPath;

        public event EventHandler IsIdleChanged;
        private DispatcherTimer idleTimer;

        private bool isidle_;


        public bool isIdle
        {
            get
            {
                return isidle_;
            }

            private set
            {
                if (isidle_ != value)
                {
                    isidle_ = value;
                    IsIdleChanged?.Invoke(this, EventArgs.Empty);
                }
            }

        }

        private void OnIdleTimerTick(object sender, Object e)
        {
            DebugUtils.WriteLine("OnIdleTimerTick");
            idleTimer.Stop();
            isIdle = true;
        }

        private void onCoreWindowKeyDown(CoreWindow sender, KeyEventArgs e)
        {
            //DebugUtils.WriteLine("onCoreWindowKeyDown");
            isIdle = false;
            idleTimer.Stop();
            idleTimer.Start();
        }

        private void onCoreWindowPointerMoved(CoreWindow sender, PointerEventArgs args)
        {
            //DebugUtils.WriteLine("onCoreWindowPointerMoved");
            isIdle = false;
            idleTimer.Stop();
            idleTimer.Start();
        }


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            idleTimer = new DispatcherTimer();
            idleTimer.Interval = TimeSpan.FromSeconds(5.0f);  // 10s idle delay
            idleTimer.Tick += OnIdleTimerTick;
            idleTimer.Start();
            Window.Current.CoreWindow.KeyDown += onCoreWindowKeyDown;// onCoreWindowPointerMoved;
            Window.Current.CoreWindow.PointerMoved += onCoreWindowPointerMoved;
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                Window.Current.Activate();
            }
        }
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
