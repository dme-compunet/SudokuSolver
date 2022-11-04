using System.Runtime.InteropServices;

namespace Compunet.Platform.PInvoke
{
    public static unsafe class User32
    {
        [DllImport("user32.dll")]
        public static extern unsafe bool GetCursorPos(POINT* lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint MonitorFromPoint(POINT pt, MonitorOptions dwFlags);

        [DllImport("user32.dll")]
        public static extern unsafe bool GetMonitorInfo(nint hMonitor, MONITORINFO* lpmi);

        #region Enums

        public enum MonitorOptions : uint
        {
            MONITOR_DEFAULTTONULL = 0x00000000,
            MONITOR_DEFAULTTOPRIMARY = 0x00000001,
            MONITOR_DEFAULTTONEAREST = 0x00000002
        }

        #endregion

        #region Structs

        public struct POINT
        {
            public int X, Y;
        }

        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        public struct MINMAXINFO
        {
            public POINT ptReserved, ptMaxSize, ptMaxPosition, ptMinTrackSize, ptMaxTrackSize;
        };

        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }

        #endregion
    }
}
