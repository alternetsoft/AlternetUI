using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a property bag that allows getting and setting properties by name.
    /// It supports both simple properties (string) and complex properties (IPropertyBag).
    /// </summary>
    public class PropertyBag : IPropertyBag
    {
        private readonly BaseDictionary<string, object> properties = new ();

        /// <inheritdoc/>
        public bool TryGetStringProperty(string name, out string? value)
        {
            if (properties.TryGetValue(name, out var objValue) && objValue is string strValue)
            {
                value = strValue;
                return true;
            }

            value = null;
            return false;
        }

        /// <inheritdoc/>
        public string? GetStringProperty(string name)
        {
            if (TryGetStringProperty(name, out string? value))
                return value;
            return null;
        }

        /// <inheritdoc/>
        public void SetStringProperty(string name, string value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public void SetComplexProperty(string name, IPropertyBag value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public bool TryGetComplexProperty(string name, out IPropertyBag? value)
        {
            if (properties.TryGetValue(name, out var objValue) && objValue is IPropertyBag bagValue)
            {
                value = bagValue;
                return true;
            }

            value = null;
            return false;
        }

        /// <inheritdoc/>
        public bool IsComplexProperty(string name)
        {
            var result = properties.TryGetValue(name, out var objValue);
            return result && objValue is IPropertyBag;
        }

        /// <inheritdoc/>
        public void SetIntProperty(string name, int value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public void SetBoolProperty(string name, bool value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public IPropertyBag CreateComplexProperty(string name)
        {
            var complexProperty = new PropertyBag();
            properties.Add(name, complexProperty);
            return complexProperty;
        }

        /// <inheritdoc/>
        public bool TryGetEnumProperty<TEnum>(string name, out TEnum value)
            where TEnum : struct, Enum
        {
            if (TryGetStringProperty(name, out string? stringValue) &&
                Enum.TryParse(stringValue, out TEnum parsedValue))
            {
                value = parsedValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetProperty<TProperty>(string name, out TProperty value)
        {
            if (properties.TryGetValue(name, out var objValue) && objValue is TProperty typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default!;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetIntProperty(string name, out int value)
        {
            if (properties.TryGetValue(name, out var objValue) && objValue is int intValue)
            {
                value = intValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetBoolProperty(string name, out bool value)
        {
            if (properties.TryGetValue(name, out var objValue) && objValue is bool boolValue)
            {
                value = boolValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetPropertyNames()
        {
            return properties.Keys;
        }
    }
}
