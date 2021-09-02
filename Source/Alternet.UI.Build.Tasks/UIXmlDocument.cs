using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.Build.Tasks
{
    internal class UIXmlDocument
    {
        private const string UINamespace = "http://schemas.alternetsoft.com/ui/2021";
        private const string UIXmlNamespace = "http://schemas.alternetsoft.com/ui/2021/uixml";
        private const string ClassAttributeNotFound = "x:Class attribute on root node was not found.";
        private const string NameAttributeName = "Name";
        private static readonly XName classAttributeName = (XNamespace)UIXmlNamespace + "Class";
        private readonly Stream xmlContent;
        private readonly ApiInfoProvider apiInfoProvider;
        private XDocument document;

        private XDocument? sanitizedDocument;

        private string? baseClassFullName;

        private string? className;

        private string? classNamespaceName;

        private IReadOnlyList<EventBinding>? eventBindings;

        private IReadOnlyList<NamedObject>? namedObjects;

        public UIXmlDocument(string resourceName, Stream xmlContent, ApiInfoProvider apiInfoProvider)
        {
            ResourceName = resourceName;
            this.xmlContent = xmlContent;
            this.apiInfoProvider = apiInfoProvider;
            document = XDocument.Load(xmlContent);

            xmlContent.Position = 0;
        }

        public XDocument SanitizedDocument => sanitizedDocument ??= Sanitize(XDocument.Load(xmlContent));

        public string BaseClassFullName => baseClassFullName ??= GetBaseClassFullName();

        public string ClassName => className ??= GetClassName();

        public string ClassNamespaceName => classNamespaceName ??= GetClassNamespaceName();

        public string ResourceName { get; }

        public IReadOnlyList<NamedObject> NamedObjects => namedObjects ??= GetNamedObjects().ToArray();

        public IReadOnlyList<EventBinding> EventBindings => eventBindings ??= GetEventBindings(apiInfoProvider, document).Select(x => x.Binding).ToArray();

        private static bool IsValidIdentifier(string text)
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

        private static IEnumerable<(EventBinding Binding, XAttribute Attribute)> GetEventBindings(ApiInfoProvider apiInfoProvider, XDocument document)
        {
            EventBinding? TryGetEventBinding(XElement element, string? objectName, Stack<int> indices, XAttribute attribute)
            {
                var assemblyName = GetTypeAssemblyName(element.Name);
                var typeFullName = GetTypeFullName(element.Name);
                string eventName = attribute.Name.LocalName;
                if (!apiInfoProvider.IsEvent(assemblyName, typeFullName, eventName))
                    return null;

                var handlerName = attribute.Value;

                if (objectName != null)
                    return new NamedObjectEventBinding(eventName, handlerName, typeFullName, objectName);

                return new IndexedObjectEventBinding(eventName, handlerName, typeFullName, indices.Reverse().ToArray());
            }

            var indices = new Stack<int>();
            var results = new List<(EventBinding, XAttribute)>();

            void CollectBindings(IEnumerable<XElement> elements)
            {
                for (int i = 0; i < elements.Count(); i++)
                {
                    var element = elements.ElementAt(i);
                    indices.Push(i);

                    var objectName = element.Attribute(NameAttributeName)?.Value;

                    foreach (var attribute in element.Attributes())
                    {
                        var binding = TryGetEventBinding(element, objectName, indices, attribute);
                        if (binding != null)
                            results.Add(new(binding, attribute));
                    }

                    CollectBindings(element.Elements());
                    indices.Pop();
                }
            }

            CollectBindings(document.Root.Elements());
            return results;
        }

        static private string GetTypeName(XName name)
        {
            return name.LocalName;
        }

        static private string GetTypeFullName(XName name)
        {
            return GetTypeNamespaceName(name) + "." + GetTypeName(name);
        }

        static private string GetTypeNamespaceName(XName name)
        {
            var ns = name.NamespaceName;
            if (ns == UINamespace)
                return "Alternet.UI";

            return ParseClrNamespaceFromXmlns(ns);
        }

        static private string GetTypeAssemblyName(XName name)
        {
            var ns = name.NamespaceName;
            if (ns == UINamespace)
                return "Alternet.UI";

            return "<unknown-assembly>";
        }

        static private string ParseClrNamespaceFromXmlns(string ns)
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

        private XDocument Sanitize(XDocument document)
        {
            void RemoveClassAttribute()
            {
                var attribute = document.Root.Attribute(classAttributeName);
                if (attribute == null)
                    throw new Exception(ClassAttributeNotFound);
                attribute.Remove();
            }

            void RemoveEventAttributes()
            {
                var attributes = GetEventBindings(apiInfoProvider, document).Select(x => x.Attribute).ToArray();
                foreach (var attribute in attributes)
                    attribute.Remove();
            }

            RemoveClassAttribute();
            RemoveEventAttributes();

            return document;
        }

        private IEnumerable<NamedObject> GetNamedObjects()
        {
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

        public sealed class NamedObject
        {
            public NamedObject(string typeFullName, string name)
            {
                TypeFullName = typeFullName;
                Name = name;
            }

            public string TypeFullName { get; }

            public string Name { get; }
        }

        public abstract class EventBinding
        {
            protected EventBinding(string eventName, string handlerName, string objectTypeFullName)
            {
                EventName = eventName;
                HandlerName = handlerName;
                ObjectTypeFullName = objectTypeFullName;
            }

            public string EventName { get; }

            public string HandlerName { get; }

            public string ObjectTypeFullName { get; }
        }

        public sealed class NamedObjectEventBinding : EventBinding
        {
            public NamedObjectEventBinding(string eventName, string handlerName, string objectTypeFullName, string objectName) :
                base(eventName, handlerName, objectTypeFullName)
            {
                ObjectName = objectName;
            }

            public string ObjectName { get; }
        }

        public sealed class IndexedObjectEventBinding : EventBinding
        {
            public IndexedObjectEventBinding(string eventName, string handlerName, string objectTypeFullName, IReadOnlyList<int> objectIndices) :
                base(eventName, handlerName, objectTypeFullName)
            {
                ObjectIndices = objectIndices;
            }

            public IReadOnlyList<int> ObjectIndices { get; }
        }
    }
}