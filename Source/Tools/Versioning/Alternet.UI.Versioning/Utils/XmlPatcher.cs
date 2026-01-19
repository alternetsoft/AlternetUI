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
    class XmlPatcher
    {
        private readonly bool omitXmlDeclaration;

        public XmlPatcher(bool omitXmlDeclaration)
        {
            this.omitXmlDeclaration = omitXmlDeclaration;
        }

        public void PatchValue(
            string filePath,
            string elementSelector,
            string newValue)
        {
            XDocument document;
            XmlReader reader;

            using (var stream = File.OpenRead(filePath))
            {
                reader = new XmlTextReader(stream);
                document = XDocument.Load(reader);
            }

            // Bind the MSBuild namespace to the "msb" prefix
            var nsmgr = new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("msb", "http://schemas.microsoft.com/developer/msbuild/2003");

            var element = document.XPathSelectElement(elementSelector, nsmgr);

            if (element == null)
                return;

            element.SetValue(newValue);
                
            using var writer = XmlWriter.Create(
                filePath,
                new XmlWriterSettings
                {
                    OmitXmlDeclaration = omitXmlDeclaration,
                    Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: !omitXmlDeclaration),
                    Indent = true
                });
            document.Save(writer);
        }

        public void PatchAttribute(
            string filePath,
            string elementSelector,
            XName attributeName,
            string newValue,
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

            if (element == null)
                return;

            element.SetAttributeValue(attributeName, newValue);

            using var writer = XmlWriter.Create(
                filePath,
                new XmlWriterSettings
                {
                    OmitXmlDeclaration = omitXmlDeclaration,
                    Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: !omitXmlDeclaration),
                    Indent = true
                });
            document.Save(writer);
        }
    }
}