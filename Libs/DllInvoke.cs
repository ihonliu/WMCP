using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WMCP.Libs {
    /// <summary>
    /// DllInvoke will invoke dll methods
    /// </summary>
    public static class DllInvoke {
        //private const string DllFilePath = @"WMCPLib.dll";
        private const string DllFilePath = Constants.DLLPATH;

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static int test(int number);
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static uint pressPlayPauseKey();
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static uint pressNextKey();
        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static uint pressPrevKey();
        //Windows.Media.MediaControl.dll
        //SystemMediaTransportControls 

        //[DllImport("winmm.dll", EntryPoint = "waveOutGetVolume")]
        //public static extern void GetWaveVolume(IntPtr devicehandle, out int Volume);

        // test method
        public static int Test(int number) {
            return test(number);
        }

        public static uint SendPlayPauseKey() {
            return pressPlayPauseKey();
        }

        public static uint SendNextKey() {
            return pressNextKey();
        }

        public static uint SendPrevKey() {
            return pressPrevKey();
        }

    }
}
