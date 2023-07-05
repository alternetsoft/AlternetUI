using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Alternet.UI.Versioning
{
    static class XmlValueReader
    {
        public static string? ReadAttributeValue(
            string filePath,
            string elementSelector,
            XName attributeName,
            IDictionary<string, string>? namespaceBindings = null)
        {
            XDocument document;
            XmlReader reader;

            using (var stream = File.OpenRead(filePath))
            {
                reader = new XmlTextReader(stream);
                document = XDocument.Load(reader);
            }

            XmlNamespaceManager? xmlNamespaceResolver = null;
            if (namespaceBindings != null)
            {
                xmlNamespaceResolver = new XmlNamespaceManager(reader.NameTable);
                foreach (var binding in namespaceBindings)
                    xmlNamespaceResolver.AddNamespace(binding.Key, binding.Value);
            }

            var element = document.XPathSelectElement(elementSelector, xmlNamespaceResolver);

            if (element is not null)
                return element.Attribute(attributeName)?.Value;
            return null;
        }
    }
}