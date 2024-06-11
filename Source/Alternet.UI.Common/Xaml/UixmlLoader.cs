#pragma warning disable
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Alternet.UI
{
    /// <summary>
    /// Creates an object graph from a source UIXML.
    /// </summary>
    public class UixmlLoader
    {
        /// <summary>
        /// This flag supports internal infrastructure and is not supposed to be used from
        /// the user code.
        /// </summary>
        public static bool DisableComponentInitialization { get; set; }

        /// <summary>
        /// Gets or sets whether to show exception dialog when uixml is loaded with errors.
        /// </summary>
        /// <remarks>
        /// Default is <c>true</c>.
        /// </remarks>
        public static bool ShowExceptionDialog { get; set; } = true;

        /// <summary>
        /// Custom method for loading Uixml from resource. Overrides default behavior.
        /// </summary>
        public static Func<string, object, Flags, bool>? LoadFromResName;

        /// <summary>
        /// Custom method for loading Uixml from stream. Overrides default behavior.
        /// </summary>
        public static Func<Stream, object, string?, Flags, bool>? LoadFromStream;

        /// <summary>
        /// Custom method for showing errors occured during load process. Overrides default behavior.
        /// </summary>
        public static Func<Exception, string?, Flags, bool>? ReportLoadException;

        [Flags]
        public enum Flags
        {
            /// <summary>
            /// Specifies whether to report error.
            /// </summary>
            ReportError = 1,

            /// <summary>
            /// Specifies whether to show error dialog.
            /// </summary>
            ShowErrorDialog = 2,

            /// <summary>
            /// Specifies whether to supress exception throw.
            /// </summary>
            NoThrowException = 3,
        }

        /// <summary>
        /// Returns an object graph created from a source XAML.
        /// </summary>
        public object Load(Stream xamlStream, Assembly localAssembly)
        {
            return Markup.Xaml.UixmlPortRuntimeXamlLoader.Load(xamlStream, localAssembly);
        }

        /// <summary>
        /// Populates an existing root object with the object property values created
        /// from the specified resource with XAML.
        /// </summary>
        public static void LoadExisting(string resName, object existingObject)
        {
            if(LoadFromResName is not null)
            {
                var result = LoadFromResName(resName, existingObject, 0);
                if (result)
                    return;
            }

            try
            {
                var uixmlStream = existingObject.GetType().Assembly.GetManifestResourceStream(resName);
                if (uixmlStream == null)
                    throw new InvalidOperationException();
                LoadExistingEx(uixmlStream, existingObject, 0, resName);
            }
            catch (Exception e)
            {
                DefaultReportLoadException(e, resName, 0);
                throw;
            }
        }

        /// <summary>
        /// Populates an existing root object with the object property values created
        /// from a source XAML.
        /// </summary>
        public void LoadExisting(Stream xamlStream, object existingObject)
        {
            LoadExistingEx(xamlStream, existingObject, Flags.ReportError);
        }

        /// <summary>
        /// Populates an existing root object with the object property values created
        /// from a source XAML.
        /// </summary>
        public static void LoadExistingEx(
            Stream xamlStream,
            object existingObject,
            Flags flags = 0,
            string? resName = default)
        {
            if(LoadFromStream is not null)
            {
                var result = LoadFromStream(xamlStream, existingObject, resName, flags);
                if (result)
                    return;
            }

            try
            {
                Markup.Xaml.UixmlPortRuntimeXamlLoader.Load(
                    xamlStream,
                    existingObject.GetType().Assembly,
                    existingObject);
            }
            catch (Exception e)
            {
                DefaultReportLoadException(e, resName, flags);
                if(!flags.HasFlag(Flags.NoThrowException))
                    throw e;
            }
        }

        public static void DefaultReportLoadException(Exception e, string? resName, Flags flags)
        {
            if(ReportLoadException is not null)
            {
                var result = ReportLoadException(e, resName, flags);
                if (result)
                    return;
            }

            if (!flags.HasFlag(Flags.ReportError))
                return;

            void BeginSection()
            {
            }

            void EndSection()
            {
            }

            void WriteLine(string s)
            {
                Debug.WriteLine(s);
            }

            void Indent()
            {
                Debug.Indent();
            }

            void Unindent()
            {
                Debug.Unindent();
            }

            string sourceUri = resName;
            int lineNumber = -1;
            int linePos = -1;

            if (e is XmlException xmlException)
            {
                lineNumber = xmlException.LineNumber;
                linePos = xmlException.LinePosition;
                sourceUri = sourceUri ?? xmlException.SourceUri;
            }

            BeginSection();
            WriteLine($"Error reading Uixml: {e.Message}");
            Indent();
            if (sourceUri is not null)
            {
                string lineStr = string.Empty;
                string charStr = string.Empty;
                if (linePos > 0)
                    charStr = $" Ch: {linePos}";
                if(lineNumber > 0)
                    lineStr = $" Ln: {lineNumber}{charStr}";

                WriteLine($"File: {sourceUri}{lineStr}");
            }

            WriteLine($"Exception type: {e.GetType()}");
            Unindent();
            EndSection();

            if (!flags.HasFlag(Flags.ShowErrorDialog))
                return;
            if (App.Initialized && !App.Current.InUixmlPreviewerMode
                && ShowExceptionDialog)
            {
                var s = $"Resource Name: {resName}";
                App.ShowExceptionWindow(e, s, false);
            }
        }
    }
}