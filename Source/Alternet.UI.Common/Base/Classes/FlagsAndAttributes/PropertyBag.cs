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
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        public PropertyBag()
        {
        }

        /// <inheritdoc/>
        public bool TryGetStringProperty(string name, out string? value)
        {
            if (properties.TryGetValue(name, out var objValue))
            {
                value = objValue?.ToString();
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
        public void SetIntProperty(string name, int value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public IPropertyBag.PropertyType GetValueType(object? value)
        {
            if (value is int)
                return IPropertyBag.PropertyType.Int;
            if (value is bool)
                return IPropertyBag.PropertyType.Bool;
            if (value is string)
                return IPropertyBag.PropertyType.String;
            if (value is null)
                return IPropertyBag.PropertyType.Null;

            var isEnum = AssemblyUtils.IsEnumType(value.GetType());

            if (isEnum)
            {
                var isFlags = AssemblyUtils.EnumIsFlags(value.GetType());
                if (isFlags)
                    return IPropertyBag.PropertyType.Flags;
                return IPropertyBag.PropertyType.Enum;
            }

            return IPropertyBag.PropertyType.Other;
        }

        /// <inheritdoc/>
        public IPropertyBag.PropertyType GetPropertyType(string name)
        {
            if (properties.TryGetValue(name, out var objValue))
            {
                return GetValueType(objValue);
            }

            return IPropertyBag.PropertyType.Other;
        }

        /// <inheritdoc/>
        public void SetBoolProperty(string name, bool value)
        {
            properties.Add(name, value);
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
            if (!properties.TryGetValue(name, out var objValue))
            {
                value = default;
                return false;
            }

            if (objValue is int intValue)
            {
                value = intValue;
                return true;
            }

            var s = objValue?.ToString();

            if (int.TryParse(s, out int parsedValue))
            {
                value = parsedValue;
                return true;
            }

            value = default;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetBoolProperty(string name, out bool value)
        {
            if (!properties.TryGetValue(name, out var objValue))
            {
                value = default;
                return false;
            }

            if (objValue is bool boolValue)
            {
                value = boolValue;
                return true;
            }

            var s = objValue?.ToString()?.ToLower();

            if (bool.TryParse(s, out bool parsedValue))
            {
                value = parsedValue;
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
