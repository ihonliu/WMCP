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
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media.Control;

#region Type aliases
using SessionManager = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionManager;
using Session = Windows.Media.Control.GlobalSystemMediaTransportControlsSession;

using EventMediaPropertiesChange = Windows.Foundation.TypedEventHandler<
    Windows.Media.Control.GlobalSystemMediaTransportControlsSession,
    Windows.Media.Control.MediaPropertiesChangedEventArgs>;
using EventPlaybackInfoChange = Windows.Foundation.TypedEventHandler<
    Windows.Media.Control.GlobalSystemMediaTransportControlsSession,
    Windows.Media.Control.PlaybackInfoChangedEventArgs>;
using EventTimelinePropertiesChange = Windows.Foundation.TypedEventHandler<
    Windows.Media.Control.GlobalSystemMediaTransportControlsSession,
    Windows.Media.Control.TimelinePropertiesChangedEventArgs>;
using EventSessionsChange = Windows.Foundation.TypedEventHandler<
    Windows.Media.Control.GlobalSystemMediaTransportControlsSessionManager,
    Windows.Media.Control.SessionsChangedEventArgs>;

using SessionsChangedEventArgs = Windows.Media.Control.SessionsChangedEventArgs;

using MediaProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionMediaProperties;
using PlaybackInfo = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackInfo;
using TimelineProperties = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionTimelineProperties;

using PlaybackStatus = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackStatus;
using PlaybackControls = Windows.Media.Control.GlobalSystemMediaTransportControlsSessionPlaybackControls;
#endregion

namespace WMCP.Libs {
    public class Timeline {

        #region Fields and properties
        private TimeSpan start;
        private TimeSpan end;
        private TimeSpan position;

        public int Percentage {
            get {
                if (start == TimeSpan.Zero && end == TimeSpan.Zero) {
                    return 100;
                }
                else {
                    return ((end - start) > TimeSpan.Zero) ? (int)((position - start) * 100 / (end - start)) : 0;
                }
            }
        }
        #endregion fields and properties

        #region Constructor
        public Timeline(TimeSpan start, TimeSpan end, TimeSpan position) {
            if (ValidData(start, end, position)) {
                this.start = start;
                this.end = end;
                this.position = position;
            }
        }
        public Timeline(Session session) {
            var tp = session.GetTimelineProperties();
            position = tp.Position;
            start = tp.StartTime;
            end = tp.EndTime;
        }
        public Timeline() {
            position = TimeSpan.Zero;
            start = TimeSpan.Zero;
            end = TimeSpan.Zero;
        }
        #endregion Constructor

        #region methods
#pragma warning disable IDE0051 // Remove unused private members
        bool ValidData() {

            bool result = true;
            result &= this.start <= this.end;
            result &= this.position <= this.end;
            result &= this.position >= this.start;
            return result;
        }
#pragma warning restore IDE0051 // Remove unused private members
        bool ValidData(TimeSpan start, TimeSpan end, TimeSpan position) {
            bool result = true;
            result &= start < end;
            result &= position < end;
            result &= position > start;
            return result;
        }
        public void AssignFromSession(Session session) {
            var tp = session.GetTimelineProperties();
            position = tp.Position;
            start = tp.StartTime;
            end = tp.EndTime;
        }
        public void Clear() {
            position = TimeSpan.Zero;
            start = TimeSpan.Zero;
            end = TimeSpan.Zero;
        }
        #endregion methods
    }
    /// <summary>
    /// Session update args
    /// </summary>
    public class SessionUpdatedEventArgs {
        public SessionUpdatedEventArgs() { }
    }
    /// <summary>
    /// Data model
    /// </summary>
    public class Model {
        #region Member and properties
        private readonly SessionManager sessionManager;
        private IReadOnlyList<Session> sessionList;
        private Session currentSession;
        public Timeline Time_line { get; } = new Timeline();
        public MediaProperties Media_Properties { get; private set; }

        public bool Playing { get; private set; }

