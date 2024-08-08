using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Microsoft.Win32.SafeHandles;

namespace Alternet.UI
{
    /// <summary>
    /// Implements custom console for Windows.
    /// </summary>
    internal class CustomWindowsConsole : IDisposable
    {
        private static CustomWindowsConsole? defaultConsole;

        private readonly TwoDimensionalBuffer<CharInfo> screenbuf;
        private readonly Coord screencoord;
        private readonly Coord topleft = new() { X = 0, Y = 0 };

        private SafeFileHandle? consolehandle;
        private SmallRect screenrect;
        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowsConsole"/> class.
        /// </summary>
        public CustomWindowsConsole()
        {
            WindowsUtils.ShowConsole();

            Width = (short)Console.WindowWidth;
            Height = (short)Console.WindowHeight;

            screenbuf = new TwoDimensionalBuffer<CharInfo>(Width, Height);
            screenrect = new SmallRect() { Left = 0, Top = 0, Right = Width, Bottom = Height };
            screencoord = new Coord() { X = Width, Y = Height };
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="CustomWindowsConsole"/> class.
        /// </summary>
        ~CustomWindowsConsole()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        internal enum ConsolePixels
        {
            None = '\0',

            Solid = (char)0xDB,

            ThreeQuarters = (char)0XB2,

            Half = (char)0XB1,

            Quarter = (char)0xB0,
        }

        internal enum ConsoleColor : short
        {
            ForegroundBlack = 0x0000,

            ForegroundDarkBlue = 0x0001,

            ForegroundDarkGreen = 0x0002,

            ForegroundDarkCyan = 0x0003,

            ForegroundDarkRed = 0x0004,

            ForegroundDarkMagenta = 0x0005,

            ForegroundDarkYellow = 0x0006,

            ForegroundGrey = 0x0007,

            ForegroundDarkGrey = 0x0008,

            ForegroundBlue = 0x0009,

            ForegroundGreen = 0x000A,

            ForegroundCyan = 0x000B,

            ForegroundRed = 0x000C,

            ForegroundMagenta = 0x000D,

            ForegroundYellow = 0x000E,

            ForegroundWhite = 0x000F,

            BackgroundBlack = 0x0000,

            BackgroundDarkBlue = 0x0010,

            BackgroundDarkGreen = 0x0020,

            BackgroundDarkCyan = 0x0030,

            BackgroundDarkRed = 0x0040,

            BackgroundDarkMagenta = 0x0050,

            BackgroundDarkYellow = 0x0060,

            BackgroundGrey = 0x0070,

            BackgroundDarkGrey = 0x0080,

            BackgroundBlue = 0x0090,

            BackgroundGreen = 0x00A0,

            BackgroundCyan = 0x00B0,

            BackgroundRed = 0x00C0,

            BackgroundMagenta = 0x00D0,

            BackgroundYellow = 0x00E0,

            BackgroundWhite = 0x00F0,
        }

        public static CustomWindowsConsole Default
        {
            get
            {
                return defaultConsole ??= new();
            }
        }

        /// <summary>
        /// Gets console width.
        /// </summary>
        public short Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets console height.
        /// </summary>
        public short Height
        {
            get;
            private set;
        }

        public virtual void BeginPaint()
        {
            WindowsUtils.ShowConsole();
        }

        public virtual void EndPaint()
        {
            consolehandle ??= CreateFile(
                "CONOUT$",
                0x40000000,
                0x02,
                IntPtr.Zero,
                FileMode.Open,
                0,
                IntPtr.Zero);

            WriteConsoleOutput(consolehandle, screenbuf.Data, screencoord, topleft, ref screenrect);
        }

        /// <summary>
        /// Clears console.
        /// </summary>
        public void Clear()
        {
            Fill(0, 0, Width, Height, (char)ConsolePixels.None, (short)ConsoleColor.BackgroundBlack);
        }

        public void Fill(
            int x,
            int y,
            int width,
            int height,
            char c = (char)ConsolePixels.None,
            short attributes = (short)ConsoleColor.BackgroundBlack)
        {
            for (int xp = x; xp < width; xp++)
            {
                for (int yp = y; yp < height; yp++)
                    SetChar(xp, yp, c, 0);
            }
        }

        public void Print(int x, int y, string text, short attributes = (int)ConsoleColor.ForegroundWhite)
        {
            for (int i = 0; i < text.Length; ++i)
            {
                SetChar(x + i, y, text[i], attributes);
            }
        }

        public void SetChar(int x, int y, char c, short attributes = (short)ConsoleColor.ForegroundWhite)
        {
            var offset = screenbuf.GetOffset(x, y);
            screenbuf.Data[offset].Attributes = attributes;
            screenbuf.Data[offset].Char.UnicodeChar = c;
        }

        public char GetChar(int x, int y)
        {
            return screenbuf.GetData(x, y).Char.UnicodeChar;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects) here.
                }

                consolehandle?.Dispose();

                disposed = true;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "WriteConsoleOutputW")]
        private static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        private struct Coord
        {
            public short X;
            public short Y;

            public Coord(short x, short y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct CharUnion
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
        private struct CharInfo
        {
            [FieldOffset(0)]
            public CharUnion Char;
            [FieldOffset(2)]
            public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
    }
}