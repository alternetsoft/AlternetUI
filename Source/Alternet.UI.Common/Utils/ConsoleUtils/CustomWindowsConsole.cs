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
    internal class CustomWindowsConsole : DisposableObject, ICustomConsole
    {
        private static CustomWindowsConsole? defaultConsole;

        private readonly TwoDimensionalBuffer<MswUtils.NativeMethods.ConsoleCharInfo> screenBuf;
        private readonly MswUtils.NativeMethods.SmallPoint screenCoord;
        private readonly MswUtils.NativeMethods.SmallPoint topLeft = new() { X = 0, Y = 0 };

        private SafeFileHandle? consoleHandle;
        private MswUtils.NativeMethods.SmallRect screenRect;
        private int paintCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindowsConsole"/> class.
        /// </summary>
        public CustomWindowsConsole()
        {
            MswUtils.ShowConsole();

            Width = (short)Console.WindowWidth;
            Height = (short)Console.WindowHeight;

            screenBuf = new TwoDimensionalBuffer<MswUtils.NativeMethods.ConsoleCharInfo>(Width, Height);

            screenRect = new MswUtils.NativeMethods.SmallRect()
            {
                Left = 0,
                Top = 0,
                Right = (short)Width,
                Bottom = (short)Height,
            };

            screenCoord = new MswUtils.NativeMethods.SmallPoint()
            {
                X = (short)Width,
                Y = (short)Height,
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
        /// Gets or sets background color of the new text.
        /// </summary>
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets foreground color of the new text.
        /// </summary>
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets screen buffer.
        /// </summary>
        public TwoDimensionalBuffer<MswUtils.NativeMethods.ConsoleCharInfo> ScreenBuf => screenBuf;

        /// <summary>
        /// Gets console width.
        /// </summary>
        public virtual int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets console height.
        /// </summary>
        public virtual int Height
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
                MswUtils.ShowConsole();
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
                consoleHandle ??= MswUtils.NativeMethods.CreateFile(
                    "CONOUT$",
                    0x40000000,
                    0x02,
                    IntPtr.Zero,
                    FileMode.Open,
                    0,
                    IntPtr.Zero);

                MswUtils.NativeMethods.WriteConsoleOutput(
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
                Fill((0, 0, Width, Height), ' ');
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
        public virtual void Fill(RectI rect, char c = ' ')
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