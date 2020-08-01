using System;
using System.Windows;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Windows.Media.Control;

// type alias section

using SessionMgr = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionManager;
using Session = Windows.Media.Control.GlobalSystemMediaTransportControlsSession;
using EventMediaPropertiesChange = Windows.Foundation.TypedEventHandler<Windows.Media.Control.GlobalSystemMediaTransportControlsSession, Windows.Media.Control.MediaPropertiesChangedEventArgs>;
using EvenPlaybackInfoChange = Windows.Foundation.TypedEventHandler<Windows.Media.Control.GlobalSystemMediaTransportControlsSession, Windows.Media.Control.PlaybackInfoChangedEventArgs>;
using EventTimelinePropertiesChange = Windows.Foundation.TypedEventHandler<Windows.Media.Control.GlobalSystemMediaTransportControlsSession, Windows.Media.Control.TimelinePropertiesChangedEventArgs>;
using SessionPlaybackInfo = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackInfo;
using SessionTimelineProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionTimelineProperties;
using SessionMediaProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties;


namespace WMCP.Libs {
    public class Controller : IDisposable {
        private Thread worker;

        public List<EventMediaPropertiesChange> MediaPropertiesChangedEvents { get; private set; } = new List<EventMediaPropertiesChange>();
        public List<EvenPlaybackInfoChange> PlaybackInfoChangedEvents { get; private set; } = new List<EvenPlaybackInfoChange>();
        public List<EventTimelinePropertiesChange> TimelinePropertiesChangedEvents { get; private set; } = new List<EventTimelinePropertiesChange>();
        private void MediaPropertiesChange(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs e) {
            updateMediaProperites();
            foreach (EventMediaPropertiesChange evnt in MediaPropertiesChangedEvents) {
                evnt?.Invoke(sender, e);
            }
        }

        private void PlaybackInfoChange(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs e) {
            updatePlaybackInfo();
            foreach (EvenPlaybackInfoChange evnt in PlaybackInfoChangedEvents) {
                evnt?.Invoke(sender, e);
            }
        }

        private void TimelinePropertiesChange(GlobalSystemMediaTransportControlsSession sender, TimelinePropertiesChangedEventArgs e) {
            updateTimelineProperties();
            foreach (EventTimelinePropertiesChange evnt in TimelinePropertiesChangedEvents) {
                evnt?.Invoke(sender, e);
            }
        }
        // Event binding section
        private void bindSession(Session session) {
            if (session != null) {
                session.MediaPropertiesChanged += MediaPropertiesChange;
                session.PlaybackInfoChanged += PlaybackInfoChange;
                session.TimelinePropertiesChanged += TimelinePropertiesChange;
            }
        }
        private void unbindSession(Session session) {
            if (session != null) {
                session.MediaPropertiesChanged -= MediaPropertiesChange;
                session.PlaybackInfoChanged -= PlaybackInfoChange;
                session.TimelinePropertiesChanged -= TimelinePropertiesChange;
            }
        }
        private void bindAllSessions() {
            if (SsnMgr != null) {
                var ssLst = SsnMgr.GetSessions();
                foreach (Session ss in ssLst)
                    bindSession(ss);
            }
        }
        private void unbindAllSessions() {
            if (SsnMgr != null) {
                var ssLst = SsnMgr.GetSessions();
                foreach (Session ss in ssLst)
                    unbindSession(ss);
            }
        }
        /// <summary>
        /// private constructor with non input parameter,
        /// preventing usage of default constructor
        /// </summary>
        private Controller() {
            bindAllSessions();
        }
        /// <summary>
        /// Factory mode constructor
        /// </summary>
        /// <returns></returns>
        public async static Task<Controller> BuildControllerAsync() {
            var c = new Controller();
            await c.updateSessionManager();
            c.updateSessionList();
            return c;
        }

        public void Dispose() {
            unbindAllSessions();
        }
        // -------------------------------------------------------
        // internal API wrapper
        // -------------------------------------------------------

