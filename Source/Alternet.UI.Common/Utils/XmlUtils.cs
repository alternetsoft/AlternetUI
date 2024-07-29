using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to the xml documents handling.
    /// </summary>
    public static class XmlUtils
    {
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
        public static void Save(StringBuilder builder, Stream stream)
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
        public static void Save(XmlDocument document, Stream stream, XmlWriterSettings? settings = null)
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
        public static bool HasAttr(XmlNode node, string name)
        {
            return node.Attributes?.GetNamedItem(name) is not null;
        }

        /// <summary>
        /// Removes attribute if it exists.
        /// </summary>
        /// <param name="node">Xml node.</param>
        /// <param name="name">Attribute name.</param>
        /// <returns><c>true</c> if attribute was found and removed, <c>false</c> otherwise.</returns>
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
    }
}
