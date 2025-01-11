using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Creates an object graph from a source UIXML stream or resource.
    /// </summary>
    public partial class UixmlLoader
    {
        /// <summary>
        /// Gets or sets default flags which customize load uixml behavior.
        /// </summary>
        public static Flags DefaultFlags
            = Flags.ReportError | Flags.LogError | Flags.ShowErrorDialog;

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

        /// <summary>
        /// Enumerates uixml load flags.
        /// </summary>
        [Flags]
        public enum Flags
        {
            /// <summary>
            /// Specifies whether to report error. If this memmber is not specified,
            /// <see cref="ShowErrorDialog"/>, <see cref="LogError"/> are ignored.
            /// </summary>
            ReportError = 1,

            /// <summary>
            /// Specifies whether to show error dialog.
            /// </summary>
            ShowErrorDialog = 2,

            /// <summary>
            /// Specifies whether to supress exception throw.
            /// </summary>
            NoThrowException = 4,

            /// <summary>
            /// Specifies whether to log error.
            /// </summary>
            LogError = 8,
        }

        /// <summary>
        /// Gets or sets whether uixml is loaded in design mode.
        /// </summary>
        public static bool IsDesignMode { get; set; } = false;

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
        /// Populates an existing root object with the object property values created
        /// from the specified resource with XAML.
        /// </summary>
        public static void LoadExisting(string resName, object existingObject)
        {
            if(LoadFromResName is not null)
            {
                var result = LoadFromResName(resName, existingObject, DefaultFlags);
                if (result)
                    return;
            }

            var uixmlStream = existingObject.GetType().Assembly.GetManifestResourceStream(resName)
                ?? throw new InvalidOperationException();
            LoadExistingEx(uixmlStream, existingObject, DefaultFlags, resName);
        }

        /// <summary>
        /// Populates an existing root object with the object property values created
        /// from a source XAML.
        /// </summary>
        public static object LoadExistingEx(
            Stream xamlStream,
            object existingObject,
            Flags flags = 0,
            string? resName = default)
        {
            if(LoadFromStream is not null)
            {
                var result = LoadFromStream(xamlStream, existingObject, resName, flags);
                if (result)
                    return existingObject;
            }

            try
            {
                Markup.Xaml.UixmlPortRuntimeXamlLoader.Load(
                    xamlStream,
                    existingObject.GetType().Assembly,
                    existingObject,
                    null,
                    IsDesignMode);
                return existingObject;
            }
            catch (Exception e)
            {
                DefaultReportLoadException(e, resName, flags);
                return existingObject;
            }
        }

        /// <summary>
        /// Builds message with filename and error position.
        /// </summary>
        /// <param name="e">Exception.</param>
        /// <param name="resName">Uixml resource name.</param>
        /// <returns></returns>
        public static string? GetErrorFileAndPos(Exception e, string? resName)
        {
            var sourceUri = resName;
            int lineNumber = -1;
            int linePos = -1;

            if (e is XmlException xmlException)
            {
                lineNumber = xmlException.LineNumber;
                linePos = xmlException.LinePosition;
                sourceUri ??= xmlException.SourceUri;
            }

            string? s2 = null;

            if (sourceUri is not null)
            {
                string lineStr = string.Empty;
                string charStr = string.Empty;
                if (linePos > 0)
                    charStr = $" Ch: {linePos}";
                if (lineNumber > 0)
                    lineStr = $" Ln: {lineNumber}{charStr}";

                s2 = $"File: {sourceUri}{lineStr}";
            }

            return s2;
        }

        /// <summary>
        /// Reports load error using the specified error information and flags.
        /// </summary>
        /// <param name="e">Exception information.</param>
        /// <param name="resName">Resource name.</param>
        /// <param name="flags">Flags.</param>
        public static void DefaultReportLoadException(Exception e, string? resName, Flags flags)
        {
            var s1 = $"Error reading uixml: {e.Message}";
            var s3 = $"Exception type: {e.GetType()}";
            var s2 = GetErrorFileAndPos(e, resName);

            if (s2 is not null)
            {
                BaseException exc = new(s1, e);
                exc.AdditionalInformation = s2;
                e = exc;
            }

            BeginSection();
            WriteLine(s1);
            if (s2 is not null)
                WriteLine(s2);
            WriteLine(s3);
            EndSection();

            if (ReportLoadException is not null)
            {
                var result = ReportLoadException(e, resName, flags);
                if (result)
                    return;
            }

            if (flags.HasFlag(Flags.ReportError))
            {
                if (flags.HasFlag(Flags.LogError))
                    App.LogError(e);

                if (!flags.HasFlag(Flags.ShowErrorDialog))
                    return;
                if (App.Initialized && !App.Current.InUixmlPreviewerMode
                    && ShowExceptionDialog)
                {
                    if (!App.ShowExceptionWindow(e))
                    {
                        App.Exit();
                    }
                }
            }

            if (!flags.HasFlag(Flags.NoThrowException))
                ExceptionUtils.Rethrow(e);

            void BeginSection()
            {
                Debug.WriteLine(LogUtils.SectionSeparator);
            }

            void EndSection()
            {
                Debug.WriteLine(LogUtils.SectionSeparator);
            }

            void WriteLine(string s)
            {
                Debug.WriteLine(s);
            }
        }

        /// <summary>
        /// Populates an existing root object with the object property values created
        /// from a source XAML.
        /// </summary>
        public object LoadExisting(Stream xamlStream, object existingObject)
        {
            return LoadExistingEx(xamlStream, existingObject, Flags.ReportError);
        }

        /// <summary>
        /// Returns an object graph created from a source XAML.
        /// </summary>
        public object Load(Stream xamlStream, Assembly localAssembly)
        {
            return Markup.Xaml.UixmlPortRuntimeXamlLoader.Load(xamlStream, localAssembly);
        }
    }
}