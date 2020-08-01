using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using Windows.ApplicationModel.ExtendedExecution.Foreground;
using System.Diagnostics;
using Windows.Media.Control;
using Windows.Networking.Sockets;
using System.Threading.Tasks;

namespace WMCP {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        // private members
        private Libs.Controller ctrller;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow() {
            InitializeComponent();

            Init();

            updateComponent();

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush")) {
                /*                Windows.UI.Xaml.Media.AcrylicBrush myBrush = new Windows.UI.Xaml.Media.AcrylicBrush();

                                myBrush.BackgroundSource = Windows.UI.Xaml.Media.AcrylicBackgroundSource.HostBackdrop;
                                myBrush.TintColor = Color.FromArgb(47, 54, 64, 100);
                                myBrush.FallbackColor = Color.FromArgb(47, 54, 64, 100);
                                myBrush.TintOpacity = 0.2;*/
                //System.Windows.Media.Brushes

                this.Background = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 202, 24, 37));
                //this.Background = myBrush;
                //page.Background = myBrush;
            }
            else {
                System.Windows.Media.SolidColorBrush myBrush = new System.Windows.Media.SolidColorBrush(Color.FromArgb(255, 202, 24, 37));
                this.Background = myBrush;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindow() {
            ctrller.Dispose();
        }

        private async void Init() {
            ctrller = await Libs.Controller.BuildControllerAsync();

            ctrller.MediaPropertiesChangedEvents.Add(MediaPropertiesChange);
            ctrller.PlaybackInfoChangedEvents.Add(PlaybackInfoChange);
            ctrller.TimelinePropertiesChangedEvents.Add(TimelinePropertiesChange);
        }

        private void MediaPropertiesChange(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs e) {
            ctrller.updateMediaProperites();
        }

        private void PlaybackInfoChange(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs e) {
            ctrller.updatePlaybackInfo();
            updateComponent();
        }

        private void TimelinePropertiesChange(GlobalSystemMediaTransportControlsSession sender, TimelinePropertiesChangedEventArgs e) {
            ctrller.updateTimelineProperties();
        }

        private void updateComponent() {
            btnPlay.Dispatcher.Invoke(new Action(() => {
                if (ctrller.currentMediaInfo != null)
                    btnPlay.Content = ctrller.currentMediaInfo.Playing ? "⏸" : "▶";
            }));
        }

        /*
        /// <summary>
        /// set window position
        /// </summary>
        private void CenterWindowsToScreen() {
            //Screen scn = Screen.FromControl(this);
        }*/

        // ------------------------EVENTS--------------------------
        /// <summary>
        /// Event method for clicking Play(▶) button
        /// </summary>
        private void btnPlay_Click(object sender, RoutedEventArgs e) {
            //WMCP.Libs.DllInvoke.SendPlayPauseKey();
            ctrller.playPause(false);
        }

        /// <summary>
        /// Event method for clicking Prev(⏮) button
        /// </summary>
        private void btnPrev_Click(object sender, RoutedEventArgs e) {
            //WMCP.Libs.DllInvoke.SendPrevKey();
            ctrller.prev();
        }

        /// <summary>
        /// Event method for clicking Next(⏭) button
        /// </summary>
        private void btnNext_Click(object sender, RoutedEventArgs e) {
            //WMCP.Libs.DllInvoke.SendNextKey();
            ctrller.next();
        }

        /// <summary>
        /// Event method for MainWindowKeyDown
        /// </summary>
        private void MainWindowInstance_KeyDown(object sender, KeyEventArgs e) {
            if (sender is MainWindow) {
                switch (e.Key) {
                    case Key.F1:
                        MessageBox.Show("what the hell");
                        break;
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e) {
            rtbInfo.Document.Blocks.Clear();
            ctrller.updateMediaInfos();

            rtbInfo.AppendText(ctrller.ToString());
        }
    }


}
