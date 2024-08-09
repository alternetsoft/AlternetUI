using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32.SafeHandles;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to Windows operating system.
    /// </summary>
    public static class WindowsUtils
    {
        private static bool consoleAllocated = false;

        /// <summary>
        /// Shows console window on the screen.
        /// </summary>
        public static void ShowConsole()
        {
            if (!consoleAllocated)
            {
                WindowsUtils.NativeMethods.AllocConsole();
                consoleAllocated = true;
            }
        }

        /// <summary>
        /// Contains native methods.
        /// </summary>
        public static class NativeMethods
        {
            /// <summary>
            /// Gets key state.
            /// </summary>
            /// <param name="vKey"></param>
            /// <returns></returns>
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern short GetAsyncKeyState(int vKey);

            /// <summary>
            /// Creates file.
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="fileAccess"></param>
            /// <param name="fileShare"></param>
            /// <param name="securityAttributes"></param>
            /// <param name="creationDisposition"></param>
            /// <param name="flags"></param>
            /// <param name="template"></param>
            /// <returns></returns>
            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern SafeFileHandle CreateFile(
                string fileName,
                [MarshalAs(UnmanagedType.U4)] uint fileAccess,
                [MarshalAs(UnmanagedType.U4)] uint fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                [MarshalAs(UnmanagedType.U4)] int flags,
                IntPtr template);

            /// <summary>
            /// Writes text to console.
            /// </summary>
            /// <param name="hConsoleOutput"></param>
            /// <param name="lpBuffer"></param>
            /// <param name="dwBufferSize"></param>
            /// <param name="dwBufferCoord"></param>
            /// <param name="lpWriteRegion"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "WriteConsoleOutputW")]
            public static extern bool WriteConsoleOutput(
                SafeFileHandle hConsoleOutput,
                ConsoleCharInfo[] lpBuffer,
                SmallPoint dwBufferSize,
                SmallPoint dwBufferCoord,
                ref SmallRect lpWriteRegion);

            /// <summary>
            /// Allocates console.
            /// </summary>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int AllocConsole();

            /// <summary>
            /// Frees console.
            /// </summary>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int FreeConsole();

            /// <summary>
            /// Gets console window.
            /// </summary>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            public static extern IntPtr GetConsoleWindow();

            /// <summary>
            /// Represents X and Y coordinates.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct SmallPoint
            {
                /// <summary>
                /// Get or sets X coordinate.
                /// </summary>
                public short X;

                /// <summary>
                /// Get or sets Y coordinate.
                /// </summary>
                public short Y;

                /// <summary>
                /// Initializes a new instance of the <see cref="SmallPoint"/> struct.
                /// </summary>
                /// <param name="x"></param>
                /// <param name="y"></param>
                public SmallPoint(short x, short y)
                {
                    X = x;
                    Y = y;
                }
            }

            /// <summary>
            /// Represents unicode or ascii character value.
            /// </summary>
            [StructLayout(LayoutKind.Explicit)]
            public struct ConsoleCharUnion
            {
                /// <summary>
                /// Gets or sets unicode character value.
                /// </summary>
                [FieldOffset(0)]
                public char UnicodeChar;

                /// <summary>
                /// Gets or sets ascii character value.
                /// </summary>
                [FieldOffset(0)]
                public byte AsciiChar;
            }

            /// <summary>
            /// Represents console character information.
            /// </summary>
            [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
            public struct ConsoleCharInfo
            {
                /// <summary>
                /// Character value.
                /// </summary>
                [FieldOffset(0)]
                public ConsoleCharUnion Char;

                /// <summary>
                /// Character foreground and background colors.
                /// </summary>
                [FieldOffset(2)]
                public short Attributes;
            }

            /// <summary>
            /// Represents small rectangle.
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct SmallRect
            {
                /// <summary>
                /// Left bound of the rectangle.
                /// </summary>
                public short Left;

                /// <summary>
                /// Top bound of the rectangle.
                /// </summary>
                public short Top;

                /// <summary>
                /// Right bound of the rectangle.
                /// </summary>
                public short Right;

                /// <summary>
                /// Bottom bound of the rectangle.
                /// </summary>
                public short Bottom;
            }
        }
    }
}