        public int SessionCount { get { return (sessionList != null) ? sessionList.Count : 0; } }
        #endregion Member and properties

        #region Constructor
        // Hide constructor
        private Model() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sessionManager"></param>
        public Model(SessionManager sessionManager) {
            this.sessionManager = sessionManager ?? throw new Libs.SessionManagerNotNull();

            this.sessionManager.SessionsChanged += SessionManager_SessionsChanged;
            this.sessionManager.CurrentSessionChanged += SessionManager_CurrentSessionChanged;

            UpdateSessionList();
            UpdateCurrentSession();
            SessionManager_CurrentSessionChanged(null, null);
        }
        /// <summary>
        /// Factory builder for Model
        /// </summary>
        /// <returns></returns>
        public static async Task<Model> BuildModel() {
            var SsnMgr = await SessionManager.RequestAsync();
            if (SsnMgr != null) return new Model(SsnMgr);
            else return null;
        }
        #endregion Constructor

        #region Event
        /// <summary>
        /// Session list changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SessionManager_SessionsChanged(SessionManager sender, SessionsChangedEventArgs args) {
            UpdateSessionList();
        }
        private void SessionManager_CurrentSessionChanged(SessionManager sender, CurrentSessionChangedEventArgs args) {
            Refresh();
        }
        /// <summary>
        /// Bind event to session
        /// </summary>
        /// <param name="session">input session</param>
        private void BindSession(Session session) {
            if (session != null) {
                session.MediaPropertiesChanged += MediaPropertiesChange;
                session.PlaybackInfoChanged += PlaybackInfoChange;
                session.PlaybackInfoChanged += PlaybackInfo_Change;
                session.TimelinePropertiesChanged += TimelinePropertiesChange;
            }
        }
        /// <summary>
        /// Unbind event from session
        /// </summary>
        /// <param name="session">input session</param>
        private void UnbindSession(Session session) {
            if (session != null) {
                session.MediaPropertiesChanged -= MediaPropertiesChange;
                session.PlaybackInfoChanged -= PlaybackInfoChange;
                session.PlaybackInfoChanged -= PlaybackInfo_Change;
                session.TimelinePropertiesChanged -= TimelinePropertiesChange;
            }
        }
        /// <summary>
        /// Unbind a dedicated event from all sessoins in session list
        /// </summary>
        /// <param name="e">MediaPropertiesChange event</param>
        private void UnbindEvent(EventMediaPropertiesChange e) {
            if (e == null) return;
            foreach (Session s in sessionList) {
                s.MediaPropertiesChanged -= e;
            }
        }
        private void UnbindEvent(EventPlaybackInfoChange e) {
            if (e == null) return;
            foreach (Session s in sessionList) {
                s.PlaybackInfoChanged -= e;
            }
        }
        private void UnbindEvent(EventTimelinePropertiesChange e) {
            if (e == null) return;
            foreach (Session s in sessionList) {
                s.TimelinePropertiesChanged -= e;
            }
        }
        private EventMediaPropertiesChange mMediaPropertiesChange = null;
        private EventPlaybackInfoChange mPlaybackInfoChange = null;
        private EventTimelinePropertiesChange mTimelinePropertiesChange = null;

        public EventMediaPropertiesChange MediaPropertiesChange {
            get { return mMediaPropertiesChange; }
            set { UnbindEvent(mMediaPropertiesChange); mMediaPropertiesChange = value; if (currentSession != null) currentSession.MediaPropertiesChanged += mMediaPropertiesChange; }
        }
        public EventPlaybackInfoChange PlaybackInfoChange {
            get { return mPlaybackInfoChange; }
            set { UnbindEvent(mPlaybackInfoChange); mPlaybackInfoChange = value; if (currentSession != null) currentSession.PlaybackInfoChanged += mPlaybackInfoChange; }
        }
        public EventTimelinePropertiesChange TimelinePropertiesChange {
            get { return mTimelinePropertiesChange; }
            set { UnbindEvent(mTimelinePropertiesChange); mTimelinePropertiesChange = value; if (currentSession != null) currentSession.TimelinePropertiesChanged += mTimelinePropertiesChange; }
        }
        private void ManuallyTriggerAllEvent() {
            PlaybackInfo_Change(null, null);
            MediaPropertiesChange?.Invoke(null, null);
            PlaybackInfoChange?.Invoke(null, null);
            TimelinePropertiesChange?.Invoke(null, null);
        }
        public void PlaybackInfo_Change(Session session, PlaybackInfoChangedEventArgs e) {
            RefreshPlayingState();
        }
        #endregion Event

