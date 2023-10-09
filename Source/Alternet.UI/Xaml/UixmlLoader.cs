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
        public void LoadExisting(string resName, object existingObject)
        {
            try
            {
                var uixmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName);
                if (uixmlStream == null)
                    throw new InvalidOperationException();
                LoadExisting(uixmlStream, this, false, resName);
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
        public void LoadExisting(
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

        private void ReportException(Exception e, string? resName)
        {
            string sourceUri = resName;

            if (e is XmlException xmlException)
            {
                int lineNumber = xmlException.LineNumber;
                int linePos = xmlException.LinePosition;
                sourceUri = sourceUri ?? xmlException.SourceUri;
            }

            if(sourceUri is null)
            {
                Debug.WriteLine(e.Message);
                return;
            }

            Debug.WriteLine($"{e.Message}. Uri: {sourceUri}");
        }
    }
}