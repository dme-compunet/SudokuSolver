using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using static Compunet.Platform.PInvoke.User32;

namespace Compunet.Platform
{
    public abstract unsafe class BasePlatformWindow : Window
    {
        private bool mRestoreIfMove;

        protected override void OnSourceInitialized(EventArgs e)
        {
            nint mWindowHandle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(mWindowHandle).AddHook(new HwndSourceHook(WindowProc));

            base.OnSourceInitialized(e);
        }

        [DebuggerStepThrough]
        private static nint WindowProc(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(nint hwnd, nint lParam)
        {
            POINT* lMousePosition = stackalloc POINT[1];
            GetCursorPos(lMousePosition);

            nint lPrimaryScreen = MonitorFromPoint(new POINT(), MonitorOptions.MONITOR_DEFAULTTOPRIMARY);

            MONITORINFO* lPrimaryScreenInfo = stackalloc MONITORINFO[1];
            lPrimaryScreenInfo->cbSize = sizeof(MONITORINFO);

            if (GetMonitorInfo(lPrimaryScreen, lPrimaryScreenInfo) == false)
            {
                return;
            }

            nint lCurrentScreen = MonitorFromPoint(*lMousePosition, MonitorOptions.MONITOR_DEFAULTTONEAREST);

            MINMAXINFO lMmi = *(MINMAXINFO*)lParam;

            if (lPrimaryScreen.Equals(lCurrentScreen) == true)
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo->rcWork.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo->rcWork.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo->rcWork.Right - lPrimaryScreenInfo->rcWork.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo->rcWork.Bottom - lPrimaryScreenInfo->rcWork.Top;
            }
            else
            {
                lMmi.ptMaxPosition.X = lPrimaryScreenInfo->rcMonitor.Left;
                lMmi.ptMaxPosition.Y = lPrimaryScreenInfo->rcMonitor.Top;
                lMmi.ptMaxSize.X = lPrimaryScreenInfo->rcMonitor.Right - lPrimaryScreenInfo->rcMonitor.Left;
                lMmi.ptMaxSize.Y = lPrimaryScreenInfo->rcMonitor.Bottom - lPrimaryScreenInfo->rcMonitor.Top;
            }

            *(MINMAXINFO*)lParam = lMmi;
        }

        private void SwitchWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;

                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
            }
        }

        private void RctHeader_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip)
                {
                    SwitchWindowState();
                }

                return;
            }

            else if (WindowState == WindowState.Maximized)
            {
                mRestoreIfMove = true;
                return;
            }

            DragMove();
        }

        private void RctHeader_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mRestoreIfMove = false;
        }

        private void RctHeader_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (mRestoreIfMove)
            {
                mRestoreIfMove = false;

                double percentHorizontal = e.GetPosition(this).X / ActualWidth;
                double targetHorizontal = RestoreBounds.Width * percentHorizontal;

                double percentVertical = e.GetPosition(this).Y / ActualHeight;
                double targetVertical = RestoreBounds.Height * percentVertical;

                WindowState = WindowState.Normal;

                POINT* lMousePosition = stackalloc POINT[1];

                GetCursorPos(lMousePosition);

                Left = lMousePosition->X - targetHorizontal;
                Top = lMousePosition->Y - targetVertical;

                DragMove();
            }
        }
    }
}
