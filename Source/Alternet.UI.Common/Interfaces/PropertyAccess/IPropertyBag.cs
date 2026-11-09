using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines an interface for a property bag that allows getting and setting string properties by name.
    /// </summary>
    public interface IPropertyBag
    {
        /// <summary>
        /// Defines the types of properties that can be stored in the property bag.
        /// </summary>
        public enum PropertyType
        {
            /// <summary>
            /// Indicates that the property value is null or not set.
            /// </summary>
            Null,

            /// <summary>
            /// Indicates that the property value is a string.
            /// </summary>
            String,

            /// <summary>
            /// Indicates that the property value is an integer number
            /// (including byte, short, long, sbyte, ushort, uint, and ulong).
            /// </summary>
            Integer,

            /// <summary>
            /// Indicates that the property value is a boolean.
            /// </summary>
            Bool,

            /// <summary>
            /// Indicates that the property value is an enumeration type.
            /// </summary>
            Enum,

            /// <summary>
            /// Indicates that the property value is a set of flags (bitwise combination of enum values).
            /// </summary>
            Flags,

            /// <summary>
            /// Indicates that the property value is a collection (e.g., an array or a list).
            /// </summary>
            Collection,

            /// <summary>
            /// Indicates that the property value is of a type not explicitly defined in this enumeration.
            /// </summary>
            Other,
        }

        /// <summary>
        /// Gets or sets the name of the XML element that represents the property bag when serialized to XML.
        /// </summary>
        string? ElementName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this property bag is the root property bag in a hierarchy.
        /// </summary>
        bool IsRoot { get; set; }

        /// <summary>
        /// Gets or sets the XML string representation of the property bag.
        /// </summary>
        string AsXmlString { get; set; }

        /// <summary>
        /// Gets or sets the JSON string representation of the property bag.
        /// </summary>
        string AsJsonString { get; set; }

        /// <summary>
        /// Clears all properties from the property bag.
        /// </summary>
        void Clear();

        /// <summary>
        /// Tries to get the value of a property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetStringProperty(string name, out string? value);

        /// <summary>
        /// Tries to get the value of a unique identifier property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetUniqueIdProperty(string name, out ObjectUniqueId value);

        /// <summary>
        /// Sets the value of a unique identifier property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set for the property.</param>
        void SetUniqueIdProperty(string name, ObjectUniqueId value);

        /// <summary>
        /// Tries to get the value of a collection property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetCollectionProperty(string name, out IEnumerable<IPropertyBag>? value);

        /// <summary>
        /// Tries to get the value of an int property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetIntProperty(string name, out int value);

        /// <summary>
        /// Gets the names of all properties in the property bag.
        /// </summary>
        /// <param name="sorted">Indicates whether the property names should be returned in sorted order.</param>
        /// <returns>An enumerable collection of property names.</returns>
        IEnumerable<string> GetPropertyNames(bool sorted = true);

        /// <summary>
        /// Gets the type of a value.
        /// </summary>
        /// <param name="value">The value to get the type of.</param>
        /// <returns>The type of the value.</returns>
        IPropertyBag.PropertyType GetValueType(object? value);

        /// <summary>
        /// Gets the type of a property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The type of the property.</returns>
        PropertyType GetPropertyType(string name);

        /// <summary>
        /// Tries to get the value of a bool property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetBoolProperty(string name, out bool value);

        /// <summary>
        /// Tries to get the value of a property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetProperty<TProperty>(string name, out TProperty value);

        /// <summary>
        /// Gets the value of a property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>The value of the property.</returns>
        string? GetStringProperty(string name);

        /// <summary>
        /// Sets the value of a property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetProperty(string name, object? value);

        /// <summary>
        /// Sets the value of a string property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetStringProperty(string name, string value);

        /// <summary>
        /// Sets the value of a collection property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="collection">The collection to set.</param>
        void SetCollectionProperty(string name, IEnumerable<IPropertyBag> collection);

        /// <summary>
        /// Sets the value of a collection property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="collection">The collection to set.</param>
        void SetCollectionProperty(string name, IEnumerable collection);

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to its string representation.
        /// </summary>
        /// <param name="date">The date and time to convert.</param>
        /// <returns>A string representation of the specified <see cref="DateTime"/>.</returns>
        string DateTimeToString(DateTime date);

        /// <summary>
        /// Attempts to convert the specified string representation of a time span to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="timeSpan">The time span to convert.</param>
        /// <returns>A string representation of the specified <see cref="TimeSpan"/>.</returns>
        string TimeSpanToString(TimeSpan timeSpan);

        /// <summary>
        /// Attempts to convert the specified string representation of a date and time to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="s">The string representation of the date and time to convert.</param>
        /// <param name="result">When this method returns, contains the converted <see cref="DateTime"/>
        /// if the conversion succeeded, or <see cref="DateTime.MinValue"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        bool TryDateTimeFromString(string? s, out DateTime result);

        /// <summary>
        /// Attempts to convert the specified string representation of a time span to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="s">The string representation of the time span to convert.</param>
        /// <param name="result">When this method returns, contains the converted <see cref="TimeSpan"/>
        /// if the conversion succeeded, or <see cref="TimeSpan.Zero"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        bool TryTimeSpanFromString(string? s, out TimeSpan result);

        /// <summary>
        /// Sets the value of a <see cref="DateTime"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetDateTimeProperty(string name, DateTime value);

        /// <summary>
        /// Sets the value of a <see cref="TimeSpan"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetTimeSpanProperty(string name, TimeSpan value);

        /// <summary>
        /// Removes a property by name from the property bag.
        /// </summary>
        /// <param name="name">The name of the property to remove.</param>
        /// <returns><c>true</c> if the property was successfully removed; otherwise, <c>false</c>.</returns>
        bool Remove(string name);

        /// <summary>
        /// Sets the value of an int property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetIntProperty(string name, int value);

        /// <summary>
        /// Sets the value of a bool property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetBoolProperty(string name, bool value);

        /// <summary>
        /// Tries to get the value of an enum property by name.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains the value of the enum property
        /// if the conversion succeeded, or the default value if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        bool TryGetEnumProperty<TEnum>(string name, out TEnum value)
            where TEnum : struct, Enum;

        /// <summary>
        /// Tries to get the value of a <see cref="DateTime"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains the value of the <see cref="DateTime"/> property
        /// if the conversion succeeded, or <c>null</c> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        bool TryGetDateTimeProperty(string name, out DateTime value);

        /// <summary>
        /// Attempts to get the value of a <see cref="TimeSpan"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains the value of the <see cref="TimeSpan"/> property
        /// if the conversion succeeded, or <see cref="TimeSpan.Zero"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        bool TryGetTimeSpanProperty(string name, out TimeSpan value);

        /// <summary>
        /// Gets all properties as a collection of name-value pairs.
        /// </summary>
        /// <returns>An enumerable collection of name-value pairs representing the properties.</returns>
        IEnumerable<KeyValuePair<string, object?>> GetAllProperties();

        /// <summary>
        /// Converts the property bag to a JSON string representation with the specified indentation level.
        /// </summary>
        /// <param name="indentLevel">The indentation level to apply to the JSON string.</param>
        /// <returns>A JSON string representation of the property bag with the specified indentation level.</returns>
        string ToIndentedJsonString(int indentLevel);

        /// <summary>
        /// Converts the property bag to a JSON string representation
        /// and writes it to the specified <see cref="IStructuredTextWriter"/>.
        /// </summary>
        /// <param name="writer">The log writer to write the JSON string to.</param>
        /// <param name="flags">Flags to control the JSON string formatting.</param>
        void ToJsonString(
            IStructuredTextWriter? writer = null,
            JsonUtils.StringFormatFlags flags = JsonUtils.StringFormatFlags.None);

        /// <summary>
        /// Converts the property bag to a XML string representation
        /// and writes it to the specified <see cref="IStructuredTextWriter"/>.
        /// </summary>
        /// <param name="writer">The log writer to write the XML string to.</param>
        /// <param name="flags">Flags to control the XML string formatting.</param>
        /// <param name="nodeName">The name of the root XML node.</param>
        void ToXmlString(
            string? nodeName,
            IStructuredTextWriter? writer = null,
            XmlUtils.StringFormatFlags flags = XmlUtils.StringFormatFlags.None);

        /// <summary>
        /// Logs the properties of the property bag.
        /// </summary>
        void Log(IStructuredTextWriter? writer = null);
    }
}