using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a delegate that is invoked for each property of a XML object during enumeration.
    /// </summary>
    /// <param name="property">The XML property.</param>
    /// <param name="name">The name of the XML property.</param>
    /// <param name="value">The value of the XML property as a .NET object.</param>
    public delegate void XmlPropertyDelegate(
        XmlElement property,
        string name,
        object? value);

    /// <summary>
    /// Contains static methods and properties related to the xml documents handling.
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// Specifies flags which are used in <see cref="ToXmlString"/> method.
        /// </summary>
        public enum StringFormatFlags
        {
            /// <summary>
            /// No flags.
            /// </summary>
            None = 0,

            /// <summary>
            /// Adds xml declaration with version and UTF-8 encoding.
            /// </summary>
            AddXmlDeclaration = 2,
        }

        /// <summary>
        /// Gets xml file prefix with version and UTF-8 encoding declaration.
        /// </summary>
        public const string XmlFilePrefix = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        /// <summary>
        /// Gets uixml namespace uri.
        /// </summary>
        public const string UIXmlNamespace = "http://schemas.alternetsoft.com/ui/2021/uixml";

        /// <summary>
        /// Enumerates the properties of a XML object and invokes the specified action for each property.
        /// </summary>
        /// <param name="xml">The XML string representing an object.</param>
        /// <param name="action">The action to invoke for each property, with the property name and value.</param>
        public static void EnumerateProperties(
            string xml,
            XmlPropertyDelegate action)
        {
            XmlDocument doc = new();
            doc.LoadXml(xml);
            EnumerateProperties(doc, action);
        }

        /// <summary>
        /// Enumerates the properties of a XML object and invokes the specified action for each property.
        /// </summary>
        /// <param name="element">The XML element representing an object.</param>
        /// <param name="action">The action to invoke for each property, with the property name and value.</param>
        public static void EnumerateProperties(
            XmlElement? element,
            XmlPropertyDelegate action)
        {
            foreach (XmlElement child in element?.Elements() ?? Array.Empty<XmlElement>())
            {
                var name = UnescapeElementName(child.Name);

                if (HasElements(child))
                {
                    if (HasOnlyElements(child, "Item"))
                    {
                        action(child, name, ConvertArrayElement(child));
                    }
                    else
                    {
                        action(child, name, ConvertObjectElement(child));
                    }
                }
                else
                {
                    var value = UnescapeElementValue(child.InnerText);
                    action(child, name, value);
                }
            }

            object? ConvertArrayElement(XmlElement element)
            {
                var elements = element.ChildNodes.OfType<XmlElement>().ToArray();

                var containsObjects = true;

                foreach (var child in elements)
                {
                    if (!HasElements(child))
                    {
                        containsObjects = false;
                        break;
                    }
                }

                if (containsObjects)
                {
                    var bagList = new List<IPropertyBag>();
                    foreach (var child in elements)
                    {
                        var bag = ConvertObjectElement(child);
                        if (bag is IPropertyBag propertyBag)
                            bagList.Add(propertyBag);
                    }

                    return bagList;
                }
                else
                {
                    var valueList = new List<object?>();
                    foreach (var child in elements)
                    {
                        var value = UnescapeElementValue(child.InnerText);
                        valueList.Add(value);
                    }

                    return (IEnumerable)valueList;
                }
            }

            object? ConvertObjectElement(XmlElement element)
            {
                var result = PropertyBag.Create();

                EnumerateProperties(element, (prop, name, value) =>
                {
                    result.SetProperty(name, value);
                });

                return result;
            }

        }

        /// <summary>
        /// Enumerates the properties of a XML document and invokes the specified action for each property.
        /// </summary>
        /// <param name="doc">The XML document representing an object.</param>
        /// <param name="action">The action to invoke for each property, with the property name and value.</param>
        public static void EnumerateProperties(
            XmlDocument doc,
            XmlPropertyDelegate action)
        {
            var root = doc.DocumentElement;
            EnumerateProperties(root, action);
        }

        /// <summary>
        /// Returns the first child element of the specified <see cref="XmlElement"/>,
        /// or null if there are no child elements.
        /// </summary>
        /// <param name="element">The XML element whose first child element is to be returned.</param>
        /// <returns>The first child XML element, or null if there are no child elements.</returns>
        public static XmlElement? FirstElement(XmlElement? element)
        {
            return element?.ChildNodes.OfType<XmlElement>().FirstOrDefault();
        }

        /// <summary>
        /// Determines whether the specified <see cref="XmlElement"/> is a collection element, 
        /// meaning it has multiple child elements with the same name.
        /// </summary>
        /// <param name="element">The XML element to check.</param>
        /// <returns><c>true</c> if the XML element is a collection element; otherwise, <c>false</c>.</returns>
        public static bool IsCollectionElement(XmlElement? element)
        {
            if (element is null)
                return false;
            var childElements = element.ChildNodes.OfType<XmlElement>().ToArray();
            if (childElements.Length == 0)
                return false;
            var firstChildName = childElements[0].Name;
            return childElements.All(e => e.Name == firstChildName);
        }

        /// <summary>
        /// Determines whether the specified <see cref="XmlElement"/> has only child elements with the specified name.
        /// </summary>
        /// <param name="element">The XML element to check for child elements with the specified name.</param>
        /// <param name="name">The name of the child elements to check for.</param>
        /// <returns><c>true</c> if the XML element has only child elements with the specified name;
        /// otherwise, <c>false</c>.</returns>
        public static bool HasOnlyElements(XmlElement? element, string name)
        {
            return element?.ChildNodes.OfType<XmlElement>().All(e => e.Name == name) ?? false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="XmlElement"/> has any child elements.
        /// </summary>
        /// <param name="element">The XML element to check for child elements.</param>
        /// <returns><c>true</c> if the XML element has any child elements; otherwise, <c>false</c>.</returns>
        public static bool HasElements(XmlElement? element)
        {
            return element?.ChildNodes.OfType<XmlElement>().Any() ?? false;
        }

        /// <summary>
        /// Enumerates the child elements of the specified <see cref="XmlElement"/>
        /// and returns them as an enumerable collection.
        /// </summary>
        /// <param name="element">The XML element whose child elements are to be enumerated.</param>
        /// <returns>An enumerable collection of child XML elements.</returns>
        public static IEnumerable<XmlElement> Elements(this XmlElement element)
        {
            return element.ChildNodes.OfType<XmlElement>();
        }

        /// <summary>
        /// Enumerates the child elements of the specified <see cref="XmlElement"/>
        /// </summary>
        /// <param name="element"></param>
        /// <param name="action"></param>
        public static void ForEachElement(
            this XmlElement element,
            Action<XmlElement> action)
        {
            foreach (XmlElement child in Elements(element))
                action(child);
        }

        /// <summary>
        /// Converts a collection of name-value pairs into a XML string representation
        /// and writes it to the specified <see cref="IStructuredTextWriter"/>.
        /// </summary>
        /// <param name="nameValuePairs">The collection of name-value pairs to convert to XML.</param>
        /// <param name="writer">The <see cref="IStructuredTextWriter"/> to write the XML string to.</param>
        /// <param name="flags">Flags that control the behavior of the XML string conversion.</param>
        /// <param name="nodeName">The name of the XML node to use for the root element.
        /// If not specified, no root element will be added.</param>
        public static void ToXmlString(
            string? nodeName,
            IEnumerable<KeyValuePair<string, object?>> nameValuePairs,
            IStructuredTextWriter? writer = null,
            StringFormatFlags flags = StringFormatFlags.None)
        {
            if (writer is null)
                return;

            if (flags.HasFlag(StringFormatFlags.AddXmlDeclaration))
            {
                writer.WriteLine(XmlFilePrefix);
            }

            var hasElement = !string.IsNullOrEmpty(nodeName);

            if (hasElement)
            {
                writer.WriteLine($"<{nodeName}>");
                writer.Indent();
            }

            var pairs = nameValuePairs.ToArray();

            for (int i = 0; i < pairs.Length; i++)
            {
                var propertyName = pairs[i].Key;
                var escapedPropertyName = propertyName;

                var value = pairs[i].Value;
                var isString = value is string;

                if (!isString)
                {
                    if (value is IEnumerable<IPropertyBag> collectionValue)
                    {
                        writer.WriteLine($"<{escapedPropertyName}>");
                        writer.Indent();
                        SerializeXmlArray("Item", collectionValue, writer);
                        writer.Unindent();
                        writer.WriteLine($"</{escapedPropertyName}>");
                        continue;
                    }
                }

                EmitValue(escapedPropertyName, value?.ToString(), writer);
            }

            if (hasElement)
            {
                writer.Unindent();
                writer.WriteLine($"</{nodeName}>");
            }
        }

        /// <summary>
        /// Serializes a collection of items into a Xml array and writes
        /// it to the specified <see cref="IStructuredTextWriter"/>.
        /// </summary>
        /// <param name="itemElementName">The name of the XML element to use for each item.</param>
        /// <param name="items">The collection of items to serialize.</param>
        /// <param name="writer">The <see cref="IStructuredTextWriter"/> to write the XML array to.</param>
        public static void SerializeXmlArray(
            string? itemElementName,
            IEnumerable<IPropertyBag> items,
            IStructuredTextWriter? writer = null)
        {
            if (writer is null)
                return;

            itemElementName ??= "Item";

            var itemsArray = items.Cast<object?>().ToArray();

            for (int i = 0; i < itemsArray.Length; i++)
            {
                var value = itemsArray[i];
                var isString = value is string;

                if (!isString)
                {
                    void SavePropertyBag(IPropertyBag propertyBag)
                    {
                        writer.Indent();
                        propertyBag.ToXmlString(itemElementName, writer);
                        writer.Unindent();
                    }

                    void SavePropertyBagSerialize(IPropertyBagSerialize serialize)
                    {
                        IPropertyBag propertyBag = PropertyBag.Create();
                        serialize.SaveTo(new SerializableObjectContext(propertyBag));
                        SavePropertyBag(propertyBag);
                    }

                    if (value is IPropertyBag propertyBag)
                    {
                        SavePropertyBag(propertyBag);
                        continue;
                    }

                    if (value is IPropertyBagSerialize serialize)
                    {
                        SavePropertyBagSerialize(serialize);
                        continue;
                    }
                }

                EmitValue(itemElementName, value?.ToString(), writer);
            }
        }

        /// <summary>
        /// Emits a XML property with the specified value to the specified <see cref="IStructuredTextWriter"/>.
        /// Depending on the type of the value, it may be emitted with or without quotes.
        /// </summary>
        /// <param name="propName">The name of the XML property.</param>
        /// <param name="value">The value to emit.</param>
        /// <param name="writer">The <see cref="IStructuredTextWriter"/> to write the XML property to.</param>
        public static void EmitValue(string? propName, string? value, IStructuredTextWriter writer)
        {
            EmitUnquotedValue(propName, value?.ToString(), writer);
        }

        /// <summary>
        /// Unescapes the specified XML element name using <see cref="XmlConvert.DecodeName(string)"/>.
        /// </summary>
        /// <param name="name">The name of the XML element to unescape.</param>
        /// <returns>The unescaped XML element name.</returns>
        public static string UnescapeElementName(string name)
        {
            string decoded = XmlConvert.DecodeName(name);
            return decoded;
        }

        /// <summary>
        /// Escapes the specified XML element name using <see cref="XmlConvert.EncodeName(string)"/>.
        /// </summary>
        /// <param name="name">The name of the XML element to escape.</param>
        /// <returns>The escaped XML element name.</returns>
        public static string EscapeElementName(string name)
        {
            string encoded = XmlConvert.EncodeName(name);
            return encoded;
        }

        /// <summary>
        /// Escapes the specified XML element value.
        /// </summary>
        /// <param name="name">The value of the XML element to escape.</param>
        /// <returns>The escaped XML element value.</returns>
        public static string EscapeElementValue(string? name)
        {
            if (name is null || name.Length == 0)
                return string.Empty;

            string encoded = SecurityElement.Escape(name);
            return encoded;
        }

        /// <summary>
        /// Unescapes the specified XML element value.
        /// </summary>
        /// <param name="name">The value of the XML element to unescape.</param>
        /// <returns>The unescaped XML element value.</returns>
        public static string UnescapeElementValue(string? name)
        {
            if (name is null || name.Length == 0)
                return string.Empty;

            string decoded = WebUtility.HtmlDecode(name);
            return decoded;
        }

        /// <summary>
        /// Emits a XML property with an unquoted value to the specified <see cref="IStructuredTextWriter"/>.
        /// </summary>
        /// <param name="propName">The name of the XML property.</param>
        /// <param name="value">The value to emit.</param>
        /// <param name="writer">The <see cref="IStructuredTextWriter"/> to write the XML property to.</param>
        public static void EmitUnquotedValue(string? propName, string? value, IStructuredTextWriter writer)
        {
            if (propName is null)
                return;

            propName = EscapeElementName(propName);

            var escapedValue = EscapeElementValue(value);
            writer.WriteLine($"<{propName}>{escapedValue}</{propName}>");
        }

        /// <summary>
        /// Creates <see cref="XmlWriter"/> using the specified <see cref="StringBuilder"/>
        /// and optional <see cref="XmlWriterSettings"/>.
        /// </summary>
        /// <param name="builder"><see cref="StringBuilder"/> object.</param>
        /// <param name="settings">Settings.</param>
        /// <returns></returns>
        public static XmlWriter CreateWriter(StringBuilder builder, XmlWriterSettings? settings = null)
        {
            if (settings is null)
                return XmlWriter.Create(builder);
            else
                return XmlWriter.Create(builder, settings);
        }

        /// <summary>
        /// Saves <see cref="StringBuilder"/> to the specified stream using UTF8 format.
        /// </summary>
        /// <param name="builder"><see cref="StringBuilder"/> object.</param>
        /// <param name="stream">Output stream.</param>
        public static void Save(
            StringBuilder builder,
            Stream stream)
        {
            string s = builder.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Saves xml document to stream using the specified settings.
        /// </summary>
        /// <param name="document">Xml document.</param>
        /// <param name="stream">Output stream.</param>
        /// <param name="settings">Save settings. Optional.</param>
        public static void Save(
            XmlDocument document,
            Stream stream,
            XmlWriterSettings? settings = null)
        {
            StringBuilder builder = new();
            using XmlWriter writer = CreateWriter(builder, settings);
            document.Save(writer);
            Save(builder, stream);
        }

        /// <summary>
        /// Gets attribute value of the specified <see cref="XmlNode"/>.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="name">Attribute name.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string? GetAttrValue(XmlNode node, string name)
        {
            return node.Attributes?.GetNamedItem(name)?.Value;
        }

        /// <summary>
        /// Gets whether attribute with the specified name exists.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="name">Attribute name.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAttr(XmlNode node, string name)
        {
            return node.Attributes?.GetNamedItem(name) is not null;
        }

        /// <summary>
        /// Removes attribute if it exists.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="name">Attribute name.</param>
        /// <returns><c>true</c> if attribute was found and removed;
        /// <c>false</c> otherwise.</returns>
        public static bool RemoveAttr(XmlNode node, string name)
        {
            if (node.Attributes is null)
                return false;
            var attrNode = node.Attributes.GetNamedItem(name);
            if (attrNode == null)
                return false;
            node.Attributes.Remove((XmlAttribute)attrNode);
            return true;
        }

        /// <summary>
        /// Makes string with xml data indented.
        /// </summary>
        /// <param name="xml">String with xml data.</param>
        /// <param name="newLineOnAttributes">Whether to add new line
        /// on every attr declaration. Optional.</param>
        /// <returns></returns>
        public static string IndentXml(string xml, bool newLineOnAttributes = false)
        {
            XElement element = XElement.Parse(xml);
            XmlWriterSettings settings = new()
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = newLineOnAttributes,
            };

            StringBuilder builder = new();
            using XmlWriter writer = XmlWriter.Create(builder, settings);
            element.Save(writer);

            return builder.ToString();
        }

        /// <summary>
        /// Loads xml document from the specified stream.
        /// </summary>
        /// <param name="document">Xml document.</param>
        /// <param name="stream">Input stream.</param>
        public static void Load(XmlDocument document, Stream stream)
        {
            using StreamReader reader = new(stream);
            document.Load(reader);
        }

        /// <summary>
        /// Saves xml document to stream with pretty indentation.
        /// </summary>
        /// <param name="document">Xml document.</param>
        /// <param name="stream">Output stream.</param>
        public static void SaveIndented(XmlDocument document, Stream stream)
        {
            XmlWriterSettings xmlWriterSettings = new()
            {
                OmitXmlDeclaration = true,
                Indent = true,
                NewLineOnAttributes = true,
            };

            Save(document, stream, xmlWriterSettings);
        }

        /// <summary>
        /// Deserializes object from the specified string with xml data.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized object.</typeparam>
        /// <param name="text">String with xml data.</param>
        /// <returns></returns>
        public static T? DeserializeFromString<T>(string text)
        {
            using TextReader reader = new StringReader(text);
            return (T?)new XmlSerializer(typeof(T)).Deserialize(reader);
        }

        /// <summary>
        /// Deserializes object from the specified stream.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized object.</typeparam>
        /// <param name="stream">Stream with xml data.</param>
        /// <returns></returns>
        public static T? Deserialize<T>(Stream stream)
        {
            return (T?)new XmlSerializer(typeof(T)).Deserialize(stream);
        }

        /// <summary>
        /// Deserializes object from the specified file.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized object.</typeparam>
        /// <param name="filename">Path to file with xml data.</param>
        /// <returns></returns>
        public static T? DeserializeFromFile<T>(string filename)
        {
            if (!FileSystem.FileExists(filename))
                return default;

            using var stream = FileSystem.OpenRead(filename);
            return Deserialize<T>(stream);
        }

        /// <summary>
        /// Serializes object to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer"><see cref="TextWriter"/> object.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns></returns>
        public static void SerializeObject(TextWriter writer, object obj)
        {
            new XmlSerializer(obj.GetType()).Serialize(writer, obj);
        }

        /// <summary>
        /// Creates <see cref="XmlAttributeOverrides"/> for use with
        /// <see cref="XmlSerializer"/>.
        /// </summary>
        /// <param name="type">Type for which property name overrides are registered.</param>
        /// <param name="overrides"><see cref="XmlAttributeOverrides"/> where
        /// to add override declarations.</param>
        /// <param name="decl">Override declarations.</param>
        /// <returns></returns>
        public static void AddPropertyNameOverrides(
            XmlAttributeOverrides overrides,
            Type type,
            params NameValue<string>[] decl)
        {
            foreach (var item in decl)
            {
                XmlElementAttribute myElementAttribute = new();
                myElementAttribute.ElementName = item.Value;
                XmlAttributes myAttributes = new();
                myAttributes.XmlElements.Add(myElementAttribute);
                overrides.Add(type, item.Name, myAttributes);
            }
        }

        /// <summary>
        /// Serializes object to the specified stream.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <returns></returns>
        /// <param name="stream">Stream to which object is serialized.</param>
        /// <param name="encoding">Encoding of the stream. Optional. If not specified, uses
        /// UTF8 encoding.</param>
        public static void SerializeObjectToStream(
            Stream stream,
            object obj,
            Encoding? encoding = null)
        {
            var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);
            SerializeObject(writer, obj);
            writer.Flush();
        }

        /// <summary>
        /// Serializes object to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <typeparam name="T">Type of the serialized object.</typeparam>
        /// <param name="writer"><see cref="TextWriter"/> object.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns></returns>
        public static void Serialize<T>(TextWriter writer, T obj)
        {
            new XmlSerializer(typeof(T)).Serialize(writer, obj);
        }

        /// <summary>
        /// Serializes object to the specified file.
        /// </summary>
        /// <typeparam name="T">Type of the serialized object.</typeparam>
        /// <param name="filename">Path to file.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns></returns>
        public static void SerializeToFile<T>(string filename, T obj)
        {
            var writer = new StreamWriter(filename);
            try
            {
                Serialize(writer, obj);
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// Serializes object to the specified file.
        /// </summary>
        /// <param name="filename">Path to file.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="encoding">Encoding of the file. Optional. If not specified, uses
        /// UTF8 encoding.</param>
        /// <returns></returns>
        public static void SerializeObjectToFile(
            string filename,
            object obj,
            Encoding? encoding = null)
        {
            var writer = new StreamWriter(filename, append: false, encoding ?? Encoding.UTF8);
            try
            {
                SerializeObject(writer, obj);
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// Deserializes object from the specified file without throwing exceptions.
        /// </summary>
        /// <typeparam name="T">Type of the deserialized object.</typeparam>
        /// <param name="filename">Path to file with xml data.</param>
        /// <returns></returns>
        public static T? Deserialize<T>(string filename)
        {
            try
            {
                return DeserializeFromFile<T>(filename);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Serializes object to the specified file without throwing exceptions.
        /// </summary>
        /// <typeparam name="T">Type of the serialized object.</typeparam>
        /// <param name="filename">Path to file.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns></returns>
        public static bool Serialize<T>(string filename, T obj)
        {
            try
            {
                SerializeToFile(filename, obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calls specified action for the each child node of the xml document.
        /// Root node is ignored.
        /// </summary>
        /// <param name="document">Xml document.</param>
        /// <param name="nodeAction">Action to call.</param>
        /// <param name="userData">User data passed to the node action.</param>
        /// <returns></returns>
        public static bool ForEachChild(
            XmlDocument document,
            Func<XmlNode, object?, bool>? nodeAction,
            object? userData = null)
        {
            if (nodeAction is null || document.DocumentElement is null)
                return false;

            return ProcessRootNode(document.DocumentElement);

            bool ProcessRootNode(XmlNode node)
            {
                bool result = false;

                foreach (XmlNode child in node)
                {
                    if (ProcessNode(child))
                    {
                        result = true;
                    }
                }

                return result;
            }

            bool ProcessNode(XmlNode node)
            {
                bool result = nodeAction(node, userData);

                foreach (XmlNode child in node)
                {
                    if (ProcessNode(child))
                    {
                        result = true;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets node attributes as an array of <see cref="XmlAttribute"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static XmlAttribute[] GetAttributes(XmlNode node)
        {
            var attr = node.Attributes;
            if (attr is null)
                return Array.Empty<XmlAttribute>();
            var result = new XmlAttribute[attr.Count];
            attr.CopyTo(result, 0);
            return result;
        }

        /// <summary>
        /// Converts xml data using the specified parameters.
        /// </summary>
        /// <param name="fromStream">Stream with xml data.</param>
        /// <param name="prm">Conversion parameters.</param>
        /// <returns></returns>
        public static Stream ConvertXml(Stream fromStream, XmlStreamConvertParams prm)
        {
            XmlDocument xDoc = new();
            XmlUtils.Load(xDoc, fromStream);

            bool convertPerformed = false;

            if (prm.RootNodeAction is not null && xDoc.DocumentElement is not null)
            {
                convertPerformed = prm.RootNodeAction(xDoc.DocumentElement, prm.UserData);
            }

            if (ForEachChild(xDoc, prm.NodeAction, prm.UserData))
                convertPerformed = true;

            prm.ConvertPerformed = convertPerformed;

            if (convertPerformed || !fromStream.CanSeek)
            {
                var memoryStream = new MemoryStream();
                XmlUtils.Save(xDoc, memoryStream, prm.WriterSettings);
                memoryStream.Position = 0L;
                return memoryStream;
            }
            else
            {
                fromStream.Position = 0L;
                return fromStream;
            }
        }

        /// <summary>
        /// Specifies parameters for <see cref="ConvertXml"/> method.
        /// </summary>
        public class XmlStreamConvertParams
        {
            /// <summary>
            /// Gets or sets function which is called for the root node.
            /// </summary>
            public Func<XmlNode, object?, bool>? RootNodeAction;

            /// <summary>
            /// Gets or sets function which is called for the every node of the xml document.
            /// </summary>
            public Func<XmlNode, object?, bool>? NodeAction;

            /// <summary>
            /// Gets or sets xml writer settings.
            /// </summary>
            public XmlWriterSettings? WriterSettings;

            /// <summary>
            /// Gets or sets <see cref="NodeAction"/> result.
            /// </summary>
            public bool ConvertPerformed;

            /// <summary>
            /// Gets or sets user data. This field can be used for any purposes.
            /// </summary>
            public object? UserData;

            /// <summary>
            /// Initializes a new instance of the <see cref="XmlStreamConvertParams"/> class.
            /// </summary>
            /// <param name="nodeAction">Function to call for the every node
            /// of the xml document.</param>
            /// <param name="writerSettings">Xml writer settings.</param>
            public XmlStreamConvertParams(
                Func<XmlNode, object?, bool> nodeAction,
                XmlWriterSettings? writerSettings = null)
            {
                NodeAction = nodeAction;
                WriterSettings = writerSettings;
            }
        }
    }
}
