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
namespace WMCP.Libs {
    public static class Inputs {
        static readonly WindowsInput.InputSimulator inputSimulator = new WindowsInput.InputSimulator();
        public static uint SendPlayPauseKey() {
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MEDIA_PLAY_PAUSE);
            return 0;
        }
        
        public static uint SendNextKey() {
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MEDIA_NEXT_TRACK);
            return 0;
        }

        public static uint SendPrevKey() {
            inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.MEDIA_PREV_TRACK);
            return 0;
        }
    }
}
