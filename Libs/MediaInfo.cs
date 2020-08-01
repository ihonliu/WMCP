// This module is copied from ***
// ToDo: Finishize this comment
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;

using MediaProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties;
using PlaybackInfo = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackInfo;
using TimelineProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionTimelineProperties;

using PlaybackStatus = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackStatus;
using PlaybackControls = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackControls;


namespace WMCP.Libs {
    public class MediaInfo {

        private readonly Mutex writeMutex = new Mutex();
        

        public string Title { get; private set; }
        public string Subtitle { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public string AlbumArtist { get; private set; }
        public string AlbumTitle { get; private set; }
        public int AlbumTrackCount { get; private set; }
        public int TrackNumber { get; private set; }
        public System.Collections.Generic.IReadOnlyList<string> Genres { get; private set; }
        public bool Playing { get { return PlayStatus == PlaybackStatus.Playing; } }
        public IRandomAccessStreamReference Thumbnail { get; private set; }

        //PlaybackInfo
        public Windows.Media.MediaPlaybackAutoRepeatMode? AutoRepeatMode { get; private set; }
        public string AutoRepeatModeAsString { get { return this.AutoRepeatMode.ToString(); } }
        public PlaybackControls Controls { get; private set; }
        public bool? IsShuffleActive { get; private set; }
        public double? PlaybackRate { get; private set; }
        public PlaybackStatus PlayStatus { get; private set; }
        public string PlayStatusAsString { get { return PlayStatus.ToString(); } }
        public Windows.Media.MediaPlaybackType? PlaybackType { get; private set; }

        //Timeline properties 
        public TimeSpan EndTime { get; private set; }
        public DateTimeOffset LastUpdatedTime { get; private set; }
        public TimeSpan MaxSeekTime { get; private set; }
        public TimeSpan MinSeekTime { get; private set; }
        public TimeSpan Position { get; private set; }
        public TimeSpan StartTime { get; private set; }

        // data setter
        public bool assignFromMediaProperties(MediaProperties inParam) {
            writeMutex.WaitOne();
            this.AlbumArtist = inParam.AlbumArtist;
            this.AlbumTitle = inParam.AlbumTitle;
            this.AlbumTrackCount = inParam.AlbumTrackCount;
            this.Artist = inParam.Artist;
            this.Genres = inParam.Genres;
            this.PlaybackType = inParam.PlaybackType;
            this.Subtitle = inParam.Subtitle;
            this.Thumbnail = inParam.Thumbnail;
            this.Title = inParam.Title;
            this.TrackNumber = inParam.TrackNumber;
            writeMutex.ReleaseMutex();
            // ToDo : add timeout processing
            return true;
        }
        public bool assignFromPlaybackInfo(PlaybackInfo inParam) {
            writeMutex.WaitOne();
            this.AutoRepeatMode = inParam.AutoRepeatMode;
            this.Controls = inParam.Controls;
            this.IsShuffleActive = inParam.IsShuffleActive;
            this.PlaybackRate = inParam.PlaybackRate;
            this.PlayStatus = inParam.PlaybackStatus;
            this.PlaybackType = inParam.PlaybackType;
            writeMutex.ReleaseMutex();
            // ToDo : add timeout processing
            return true;
        }
        public bool assignFromTimelineProperties(TimelineProperties inParam) {
            writeMutex.WaitOne();
            this.EndTime = inParam.EndTime;
            this.LastUpdatedTime = inParam.LastUpdatedTime;
            this.MaxSeekTime = inParam.MaxSeekTime;
            this.MinSeekTime = inParam.MinSeekTime;
            this.Position = inParam.Position;
            this.StartTime = inParam.StartTime;
            writeMutex.ReleaseMutex();
            // ToDo : add timeout processing
            return true;
        }

        public override string ToString() {
            string result = "";
            result += string.Format("{0}: {1}\n", nameof(Title), Title);
            result += string.Format("{0}: {1}\n", nameof(Subtitle), Subtitle);
            result += string.Format("{0}: {1}\n", nameof(Artist), Artist);
            result += string.Format("{0}: {1}\n", nameof(Album), Album);
            result += string.Format("{0}: {1}\n", nameof(AlbumArtist), AlbumArtist);
            result += string.Format("{0}: {1}\n", nameof(AlbumTitle), AlbumTitle);
            result += string.Format("{0}: {1}\n", nameof(AlbumTrackCount), AlbumTrackCount);
            result += string.Format("{0}: {1}\n", nameof(TrackNumber), TrackNumber);
            result += string.Format("{0}: {1}\n", nameof(Genres), Genres);
            result += string.Format("{0}: {1}\n", nameof(Playing), Playing);
            result += string.Format("{0}: {1}\n", nameof(Thumbnail), Thumbnail);
            result += string.Format("{0}: {1}\n", nameof(AutoRepeatMode), AutoRepeatMode);
            result += string.Format("{0}: {1}\n", nameof(AutoRepeatModeAsString), AutoRepeatModeAsString);
            result += string.Format("{0}: {1}\n", nameof(Controls), Controls);
            result += string.Format("{0}: {1}\n", nameof(IsShuffleActive), IsShuffleActive);
            result += string.Format("{0}: {1}\n", nameof(PlaybackRate), PlaybackRate);
            result += string.Format("{0}: {1}\n", nameof(PlayStatus), PlayStatus);
            result += string.Format("{0}: {1}\n", nameof(PlayStatusAsString), PlayStatusAsString);
            result += string.Format("{0}: {1}\n", nameof(PlaybackType), PlaybackType);
            result += string.Format("{0}: {1}\n", nameof(EndTime), EndTime);
            result += string.Format("{0}: {1}\n", nameof(LastUpdatedTime), LastUpdatedTime);
            result += string.Format("{0}: {1}\n", nameof(MaxSeekTime), MaxSeekTime);
            result += string.Format("{0}: {1}\n", nameof(MinSeekTime), MinSeekTime);
            result += string.Format("{0}: {1}\n", nameof(Position), Position);
            result += string.Format("{0}: {1}\n", nameof(StartTime), StartTime);

            //result += String.Format("{0}:{1}\n", nameof(Title), Title);
            return result;
        }
    }
}