        private SessionMgr SsnMgr;
        private IReadOnlyList<Session> SsList;
        private int SessionCount {
            get {
                return (SsList != null) ? SsList.Count : 0;
            }
        }
        private async Task<bool> updateSessionManager() {
            try {
                SsnMgr = await SessionMgr.RequestAsync();
                return true;
            }
            catch { return false; }
        }
        private async Task<bool> autoSessionMgr() {
            if (SsnMgr == null) return await updateSessionManager();
            updateSessionList();
            return SsnMgr != null;
        }
        private bool updateSessionList() {
            SsList = SsnMgr?.GetSessions();
            return SsList != null;
        }
        public IReadOnlyList<Session> getSessions { get { return SsList; } }
        private async Task<Session> getSession() {
            try {
                return SsnMgr.GetCurrentSession();
            }
            catch { await autoSessionMgr(); return null; }
        }
        public bool hasSessions() {
            return SsList?.Count != 0;
        }
        private bool hasSession(int index) {
            return (SsList != null) ? (uint)index < SsList.Count : false;
        }
        protected async Task<bool> nextTrack(int index = 0) {
            try { return await getSessions[index].TrySkipNextAsync(); }
            catch { return false; }
        }
        protected async Task<bool> prevTrack(int index = 0) {
            try { return await getSessions[index].TrySkipPreviousAsync(); }
            catch { return false; }
        }
        protected async Task<bool> pauseTrack(int index = 0) {
            try { return await getSessions[index].TryPauseAsync(); }
            catch { return false; }
        }
        protected async Task<bool> playTrack(int index = 0) {
            try { return await getSessions[index].TryPlayAsync(); }
            catch { return false; }
        }
        protected async Task<bool> playPauseTrack(int index = 0) {
            try { return await getSessions[index].TryTogglePlayPauseAsync(); }
            catch { return false; }
        }
        public async Task<SessionMediaProperties> getMediaProperties(int index = 0) {
            try { return await getSessions[index].TryGetMediaPropertiesAsync(); }
            catch { return null; }
        }
        public SessionPlaybackInfo getPlaybackInfo(int index = 0) {
            try {
                return getSessions[index].GetPlaybackInfo();
            }
            catch { return null; }
        }

        public SessionTimelineProperties getTimelineProperties(int index = 0) {
            try { return getSessions[index].GetTimelineProperties(); }
            catch { return null; }
        }
        // --------------------------------------------------------
        // Custom function wrapper
        // --------------------------------------------------------

        /// <summary>
        /// play music
        /// </summary>
        /// <param name="bShowDialog"></param>
        public async void play(bool bShowDialog = false) {
            var result = await playTrack();
            if (bShowDialog) MessageBox.Show(result.ToString());
        }

        public async void pause(bool bShowDialog = false) {
            var result = await playTrack();
            if (bShowDialog) MessageBox.Show(result.ToString());
        }
        public async void prev(bool bShowDialog = false) {
            var result = await prevTrack();
            if (bShowDialog) MessageBox.Show(result.ToString());
        }
        public async void next(bool bShowDialog = false) {
            var result = await nextTrack();
            if (bShowDialog) MessageBox.Show(result.ToString());
        }

        public async void playPause(bool bShowDialog = false) {
            var result = await playPauseTrack();
            if (bShowDialog) MessageBox.Show(result.ToString());
        }
        public List<MediaInfo> mediaInfos { get; } = new List<MediaInfo>();
        public MediaInfo currentMediaInfo {
            get {
                if (mediaInfos.Count > 0) return mediaInfos[0];
                else return null;
            }
        }
        public void updateMediaInfos() {
            mediaInfos.Clear();
            for (int i = 0; i < SessionCount; ++i) {
                mediaInfos.Add(new MediaInfo());
                updateAll(i);
            }
        }

        public async void updateMediaProperites(int index = 0) {
            var r = await getMediaProperties(index);
            if (r != null) {
                mediaInfos[index].assignFromMediaProperties(r);
            }
        }
        public void updatePlaybackInfo(int index = 0) {
            var r = getPlaybackInfo(index);
            if (r != null) {
                mediaInfos[index].assignFromPlaybackInfo(r);
            }
        }
        public void updateTimelineProperties(int index = 0) {
            var r = getTimelineProperties(index);
            if (r != null) {
                mediaInfos[index].assignFromTimelineProperties(r);
            }
        }
        public void updateAll(int index = 0) {
            updateMediaProperites(index);
            updatePlaybackInfo(index);
            updateTimelineProperties(index);
        }
        public override string ToString() {
            string result = "";
            result += String.Format("{0}:{1}\n", "MediaPropertiesChangedEvents", MediaPropertiesChangedEvents.Count);
            result += String.Format("{0}:{1}\n", "PlaybackInfoChangedEvents", PlaybackInfoChangedEvents.Count);
            result += String.Format("{0}:{1}\n", "TimelinePropertiesChangedEvents", TimelinePropertiesChangedEvents.Count);
            foreach (MediaInfo m in mediaInfos) {
                result += m.ToString();
            }
            return result;
        }

    }
}
