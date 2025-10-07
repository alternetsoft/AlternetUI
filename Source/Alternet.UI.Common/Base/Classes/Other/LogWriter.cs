using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a collection of log writer implementations for writing
    /// log messages to various outputs.
    /// </summary>
    /// <remarks>The <see cref="LogWriter"/> class contains nested implementations of the <see
    /// cref="ILogWriter"/> interface, including <see cref="NullLogWriter"/>,
    /// <see cref="DebugLogWriter"/>, <see cref="ConsoleLogWriter"/> and others.
    /// These implementations allow for logging to different targets, such as the debug
    /// output or the console. Additionally, the class defines common logging behavior
    /// that can be shared across different log writer implementations.</remarks>
    public static class LogWriter
    {
        /// <summary>
        /// Represents a no-op implementation of the <see cref="ILogWriter"/> interface.
        /// </summary>
        /// <remarks>This instance can be used when logging is not required, as it performs
        /// no operations
        /// and discards all log messages. It is useful for scenarios where a logging
        /// dependency is required but no
        /// actual logging should occur.</remarks>
        public static readonly ILogWriter Null = new NullLogWriter();

        private static ILogWriter? current;
        private static ILogWriter debug = new DebugLogWriter();
        private static ILogWriter application = new ApplicationLogWriter();
        private static ILogWriter console = new ConsoleLogWriter();
        private static MultiLogWriter multi = new();
        private static int indentLength = 4;

        static LogWriter()
        {
            multi.Add(() => application);
            current = multi;
        }

        /// <summary>
        /// Represents the default number of spaces used for indentation.
        /// </summary>
        /// <remarks>This field is commonly used to define the standard indentation length in formatting
        /// operations.</remarks>
        public static int IndentLength
        {
            get => indentLength;

            set
            {
                if (value < 0)
                    value = 0;
                if (value == indentLength)
                    return;
                indentLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the current instance of the <see cref="ILogWriter"/> used for logging.
        /// </summary>
        public static ILogWriter Current
        {
            get => current ?? Null;
            set => current = value;
        }

        /// <summary>
        /// Gets or sets the shared instance of <see cref="MultiLogWriter"/> used
        /// for logging operations.
        /// </summary>
        public static MultiLogWriter Multi
        {
            get => multi;
            set
            {
                if (value == null)
                {
                    multi = new MultiLogWriter();
                }
                else
                    multi = value;
            }
        }

        /// <summary>
        /// Gets or sets the log writer used for debug-level logging.
        /// </summary>
        /// <remarks>Assigning a value to this property allows customization of how debug-level log
        /// messages are written. If not set, the default implementation may be used,
        /// depending on the application's
        /// logging configuration.</remarks>
        public static ILogWriter Debug
        {
            get => debug;
            set
            {
                if (value == null)
                    debug = new DebugLogWriter();
                else
                    debug = value;
            }
        }

        /// <summary>
        /// Gets or sets the log writer instance used for application-level logging.
        /// </summary>
        public static ILogWriter Application
        {
            get => application;
            set
            {
                if (value == null)
                    application = new ApplicationLogWriter();
                else
                    application = value;
            }
        }

        /// <summary>
        /// Gets or sets the log writer for console output.
        /// </summary>
        public static ILogWriter Console
        {
            get => console;
            set
            {
                if (value == null)
                    console = new ConsoleLogWriter();
                else
                    console = value;
            }
        }

        /// <summary>
        /// Provides a mechanism to write log messages to multiple <see cref="ILogWriter"/> instances.
        /// </summary>
        /// <remarks>The <see cref="MultiLogWriter"/> class allows you to aggregate multiple log writers
        /// and forward log messages to all of them. This is useful when you
        /// need to log messages to multiple destinations simultaneously,
        /// such as a file, console, or remote logging service.</remarks>
        public class MultiLogWriter : ILogWriter
        {
            private readonly List<Func<ILogWriter>> writers = new();

            /// <summary>
            /// Initializes a new instance of the <see cref="MultiLogWriter"/> class
            /// with the specified log writers.
            /// </summary>
            /// <remarks>This constructor allows you to create a composite log writer that forwards
            /// log messages to multiple underlying log writers. Each writer in
            /// the <paramref name="writers"/> array is
            /// added to the internal collection of log writers.</remarks>
            /// <param name="writers">An array of <see cref="ILogWriter"/> instances
            /// to which log messages will be forwarded. Cannot be null.</param>
            public MultiLogWriter(params ILogWriter[] writers)
            {
                foreach (var writer in writers)
                    Add(writer);
            }

            /// <summary>
            /// Gets the collection of log writers used to process log entries.
            /// </summary>
            /// <remarks>Use this property to inspect the current log writers.
            /// To modify the collection, update the underlying configuration or use the appropriate
            /// methods to add or remove writers.</remarks>
            public IList<Func<ILogWriter>> Writers => writers;

            /// <summary>
            /// Adds a log writer to the collection of writers.
            /// </summary>
            /// <param name="writer">The <see cref="ILogWriter"/> instance to add.
            /// Cannot be <see langword="null"/>.</param>
            /// <exception cref="ArgumentNullException">Thrown if <paramref name="writer"/>
            /// is <see langword="null"/>.</exception>
            public void Add(ILogWriter writer)
            {
                if (writer == null)
                    throw new ArgumentNullException(nameof(writer));
                writers.Add(() => writer);
            }

            /// <summary>
            /// Adds a log writer factory function to the collection of log writers.
            /// </summary>
            /// <param name="writerFunc">A function that returns an instance
            /// of <see cref="ILogWriter"/>.  This function cannot be <see
            /// langword="null"/>.</param>
            /// <exception cref="ArgumentNullException">Thrown
            /// if <paramref name="writerFunc"/> is <see langword="null"/>.</exception>
            public void Add(Func<ILogWriter> writerFunc)
            {
                if (writerFunc == null)
                    throw new ArgumentNullException(nameof(writerFunc));
                writers.Add(writerFunc);
            }

            /// <inheritdoc/>
            public void Indent()
            {
                foreach (var writer in writers)
                {
                    try
                    {
                        writer()?.Indent();
                    }
                    catch
                    {
                    }
                }
            }

            /// <inheritdoc/>
            public void Unindent()
            {
                foreach (var writer in writers)
                {
                    try
                    {
                        writer()?.Unindent();
                    }
                    catch
                    {
                    }
                }
            }

            /// <inheritdoc/>
            public void WriteLine(string message)
            {
                foreach (var writer in writers)
                {
                    try
                    {
                        writer()?.WriteLine(message);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Represents a no-op implementation of the <see cref="ILogWriter"/> interface.
        /// </summary>
        /// <remarks>This class provides an implementation of <see cref="ILogWriter"/> that performs no
        /// operations. It can be used in scenarios where logging is optional or needs
        /// to be disabled without modifying
        /// the code that depends on an <see cref="ILogWriter"/> instance.</remarks>
        public class NullLogWriter : ILogWriter
        {
            /// <inheritdoc/>
            public void Indent()
            {
            }

            /// <inheritdoc/>
            public void Unindent()
            {
            }

            /// <inheritdoc/>
            public void WriteLine(string message)
            {
            }
        }

        /// <summary>
        /// Provides an implementation of the <see cref="ILogWriter"/> interface that
        /// writes log messages to the debug output.
        /// </summary>
        /// <remarks>This class uses the <see cref="System.Diagnostics.Debug"/> class to write messages,
        /// making it suitable for debugging scenarios. Messages are written to the debug
        /// listeners configured for the application.</remarks>
        public class DebugLogWriter : ILogWriter
        {
            /// <inheritdoc/>
            public void Indent()
            {
                System.Diagnostics.Debug.Indent();
            }

            /// <inheritdoc/>
            public void Unindent()
            {
                System.Diagnostics.Debug.Unindent();
            }

            /// <inheritdoc/>
            public void WriteLine(string message)
            {
                System.Diagnostics.Debug.WriteLine(message);
            }
        }

        /// <summary>
        /// Provides a base class for custom log writers that support message formatting with indentation.
        /// </summary>
        /// <remarks>This abstract class defines the core functionality for log writers, including
        /// managing indentation levels and formatting messages. Derived classes must implement the
        /// <see cref="WriteLine(string)"/> method to define
        /// how log messages are written to the output.</remarks>
        public abstract class CustomLogWriter : DisposableObject, ILogWriter
        {
            private int indentLevel = 0;

            /// <summary>
            /// Gets the current indentation level used for formatting output.
            /// </summary>
            public int IndentLevel => indentLevel;

            /// <inheritdoc/>
            public virtual void Indent()
            {
                indentLevel++;
            }

            /// <inheritdoc/>
            public virtual void Unindent()
            {
                if (indentLevel > 0)
                    indentLevel--;
                else
                {
                    if (DebugUtils.IsDebugDefinedAndAttached)
                    {
                        throw new InvalidOperationException("Indent level cannot be less than zero.");
                    }
                }
            }

            /// <summary>
            /// Formats the specified message by applying the current indentation level.
            /// </summary>
            /// <param name="message">The message to format. Cannot be <see langword="null"/>.</param>
            /// <returns>The formatted message with the appropriate indentation applied.</returns>
            public virtual string FormatMessage(string message)
            {
                if(indentLevel == 0)
                {
                    return message;
                }
                else
                {
                    var indent = new string(' ', indentLevel * IndentLength);
                    return $"{indent}{message}";
                }
            }

            /// <inheritdoc/>
            public abstract void WriteLine(string message);
        }

        /// <summary>
        /// Provides a log writer implementation that writes log messages using a user-defined action.
        /// </summary>
        /// <remarks>The <see cref="ActionLogWriter"/> class allows you to define a custom logging
        /// behavior by specifying an <see cref="Action{T}"/> delegate
        /// that processes log messages.</remarks>
        public class ActionLogWriter : CustomLogWriter
        {
            private readonly Action<string> logAction;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActionLogWriter"/>
            /// class with the specified logging action.
            /// </summary>
            /// <param name="logAction">The action to invoke for logging messages.
            /// This action is called with the log message as its parameter.</param>
            /// <exception cref="ArgumentNullException">Thrown
            /// if <paramref name="logAction"/> is <see langword="null"/>.</exception>
            public ActionLogWriter(Action<string> logAction)
            {
                this.logAction = logAction ?? throw new ArgumentNullException(nameof(logAction));
            }

            /// <inheritdoc/>
            public override void WriteLine(string message)
            {
                logAction(FormatMessage(message));
            }
        }

        /// <summary>
        /// Provides functionality for writing application log messages with
        /// optional indentation support.
        /// </summary>
        /// <remarks>This class implements the <see cref="ILogWriter"/> interface
        /// and allows log messages
        /// to be written with a configurable level of indentation.
        /// The log messages are written using
        /// the application's logging mechanism using <see cref="App.Log"/>.</remarks>
        public class ApplicationLogWriter : CustomLogWriter
        {
            /// <inheritdoc/>
            public override void WriteLine(string message)
            {
                App.Log(FormatMessage(message));
            }
        }

        /// <summary>
        /// Provides a log writer implementation that writes log messages
        /// to a specified <see cref="TextWriter"/>.
        /// </summary>
        /// <remarks>This class allows writing log messages with optional indentation to any
        /// <see cref="TextWriter"/> instance, such as a <see cref="System.IO.StreamWriter"/>
        /// or <see cref="System.IO.StringWriter"/>.</remarks>
        public class TextWriterLogWriter : CustomLogWriter
        {
            private readonly TextWriter textWriter;

            /// <summary>
            /// Initializes a new instance of the <see cref="TextWriterLogWriter"/>
            /// class using the specified <see cref="TextWriter"/>.
            /// </summary>
            /// <param name="textWriter">The <see cref="TextWriter"/> instance
            /// to which log messages will be written. Cannot be <see langword="null"/>.</param>
            /// <exception cref="ArgumentNullException">Thrown if
            /// <paramref name="textWriter"/> is <see langword="null"/>.</exception>
            public TextWriterLogWriter(TextWriter textWriter)
            {
                this.textWriter = textWriter
                    ?? throw new ArgumentNullException(nameof(textWriter));
            }

            /// <inheritdoc/>
            public override void WriteLine(string message)
            {
                textWriter.WriteLine(FormatMessage(message));
            }
        }

        /// <summary>
        /// Provides a log writer implementation that writes log messages to a specified
        /// <see cref="StringBuilder"/> instance, with support for indentation.
        /// </summary>
        /// <remarks>This class is useful for scenarios where log messages need to be
        /// captured in-memory for further processing or inspection.</remarks>
        public class StringBuilderLogWriter : CustomLogWriter
        {
            private StringBuilder stringBuilder;

            /// <summary>
            /// Initializes a new instance of the <see cref="StringBuilderLogWriter"/>
            /// class using an empty <see cref="StringBuilder"/> instance.
            /// </summary>
            /// <remarks>This constructor creates a new <see cref="StringBuilder"/> internally to
            /// store log messages. Use this constructor when you do not need to provide an existing
            /// <see cref="StringBuilder"/>.</remarks>
            public StringBuilderLogWriter()
            {
                this.stringBuilder = new ();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="StringBuilderLogWriter"/>
            /// class using the specified <see cref="StringBuilder"/>.
            /// </summary>
            /// <param name="stringBuilder">The <see cref="StringBuilder"/> instance
            /// to which log messages will be written. Cannot be <see langword="null"/>.</param>
            /// <exception cref="ArgumentNullException">Thrown if
            /// <paramref name="stringBuilder"/> is <see langword="null"/>.</exception>
            public StringBuilderLogWriter(StringBuilder stringBuilder)
            {
                this.stringBuilder = stringBuilder ?? new StringBuilder();
            }

            /// <summary>
            /// Gets the underlying <see cref="StringBuilder"/> instance used for logging.
            /// </summary>
            public StringBuilder StringBuilder
            {
                get
                {
                    return stringBuilder;
                }

                set
                {
                    if (value == null)
                        value = new StringBuilder();
                    stringBuilder = value;
                }
            }

            /// <inheritdoc/>
            public override void WriteLine(string message)
            {
                stringBuilder.AppendLine(FormatMessage(message));
            }
        }

        /// <summary>
        /// Provides functionality for writing indented log messages to the console.
        /// </summary>
        /// <remarks>This class implements the <see cref="ILogWriter"/> interface to allow writing log
        /// messages with adjustable indentation levels. Log messages
        /// are written to the console using the <see cref="Console.WriteLine(string)"/> method.</remarks>
        public class ConsoleLogWriter : CustomLogWriter
        {
            /// <inheritdoc/>
            public override void WriteLine(string message)
            {
                Console.WriteLine(FormatMessage(message));
            }
        }
    }
}
