using Alternet.Drawing;
using System.Diagnostics;

namespace Alternet.UI
{
    /// <summary>
    ///  Provides methods to place data on and retrieve data from the system clipboard.
    /// </summary>
    public static class Clipboard
    {
        /// <summary>
        /// Retrieves the data that is currently on the system <see cref='Clipboard'/>,
        /// or <see langword="null"/> if there is no data on the Clipboard.
        /// </summary>
        public static IDataObject? GetDataObject()
        {
            var unmanagedDataObject = Application.Current.NativeClipboard.GetDataObject();
            if (unmanagedDataObject == null)
                return null;

            return new UnmanagedDataObjectAdapter(unmanagedDataObject);
        }

        /// <summary>
        /// Clears the Clipboard and then adds data to it.
        /// </summary>
        /// <param name="value">The data to place on the Clipboard.</param>
        public static void SetDataObject(IDataObject value)
        {
            Application.Current.NativeClipboard.SetDataObject(UnmanagedDataObjectService.GetUnmanagedDataObject(value));
        }

        /// <summary>
        /// Removes all data from the <see cref='Clipboard'/>.
        /// </summary>
        public static void Clear() => SetDataObject(new DataObject());

        /// <summary>
        /// Indicates whether there is data on the <see cref='Clipboard'/> that is in the specified format or can be converted to that format.
        /// </summary>
        /// <param name="format">The format of the data to look for. See DataFormats for predefined formats.</param>
        public static bool ContainsData(string format) => GetDataObject()?.GetDataPresent(format) ?? false;

        /// <summary>
        /// Gets a value indicating whether there is data on the <see cref='Clipboard'/> that is in the <see cref="DataFormats.Files"/> format.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static bool ContainsFiles => ContainsData(DataFormats.Files);

        /// <summary>
        /// Gets a value indicating whether there is data on the <see cref='Clipboard'/> that is in the <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static bool ContainsBitmap => ContainsData(DataFormats.Bitmap);

        /// <summary>
        /// Gets a value indicating whether there is data on the <see cref='Clipboard'/> in the <see cref="DataFormats.Text"/> format.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static bool ContainsText => ContainsData(DataFormats.Text);

        /// <summary>
        /// Retrieves data from the Clipboard in the specified format.
        /// </summary>
        /// <param name="format">The format of the data to retrieve. See DataFormats for predefined formats.</param>
        /// <returns>
        /// An Object representing the Clipboard data or null if the Clipboard does not contain any data
        /// that is in the specified format or can be converted to that format.
        /// </returns>
        public static object? GetData(string format) => GetDataObject()?.GetData(format);

        /// <summary>
        /// Retrieves an array of file names from the Clipboard in the <see cref="DataFormats.Files"/> format.
        /// </summary>
        public static string[]? GetFiles() => GetData(DataFormats.Files) as string[];

        /// <summary>
        /// Retrieves bitmap data from the Clipboard in the <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public static Bitmap? GetBitmap()
        {
            var image = GetData(DataFormats.Bitmap) as Image;
            if (image == null)
                return null;

            return new Bitmap(image);
        }

        /// <summary>
        /// Retrieves text data from the Clipboard in the <see cref="DataFormats.Text"/> format.
        /// </summary>
        public static string? GetText() => GetData(DataFormats.Text) as string;

        /// <summary>
        /// Adds an array of file names to the data object in the <see cref="DataFormats.Files"/> format.
        /// </summary>
        public static void SetFiles(string[] value) => SetData(DataFormats.Files, value);

        /// <summary>
        /// Adds a bitmap to the data object in the <see cref="DataFormats.Bitmap"/> format.
        /// </summary>
        public static void SetBitmap(Bitmap value) => SetData(DataFormats.Bitmap, value);

        /// <summary>
        /// Adds a text string to the data object in the <see cref="DataFormats.Text"/> format.
        /// </summary>
        public static void SetText(string value) => SetData(DataFormats.Text, value);

        /// <summary>
        /// Clears the Clipboard and then adds data in the specified format.
        /// </summary>
        /// <param name="format">The format of the data to set. See DataFormats for predefined formats.</param>
        /// <param name="data">An Object representing the data to add.</param>
        public static void SetData(string format, object data) => SetDataObject(new DataObject(format, data));
    }
}