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

namespace WMCP.Libs {
    public class Controller : IDisposable {
        private readonly Model model;
        #region Constructor and destructor
        /// <summary>
        /// private constructor with non input parameter,
        /// preventing usage of default constructor
        /// </summary>
        private Controller() { }
        private Controller(Libs.Model model) {
            this.model = model;
        }
        public static Controller BuildControllerAsync(Libs.Model model) {
            if (model == null) throw new ModelCannotBeNull();
            var c = new Controller(model);
            return c;
        }
        public void Dispose() { }
        #endregion Constructor and destructor

        # region Custom function wrapper
        public async void Play(int index = 0) {
            await model.PlayPauseTrack(index);
        }
        public async void Pause(int index = 0) {
            await model.PlayPauseTrack(index);
        }
        public async void Previous(int index = 0) {
            await model.PreviousTrack(index);
        }
        public async void Next(int index = 0) {
            await model.NextTrack(index);
        }
        public async void PlayPause(int index = 0) {
            await model.PlayPauseTrack(index);
        }
        public void NextSession() {
            model.NextSession();
        }
        #endregion Custom function wrapper
    }
    public class ModelCannotBeNull : Exception {
        public ModelCannotBeNull() { }
    }
}
