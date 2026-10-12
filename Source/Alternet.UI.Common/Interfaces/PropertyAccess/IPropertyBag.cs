using System;
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
            /// Indicates that the property value is an integer.
            /// </summary>
            Int,

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
            /// Indicates that the property value is of a type not explicitly defined in this enumeration.
            /// </summary>
            Other,
        }

        /// <summary>
        /// Defines flags for controlling the behavior of the <see cref="ToJsonString"/> method.
        /// </summary>
        public enum JsonStringFlags
        {
            /// <summary>
            /// Indicates that no special formatting should be applied to the JSON string.
            /// </summary>
            None = 0,

            /// <summary>
            /// Indicates that a comma should be added after the JSON string representation of the property bag.
            /// </summary>
            AddComma = 1,
        }

        /// <summary>
        /// Gets the JSON string representation of the property bag.
        /// </summary>
        public string AsJsonString
        {
            get
            {
                var writer = new LogWriter.StringBuilderLogWriter();
                ToJsonString(writer);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Tries to get the value of a property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetStringProperty(string name, out string? value);

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
        /// <returns>An enumerable collection of property names.</returns>
        IEnumerable<string> GetPropertyNames();

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
        /// Sets the value of a string property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetStringProperty(string name, string value);

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
        /// Converts the property bag to a JSON string representation
        /// and writes it to the specified <see cref="ILogWriter"/>.
        /// </summary>
        /// <param name="writer">The log writer to write the JSON string to.</param>
        /// <param name="flags">Flags to control the JSON string formatting.</param>
        public void ToJsonString(ILogWriter? writer = null, JsonStringFlags flags = JsonStringFlags.None)
        {
            if (writer is null)
                return;

            writer.WriteLine("{");
            writer.Indent();

            var propNames = GetPropertyNames().ToArray();
 
            for (int i = 0; i < propNames.Length; i++)
            {
                var propertyName = propNames[i];
                var isLastProperty = (i == propNames.Length - 1);
                var valueType = GetPropertyType(propertyName);

                void EmitQuotedValue(string? value)
                {
                    writer.WriteLine($"\"{propertyName}\" : \"{value}\"{(isLastProperty ? "" : ",")}");
                }

                void EmitValue(string? value)
                {
                    writer.WriteLine($"\"{propertyName}\" : {value}{(isLastProperty ? "" : ",")}");
                }

                switch (valueType)
                {
                    default:
                    case PropertyType.String:
                    case PropertyType.Flags:
                    case PropertyType.Enum:
                        if (TryGetStringProperty(propertyName, out var stringValue))
                        {
                            EmitQuotedValue(stringValue);
                        }
                        break;
                    case PropertyType.Int:
                        if (TryGetIntProperty(propertyName, out var intValue))
                        {
                            EmitValue(intValue.ToString());
                        }
                        break;
                    case PropertyType.Bool:
                        if (TryGetBoolProperty(propertyName, out var boolValue))
                        {
                            EmitValue(boolValue.ToString().ToLower());
                        }
                        break;
                }
            }

            writer.Unindent();
            writer.WriteLine(flags.HasFlag(JsonStringFlags.AddComma) ? "}," : "}");
        }

        /// <summary>
        /// Logs the properties of the property bag.
        /// </summary>
        public void Log(ILogWriter? writer = null)
        {
            writer ??= LogWriter.Debug;
            ToJsonString(writer);
        }
    }
}