using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /* https://learn.microsoft.com/en-us/windows/win32/menurc/using-carets */

    public class WindowsCaretHandler : PlessCaretHandler
    {
        public WindowsCaretHandler(Control control, int width, int height)
            : base(control, width, height)
        {
        }

        /// <inheritdoc/>
        protected override void Changed()
        {
        }

        private static class Win32
        {
            [DllImportAttribute("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CreateCaret(IntPtr handle, IntPtr hBitmap, int nWidth, int nHeight);

            [DllImportAttribute("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool DestroyCaret();

            [DllImportAttribute("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetCaretPos(int x, int y);

            [DllImportAttribute("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool ShowCaret(IntPtr handle);
        }
    }
}
