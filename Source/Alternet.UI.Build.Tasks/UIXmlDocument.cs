using System;
using System.IO;
using System.Xml.Linq;

namespace Alternet.UI.Build.Tasks
{
    internal class UIXmlDocument
    {
        private const string UINamespace = "http://schemas.alternetsoft.com/ui";
        private const string UIXmlNamespace = "http://schemas.alternetsoft.com/uixml";
        private XDocument document;

        private string? baseClassFullName;

        private string? className;

        private string? classNamespaceName;

        public UIXmlDocument(string resourceName, Stream xmlContent)
        {
            ResourceName = resourceName;
            document = XDocument.Load(xmlContent);
        }

        public string BaseClassFullName => baseClassFullName ??= GetBaseClassFullName();

        public string ClassName => className ??= GetClassName();

        public string ClassNamespaceName => classNamespaceName ??= GetClassNamespaceName();

        public string ResourceName { get; }

        public bool IsValidIdentifier(string text)
        {
            if (string.IsNullOrEmpty(text))
                return false;
            if (!char.IsLetter(text[0]) && text[0] != '_')
                return false;
            for (int ix = 1; ix < text.Length; ++ix)
                if (!char.IsLetterOrDigit(text[ix]) && text[ix] != '_')
                    return false;
            return true;
        }

        private string GetBaseClassFullName()
        {
            return GetTypeFullName(document.Root.Name);
        }

        private string GetClassName()
        {
            return GetTypeNameFromFullName(GetClassFullName());
        }

        private string GetClassFullName()
        {
            var attribute = document.Root.Attribute((XNamespace)UIXmlNamespace + "Class");
            if (attribute == null)
                throw new Exception("x:Class attribute on root node was not found.");
            return attribute.Value;
        }

        private string GetClassNamespaceName()
        {
            return GetTypeNamespaceFromFullName(GetClassFullName());
        }

        private string GetTypeName(XName name)
        {
            return name.LocalName;
        }

        private string GetTypeNameFromFullName(string fullName)
        {
            int i = fullName.LastIndexOf('.');
            if (i == -1)
                return fullName;

            return fullName.Substring(i + 1);
        }

        private string GetTypeNamespaceFromFullName(string fullName)
        {
            int i = fullName.LastIndexOf('.');
            if (i == -1)
                return "";

            return fullName.Substring(0, i);
        }

        private string GetTypeFullName(XName name)
        {
            return GetTypeNamespaceName(name) + "." + GetTypeName(name);
        }

        private string GetTypeNamespaceName(XName name)
        {
            var ns = name.NamespaceName;
            if (ns == UINamespace)
                return "Alternet.UI";

            return ParseClrNamespaceFromXmlns(ns);
        }

        private string ParseClrNamespaceFromXmlns(string ns)
        {
            const string ClrNamespacePrefix = "clr-namespace:";
            int startIndex = ns.IndexOf(ClrNamespacePrefix);
            if (startIndex == -1)
                throw new Exception("CLR type namespace declaration must start with 'clr-namespace:'");

            startIndex += ClrNamespacePrefix.Length;
            int endIndex = ns.IndexOf(';', startIndex);
            if (endIndex == -1)
                endIndex = ns.Length;

            var value = ns.Substring(startIndex, endIndex - startIndex);
            if (value == "")
                return value;

            if (!IsValidIdentifier(value))
                throw new Exception($"'{value}' is not a valid CLR namespace name.");

            return value;
        }
    }
}