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
        private static ConsoleWriter? consoleOut;
        private static ConsoleWriter? consoleError;

        /// <summary>
        /// Binds system console output to <see cref="BaseApplication.Log"/>.
        /// </summary>
        public static void BindConsoleOutput(string prefix = "Output> ")
        {
            if (consoleOut is null)
            {
                consoleOut = CreateWriter(prefix);
                Console.SetOut(consoleOut);
            }
        }

        /// <summary>
        /// Binds console error to <see cref="BaseApplication.Log"/>.
        /// </summary>
        public static void BindConsoleError(string prefix = "Error> ")
        {
            if (consoleError is null)
            {
                consoleError = CreateWriter(prefix);
                Console.SetError(consoleError);
            }
        }

        /// <summary>
        /// Unbinds system console output from <see cref="BaseApplication.Log"/>.
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
        /// Unbinds console error from <see cref="BaseApplication.Log"/>.
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
                Application.AddIdleTask(() =>
                {
                    var s = e.Message?.TrimEndEol();

                    if (string.IsNullOrWhiteSpace(s))
                        return;

                    Application.Log($"{prefix}{s}");
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