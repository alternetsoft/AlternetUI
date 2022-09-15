using System;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a format-independent mechanism for transferring data.
    /// </summary>
    public interface IDataObject
    {
        /// <summary>
        /// Retrieves a data object in a specified format; the data format is specified by a string.
        /// </summary>
        /// <param name="format">
        /// A string that specifies what format to retrieve the data as.
        /// See the <see cref="DataFormats"/> class for a set of predefined data formats.
        /// </param>
        /// <returns>
        /// A data object with the data in the specified format,
        /// or <c>null</c> if the data is not available in the specified format.
        /// </returns>
        object? GetData(string format);

        /// <summary>
        /// Stores the specified data in this data object, along with one or more specified data formats. The data format is specified by a string.
        /// </summary>
        /// <param name="format">
        /// A string that specifies what format to store the data in.
        /// See the <see cref="DataFormats"/> class for a set of pre-defined data formats.
        /// </param>
        /// <param name="data">The data to store in this data object.</param>
        void SetData(string format, object data);

        /// <summary>
        /// Stores the specified data in this data object, automatically converting the data format from the source object type.
        /// </summary>
        /// <param name="data">
        /// The data to store in this data object.
        /// </param>
        void SetData(object data);

        /// <summary>
        /// Determines whether data stored in this instance is associated with, or can be converted to, the specified format.
        /// </summary>
        /// <param name="format">
        /// The format for which to check. See <see cref="DataFormats"/> for predefined formats.
        /// </param>
        bool GetDataPresent(string format);

        /// <summary>
        /// Returns a list of all formats that data stored in this instance is associated with or can be converted to.
        /// </summary>
        /// <returns>
        /// An array of the names that represents a list of all formats that are supported by the data stored in this object.
        /// </returns>
        string[] GetFormats();
    }

}