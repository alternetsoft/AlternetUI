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
        internal static void LoadExistingEx(
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
                throw e;
            }
        }

        private static void ReportException(Exception e, string? resName)
        {
            string sourceUri = resName;
            int lineNumber = -1;
            int linePos = -1;

            if (e is XmlException xmlException)
            {
                lineNumber = xmlException.LineNumber;
                linePos = xmlException.LinePosition;
                sourceUri = sourceUri ?? xmlException.SourceUri;
            }

            Debug.WriteLine($"Error reading Uixml: {e.Message}");
            Debug.Indent();
            if (sourceUri is not null)
            {
                string lineStr = string.Empty;
                string charStr = string.Empty;
                if (linePos > 0)
                    charStr = $" Ch: {linePos}";
                if(lineNumber > 0)
                    lineStr = $" Ln: {lineNumber}{charStr}";

                Debug.WriteLine($"File: {sourceUri}{lineStr}");
            }

            Debug.WriteLine($"Exception type: {e.GetType()}");
            Debug.Unindent();

        }
    }
}