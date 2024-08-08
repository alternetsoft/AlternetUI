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

        private static SafeFileHandle? consoleHandle;

        private static short ConsoleWidth => (short)Console.WindowWidth;

        private static short ConsoleHeight => (short)Console.WindowHeight;

        private static NativeMethods.Coord ConsoleCursor =>
            new NativeMethods.Coord((short)Console.CursorLeft, (short)Console.CursorTop);

        private static NativeMethods.SmallRect ConsoleWriteRegion =>
            new NativeMethods.SmallRect()
            {
                Left = 0,
                Top = 0,
                Right = ConsoleWidth,
                Bottom = ConsoleHeight,
            };

        private static NativeMethods.Coord ConsoleBufferSize =>
            new NativeMethods.Coord(ConsoleWidth, ConsoleHeight);

        private static NativeMethods.Coord ConsoleBufferCoord =>
            new NativeMethods.Coord(0, 0);

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
        /// Writes text to console.
        /// </summary>
        /// <param name="s">Text to write.</param>
        /// <param name="fg">Foreground color.</param>
        /// <param name="bg">Background color.</param>
        internal static void WriteToConsole(
            string s,
            ConsoleColor fg = ConsoleColor.White,
            ConsoleColor bg = ConsoleColor.Black)
        {
            ShowConsole();

            var consoleBuffer = new NativeMethods.CharInfo[ConsoleWidth * ConsoleHeight];

            consoleHandle ??= NativeMethods.CreateFile(
                   "CONOUT$",
                   0x40000000,
                   2,
                   IntPtr.Zero,
                   FileMode.Open,
                   0,
                   IntPtr.Zero);

            char[] text = s.ToCharArray();

            NativeMethods.Coord cursor = ConsoleCursor;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    cursor.X = 0;
                    cursor.Y++;
                }
                else
                {
                    int index = (cursor.Y * ConsoleWidth) + cursor.X;

                    // Set color
                    // (Crazy heckin bitwise crap, don't touch.)
                    consoleBuffer[index].Attributes = (short)((int)fg | ((int)bg));

                    // Set character
                    consoleBuffer[index].Char.AsciiChar = (byte)text[i];

                    // Increment cursor
                    cursor.X++;
                }

                // Make sure that cursor does not exceed bounds of window
                if (cursor.X >= ConsoleWidth)
                {
                    cursor.X = 0;
                    cursor.Y++;
                }

                if (cursor.Y >= ConsoleHeight)
                {
                    cursor.Y = 0;
                }
            }

            var writeRegion = ConsoleWriteRegion;
            NativeMethods.WriteConsoleOutput(
                consoleHandle,
                consoleBuffer,
                ConsoleBufferSize,
                ConsoleBufferCoord,
                ref writeRegion);

            Console.SetCursorPosition(cursor.X, cursor.Y);
        }

        /// <summary>
        /// Contains native methods.
        /// </summary>
        public static class NativeMethods
        {
            /// <summary>
            /// Allocates console.
            /// </summary>
            /// <returns></returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern int AllocConsole();

            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern int FreeConsole();

            [DllImport("kernel32.dll")]
            internal static extern IntPtr GetConsoleWindow();

            [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            internal static extern SafeFileHandle CreateFile(
                string fileName,
                [MarshalAs(UnmanagedType.U4)] uint fileAccess,
                [MarshalAs(UnmanagedType.U4)] uint fileShare,
                IntPtr securityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
                [MarshalAs(UnmanagedType.U4)] int flags,
                IntPtr template);

            [DllImport("Kernel32.dll", SetLastError = true)]
            internal static extern bool WriteConsoleOutput(
                SafeFileHandle hConsoleOutput,
                CharInfo[] lpBuffer,
                Coord dwBufferSize,
                Coord dwBufferCoord,
                ref SmallRect lpWriteRegion);

            [StructLayout(LayoutKind.Sequential)]
            internal struct Coord
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
            internal struct CharUnion
            {
                [FieldOffset(0)]
                public char UnicodeChar;

                [FieldOffset(0)]
                public byte AsciiChar;
            }

            [StructLayout(LayoutKind.Explicit)]
            internal struct CharInfo
            {
                [FieldOffset(0)]
                public CharUnion Char;

                [FieldOffset(2)]
                public short Attributes;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct SmallRect
            {
                public short Left;
                public short Top;
                public short Right;
                public short Bottom;
            }
        }
    }
}
