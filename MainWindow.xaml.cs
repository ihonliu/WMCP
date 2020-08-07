//    WMCP Windows media control panel
//    Copyright (c) 2020 - 2020 Ihon Liu
//    
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Windows.Media.Control;
using Windows.Storage.Streams;
using System.Linq;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


using Session = Windows.Media.Control.GlobalSystemMediaTransportControlsSession;

namespace WMCP {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        #region Fields and properties
        private Libs.Controller ctrller;
        private Libs.Model model;
        private bool pinned = false;
        private Action updateMediaProperties, updatePlaybackInfo, updateTimelineProperties;
        #endregion Fields and properties

        #region Constructor and destructor
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            Init();
            var pt = Libs.TaskbarHelper.GetTrayPopupWindowPostion((int)Width, (int)Height);
            Top = pt.Y;
            Left = pt.X;
        }
        /// <summary>
        /// Destructor
        /// </summary>
        ~MainWindow() {
            ctrller.Dispose();
        }
        #endregion Constructor and destructor

        #region Methods
        private async void Init() {
            model = await Libs.Model.BuildModel();

            ctrller = Libs.Controller.BuildControllerAsync(model);
            updateMediaProperties = new Action(async () => {
                if (model != null && model.Media_Properties != null) {
                    LblArtist.Content = model.Media_Properties.Artist;
                    LblTitle.Content = model.Media_Properties.Title;

                    try {
                        System.IO.Stream stream = System.IO.WindowsRuntimeStreamExtensions.AsStream(await model.Media_Properties.Thumbnail.OpenReadAsync());

                        var imgSrc = new BitmapImage();
                        imgSrc.BeginInit();
                        imgSrc.StreamSource = stream;
                        imgSrc.EndInit();
                        imAlbum.Source = imgSrc;
                    }
                    catch (System.NullReferenceException) {
                        var imgSrc = new BitmapImage();
                        imAlbum.Source = imgSrc;
                    }
                    catch {
                        throw;
                    }
                }
            });
            updatePlaybackInfo = new Action(() => { BtnPlay.Content = model.Playing ? "\uE769" : "\uE768"; });
            updateTimelineProperties = new Action(() => { PbTimeline.Value = model.Time_line.Percentage; });

            model.MediaPropertiesChange += MediaPropertiesChanged;
            model.PlaybackInfoChange += PlaybackInfoChanged;
            model.TimelinePropertiesChange += TimelinePropertiesChanged;
            model.Refresh();
        }
        #endregion

        #region Events
        /// <summary>
        /// Event for time line properties changed
        /// </summary>
        /// <param name="session"></param>
        /// <param name="e"></param>
        private void MediaPropertiesChanged(Session session, MediaPropertiesChangedEventArgs e) {
            this.Dispatcher.Invoke(updateMediaProperties);
        }
        /// <summary>
        /// Event for time line properties changed
        /// </summary>
        /// <param name="session"></param>
        /// <param name="e"></param>
        private void PlaybackInfoChanged(Session session, PlaybackInfoChangedEventArgs e) {
            this.Dispatcher.Invoke(updatePlaybackInfo);
        }
        /// <summary>
        /// Event for time line properties changed
        /// </summary>
        /// <param name="session"></param>
        /// <param name="e"></param>
        private void TimelinePropertiesChanged(Session session, TimelinePropertiesChangedEventArgs e) {
            this.Dispatcher.Invoke(updateTimelineProperties);
        }
        /// <summary>
        /// Event method for clicking Play(▶) button
        /// </summary>
        private void BtnPlay_Click(object sender, RoutedEventArgs e) {
            ctrller.PlayPause();
        }
        /// <summary>
        /// Event method for clicking Prev(⏮) button
        /// </summary>
        private void BtnPrev_Click(object sender, RoutedEventArgs e) {
            ctrller.Previous();
        }
        /// <summary>
        /// Event method for clicking Next(⏭) button
        /// </summary>
        private void BtnNext_Click(object sender, RoutedEventArgs e) {
            ctrller.Next();
        }


        private static readonly Key[] debugKeySequ = new Key[] { Key.Up, Key.Up, Key.Down, Key.Down, Key.Left, Key.Right, Key.Left, Key.Right, Key.B, Key.A, Key.B, Key.A };
#if !DEBUG
        private static int debugSequPos = 0;
#endif
        /// <summary>
        /// Event method for MainWindowKeyDown
        /// </summary>
        /// state
        private void CheckDebugKeySequ(Key key) {
            DebugWindow tmp;
#if DEBUG
            tmp = new DebugWindow {
                Top = this.Top,
                Left = this.Left + this.Width
            };
            tmp.Show();
            return;
#else
            if (debugKeySequ[debugSequPos] == key) {
                debugSequPos++;
                if (debugSequPos == debugKeySequ.Length) {
                    tmp = new DebugWindow();
                    tmp.Show();
                }
            }
            else
                debugSequPos = 0;
#endif
        }
        private void MainWindowInstance_KeyDown(object sender, KeyEventArgs e) {
            if (sender is MainWindow) {
                if (debugKeySequ.Contains(e.Key)) {
                    CheckDebugKeySequ(e.Key);
                    return;
                }
                switch (e.Key) {
                    case Key.F1:
                        MessageBox.Show("what the hell");
                        break;
                    case Key.Escape:
                        Close();
                        break;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (sender is Button button) {
                pinned = !pinned;
                button.Content = pinned ? "\uE840" : "\uE718";
                this.Topmost = pinned;
            }
        }

        public new void Close() {
            base.Close();
            Application.Current.MainWindow = null;
        }

        private void Button_Click_NextSession(object sender, RoutedEventArgs e) {
            //ToDo: Not implemented because I don't know how to change currentSession
        }

        private void MainWindowInstance_LostFocus(object sender, RoutedEventArgs e) {
            Close();
        }

        private void MainWindowInstance_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Button_Click_PrevSession(object sender, RoutedEventArgs e) {
            //ToDo: Not implemented because I don't know how to change currentSession
        }

        #endregion Events
    }
}