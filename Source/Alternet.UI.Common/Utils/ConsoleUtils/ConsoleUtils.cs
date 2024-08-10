using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if(customConsole is null)
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
                App.AddIdleTask(() =>
                {
                    var s = e.Message?.TrimEndEol();

                    if (string.IsNullOrWhiteSpace(s))
                        return;

                    App.Log($"{prefix}{s}");
                });
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