using System;
using System.Collections.Generic;
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
            if (value.GetDataPresent(DataFormats.Text))
                result.AppendLine("Text: " + value.GetData(DataFormats.Text));
            if (value.GetDataPresent(DataFormats.Files))
            {
                result.AppendLine("Files: "
                    + string.Join("\n", (string[])value.GetData(DataFormats.Files)!));
            }

            if (value.GetDataPresent(DataFormats.Bitmap))
            {
                var data = value.GetData(DataFormats.Bitmap);
                var bitmap = data as Image;
                result.AppendLine($"Bitmap: {bitmap?.Size}");
            }

            result.AppendLine().AppendLine("All formats: "
                + string.Join(";", value.GetFormats()));

            return result.ToString();
        }

        /// <inheritdoc/>
        public virtual object? GetData(string format)
        {
            if (format is null)
                throw new ArgumentNullException(nameof(format));

            if (!data.TryGetValue(format, out var value))
                return null;
            return value;
        }

        /// <inheritdoc/>
        public virtual bool GetDataPresent(string format)
        {
            if (format is null)
                throw new ArgumentNullException(nameof(format));

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
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            SetData(ClipboardUtils.DetectFormatFromData(data), data);
        }

        /// <summary>
        /// Retrieves an array of file names from the data object in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public virtual string[]? GetFiles() => GetData(DataFormats.Files) as string[];

        /// <summary>
        /// Retrieves bitmap data from the data object in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public virtual Image? GetBitmap() => GetData(DataFormats.Bitmap) as Image;

        /// <summary>
        /// Retrieves text data from the data object in the
        /// <see cref="DataFormats.Text"/> format.
        /// </summary>
        public virtual string? GetText() => GetData(DataFormats.Text) as string;

        /// <summary>
        /// Adds an array of file names to the data object in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public virtual void SetFiles(params string[] value) =>
            SetData(DataFormats.Files, value);

        /// <summary>
        /// Adds a bitmap to the data object in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public virtual void SetBitmap(Image value) =>
            SetData(DataFormats.Bitmap, value);

        /// <summary>
        /// Adds a text string to the data object in the
        /// <see cref="DataFormats.Text"/> format.
        /// </summary>
        public virtual void SetText(string value) => SetData(DataFormats.Text, value);

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