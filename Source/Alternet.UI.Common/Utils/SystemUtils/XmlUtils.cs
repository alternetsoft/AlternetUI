using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the xml documents handling.
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// Gets uixml namespace uri.
        /// </summary>
        public const string UIXmlNamespace = "http://schemas.alternetsoft.com/ui/2021/uixml";

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
            foreach(var item in decl)
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
