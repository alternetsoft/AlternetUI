using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements a basic data transfer mechanism.
    /// </summary>
    [DebuggerDisplay("{ToDebugString()}")]
    public class DataObject : IDataObject
    {
        /// <summary>
        /// Gets an empty <see cref="DataObject"/>.
        /// </summary>
        public static readonly DataObject Empty = new EmptyDataObject();

        /// <summary>
        /// Occurs when a type resolution is requested for a given type name.
        /// </summary>
        public static Func<string, Type?>? TypeResolveRequested;

        private readonly Dictionary<string, object> data = new(StringComparer.Ordinal);

        static DataObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DataObject"/> class.
        /// </summary>
        public DataObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DataObject"/> class with
        /// the specified data, along with one or more specified data formats.
        /// The data format is specified by a string.
        /// </summary>
        /// <param name="format">
        /// A string that specifies what format to store the data in.
        /// See the <see cref="DataFormats"/> class for a set of pre-defined
        /// data formats.
        /// </param>
        /// <param name="data">
        /// The data to store in this data object.
        /// </param>
        public DataObject(string format, object data)
        {
            if (format is null)
                throw new ArgumentNullException(nameof(format));
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            SetData(format, data);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DataObject"/> class with
        /// the specified data in this data object, automatically converting the
        /// data format from the source object type.
        /// </summary>
        /// <param name="data">
        /// The data to store in this data object.
        /// </param>
        public DataObject(object data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            SetData(data);
        }

        /// <summary>
        /// Occurs when <see cref="DataObject"/> is serialized to the stream
        /// using <see cref="DataFormats.Serializable"/> format.
        /// </summary>
        public static event EventHandler<SerializeDataObjectEventArgs>? GlobalSerializeDataObject;

        /// <summary>
        /// Occurs when <see cref="DataObject"/> is deserialized from the stream
        /// using <see cref="DataFormats.Serializable"/> format.
        /// </summary>
        public static event EventHandler<SerializeDataObjectEventArgs>? GlobalDeserializeDataObject;

        /// <summary>
        /// Gets a value indicating whether the data object contains data in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public virtual bool ContainsFiles => GetDataPresent(DataFormats.Files);

        /// <summary>
        /// Gets a value indicating whether the data object contains data in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public virtual bool ContainsBitmap => GetDataPresent(DataFormats.Bitmap);

        /// <summary>
        /// Gets a value indicating whether the data object contains data in the
        /// <see cref="DataFormats.Text"/> format.
        /// </summary>
        public virtual bool ContainsText => GetDataPresent(DataFormats.Text);

        /// <summary>
        /// Gets object properties formatted for debug purposes.
        /// </summary>
        public static string ToDebugString(IDataObject value)
        {
            var result = new StringBuilder();

            var allFormats = string.Join(", ", value.GetFormats());

            result
                .AppendLine("All Formats: " + allFormats)
                .AppendLine();

            if (value.GetDataPresent(DataFormats.Serializable))
            {
                var serializableData = value.GetData(DataFormats.Serializable);
                result.AppendLine(
                    $"[Serializable {serializableData?.GetType()}]: {serializableData}");
            }

            if (value.GetDataPresent(DataFormats.Text))
                result.AppendLine("[Text]: " + value.GetData(DataFormats.Text));

            if (value.GetDataPresent(DataFormats.Files))
            {
                var data = value.GetData(DataFormats.Files);

                if (data is not string[] files || files.Length == 0)
                    result.AppendLine("[Files]: EMPTY");
                else
                {
                    result.AppendLine("[Files]:");
                    for (int i = 0; i < files.Length; i++)
                    {
                        result.AppendLine($"    [{i}] => {files[i]}");
                    }
                }
            }

            if (value.GetDataPresent(DataFormats.Bitmap))
            {
                var data = value.GetData(DataFormats.Bitmap);
                var bitmap = data as Image;
                result.AppendLine($"[Bitmap]: {bitmap?.Size}");
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts a data object to a memory stream containing its XML representation.
        /// </summary>
        /// <param name="dataObject">The data object to serialize to XML.</param>
        /// <param name="settings">Optional XML writer settings to customize the serialization process.</param>
        /// <returns>A memory stream containing the XML representation of the data object.</returns>
        public static MemoryStream DataObjectToXmlMemoryStream(
            object? dataObject,
            XmlWriterSettings? settings = null)
        {
            MemoryStream stream = new();
            SerializeDataObject(stream, dataObject, settings);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// Converts a data object to its XML string representation.
        /// </summary>
        /// <param name="dataObject">The data object to serialize to XML.</param>
        /// <param name="settings">Optional XML writer settings to customize the serialization process.</param>
        /// <returns>A string containing the XML representation of the data object, or null
        /// if the data object is null.</returns>
        public static string? DataObjectToXmlString(
            object? dataObject,
            XmlWriterSettings? settings = null)
        {
            var stream = DataObjectToXmlMemoryStream(dataObject, settings);
            var str = StreamUtils.StringFromStreamOrNull(stream);
            return str;
        }

        /// <summary>
        /// Converts a data object to its XML string representation enclosed in a CDATA section.
        /// </summary>
        /// <param name="dataObject">The data object to serialize to XML.</param>
        /// <param name="settings">Optional XML writer settings to customize the serialization process.</param>
        /// <returns>A string containing the XML representation of the data object enclosed
        /// in a CDATA section, or null if the data object is null.</returns>
        public static string DataObjectToCDataXmlString(
            object? dataObject,
            XmlWriterSettings? settings = null)
        {
            var str = DataObjectToXmlString(dataObject, settings);
            var strEnclosed = StringUtils.WithCDATA(str);
            return strEnclosed;
        }

        /// <summary>
        /// Serializes data object to the specified stream. This method is called
        /// when serialization with <see cref="DataFormats.Serializable"/> format
        /// is performed.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="GlobalSerializeDataObject"/> event
        /// if it is specified or uses default serialization method.
        /// </remarks>
        /// <param name="dataObject">Data object to serialize.</param>
        /// <param name="stream">Stream where to serialize.</param>
        /// <param name="settings">Writer settings.</param>
        public static void SerializeDataObject(
            Stream stream,
            object? dataObject,
            XmlWriterSettings? settings = null)
        {
            if (dataObject is null)
                return;

            if(dataObject is IDictionary dictionary)
            {
                var xmlDictionaryItems = new XmlDictionaryItems(dictionary, settings);
                dataObject = xmlDictionaryItems;
            }

            if (GlobalSerializeDataObject is not null)
            {
                SerializeDataObjectEventArgs e = new(dataObject, stream);

                GlobalSerializeDataObject?.Invoke(null, e);
                if (e.Handled)
                    return;
            }

            XmlRootAttribute xRoot = new();
            xRoot.ElementName = dataObject.GetType().FullName ?? dataObject.GetType().Name;
            xRoot.Namespace = XmlUtils.UIXmlNamespace;
            xRoot.IsNullable = false;

            if(settings is null)
            {
                var writer = new StreamWriter(stream, Encoding.UTF8);
                new XmlSerializer(dataObject.GetType(), xRoot).Serialize(writer, dataObject);
            }
            else
            {
                settings.Encoding = Encoding.UTF8;
                using XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
                new XmlSerializer(dataObject.GetType(), xRoot).Serialize(xmlWriter, dataObject);
            }
        }

        /// <summary>
        /// This is internal method for testing purposes.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TestHashTableToClipboard()
        {
            var sampleTable = new Hashtable
            {
                { 1, "Apple" },
                { 2, "Banana" },
                { 3, "Cherry" },
                { "Key4", 42 },
                { "Key5", true },
            };

            App.DoInsideBusyCursor(() =>
            {
                Clipboard.SetData(DataFormats.Serializable, sampleTable);
                App.Log("Clipboard.SetData Done");
            });
        }

        /// <summary>
        /// This is internal method for testing purposes.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TestHashTableFromClipboard()
        {
#pragma warning disable
            App.DoInsideBusyCursor(() =>
            {
                var data = Clipboard.GetData(DataFormats.Serializable);
                App.Log("Clipboard.GetData Done");
            });
#pragma warning restore
        }

        /// <summary>
        /// Deserializes a data object from its XML string representation.
        /// </summary>
        /// <param name="s">The XML string representation of the data object.</param>
        /// <returns>The deserialized data object, or null if the input string
        /// is null or deserialization fails.</returns>
        public static object? DeserializeDataObjectFromXmlString(string? s)
        {
            if (s is null)
                return null;
            MemoryStream stream = new();
            StreamUtils.StringToStream(stream, s);
            stream.Seek(0, SeekOrigin.Begin);
            var result = DeserializeDataObject(stream);
            return result;
        }

        /// <summary>
        /// Finds a type by its name, resolving it using the provided type resolver
        /// (<see cref="TypeResolveRequested"/>) or the default type resolver.
        /// </summary>
        /// <param name="typeName">The name of the type to find.</param>
        /// <returns>The resolved <see cref="Type"/> if found; otherwise, <see langword="null"/>.</returns>
        public static Type? FindTypeByName(string typeName)
        {
            if (typeName is null)
                return null;

            typeName = typeName.Replace("_x002B_", "+");

            var result = TypeResolveRequested?.Invoke(typeName);
            result ??= UixmlLoader.FindType(typeName);

            return result;
        }

        /// <summary>
        /// Deserializes data object from the specified stream. This method is called
        /// when deserialization with <see cref="DataFormats.Serializable"/> format
        /// is performed.
        /// </summary>
        /// <remarks>
        /// This method calls <see cref="GlobalDeserializeDataObject"/> event
        /// if it is specified or uses default deserialization method.
        /// </remarks>
        /// <param name="stream">Stream with data for the deserialization.</param>
        public static object? DeserializeDataObject(Stream stream)
        {
            try
            {
                return Internal();
            }
            catch (Exception e)
            {
                App.LogError(e);
                return null;
            }

            object? Internal()
            {
                if (stream is null)
                    return null;

                if (GlobalDeserializeDataObject is not null)
                {
                    SerializeDataObjectEventArgs e = new(null, stream);

                    GlobalDeserializeDataObject?.Invoke(null, e);
                    if (e.Handled)
                        return e.Data;
                }

                Type? type = null;

                var memoryStream = StreamUtils.CreateMemoryStream(stream);
                var reader = new StreamReader(memoryStream, Encoding.UTF8);
                var xmlReader = XmlReader.Create(reader);
                xmlReader.MoveToContent();

                while (!xmlReader.EOF && xmlReader.ReadState == ReadState.Interactive)
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        var uri = xmlReader.NamespaceURI;

                        if (uri == XmlUtils.UIXmlNamespace)
                        {
                            var typeName = xmlReader.Name;
                            type = FindTypeByName(typeName);
                            break;
                        }
                    }

                    xmlReader.Read();
                }

                if (type is null)
                    return null;

                XmlRootAttribute xRoot = new();
                xRoot.ElementName = type.FullName ?? type.Name;
                xRoot.Namespace = XmlUtils.UIXmlNamespace;
                xRoot.IsNullable = false;

                memoryStream.Seek(0, SeekOrigin.Begin);
                reader = new StreamReader(memoryStream, Encoding.UTF8);
                var data = new XmlSerializer(type, xRoot).Deserialize(reader);

                if(data is XmlDictionaryItems xmlDictionaryItems)
                {
                    data = xmlDictionaryItems.AsDictionary;
                }

                return data;
            }
        }

        /// <summary>
        /// Gets this object properties formatted for debug purposes.
        /// </summary>
        /// <returns></returns>
        public string ToDebugString()
        {
            return ToDebugString(this);
        }

        /// <inheritdoc/>
        public virtual object? GetData(string format)
        {
            if (format is null)
                return null;

            if (!data.TryGetValue(format, out var value))
                return null;
            return value;
        }

        /// <inheritdoc/>
        public virtual bool GetDataPresent(string format)
        {
            if (format is null)
                return false;

            return data.ContainsKey(format);
        }

        /// <inheritdoc/>
        public virtual string[] GetFormats()
        {
            return data.Keys.Cast<string>().ToArray();
        }

        /// <inheritdoc/>
        public virtual void SetData(string format, object data)
        {
            if (format is null)
                throw new ArgumentNullException(nameof(format));
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            this.data[format] = ClipboardUtils.SetDataTransform(format, data);
        }

        /// <inheritdoc/>
        public virtual void SetData(object data)
        {
            data ??= string.Empty;
            SetData(ClipboardUtils.DetectFormatFromData(data), data);
        }

        /// <summary>
        /// Retrieves an array of file names from the data object in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public virtual string[]? GetFiles()
        {
            return GetData(DataFormats.Files) as string[];
        }

        /// <summary>
        /// Retrieves bitmap data from the data object in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public virtual Image? GetBitmap()
        {
            return GetData(DataFormats.Bitmap) as Image;
        }

        /// <summary>
        /// Retrieves text data from the data object in the
        /// <see cref="DataFormats.Text"/> format.
        /// </summary>
        public virtual string? GetText()
        {
            return GetData(DataFormats.Text) as string;
        }

        /// <summary>
        /// Adds an array of file names to the data object in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public virtual void SetFiles(params string[] value)
        {
            SetData(DataFormats.Files, value);
        }

        /// <summary>
        /// Adds a bitmap to the data object in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public virtual void SetBitmap(Image value)
        {
            SetData(DataFormats.Bitmap, value);
        }

        /// <summary>
        /// Adds a text string to the data object in the
        /// <see cref="DataFormats.Text"/> format.
        /// </summary>
        public virtual void SetText(string value)
        {
            SetData(DataFormats.UnicodeText, value);
        }

        /// <summary>
        /// Stores text data in this data object. The format of the text
        /// data to store is specified
        /// with a member of <see cref="TextDataFormat" />.</summary>
        /// <param name="textData">A string that contains the text data to store
        /// in the data object.</param>
        /// <param name="format">A member of <see cref="TextDataFormat" /> that specifies the
        /// text data format to store.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="textData" /> is <see langword="null" />.</exception>
        public virtual void SetText(string textData, TextDataFormat format)
        {
            textData ??= string.Empty;

            if (!DataFormats.IsValidTextDataFormat(format))
            {
                throw new InvalidEnumArgumentException(
                    nameof(format),
                    (int)format,
                    typeof(TextDataFormat));
            }

            SetData(DataFormats.ConvertToDataFormats(format), textData);
        }

        /// <inheritdoc/>
        public virtual bool HasFormat(ClipboardDataFormatId format)
        {
            switch (format)
            {
                case ClipboardDataFormatId.UnicodeText:
                case ClipboardDataFormatId.Text:
                case ClipboardDataFormatId.OemText:
                    return ContainsText;
                case ClipboardDataFormatId.Filename:
                    return ContainsFiles;
                case ClipboardDataFormatId.Dib:
                    return ContainsBitmap || GetDataPresent(DataFormats.Dib);
                case ClipboardDataFormatId.Bitmap:
                    return ContainsBitmap;
                case ClipboardDataFormatId.EnhMetaFile:
                    return GetDataPresent(DataFormats.EnhancedMetafile);
                case ClipboardDataFormatId.MetaFile:
                    return GetDataPresent(DataFormats.MetafilePicture);
                case ClipboardDataFormatId.Dif:
                    return GetDataPresent(DataFormats.Dif);
                case ClipboardDataFormatId.Tiff:
                    return GetDataPresent(DataFormats.Tiff);
                case ClipboardDataFormatId.Palette:
                    return GetDataPresent(DataFormats.Palette);
                case ClipboardDataFormatId.PenData:
                    return GetDataPresent(DataFormats.PenData);
                case ClipboardDataFormatId.Riff:
                    return GetDataPresent(DataFormats.Riff);
                case ClipboardDataFormatId.Wave:
                    return GetDataPresent(DataFormats.WaveAudio);
                case ClipboardDataFormatId.Locale:
                    return GetDataPresent(DataFormats.Locale);
                case ClipboardDataFormatId.Html:
                    return GetDataPresent(DataFormats.Html);
                case ClipboardDataFormatId.Png:
                    return ContainsBitmap;
                case ClipboardDataFormatId.Sylk:
                    return GetDataPresent(DataFormats.SymbolicLink);
                case ClipboardDataFormatId.Private:
                    return data.Keys.Count > 0;
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        public virtual bool HasFormat(string format)
        {
            return GetDataPresent(format);
        }

        /// <summary>
        /// Represents a collection of dictionary items prepared for the serialized into XML format.
        /// Used in <see cref="SerializeDataObject"/> for <see cref="IDictionary"/> serialization.
        /// </summary>
        [XmlRoot("XmlDictionaryItems")]
        public class XmlDictionaryItems
        {
            private const string NullableKey = "NullValue-71352CE74DAF4437BF45031D512C7489";

            /// <summary>
            /// Initializes a new instance of the <see cref="XmlDictionaryItems"/> class.
            /// </summary>
            public XmlDictionaryItems()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="XmlDictionaryItems"/> class
            /// with the specified dictionary and optional XML writer settings.
            /// </summary>
            /// <param name="dictionary">The dictionary to be serialized into XML.</param>
            /// <param name="settings">Optional XML writer settings to customize the serialization process.</param>
            public XmlDictionaryItems(IDictionary dictionary, XmlWriterSettings? settings = null)
            {
                DictionaryType = dictionary.GetType().ToString();

                settings ??= new()
                {
                    OmitXmlDeclaration = true,
                    Indent = true,
                };

                foreach (var key in dictionary.Keys)
                {
                    var keyStringEnclosed = DataObjectToXmlString(key, settings);
                    var valueStringEnclosed
                        = DataObjectToXmlString(dictionary[key], settings) ?? NullableKey;

                    if (keyStringEnclosed is null)
                        continue;

                    Items.Add(new XmlDictionaryItem(keyStringEnclosed, valueStringEnclosed));
                }
            }

            /// <summary>
            /// Gets or sets the collection of dictionary items.
            /// </summary>
            [XmlArray("Items")]
            [XmlArrayItem("Item")]
            public BaseCollection<XmlDictionaryItem> Items { get; set; } = new();

            /// <summary>
            /// Gets or sets the type of the dictionary being serialized or deserialized.
            /// </summary>
            [XmlElement("DictionaryType")]
            public string DictionaryType { get; set; } = string.Empty;

            /// <summary>
            /// Gets the dictionary representation of the XML dictionary items.
            /// </summary>
            /// <remarks>
            /// This property attempts to create an instance of the dictionary type specified
            /// in <see cref="DictionaryType"/> and populate it with the key-value pairs
            /// stored in the <see cref="Items"/> collection. If the dictionary type cannot
            /// be found or instantiated, this property returns null.
            /// </remarks>
            [XmlIgnore]
            public IDictionary? AsDictionary
            {
                get
                {
                    var type = FindTypeByName(DictionaryType);

                    if (type is null)
                        return null;
                    if (Activator.CreateInstance(type) is not IDictionary instance)
                        return null;

                    foreach (var item in Items)
                    {
                        var key = DeserializeDataObjectFromXmlString(item.Key);
                        var value = DeserializeDataObjectFromXmlString(item.Value);

                        if (value?.ToString() == NullableKey)
                            value = null;

                        if (key is null)
                            continue;
                        instance.Add(key, value);
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// Represents a key-value pair used in XML serialization of dictionary items.
        /// </summary>
        public class XmlDictionaryItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="XmlDictionaryItem"/> class.
            /// </summary>
            public XmlDictionaryItem()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DataObject.XmlDictionaryItem"/> class
            /// with the specified key and value.
            /// </summary>
            /// <param name="key">The key of the dictionary item.</param>
            /// <param name="value">The value of the dictionary item.</param>
            public XmlDictionaryItem(string key, string value)
            {
                Key = key;
                Value = value;
            }

            /// <summary>
            /// Gets or sets the key of the dictionary item.
            /// </summary>
            [XmlElement("Key")]
            public string Key { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the value of the dictionary item.
            /// </summary>
            [XmlElement("Value")]
            public string Value { get; set; } = string.Empty;
        }

        internal class EmptyDataObject : DataObject
        {
            /// <inheritdoc/>
            public override void SetData(string format, object data)
            {
            }

            /// <inheritdoc/>
            public override void SetData(object data)
            {
            }
        }
    }
}