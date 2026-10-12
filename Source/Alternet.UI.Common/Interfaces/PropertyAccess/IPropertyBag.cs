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
        /// Tries to get the value of a complex property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains
        /// the value of the property if found; otherwise, the default value for the type.</param>
        /// <returns><c>true</c> if the property was found; otherwise, <c>false</c>.</returns>
        bool TryGetComplexProperty(string name, out IPropertyBag? value);

        /// <summary>
        /// Determines whether a property with the specified name is a simple property (string)
        /// or a complex property (IStringPropertyBag).
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns><c>true</c> if the property is a simple property; otherwise, <c>false</c>.</returns>
        bool IsComplexProperty(string name);

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
        /// Sets the value of a complex property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        void SetComplexProperty(string name, IPropertyBag value);

        /// <summary>
        /// Creates a new complex property with the specified name and returns it.
        /// </summary>
        /// <param name="name">The name of the complex property to create.</param>
        /// <returns>The newly created complex property.</returns>
        IPropertyBag CreateComplexProperty(string name);

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
                if (IsComplexProperty(propertyName))
                {
                    if (TryGetComplexProperty(propertyName, out var complexProperty))
                    {
                        writer.WriteLine($"\"{propertyName}\" : ");
                        writer.Indent();

                        var subFlags = flags;

                        if(isLastProperty)
                        {
                            subFlags &= ~JsonStringFlags.AddComma;
                        }
                        else
                        {
                            subFlags |= JsonStringFlags.AddComma;
                        }

                        complexProperty?.ToJsonString(writer, subFlags);
                        writer.Unindent();
                    }
                }
                else
                {
                    if (TryGetStringProperty(propertyName, out var stringValue))
                    {
                        writer.WriteLine($"\"{propertyName}\" : \"{stringValue}\"{(isLastProperty ? "" : ",")}");
                    }
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