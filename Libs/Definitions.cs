// Definitions for constants


namespace WMCP.Libs {
    public static class Constants {
#if (isX64)
    public const string DLLPATH = @"..\..\..\WMCPLib64.dll";
#else
        public const string DLLPATH = @"..\..\..\WMCPLib.dll";
#endif
    }
}