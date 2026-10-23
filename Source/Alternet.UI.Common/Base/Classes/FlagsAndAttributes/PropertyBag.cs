using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a property bag that allows getting and setting properties by name.
    /// It supports both simple properties (string) and complex properties (IPropertyBag).
    /// </summary>
    public partial class PropertyBag : IPropertyBag
    {
        private readonly BaseDictionary<string, object> properties = new();
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        public PropertyBag()
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="PropertyBag"/> class and returns it.
        /// </summary>
        /// <returns>A new instance of the <see cref="PropertyBag"/> class.</returns>
        public static PropertyBag Create()
        {
            return new PropertyBag();
        }

        /// <inheritdoc/>
        public virtual bool TryGetCollectionProperty(string name, out IEnumerable? value)
        {
            if (properties.TryGetValue(name, out var objValue) && objValue is IEnumerable collectionValue)
            {
                value = collectionValue;
                return true;
            }

            value = null;
            return false;
        }

        /// <inheritdoc/>
        public virtual bool TryGetStringProperty(string name, out string? value)
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
        public virtual string? GetStringProperty(string name)
        {
            if (TryGetStringProperty(name, out string? value))
                return value;
            return null;
        }

        /// <inheritdoc/>
        public virtual void SetStringProperty(string name, string value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public virtual void SetIntProperty(string name, int value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public virtual IPropertyBag.PropertyType GetValueType(object? value)
        {
            if (value is IEnumerable)
                return IPropertyBag.PropertyType.Collection;
            if (value is int or byte or short or long or sbyte or ushort or uint or ulong)
                return IPropertyBag.PropertyType.Integer;
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
        public virtual IPropertyBag.PropertyType GetPropertyType(string name)
        {
            if (properties.TryGetValue(name, out var objValue))
            {
                return GetValueType(objValue);
            }

            return IPropertyBag.PropertyType.Other;
        }

        /// <inheritdoc/>
        public virtual void SetBoolProperty(string name, bool value)
        {
            properties.Add(name, value);
        }

        /// <inheritdoc/>
        public virtual bool TryGetEnumProperty<TEnum>(string name, out TEnum value)
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
        public virtual bool TryGetProperty<TProperty>(string name, out TProperty value)
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
        public virtual void Clear()
        {
            properties.Clear();
        }

        /// <inheritdoc/>
        public virtual bool TryGetIntProperty(string name, out int value)
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
        public virtual bool TryGetBoolProperty(string name, out bool value)
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
        public virtual void SetCollectionProperty(string name, IEnumerable collection)
        {
            properties[name] = collection;
        }

        /// <inheritdoc/>
        public virtual IEnumerable<string> GetPropertyNames(bool sorted)
        {
            return sorted ? properties.Keys.OrderBy(k => k) : properties.Keys;
        }

        /// <inheritdoc/>
        public virtual bool Remove(string name)
        {
            return properties.Remove(name);
        }
    }
}
