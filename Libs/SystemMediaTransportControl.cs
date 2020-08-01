using System;
//using System.Collections.Generic;
//using System.Text;
using Windows.Media;
using System.Runtime.InteropServices;

namespace WMCP.Libs {
    public class SystemMediaTransportControl {
        private delegate IntPtr GetAlbumArtDelegate([MarshalAs(UnmanagedType.LPWStr)] string filename, [MarshalAs(UnmanagedType.LPWStr)] string type);

        private GetAlbumArtDelegate GetAlbumArtFunc;
        public MediaInfo mMCPS;


        private static SystemMediaTransportControls player;
        private static SystemMediaTransportControlsDisplayUpdater updater;

        /// <summary>
        /// Configure 
        /// </summary>
        public virtual void Config() {
            System.Windows.MessageBox.Show("System media transport control configuration options.",
                "System media configuration",
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        public SystemMediaTransportControl() {
            Initialize();
        }

        public void Initialize() {
#pragma warning disable CS0618
            player = Windows.Media.Playback.BackgroundMediaPlayer.Current.SystemMediaTransportControls;
            //Windows.Media.Playback.MediaPlayer;
#pragma warning restore CS0618
            player.ButtonPressed += Player_ButtonPressed;
            updater = player.DisplayUpdater;

            player.IsPlayEnabled = true;
            player.IsPauseEnabled = true;
            player.IsStopEnabled = true;
            player.IsPreviousEnabled = true;
            player.IsNextEnabled = true;

            player.PropertyChanged += Player_PropertyChanged;

            updater.Update();
        }

        private void Player_PropertyChanged(SystemMediaTransportControls sender, SystemMediaTransportControlsPropertyChangedEventArgs args) {
            throw new NotImplementedException();
        }

        private void Player_ButtonPressed(SystemMediaTransportControls sender, SystemMediaTransportControlsButtonPressedEventArgs args) {
            System.Windows.MessageBox.Show("btn pressed");
        }

        public async void Update() {
            System.Windows.MessageBox.Show(player.PlaybackStatus.ToString());
            //Console.WriteLine("123");
            //Console.WriteLine(player.PlaybackStatus.ToString());


        }
    }
}
