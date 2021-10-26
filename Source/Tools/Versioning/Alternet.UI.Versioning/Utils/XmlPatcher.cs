using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Alternet.UI.Versioning
{
    static class XmlPatcher
    {
        public static void PatchAttribute(string filePath, string elementSelector, XName attributeName, string newValue)
        {
            using var fs = File.OpenRead(filePath);
            var document = XDocument.Load(fs);

            var element = document.XPathSelectElement(elementSelector);

            if (element == null)
                return;

            element.SetAttributeValue(attributeName, newValue);

            document.Save(filePath);
        }
    }
}