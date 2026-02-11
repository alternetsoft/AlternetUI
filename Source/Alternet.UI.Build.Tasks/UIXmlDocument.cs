using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Alternet.UI.Build.Tasks
{
    public class UIXmlDocument
    {
        private const string EditorNamespace = "http://schemas.alternetsoft.com/editor/2024";
        private const string UINamespace = "http://schemas.alternetsoft.com/ui/2021";
        private const string UIXmlNamespace = "http://schemas.alternetsoft.com/ui/2021/uixml";
        private const string DebuggerUINamespace = "http://schemas.alternetsoft.com/debuggerui/2024";

        private const string ClassAttributeNotFound = "x:Class attribute on root node was not found.";
        private const string NameAttributeName = "Name";
        private static readonly XName classAttributeName = (XNamespace)UIXmlNamespace + "Class";
        private readonly Stream xmlContent;
        private readonly string xmlContentAsString;
        private readonly ApiInfoProvider apiInfoProvider;
        private readonly XDocument document;

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

            xmlContentAsString = new StreamReader(xmlContent, System.Text.Encoding.UTF8).ReadToEnd();

            this.apiInfoProvider = apiInfoProvider;
            document = XDocument.Parse(xmlContentAsString);

            xmlContent.Position = 0;
        }

        public XDocument SanitizedDocument => sanitizedDocument ??= Sanitize(XDocument.Load(xmlContent));

        public string BaseClassFullName => baseClassFullName ??= GetBaseClassFullName();

        public string ClassName => className ??= GetClassName();

        public string ClassNamespaceName => classNamespaceName ??= GetClassNamespaceName();

        public string ResourceName { get; }

        public string XmlContentAsString
        {
            get
            {
                return xmlContentAsString;
            }
        }

        public IReadOnlyList<NamedObject> NamedObjects => namedObjects ??= GetNamedObjects().ToArray();

        public IReadOnlyList<EventBinding> EventBindings
            => eventBindings ??= GetEventBindings(apiInfoProvider, document)
            .Select(x => x.Binding).ToArray();

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

        public abstract record AccessorInfo;
        public record IndexAccessorInfo(int Index, string CollectionName) : AccessorInfo;
        public record MemberAccessorInfo(string Name) : AccessorInfo;

        private static IEnumerable<(EventBinding Binding, XAttribute Attribute)> GetEventBindings(
            ApiInfoProvider apiInfoProvider,
            XDocument document)
        {
            EventBinding? TryGetEventBinding(
                XElement element,
                string? objectName,
                Stack<AccessorInfo> accessors,
                XAttribute attribute)
            {
                var assemblyName = GetTypeAssemblyName(element.Name);
                var typeFullName = GetTypeFullName(element.Name);
                string eventName = attribute.Name.LocalName;
                if (!apiInfoProvider.IsEvent(assemblyName, typeFullName, eventName))
                    return null;

                var handlerName = attribute.Value;

                if (objectName != null)
                    return new NamedObjectEventBinding(eventName, handlerName, typeFullName, objectName);

                return new IndexedObjectEventBinding(
                    eventName,
                    handlerName,
                    typeFullName,
                    accessors.Reverse().ToArray());
            }

            var accessors = new Stack<AccessorInfo>();
            var results = new List<(EventBinding, XAttribute)>();
            const string DefaultCollectionName = "ContentElements";

            void CollectBindings(IEnumerable<XElement> elements, string collectionName)
            {
                int index = 0;
                for (int i = 0; i < elements.Count(); i++)
                {
                    var element = elements.ElementAt(i);
                    var indexOfDot = element.Name.LocalName.IndexOf(".");
                    bool propertyAccess = indexOfDot != -1; // cases like <Grid.RowDefinitions>
                    
                    if (propertyAccess)
                    {
                        accessors.Push(
                            new MemberAccessorInfo(element.Name.LocalName.Substring(indexOfDot + 1)));
                    }
                    else
                    {
                        accessors.Push(new IndexAccessorInfo(index, collectionName));
                        index++;
                    }

                    var objectName = element.Attribute(NameAttributeName)?.Value;

                    foreach (var attribute in element.Attributes())
                    {
                        var binding = TryGetEventBinding(element, objectName, accessors, attribute);
                        if (binding != null)
                            results.Add(new(binding, attribute));
                    }

                    if (propertyAccess)
                    {
                        var childElements = element.Elements();
                        if (childElements.Count() == 1)
                            CollectBindings(childElements.Single().Elements(), DefaultCollectionName);
                        else
                            CollectBindings(childElements, "");
                    }
                    else
                    {
                        CollectBindings(element.Elements(), DefaultCollectionName);
                    }

                    accessors.Pop();
                }
            }

            CollectBindings(document.Root.Elements(), DefaultCollectionName);
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
            if(ns == EditorNamespace)
                return "Alternet.Editor.AlternetUI";
            if (ns == DebuggerUINamespace)
                return "Alternet.Scripter.Debugger.UI.AlternetUI";

            return ParseClrNamespaceFromXmlns(ns);
        }

        static private string GetTypeAssemblyName(XName name)
        {
            var ns = name.NamespaceName;
            if (ns == UINamespace)
                return "Alternet.UI.Common";
            if (ns == EditorNamespace)
                return "Alternet.Editor.AlterNetUI.v10";
            if (ns == DebuggerUINamespace)
                return "Alternet.Scripter.Debugger.UI.AlternetUI";

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
                var attribute = document.Root.Attribute(classAttributeName)
                    ?? throw new Exception(ClassAttributeNotFound);
                attribute.Remove();
            }

            void RemoveEventAttributes()
            {
                var attributes
                    = GetEventBindings(apiInfoProvider, document).Select(x => x.Attribute).ToArray();
                foreach (var attribute in attributes)
                    attribute.Remove();
            }

            RemoveClassAttribute();
            RemoveEventAttributes();

            return document;
        }

        private IEnumerable<NamedObject> GetNamedObjects()
        {
            var namedElements = document.Root.DescendantsAndSelf()
                .Where(e => e.Attributes().Any(a => a.Name == NameAttributeName));
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
            var attribute = document.Root.Attribute(classAttributeName)
                ?? throw new Exception(ClassAttributeNotFound);
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
            public IndexedObjectEventBinding(string eventName, string handlerName, string objectTypeFullName, IReadOnlyList<AccessorInfo> accessors) :
                base(eventName, handlerName, objectTypeFullName)
            {
                Accessors = accessors;
            }

            public IReadOnlyList<AccessorInfo> Accessors { get; }
        }
    }
}