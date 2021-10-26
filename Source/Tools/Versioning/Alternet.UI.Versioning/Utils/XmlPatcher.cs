using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Alternet.UI.Versioning
{
    static class XmlPatcher
    {
        public static void PatchAttribute(string filePath, string elementSelector, XName attributeName, string newValue)
        {
            XDocument document;
            using (var stream = File.OpenRead(filePath))
                document = XDocument.Load(stream);

            var element = document.XPathSelectElement(elementSelector);

            if (element == null)
                return;

            element.SetAttributeValue(attributeName, newValue);

            using (var writer = XmlWriter.Create(
                filePath,
                new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
                    Indent = true
                }))
                document.Save(writer);
        }
    }
}