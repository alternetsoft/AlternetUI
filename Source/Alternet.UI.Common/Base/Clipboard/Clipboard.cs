using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods to place data on and retrieve data from the system clipboard.
    /// </summary>
    /// <remarks>
    /// Some platforms require using of async methods. Use <see cref="AsyncRequired"/>
    /// to check this.
    /// </remarks>
    /// <remarks>
    /// Some platforms support only text format on the clipboard.
    /// Use <see cref="OnlyText"/> to check this.
    /// </remarks>
    public static class Clipboard
    {
        private static IClipboardHandler? handler;

        /// <summary>
        /// Gets or sets <see cref="IClipboardHandler"/> used by the <see cref="Clipboard"/>.
        /// </summary>
        public static IClipboardHandler Handler
        {
            get => handler ??= App.Handler.CreateClipboardHandler();

            set => handler = value;
        }

        /// <summary>
        /// Gets whether async methods are required to use when working with the clipboard.
        /// </summary>
        public static bool AsyncRequired => Handler.AsyncRequired;

        /// <summary>
        /// Gets whether only text format is supported by the clipboard.
        /// </summary>
        public static bool OnlyText => Handler.OnlyText;

        /// <summary>
        /// Gets a value indicating whether there is data on the
        /// <see cref='Clipboard'/> that is in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public static bool ContainsFiles() => ContainsData(DataFormats.Files);

        /// <summary>
        /// Gets a value indicating whether there is data on the
        /// <see cref='Clipboard'/> that is in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public static bool ContainsBitmap() => ContainsData(DataFormats.Bitmap);

        /// <summary>
        /// Gets a value indicating whether there is data on the
        /// <see cref='Clipboard'/> in the <see cref="DataFormats.Text"/> format.
        /// </summary>
        public static bool ContainsText() => ContainsData(DataFormats.Text);

        /// <summary>
        /// Gets a value indicating whether there is data on the
        /// <see cref='Clipboard'/> in the specified <see cref="TextDataFormat"/> text format.
        /// </summary>
        public static bool ContainsText(TextDataFormat format)
        {
            if (format == TextDataFormat.Text || format == TextDataFormat.UnicodeText)
                return ContainsData(DataFormats.Text);
            return false;
        }

        /// <summary>
        /// Retrieves the data that is currently on the system clipboard,
        /// or <see langword="null"/> if there is no data on the clipboard.
        /// </summary>
        public static IDataObject? GetDataObject()
        {
            return Handler.GetData();
        }

        /// <summary>
        /// Retrieves the data that is currently on the system clipboard,
        /// or <see langword="null"/> if there is no data on the clipboard.
        /// Operation is performed asynchroniously.
        /// </summary>
        public static Task<IDataObject?> GetDataObjectAsync()
        {
            return Handler.GetDataAsync();
        }

        /// <summary>
        /// Retrieves the data that is currently on the system clipboard,
        /// or <see langword="null"/> if there is no data on the clipboard.
        /// Operation is performed asynchroniously.
        /// </summary>
        /// <param name="action">Action to call after operation is completed.</param>
        public static void GetDataObjectAsync(Action<IDataObject?> action)
        {
            Handler.GetDataAsync(action);
        }

        /// <summary>
        /// Sets data to the clipboard asynchroniously.
        /// </summary>
        /// <param name="value">The data to place on the clipboard.</param>
        public static Task SetDataObjectAsync(IDataObject? value)
        {
            return Handler.SetDataAsync(value);
        }

        /// <summary>
        /// Sets data to the clipboard.
        /// </summary>
        /// <param name="value">The data to place on the clipboard.</param>
        public static void SetDataObject(IDataObject? value)
        {
            SetDataObject(value, copy: false);
        }

        /// <summary>
        /// Places a specified data object on the system Clipboard and accepts a Boolean
        /// parameter that indicates whether the data object should be left on the Clipboard
        /// when the application exits.</summary>
        /// <param name="data">A data object (an object that implements <see cref="IDataObject" />)
        /// to place on the system Clipboard.</param>
        /// <param name="copy">
        ///   <see langword="true" /> to leave the data on the system clipboard when
        ///   the application exits; <see langword="false" /> to clear the data
        ///   from the system Clipboard when the application exits.</param>
        public static void SetDataObject(IDataObject? data, bool copy)
        {
            Handler.SetData(data ?? DataObject.Empty);
        }

        /// <summary>
        /// Removes all data from the <see cref='Clipboard'/>.
        /// </summary>
        public static void Clear() => SetDataObject(DataObject.Empty);

        /// <summary>
        /// Indicates whether there is data on the <see cref='Clipboard'/>
        /// that is in the specified format.
        /// </summary>
        /// <param name="format">The format of the data to look for.</param>
        public static bool HasFormat(ClipboardDataFormatId format)
        {
            if (!DataFormats.IsValidOnWindowsMacLinux(format))
                return false;
            var present = Handler.HasFormat(format);
            return present;
        }

        /// <summary>
        /// Indicates whether there is data on the <see cref='Clipboard'/>
        /// that is in the specified format.
        /// </summary>
        /// <param name="format">The format of the data to look for.</param>
        /// <remarks>
        /// This method is different from <see cref="ContainsData(string)"/> as
        /// it doesn't get data from the clipboard, it only checks for the format.
        /// This method works faster than <see cref="ContainsData(string)"/>.
        /// </remarks>
        public static bool HasFormat(string format)
        {
            var result = Handler.HasFormat(format);
            return result;
        }

        /// <summary>
        /// Flushes clipboard data so application leaves
        /// the data on the system clipboard when the application exits.
        /// </summary>
        /// <returns></returns>
        public static bool Flush()
        {
            var result = Handler.Flush();
            return result;
        }

        /// <summary>
        /// Indicates whether there is data on the <see cref='Clipboard'/>
        /// that is in the specified format or can be converted to that format.
        /// </summary>
        /// <param name="format">The format of the data to look for.
        /// See <see cref="DataFormats"/> for predefined formats.</param>
        public static bool ContainsData(string format)
        {
            return GetDataObject()?.GetDataPresent(format) ?? false;
        }

        /// <summary>
        /// Retrieves data from the Clipboard in the specified format.
        /// </summary>
        /// <param name="format">The format of the data to retrieve.
        /// See DataFormats for predefined formats.</param>
        /// <returns>
        /// An Object representing the Clipboard data or null if the
        /// Clipboard does not contain any data
        /// that is in the specified format or can be converted to that format.
        /// </returns>
        public static object? GetData(string format)
        {
            return GetDataObject()?.GetData(format);
        }

        /// <summary>
        /// Retrieves an array of file names from the Clipboard in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public static string[]? GetFiles()
        {
            return GetData(DataFormats.Files) as string[];
        }

        /// <summary>
        /// Retrieves bitmap data from the Clipboard in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public static Bitmap? GetBitmap()
        {
            if (GetData(DataFormats.Bitmap) is not Image image)
                return null;

            return new Bitmap(image);
        }

        /// <summary>
        /// Retrieves text data from the Clipboard in the
        /// <see cref="DataFormats.Text"/> format.
        /// </summary>
        public static string? GetText()
        {
            return GetData(DataFormats.Text) as string;
        }

        /// <summary>
        /// Adds an array of file names to the data object in the
        /// <see cref="DataFormats.Files"/> format.
        /// </summary>
        public static void SetFiles(string[] value)
        {
            SetData(DataFormats.Files, value);
        }

        /// <summary>
        /// Adds a bitmap to the data object in the
        /// <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public static void SetBitmap(Image value)
        {
            SetData(DataFormats.Bitmap, value);
        }

        /// <summary>
        /// Adds a text string to the data object in the
        /// <see cref="DataFormats.Text"/> format.
        /// </summary>
        public static void SetText(string value)
        {
            SetData(DataFormats.Text, value);
        }

        /// <summary>
        /// Clears the Clipboard and then adds data in the specified format.
        /// </summary>
        /// <param name="format">The format of the data to set.
        /// See DataFormats for predefined formats.</param>
        /// <param name="data">An Object representing the data to add.</param>
        public static void SetData(string format, object data)
        {
            SetDataObject(new DataObject(format, data));
        }
    }
}