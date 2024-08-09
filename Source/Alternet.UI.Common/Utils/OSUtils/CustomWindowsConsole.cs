using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

using Alternet.Drawing;

using Microsoft.Win32.SafeHandles;

namespace Alternet.UI
{
    /// <summary>
    /// Implements custom console for Windows.
    /// </summary>
    public class CustomWindowsConsole : DisposableObject
    {
        /// <summary>
        /// Gets or sets background color of the new text.
        /// </summary>
        public ConsoleColor BackColor = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets foreground color of the new text.
        /// </summary>
        public ConsoleColor TextColor = ConsoleColor.White;

        private static CustomWindowsConsole? defaultConsole;

        private readonly TwoDimensionalBuffer<WindowsUtils.NativeMethods.ConsoleCharInfo> screenBuf;
        private readonly WindowsUtils.NativeMethods.SmallPoint screenCoord;
        private readonly WindowsUtils.NativeMethods.SmallPoint topLeft = new() { X = 0, Y = 0 };

        private SafeFileHandle? consoleHandle;
        private WindowsUtils.NativeMethods.SmallRect screenRect;
        private int paintCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowsConsole"/> class.
        /// </summary>
        public CustomWindowsConsole()
        {
            WindowsUtils.ShowConsole();

            Width = (short)Console.WindowWidth;
            Height = (short)Console.WindowHeight;

            screenBuf = new TwoDimensionalBuffer<WindowsUtils.NativeMethods.ConsoleCharInfo>(Width, Height);

            screenRect = new WindowsUtils.NativeMethods.SmallRect()
            {
                Left = 0,
                Top = 0,
                Right = Width,
                Bottom = Height,
            };

            screenCoord = new WindowsUtils.NativeMethods.SmallPoint()
            {
                X = Width,
                Y = Height,
            };
        }

        /// <summary>
        /// Gets or sets default <see cref="CustomWindowsConsole"/> object.
        /// </summary>
        public static CustomWindowsConsole Default
        {
            get
            {
                return defaultConsole ??= new();
            }

            set
            {
                defaultConsole = value;
            }
        }

        /// <summary>
        /// Gets screen buffer.
        /// </summary>
        public TwoDimensionalBuffer<WindowsUtils.NativeMethods.ConsoleCharInfo> ScreenBuf => screenBuf;

        /// <summary>
        /// Gets console width.
        /// </summary>
        public virtual short Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets console height.
        /// </summary>
        public virtual short Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Begins console updates.
        /// </summary>
        public virtual void BeginUpdate()
        {
            if (paintCounter == 0)
            {
                WindowsUtils.ShowConsole();
            }

            paintCounter++;
        }

        /// <summary>
        /// Ends console updates.
        /// </summary>
        public virtual void EndUpdate()
        {
            paintCounter--;

            if (paintCounter == 0)
            {
                consoleHandle ??= WindowsUtils.NativeMethods.CreateFile(
                    "CONOUT$",
                    0x40000000,
                    0x02,
                    IntPtr.Zero,
                    FileMode.Open,
                    0,
                    IntPtr.Zero);

                WindowsUtils.NativeMethods.WriteConsoleOutput(
                    consoleHandle,
                    screenBuf.Data,
                    screenCoord,
                    topLeft,
                    ref screenRect);
            }
        }

        /// <summary>
        /// Clears console.
        /// </summary>
        public virtual void Clear()
        {
            BeginUpdate();
            try
            {
                Fill((0, 0, Width, Height), '\0');
                Console.SetCursorPosition(0, 0);
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Fills rectangle region with the character.
        /// </summary>
        /// <param name="rect">Rectangle to fill.</param>
        /// <param name="c">Fill character.</param>
        public virtual void Fill(RectI rect, char c = '\0')
        {
            BeginUpdate();
            try
            {
                for (int xp = rect.X; xp < rect.Width; xp++)
                {
                    for (int yp = rect.Y; yp < rect.Height; yp++)
                        SetChar(xp, yp, c);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Writes an empty line to console. Caret position is changed to the beginning of the next line.
        /// </summary>
        public virtual void WriteLine()
        {
            BeginUpdate();
            try
            {
                PointI cursor = (Console.CursorLeft, Console.CursorTop);
                cursor.X = 0;
                cursor.Y++;
                Console.SetCursorPosition(cursor.X, cursor.Y);
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Writes text to console at the current caret position.
        /// After write operation is performed, caret is moved
        /// to the beginning of the next line.
        /// </summary>
        /// <param name="text">Text to write.</param>
        public virtual void WriteLine(string text)
        {
            Write($"{text}\n");
        }

        /// <summary>
        /// Writes text to console at the current caret position.
        /// After write operation is performed, caret is moved to the position after the text.
        /// </summary>
        /// <param name="text">Text to write.</param>
        public virtual void Write(string text)
        {
            BeginUpdate();
            try
            {
                Fn();
            }
            finally
            {
                EndUpdate();
            }

            void Fn()
            {
                PointI cursor = (Console.CursorLeft, Console.CursorTop);

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '\n')
                    {
                        cursor.X = 0;
                        cursor.Y++;
                    }
                    else
                    {
                        SetChar(cursor.X, cursor.Y, text[i]);
                        cursor.X++;
                    }

                    if (cursor.X >= Width)
                    {
                        cursor.X = 0;
                        cursor.Y++;
                    }

                    if (cursor.Y >= Height)
                    {
                        cursor.Y = 0;
                    }
                }

                Console.SetCursorPosition(cursor.X, cursor.Y);
            }
        }

        /// <summary>
        /// Writes text at the specified position. Caret position is not changed.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="text">Text to write.</param>
        public virtual void Print(int x, int y, string text)
        {
            BeginUpdate();
            try
            {
                for (int i = 0; i < text.Length; ++i)
                {
                    SetChar(x + i, y, text[i]);
                }
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Writes character at the specified position. Caret position is not changed.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="c">Character to write.</param>
        public virtual void SetChar(int x, int y, char c)
        {
            var attributes
                = (int)TextColor | ((int)BackColor << 4);

            var offset = screenBuf.GetOffset(x, y);
            screenBuf.Data[offset].Attributes = (short)attributes;
            screenBuf.Data[offset].Char.UnicodeChar = c;
        }

        /// <summary>
        /// Gets character at the specified position.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <returns></returns>
        public virtual char GetChar(int x, int y)
        {
            return screenBuf.GetData(x, y).Char.UnicodeChar;
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            consoleHandle?.Dispose();
            consoleHandle = null;
        }
    }
}