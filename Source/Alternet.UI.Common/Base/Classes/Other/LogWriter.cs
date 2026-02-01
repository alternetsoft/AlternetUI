using System;
using System.Collections;
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
        /// Represents the default character used to separate values in formatted output.
        /// This is used in <see cref="WriteSeparator(ILogWriter)"/>.
        /// </summary>
        public static char SeparatorChar = '-';

        /// <summary>
        /// Specifies the default length, in characters, for a separator line.
        /// This is used in <see cref="WriteSeparator(ILogWriter)"/>.
        /// </summary>
        public static int SeparatorLength = 40;

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
        /// Writes a separator line to the log using the specified log writer.
        /// </summary>
        /// <remarks>The separator line consists of a repeated character defined by the log writer
        /// implementation. This method is typically used to visually separate sections in the log output.</remarks>
        /// <param name="writer">The log writer to which the separator line will be written. Cannot be null.</param>
        public static ILogWriter WriteSeparator(this ILogWriter writer)
        {
            writer.WriteLine(new string(LogWriter.SeparatorChar, LogWriter.SeparatorLength));
            return writer;
        }

        /// <summary>
        /// Writes an empty line to the log output using the specified log writer.
        /// </summary>
        /// <param name="writer">The log writer to which the empty line will be written. Cannot be null.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance that was provided, to support method chaining.</returns>
        public static ILogWriter WriteLine(this ILogWriter writer)
        {
            writer.WriteLine(string.Empty);
            return writer;
        }

        /// <summary>
        /// Writes each string in the specified collection to the log, appending a new line after each entry.
        /// </summary>
        /// <remarks>This method writes each string in <paramref name="lines"/> to the log by calling <see
        /// cref="ILogWriter.WriteLine(string)"/> for each element. The order of the lines is preserved.</remarks>
        /// <param name="writer">The log writer to which the lines will be written. Cannot be null.</param>
        /// <param name="lines">The collection of strings to write to the log. Cannot be null.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance that was provided, to support method chaining.</returns>
        public static ILogWriter WriteLines(this ILogWriter writer, IEnumerable<string> lines)
        {
            foreach (var line in lines)
                writer.WriteLine(line);
            return writer;
        }

        /// <summary>
        /// Writes each item in the specified collection to the log as a separate line.
        /// </summary>
        /// <remarks>If an item in <paramref name="lines"/> is null, an empty line is written for that
        /// item. This method enables fluent-style logging by returning the original writer.</remarks>
        /// <param name="writer">The log writer to which the lines will be written. Cannot be null.</param>
        /// <param name="lines">The collection of items to write. Each item is converted to a string and written as a separate line. Cannot
        /// be null.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance that was provided, to support method chaining.</returns>
        public static ILogWriter WriteLines(this ILogWriter writer, IEnumerable lines)
        {
            foreach (var line in lines)
                writer.WriteLine(line?.ToString() ?? string.Empty);
            return writer;
        }

        /// <summary>
        /// Writes each string in the specified collection to the log as a separate line.
        /// </summary>
        /// <remarks>This method is an extension method for <see cref="ILogWriter"/> and allows writing
        /// multiple lines in a single call. If the <paramref name="lines"/> array is empty, no lines are
        /// written.</remarks>
        /// <param name="writer">The log writer to which the lines will be written. Cannot be null.</param>
        /// <param name="lines">An array of strings to write to the log. Each string is written as a separate line. Cannot be null.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance that was provided, to support method chaining.</returns>
        public static ILogWriter WriteLines(this ILogWriter writer, params string[] lines)
        {
            foreach (var line in lines)
                writer.WriteLine(line);
            return writer;
        }

        /// <summary>
        /// Writes each specified line to the log using the provided writer.
        /// </summary>
        /// <remarks>This method is an extension method that allows writing multiple lines to the log in a
        /// single call. It is useful for fluent logging scenarios.</remarks>
        /// <param name="writer">The log writer to which the lines will be written. Cannot be null.</param>
        /// <param name="lines">An array of objects to write as individual lines. Each object is converted to its string representation.
        /// Null values are written as empty lines.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance that was provided, to support method chaining.</returns>
        public static ILogWriter WriteLines(this ILogWriter writer, params object?[] lines)
        {
            foreach (var line in lines)
                writer.WriteLine(line?.ToString() ?? string.Empty);
            return writer;
        }

        /// <summary>
        /// Writes a key-value pair to the log in the format "key: value".
        /// </summary>
        /// <remarks>This method is an extension method for <see cref="ILogWriter"/> and enables fluent
        /// logging of key-value pairs. The output format is intended for human-readable logs and may not be suitable
        /// for structured logging scenarios.</remarks>
        /// <param name="writer">The log writer to which the key-value pair will be written. Cannot be null.</param>
        /// <param name="key">The key to write. Typically identifies the value being logged. Cannot be null.</param>
        /// <param name="value">The value associated with the key. If null, the string "null" is written.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance, to support method chaining.</returns>
        public static ILogWriter WriteKeyValue(this ILogWriter writer, object key, object? value)
        {
            writer.WriteLine($"{key}: {value?.ToString() ?? "null"}");
            return writer;
        }

        /// <summary>
        /// Begins a new indented log section, optionally with a title, and returns the log writer for further logging
        /// within the section.
        /// </summary>
        /// <remarks>A separator line is written before and after the section title, if provided. The log
        /// writer's indentation level is increased for all subsequent log entries until the indentation is changed or
        /// reset.</remarks>
        /// <param name="writer">The log writer to which the section will be written. Cannot be null.</param>
        /// <param name="sectionTitle">The optional title for the log section. If specified, the title
        /// is written at the start of the section.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance, allowing for method chaining within the new section.</returns>
        public static ILogWriter BeginSection(this ILogWriter writer, string? sectionTitle = null)
        {
            writer.WriteSeparator();

            if (sectionTitle != null)
            {
                writer.WriteLine(sectionTitle);
                writer.WriteSeparator();
            }

            writer.Indent();
            return writer;
        }

        /// <summary>
        /// Ends the current log section by decreasing the indentation level and writing a section separator.
        /// </summary>
        /// <remarks>Call this method to mark the end of a logical section in the log output. This is
        /// typically used in conjunction with methods that begin a section to maintain structured and readable
        /// logs.</remarks>
        /// <param name="writer">The log writer to operate on. Cannot be null.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance, enabling method chaining.</returns>
        public static ILogWriter EndSection(this ILogWriter writer)
        {
            writer.Unindent();
            writer.WriteSeparator();
            return writer;
        }

        /// <summary>
        /// Writes detailed information about the specified exception to the log,
        /// including its type, message, and stack trace.
        /// </summary>
        /// <remarks>This method writes the exception details within a dedicated section in the log. If
        /// the exception's stack trace is null, an empty string is written in its place.</remarks>
        /// <param name="writer">The log writer to which the exception details will be written. Cannot be null.</param>
        /// <param name="ex">The exception whose details are to be logged. Cannot be null.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance that was provided, to support method chaining.</returns>
        public static ILogWriter WriteException(this ILogWriter writer, Exception ex)
        {
            writer.BeginSection("Exception occurred");
            writer.WriteLine($"Type: {ex.GetType().FullName}");
            writer.WriteLine($"Message: {ex.Message}");
            writer.WriteLine("Stack Trace:");
            writer.WriteLine(ex.StackTrace ?? string.Empty);
            writer.EndSection();
            return writer;
        }

        /// <summary>
        /// Writes detailed information about an exception, including its type, message, and stack trace, to the log
        /// writer as a formatted section.
        /// </summary>
        /// <remarks>The exception details are written as a distinct section in the log output. This
        /// method is intended to standardize exception logging and improve log readability.</remarks>
        /// <param name="writer">The log writer to which the exception details are written. Cannot be null.</param>
        /// <param name="message">A descriptive message providing context for the exception.</param>
        /// <param name="ex">The exception to log. Cannot be null.</param>
        /// <returns>The same <see cref="ILogWriter"/> instance that was provided, to support method chaining.</returns>
        public static ILogWriter WriteException(this ILogWriter writer, string message, Exception ex)
        {
            writer.BeginSection("Exception occurred");
            writer.WriteLine(message);
            writer.WriteLine($"Type: {ex.GetType().FullName}");
            writer.WriteLine($"Message: {ex.Message}");
            writer.WriteLine("Stack Trace:");
            writer.WriteLine(ex.StackTrace ?? string.Empty);
            writer.EndSection();
            return writer;
        }

        /// <summary>
        /// Ensures that a non-null <see cref="ILogWriter"/> instance is returned.
        /// </summary>
        /// <param name="writer">The <see cref="ILogWriter"/> instance to validate.
        /// Can be <see langword="null"/>.</param>
        /// <returns>The provided <paramref name="writer"/> if it
        /// is not <see langword="null"/>; otherwise, the current default
        /// writer or a debug writer.</returns>
        public static ILogWriter Safe(ILogWriter? writer = null)
        {
            return writer ?? current ?? Debug;
        }

        /// <summary>
        /// Creates an instance of an <see cref="ILogWriter"/> that writes
        /// log messages using the specified action.
        /// </summary>
        /// <param name="logAction">The action to invoke for each log message.
        /// Cannot be <see langword="null"/>.</param>
        /// <returns>An <see cref="ILogWriter"/> that uses the provided action
        /// to handle log messages.</returns>
        public static ILogWriter Create(Action<string> logAction)
        {
            return new ActionLogWriter(logAction);
        }

        /// <summary>
        /// Creates a new instance of an <see cref="ILogWriter"/> that writes
        /// log messages to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="textWriter">The <see cref="TextWriter"/> to which
        /// log messages will be written. Cannot be <see langword="null"/>.</param>
        /// <returns>An <see cref="ILogWriter"/> instance
        /// configured to write log messages to the specified <see cref="TextWriter"/>.</returns>
        public static ILogWriter Create(TextWriter textWriter)
        {
            return new TextWriterLogWriter(textWriter);
        }

        /// <summary>
        /// Creates a new instance of an <see cref="ILogWriter"/> that writes
        /// log messages to the specified <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder">The <see cref="StringBuilder"/>
        /// to which log messages will be written. Cannot be <see langword="null"/>.</param>
        /// <returns>An <see cref="ILogWriter"/> instance configured to write to
        /// the specified <see cref="StringBuilder"/>.</returns>
        public static ILogWriter Create(StringBuilder stringBuilder)
        {
            return new StringBuilderLogWriter(stringBuilder);
        }

        /// <summary>
        /// Creates a new instance of a log writer that writes log messages to multiple loggers.
        /// </summary>
        /// <remarks>The returned log writer forwards each log message to all provided
        /// loggers in the order they are specified.</remarks>
        /// <param name="loggers">An array of log writers to which log messages
        /// will be forwarded. Cannot be null.</param>
        /// <returns>An <see cref="ILogWriter"/> instance that writes
        /// log messages to all specified loggers.</returns>
        public static ILogWriter Create(params ILogWriter[] loggers)
        {
            return new MultiLogWriter(loggers);
        }

        /// <summary>
        /// Writes the content of the specified <see cref="StringBuilder"/> to the output, line by line.
        /// This is a convenience method that splits the content into lines and writes each line.
        /// </summary>
        /// <remarks>The content of the <paramref name="message"/> is split into lines, and each
        /// line is written separately.</remarks>
        /// <param name="writer">The <see cref="ILogWriter"/> instance to use for writing log messages.
        /// Cannot be <see langword="null"/>.</param>
        /// <param name="message">The <see cref="StringBuilder"/> containing the message to write.
        /// Cannot be <see langword="null"/>.</param>
        public static void Write(this ILogWriter writer, StringBuilder message)
        {
            var strings = StringUtils.ToStrings(message);
            foreach (var s in strings)
                writer.WriteLine(s);
        }

        /// <summary>
        /// Provides a mechanism to write log messages to multiple <see cref="ILogWriter"/> instances.
        /// </summary>
        /// <remarks>The <see cref="MultiLogWriter"/> class allows you to aggregate multiple log writers
        /// and forward log messages to all of them. This is useful when you
        /// need to log messages to multiple destinations simultaneously,
        /// such as a file, console, or remote logging service.</remarks>
        public class MultiLogWriter : BaseLogWriter
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
            public override ILogWriter Indent()
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

                return this;
            }

            /// <inheritdoc/>
            public override ILogWriter Unindent()
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

                return this;
            }

            /// <inheritdoc/>
            public override ILogWriter WriteLine(string message)
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

                return this;
            }
        }

        /// <summary>
        /// Represents a no-op implementation of the <see cref="ILogWriter"/> interface.
        /// </summary>
        /// <remarks>This class provides an implementation of <see cref="ILogWriter"/> that performs no
        /// operations. It can be used in scenarios where logging is optional or needs
        /// to be disabled without modifying
        /// the code that depends on an <see cref="ILogWriter"/> instance.</remarks>
        public class NullLogWriter : BaseLogWriter
        {
            /// <inheritdoc/>
            public override ILogWriter Indent()
            {
                return this;
            }

            /// <inheritdoc/>
            public override ILogWriter Unindent()
            {
                return this;
            }

            /// <inheritdoc/>
            public override ILogWriter WriteLine(string message)
            {
                return this;
            }
        }

        /// <summary>
        /// Provides an implementation of the <see cref="ILogWriter"/> interface that
        /// writes log messages to the debug output.
        /// </summary>
        /// <remarks>This class uses the <see cref="System.Diagnostics.Debug"/> class to write messages,
        /// making it suitable for debugging scenarios. Messages are written to the debug
        /// listeners configured for the application.</remarks>
        public class DebugLogWriter : BaseLogWriter
        {
            /// <inheritdoc/>
            public override ILogWriter Indent()
            {
                System.Diagnostics.Debug.Indent();
                return this;
            }

            /// <inheritdoc/>
            public override ILogWriter Unindent()
            {
                System.Diagnostics.Debug.Unindent();
                return this;
            }

            /// <inheritdoc/>
            public override ILogWriter WriteLine(string message)
            {
                System.Diagnostics.Debug.WriteLine(message);
                return this;
            }
        }

        /// <summary>
        /// Provides a base implementation for log writers, defining the core methods
        /// for writing log messages and
        /// managing indentation levels.
        /// </summary>
        /// <remarks>This abstract class serves as a foundation for creating custom log writers.
        /// Derived classes must implement the methods to handle log message
        /// output and indentation behavior. The class ensures
        /// consistent behavior across different log writer implementations.</remarks>
        public abstract class BaseLogWriter : DisposableObject, ILogWriter
        {
            /// <inheritdoc/>
            public abstract ILogWriter Indent();

            /// <inheritdoc/>
            public abstract ILogWriter Unindent();

            /// <inheritdoc/>
            public abstract ILogWriter WriteLine(string message);
        }

        /// <summary>
        /// Provides a base class for custom log writers that support message formatting with indentation.
        /// </summary>
        /// <remarks>This abstract class defines the core functionality for log writers, including
        /// managing indentation levels and formatting messages. Derived classes must implement the
        /// <see cref="BaseLogWriter.WriteLine(string)"/> method to define
        /// how log messages are written to the output.</remarks>
        public abstract class CustomLogWriter : BaseLogWriter
        {
            private int indentLevel = 0;

            /// <summary>
            /// Gets the current indentation level used for formatting output.
            /// </summary>
            public int IndentLevel => indentLevel;

            /// <inheritdoc/>
            public override ILogWriter Indent()
            {
                indentLevel++;
                return this;
            }

            /// <inheritdoc/>
            public override ILogWriter Unindent()
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

                return this;
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
            public override ILogWriter WriteLine(string message)
            {
                logAction(FormatMessage(message));
                return this;
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
            public override ILogWriter WriteLine(string message)
            {
                App.Log(FormatMessage(message));
                return this;
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
            public override ILogWriter WriteLine(string message)
            {
                textWriter.WriteLine(FormatMessage(message));
                return this;
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
            public override ILogWriter WriteLine(string message)
            {
                stringBuilder.AppendLine(FormatMessage(message));
                return this;
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
            public override ILogWriter WriteLine(string message)
            {
                Console.WriteLine(FormatMessage(message));
                return this;
            }
        }
    }
}
