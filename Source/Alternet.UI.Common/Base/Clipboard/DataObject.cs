using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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

        private readonly Dictionary<string, object> data = new(StringComparer.Ordinal);

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