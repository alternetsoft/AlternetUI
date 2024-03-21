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
            try
            {
                var uixmlStream = existingObject.GetType().Assembly.GetManifestResourceStream(resName);
                if (uixmlStream == null)
                    throw new InvalidOperationException();
                LoadExistingEx(uixmlStream, existingObject, false, resName);
            }
            catch (Exception e)
            {
                ReportException(e, resName);
                throw;
            }
        }

        /// <summary>
        /// Populates an existing root object with the object property values created
        /// from a source XAML.
        /// </summary>
        public void LoadExisting(Stream xamlStream, object existingObject)
        {
            LoadExistingEx(xamlStream, existingObject, true);
        }

        /// <summary>
        /// Populates an existing root object with the object property values created
        /// from a source XAML.
        /// </summary>
        public static void LoadExistingEx(
            Stream xamlStream,
            object existingObject,
            bool report = true,
            string? resName = default)
        {
            try
            {
                Markup.Xaml.UixmlPortRuntimeXamlLoader.Load(
                    xamlStream,
                    existingObject.GetType().Assembly,
                    existingObject);

            }
            catch (Exception e)
            {
                if(report)
                    ReportException(e, resName);
                if (Application.Initialized && !Application.Current.InUixmlPreviewerMode
                    && ShowExceptionDialog)
                {
                    var s = $"Resource Name: {resName}";
                    ThreadExceptionWindow.Show(e, s, false);
                }

                throw e;
            }
        }

        private static void ReportException(Exception e, string? resName)
        {
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
        }
    }
}