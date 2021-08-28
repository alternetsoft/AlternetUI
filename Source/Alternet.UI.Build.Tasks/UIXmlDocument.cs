using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.Build.Tasks
{
    internal class UIXmlDocument
    {
        private const string UINamespace = "http://schemas.alternetsoft.com/ui";
        private const string UIXmlNamespace = "http://schemas.alternetsoft.com/uixml";
        private const string ClassAttributeNotFound = "x:Class attribute on root node was not found.";
        private static readonly XName classAttributeName = (XNamespace)UIXmlNamespace + "Class";
        private readonly Stream xmlContent;
        private XDocument document;

        private XDocument? sanitizedDocument;

        private string? baseClassFullName;

        private string? className;

        private string? classNamespaceName;

        private IReadOnlyList<NamedObject>? namedObjects;

        public UIXmlDocument(string resourceName, Stream xmlContent)
        {
            ResourceName = resourceName;
            this.xmlContent = xmlContent;
            document = XDocument.Load(xmlContent);

            xmlContent.Position = 0;
        }

        public XDocument SanitizedDocument => sanitizedDocument ??= Sanitize(XDocument.Load(xmlContent));

        public string BaseClassFullName => baseClassFullName ??= GetBaseClassFullName();

        public string ClassName => className ??= GetClassName();

        public string ClassNamespaceName => classNamespaceName ??= GetClassNamespaceName();

        public string ResourceName { get; }

        public IReadOnlyList<NamedObject> NamedObjects => namedObjects ??= GetNamedObjects().ToArray();

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

        private XDocument Sanitize(XDocument document)
        {
            var attribute = document.Root.Attribute(classAttributeName);
            if (attribute == null)
                throw new Exception(ClassAttributeNotFound);
            attribute.Remove();
            return document;
        }

        private IEnumerable<NamedObject> GetNamedObjects()
        {
            const string NameAttributeName = "Name";
            var namedElements = document.Root.DescendantsAndSelf().Where(e => e.Attributes().Any(a => a.Name == NameAttributeName));
            return namedElements.Select(
                x => new NamedObject(GetTypeFullName(x.Name), x.Attribute(NameAttributeName).Value));
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
            var attribute = document.Root.Attribute(classAttributeName);
            if (attribute == null)
                throw new Exception(ClassAttributeNotFound);
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

        public class NamedObject
        {
            public NamedObject(string typeFullName, string name)
            {
                TypeFullName = typeFullName;
                Name = name;
            }

            public string TypeFullName { get; }

            public string Name { get; }
        }
    }
}