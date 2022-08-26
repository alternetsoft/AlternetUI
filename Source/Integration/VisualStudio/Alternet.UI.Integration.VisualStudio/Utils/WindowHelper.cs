using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Alternet.UI.Integration.VisualStudio.Utils
{
    internal static class WindowHelper
    {
        public static System.Drawing.Point GetWindowLocation(Window window)
        {
            RECT rect;
            GetWindowRect(new WindowInteropHelper(window).Handle, out rect);
            return new System.Drawing.Point(rect.Left, rect.Top);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}