using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Methods related to <see cref="Console"/>.
    /// </summary>
    public static class ConsoleUtils
    {
        /// <summary>
        /// Gets or sets default string prefix for console messages.
        /// </summary>
        public static string DefaultOutputPrefix = "Output> ";

        /// <summary>
        /// Gets or sets default string prefix for console error messages.
        /// </summary>
        public static string DefaultErrorPrefix = "Error> ";

        private static BaseConcurrentStack<(ConsoleColor Foreground, ConsoleColor Background)>? consoleColors;

        private static ConsoleWriter? consoleOut;
        private static ConsoleWriter? consoleError;
        private static ICustomConsole? customConsole;
        private static ICustomConsole? dummyConsole;

        /// <summary>
        /// Gets dummy <see cref="ICustomConsole"/> interface provider which does nothing.
        /// </summary>
        public static ICustomConsole DummyConsole
        {
            get
            {
                return dummyConsole ??= new DummyConsole();
            }
        }

        /// <summary>
        /// Gets or sets default <see cref="ICustomConsole"/> interface provider.
        /// </summary>
        public static ICustomConsole CustomConsole
        {
            get
            {
                if (customConsole is null)
                {
                    if (App.IsWindowsOS)
                        customConsole = CustomWindowsConsole.Default;
                    else
                        customConsole = DummyConsole;
                }

                return customConsole;
            }

            set
            {
                customConsole = value;
            }
        }

        /// <summary>
        /// Binds system console output to <see cref="App.Log"/>.
        /// </summary>
        public static void BindConsoleOutput(string? prefix = null)
        {
            prefix ??= DefaultOutputPrefix;

            if (consoleOut is null)
            {
                consoleOut = CreateWriter(prefix);
                Console.SetOut(consoleOut);
            }
        }

        /// <summary>
        /// Binds console error to <see cref="App.Log"/>.
        /// </summary>
        public static void BindConsoleError(string? prefix = null)
        {
            prefix ??= DefaultErrorPrefix;

            if (consoleError is null)
            {
                consoleError = CreateWriter(prefix);
                Console.SetError(consoleError);
            }
        }

        /// <summary>
        /// Pushes console colors to the stack and sets new console colors.
        /// Use <see cref="PopConsoleColors"/> to restore previous colors.
        /// </summary>
        /// <param name="foreground">The foreground color to set. If null, the current foreground color is used.</param>
        /// <param name="background">The background color to set. If null, the current background color is used.</param>
        public static void PushConsoleColors(ConsoleColor? foreground, ConsoleColor? background)
        {
            consoleColors ??= new BaseConcurrentStack<(ConsoleColor Foreground, ConsoleColor Background)>();
            consoleColors.Push((Console.ForegroundColor, Console.BackgroundColor));
            Console.ForegroundColor = foreground ?? Console.ForegroundColor;
            Console.BackgroundColor = background ?? Console.BackgroundColor;
        }

        /// <summary>
        /// Restores console colors pushed with <see cref="PushConsoleColors"/> to the previous state.
        /// </summary>
        public static void PopConsoleColors()
        {
            if (consoleColors is null || consoleColors.Count == 0)
                return;
            var result = consoleColors.TryPop(out var colors);
            if (result)
            {
                Console.ForegroundColor = colors.Foreground;
                Console.BackgroundColor = colors.Background;
            }
        }

        /// <summary>
        /// Converts <see cref="ConsoleColor"/> to <see cref="Color"/>.
        /// </summary>
        /// <param name="consoleColor">The console color to convert.</param>
        /// <returns>The corresponding <see cref="Color"/>.</returns>
        public static Color ToColor(ConsoleColor consoleColor)
        {
            return consoleColor switch
            {
                ConsoleColor.Black => Color.Black,
                ConsoleColor.DarkBlue => Color.DarkBlue,
                ConsoleColor.DarkGreen => Color.DarkGreen,
                ConsoleColor.DarkCyan => Color.DarkCyan,
                ConsoleColor.DarkRed => Color.DarkRed,
                ConsoleColor.DarkMagenta => Color.DarkMagenta,
                ConsoleColor.DarkYellow => Color.Olive,
                ConsoleColor.Gray => Color.Gray,
                ConsoleColor.DarkGray => Color.DarkGray,
                ConsoleColor.Blue => Color.Blue,
                ConsoleColor.Green => Color.Green,
                ConsoleColor.Cyan => Color.Cyan,
                ConsoleColor.Red => Color.Red,
                ConsoleColor.Magenta => Color.Magenta,
                ConsoleColor.Yellow => Color.Yellow,
                ConsoleColor.White => Color.White,
                _ => Color.Transparent,
            };
        }

        /// <summary>
        /// Determines whether the specified <see cref="ConsoleColor"/> is a dark color.
        /// </summary>
        /// <param name="consoleColor">The console color to check.</param>
        /// <returns><c>true</c> if the specified console color is dark; otherwise, <c>false</c>.</returns>
        public static bool IsDarkColor(ConsoleColor consoleColor)
        {
            var color = ToColor(consoleColor);
            return color.IsDark();
        }

        /// <summary>
        /// Pushes console colors to the stack and sets console foreground color to red.
        /// </summary>
        public static void PushErrorConsoleColors()
        {
            if (IsDarkColor(Console.BackgroundColor))
                PushConsoleColors(ConsoleColor.Red, null);
            else
                PushConsoleColors(ConsoleColor.DarkRed, null);
        }

        /// <summary>
        /// Pushes console colors to the stack and sets console foreground color to yellow.
        /// </summary>
        public static void PushWarningConsoleColors()
        {
            if (IsDarkColor(Console.BackgroundColor))
                PushConsoleColors(ConsoleColor.Yellow, null);
            else
                PushConsoleColors(ConsoleColor.DarkYellow, null);
        }

        /// <summary>
        /// Writes the specified message to the console with red foreground color.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void WriteLineError(string? message)
        {
            PushErrorConsoleColors();
            Console.WriteLine(message);
            PopConsoleColors();
        }

        /// <summary>
        /// Writes the specified message to the console with yellow foreground color.
        /// </summary>
        /// <param name="message">The message to write.</param>
        public static void WriteLineWarning(string? message)
        {
            PushWarningConsoleColors();
            Console.WriteLine(message);
            PopConsoleColors();
        }

        /// <summary>
        /// Unbinds system console output from <see cref="App.Log"/>.
        /// </summary>
        public static void UnbindConsoleOutput()
        {
            if (consoleOut is not null)
            {
                var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
                consoleOut.Close();
                consoleOut.Dispose();
                consoleOut = null;
            }
        }

        /// <summary>
        /// Unbinds console error from <see cref="App.Log"/>.
        /// </summary>
        public static void UnbindConsoleError()
        {
            if (consoleError is not null)
            {
                var standardOutput = new StreamWriter(Console.OpenStandardError());
                standardOutput.AutoFlush = true;
                Console.SetError(standardOutput);
                consoleError.Close();
                consoleError.Dispose();
                consoleError = null;
            }
        }

        private static ConsoleWriter CreateWriter(string prefix = "> ")
        {
            var writer = new ConsoleWriter();
            writer.WriteEvent += ConsoleMessageReceived;
            return writer;

            void ConsoleMessageReceived(object? sender, LogMessageEventArgs e)
            {
                var s = e.Message?.TrimEndEol();

                if (string.IsNullOrWhiteSpace(s))
                    return;

                App.Log($"{prefix}{s}");
            }
        }

        internal class ConsoleWriter : TextWriter
        {
            public event EventHandler<LogMessageEventArgs>? WriteEvent;

            public override Encoding Encoding
            {
                get
                {
                    return Encoding.UTF8;
                }
            }

            public override void Write(char[] buffer, int index, int count)
            {
                WriteEvent?.Invoke(
                    this,
                    new LogMessageEventArgs(new string(buffer, index, count)));
                base.Write(buffer, index, count);
            }
        }
    }
}