        #region Refresh and update methods
        private void RefreshPlayingState() {
            Playing = (currentSession != null) && (currentSession.GetPlaybackInfo().PlaybackStatus == PlaybackStatus.Playing);
        }

        public void UpdateSessionList() {
            sessionList = sessionManager.GetSessions();
        }
        public void UpdateCurrentSession() {
            currentSession = this.sessionManager.GetCurrentSession();
            if (currentSession != null)
                Time_line.AssignFromSession(currentSession);
            else
                Time_line.Clear();
        }
        public async Task<MediaProperties> UpdateLocalMediaProperties(Session session = null) {
            if (session == null) {
                if (currentSession != null)
                    return await UpdateLocalMediaProperties(currentSession);
                else return null;
            }
            try {
                Media_Properties = await session.TryGetMediaPropertiesAsync();
            }
            catch {
                return null;
            }

            return Media_Properties;
        }
        /// <summary>
        /// Refresh all members and trigger all events
        /// </summary>
        public async void Refresh() {
            UpdateSessionList();
            UpdateCurrentSession();
            foreach (Session s in sessionList) {
                UnbindSession(s);
            }
            await UpdateLocalMediaProperties(currentSession);
            BindSession(currentSession);
            ManuallyTriggerAllEvent();
        }
        #endregion Refresh and update methods

        #region Play control
        public async Task<bool> NextTrack(int index = 0) {
            bool result = false;
            try {
                if (((uint)index) < SessionCount) {
                    if (index == 0) result = await currentSession.TrySkipNextAsync();
                    else result = await sessionList[index].TrySkipNextAsync();
                }
            }
            catch { }
            return result;
        }
        public async Task<bool> PreviousTrack(int index = 0) {
            bool result = false;
            try {
                if (((uint)index) < SessionCount) {
                    if (index == 0) result = await currentSession.TrySkipPreviousAsync();
                    else result = await sessionList[index].TrySkipPreviousAsync();
                }
            }
            catch { }
            return result;
        }
        public async Task<bool> PlayPauseTrack(int index = 0) {
            bool result = false;
            try {
                if (((uint)index) < SessionCount) {
                    if (index == 0) result = await currentSession.TryTogglePlayPauseAsync();
                    else result = await sessionList[index].TryTogglePlayPauseAsync();
                }
            }
            catch { }
            return result;
        }
        public async Task<bool> PauseTrack(int index = 0) {
            bool result = false;
            try {
                if (((uint)index) < SessionCount) {
                    if (index == 0) result = await currentSession.TryPauseAsync();
                    else result = await sessionList[index].TryPauseAsync();
                }
            }
            catch { }
            return result;
        }
        public async Task<bool> PlayTrack(int index = 0) {
            bool result = false;
            try {
                if (((uint)index) < SessionCount) {
                    if (index == 0) result = await currentSession.TryPlayAsync();
                    else result = await sessionList[index].TryPlayAsync();
                }
            }
            catch { }
            return result;
        }
        public void NextSession() {
            //ToDo: Not implemented because I don't know which API I can invoke to change current session
            //sessionList.
        }
        #endregion Play control

    }
    /// <summary>
    /// Expception session manager not null
    /// this exception should be throwed when Session manager is null
    /// </summary>
    public class SessionManagerNotNull : Exception {
        public SessionManagerNotNull() { }
    }
}
