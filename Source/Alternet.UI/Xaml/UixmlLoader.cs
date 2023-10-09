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
        /// from a source XAML.
        /// </summary>
        public void LoadExisting(Stream xamlStream, object existingObject)
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
                if(e is XmlException xmlException)
                {
                    int lineNumber = xmlException.LineNumber;
                    int linePos = xmlException.LinePosition;
                    string sourceUri = xmlException.SourceUri;
                }

                Debug.WriteLine(e.Message);

                throw e;
            }
        }
    }
}