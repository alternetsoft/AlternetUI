using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly BaseDictionary<string, object?> properties = new();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBag"/> class.
        /// </summary>
        public PropertyBag()
        {
        }

        /// <inheritdoc/>
        public virtual bool IsRoot { get; set; }

        /// <inheritdoc/>
        public virtual string? ElementName { get; set; }

        /// <summary>
        /// Gets or sets the XML string representation of the property bag.
        /// </summary>
        public virtual string AsXmlString
        {
            get
            {
                var writer = new LogWriter.StringBuilderLogWriter();
                ToXmlString(ElementName, writer);
                return writer.ToString();
            }

            set
            {
                Clear();

                XmlUtils.EnumerateProperties(value, (property, name, propertyValue) =>
                {
                    ConvertAndSetProperty(name, propertyValue);
                });
            }
        }
        
        /// <summary>
        /// Gets or sets the JSON string representation of the property bag.
        /// </summary>
        public virtual string AsJsonString
        {
            get
            {
                var writer = new LogWriter.StringBuilderLogWriter();
                ToJsonString(writer);
                return writer.ToString();
            }

            set
            {
                Clear();

                JsonUtils.EnumerateProperties(value, (property, name, propertyValue) =>
                {
                    ConvertAndSetProperty(name, propertyValue);
                });
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISerializableObjectManager"/> instance
        /// used for managing the serialization and deserialization of this instance.
        /// </summary>
        [Browsable(false)]
        public ISerializableObjectManager? SerializationManager { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="PropertyBag"/> class and returns it.
        /// </summary>
        /// <returns>A new instance of the <see cref="PropertyBag"/> class.</returns>
        public static PropertyBag Create()
        {
            return new PropertyBag();
        }

        /// <summary>
        /// Sets the value of a collection property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="collection">The collection to set.</param>
        public virtual void SetCollectionProperty(string name, IEnumerable collection)
        {
            var propertyBags = new List<IPropertyBag>();
            foreach (var item in collection)
            {
                if (item is IPropertyBag propertyBag)
                {
                    propertyBags.Add(propertyBag);
                }
                else if (item is IPropertyBagSerialize serialize)
                {
                    var newPropertyBag = PropertyBag.Create();
                    serialize.SaveTo(new SerializableObjectContext(newPropertyBag));
                    propertyBags.Add(newPropertyBag);
                }
            }

            SetCollectionProperty(name, propertyBags);
        }

        /// <summary>
        /// Converts the specified <see cref="DateTime"/> to its string representation.
        /// </summary>
        /// <param name="date">The date and time to convert.</param>
        /// <returns>A string representation of the specified <see cref="DateTime"/>.</returns>
        public virtual string DateTimeToString(DateTime date)
        {
            return EffectiveDateTimeSerialization().DateTimeToString(date);
        }

        /// <summary>
        /// Converts the specified <see cref="DateOnly"/> to its string representation.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>A string representation of the specified <see cref="DateOnly"/>.</returns>
        public virtual string DateToString(DateOnly date)
        {
            return EffectiveDateTimeSerialization().DateToString(date);
        }

        /// <summary>
        /// Converts the specified <see cref="TimeOnly"/> to its string representation.
        /// </summary>
        /// <param name="time">The time to convert.</param>
        /// <returns>A string representation of the specified <see cref="TimeOnly"/>.</returns>
        public virtual string TimeToString(TimeOnly time)
        {
            return EffectiveDateTimeSerialization().TimeToString(time);
        }

        /// <summary>
        /// Converts the specified <see cref="DateOnly"/> to its string representation.
        /// </summary>
        /// <param name="timeSpan">The time span to convert.</param>
        /// <returns>A string representation of the specified <see cref="TimeSpan"/>.</returns>
        public virtual string TimeSpanToString(TimeSpan timeSpan)
        {
            return EffectiveDateTimeSerialization().TimeSpanToString(timeSpan);
        }

        /// <summary>
        /// Attempts to convert the specified string representation of a date and time to a <see cref="DateTime"/>.
        /// </summary>
        /// <param name="s">The string representation of the date and time to convert.</param>
        /// <param name="result">When this method returns, contains the converted <see cref="DateTime"/>
        /// if the conversion succeeded, or <see cref="DateTime.MinValue"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public virtual bool TryDateTimeFromString(string? s, out DateTime result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = DateTime.MinValue;
                return false;
            }

            return DateTimeSerialization.Instance.TryDateTimeFromString(s, out result);
        }

        /// <summary>
        /// Attempts to convert the specified string representation of a time to a <see cref="TimeOnly"/>.
        /// </summary>
        /// <param name="s">The string representation of the time to convert.</param>
        /// <param name="result">When this method returns, contains the converted <see cref="TimeOnly"/>
        /// if the conversion succeeded, or <see cref="TimeOnly.MinValue"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public virtual bool TryTimeFromString(string? s, out TimeOnly result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = TimeOnly.MinValue;
                return false;
            }

            return DateTimeSerialization.Instance.TryTimeFromString(s, out result);
        }

        /// <summary>
        /// Attempts to convert the specified string representation of a date to a <see cref="DateOnly"/>.
        /// </summary>
        /// <param name="s">The string representation of the date to convert.</param>
        /// <param name="result">When this method returns, contains the converted <see cref="DateOnly"/>
        /// if the conversion succeeded, or <see cref="DateOnly.MinValue"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public virtual bool TryDateFromString(string? s, out DateOnly result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = DateOnly.MinValue;
                return false;
            }

            return DateTimeSerialization.Instance.TryDateFromString(s, out result);
        }

        /// <summary>
        /// Attempts to convert the specified string representation of a time span to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="s">The string representation of the time span to convert.</param>
        /// <param name="result">When this method returns, contains the converted <see cref="TimeSpan"/>
        /// if the conversion succeeded, or <see cref="TimeSpan.Zero"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public virtual bool TryTimeSpanFromString(string? s, out TimeSpan result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = TimeSpan.Zero;
                return false;
            }

            return DateTimeSerialization.Instance.TryTimeSpanFromString(s, out result);
        }

        /// <summary>
        /// Sets the value of a <see cref="DateTime"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        public virtual void SetDateTimeProperty(string name, DateTime value)
        {
            SetStringProperty(name, DateTimeToString(value));
        }

        /// <summary>
        /// Sets the value of a <see cref="TimeSpan"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        public virtual void SetTimeSpanProperty(string name, TimeSpan value)
        {
            var s = TimeSpanToString(value);
            SetStringProperty(name, s);
        }

        /// <summary>
        /// Tries to get the value of a <see cref="DateTime"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains the value of the <see cref="DateTime"/> property
        /// if the conversion succeeded, or <c>null</c> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public virtual bool TryGetDateTimeProperty(string name, out DateTime value)
        {
            if (TryGetStringProperty(name, out var str) &&
                TryDateTimeFromString(str, out var dateTime))
            {
                value = dateTime;
                return true;
            }

            value = DateTime.MinValue;
            return false;
        }

        /// <summary>
        /// Attempts to get the value of a <see cref="TimeSpan"/> property by name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">When this method returns, contains the value of the <see cref="TimeSpan"/> property
        /// if the conversion succeeded, or <see cref="TimeSpan.Zero"/> if the conversion failed.</param>
        /// <returns><c>true</c> if the conversion succeeded; otherwise, <c>false</c>.</returns>
        public virtual bool TryGetTimeSpanProperty(string name, out TimeSpan value)
        {
            if (TryGetStringProperty(name, out var str) &&
                TryTimeSpanFromString(str, out var timeSpan))
            {
                value = timeSpan;
                return true;
            }

            value = TimeSpan.Zero;
            return false;
        }

        /// <summary>
        /// Gets all properties as a collection of name-value pairs.
        /// </summary>
        /// <returns>An enumerable collection of name-value pairs representing the properties.</returns>
        public virtual IEnumerable<KeyValuePair<string, object?>> GetAllProperties()
        {
            var propNames = GetPropertyNames().ToArray();

            for (int i = 0; i < propNames.Length; i++)
            {
                var propertyName = propNames[i];
                var valueType = GetPropertyType(propertyName);

                switch (valueType)
                {
                    case IPropertyBag.PropertyType.Collection:
                        if (TryGetCollectionProperty(propertyName, out var collectionValue))
                        {
                            yield return new KeyValuePair<string, object?>(propertyName, collectionValue);
                        }

                        break;
                    default:
                    case IPropertyBag.PropertyType.String:
                    case IPropertyBag.PropertyType.Flags:
                    case IPropertyBag.PropertyType.Enum:
                        if (TryGetStringProperty(propertyName, out var stringValue))
                        {
                            yield return new KeyValuePair<string, object?>(propertyName, stringValue);
                        }
                        break;
                    case IPropertyBag.PropertyType.Integer:
                        if (TryGetIntProperty(propertyName, out var intValue))
                        {
                            yield return new KeyValuePair<string, object?>(propertyName, intValue);
                        }
                        break;
                    case IPropertyBag.PropertyType.Bool:
                        if (TryGetBoolProperty(propertyName, out var boolValue))
                        {
                            yield return new KeyValuePair<string, object?>(propertyName, boolValue);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Converts the property bag to a JSON string representation with the specified indentation level.
        /// </summary>
        /// <param name="indentLevel">The indentation level to apply to the JSON string.</param>
        /// <returns>A JSON string representation of the property bag with the specified indentation level.</returns>
        public virtual string ToIndentedJsonString(int indentLevel)
        {
            var writer = new LogWriter.StringBuilderLogWriter();
            writer.IndentLevel = indentLevel;
            ToJsonString(writer);
            return writer.ToString();
        }

        /// <summary>
        /// Converts the property bag to a JSON string representation
        /// and writes it to the specified <see cref="IStructuredTextWriter"/>.
        /// </summary>
        /// <param name="writer">The log writer to write the JSON string to.</param>
        /// <param name="flags">Flags to control the JSON string formatting.</param>
        public virtual void ToJsonString(
            IStructuredTextWriter? writer = null,
            JsonUtils.StringFormatFlags flags = JsonUtils.StringFormatFlags.None)
        {
            var properties = GetAllProperties();
            JsonUtils.ToJsonString(properties, writer, flags);
        }

        /// <summary>
        /// Converts the property bag to a XML string representation
        /// and writes it to the specified <see cref="IStructuredTextWriter"/>.
        /// </summary>
        /// <param name="writer">The log writer to write the XML string to.</param>
        /// <param name="flags">Flags to control the XML string formatting.</param>
        /// <param name="nodeName">The name of the root XML node.</param>
        public virtual void ToXmlString(
            string? nodeName,
            IStructuredTextWriter? writer = null,
            XmlUtils.StringFormatFlags flags = XmlUtils.StringFormatFlags.None)
        {
            var properties = GetAllProperties();
            
            if (IsRoot)
            {
                flags |= XmlUtils.StringFormatFlags.AddXmlDeclaration;
            }

            XmlUtils.ToXmlString(nodeName, properties, writer, flags);
        }

        /// <summary>
        /// Logs the properties of the property bag.
        /// </summary>
        public virtual void Log(IStructuredTextWriter? writer = null)
        {
            writer ??= LogWriter.Debug;
            ToJsonString(writer);
        }

        /// <inheritdoc/>
        public virtual bool TryGetCollectionProperty(string name, out IEnumerable<IPropertyBag>? value)
        {
            if (properties.TryGetValue(name, out var objValue) && objValue is IEnumerable<IPropertyBag> collectionValue)
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
            if (value is IEnumerable<IPropertyBag>)
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
        public virtual bool TryGetUniqueIdProperty(string name, out ObjectUniqueId value)
        {
            if (TryGetStringProperty(name, out string? stringValue))
            {
                var parsedValue = ObjectUniqueId.FromString(stringValue);

                if (parsedValue != null)
                {
                    value = parsedValue.Value;
                    return true;
                }
            }

            value = ObjectUniqueId.Empty;
            return false;
        }

        /// <inheritdoc/>
        public virtual void SetUniqueIdProperty(string name, ObjectUniqueId value)
        {
            SetStringProperty(name, value.ToString());
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
        public virtual void SetCollectionProperty(string name, IEnumerable<IPropertyBag> collection)
        {
            properties[name] = collection;
        }

        /// <inheritdoc/>
        public virtual void SetProperty(string name, object? value)
        {
            properties[name] = value;
        }

        /// <inheritdoc/>
        public virtual IEnumerable<string> GetPropertyNames(bool sorted = true)
        {
            return sorted ? properties.Keys.OrderBy(k => k) : properties.Keys;
        }

        /// <inheritdoc/>
        public virtual bool Remove(string name)
        {
            return properties.Remove(name);
        }

        /// <summary>
        /// Converts the specified value to the appropriate property type and sets it in the property bag.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value to set.</param>
        protected virtual void ConvertAndSetProperty(string name, object? value)
        {
            if (value is null)
                return;
            if (value is bool boolValue)
            {
                SetBoolProperty(name, boolValue);
                return;
            }

            if (value is int intValue)
            {
                SetIntProperty(name, intValue);
                return;
            }

            if (value is IEnumerable<IPropertyBag> propertyBags)
            {
                SetCollectionProperty(name, propertyBags);
                return;
            }

            if (value is IEnumerable collection)
            {
                SetCollectionProperty(name, collection);
                return;
            }

            var s = value.ToString();

            if (s is null)
                return;

            SetStringProperty(name, s);
        }

        /// <inheritdoc/>
        public void SetDateProperty(string name, DateOnly value)
        {
            SetStringProperty(name, DateToString(value));
        }

        /// <inheritdoc/>
        public void SetTimeProperty(string name, TimeOnly value)
        {
            SetStringProperty(name, TimeToString(value));
        }

        /// <inheritdoc/>
        public bool TryGetTimeProperty(string name, out TimeOnly value)
        {
            if (TryGetStringProperty(name, out var str) &&
                TryTimeFromString(str, out var timeValue))
            {
                value = timeValue;
                return true;
            }

            value = TimeOnly.MinValue;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetDateProperty(string name, out DateOnly value)
        {
            if (TryGetStringProperty(name, out var str) &&
                TryDateFromString(str, out var dateValue))
            {
                value = dateValue;
                return true;
            }

            value = DateOnly.MinValue;
            return false;
        }

        /// <summary>
        /// Gets the effective <see cref="IDateTimeSerialization"/> instance to
        /// use for date and time serialization and deserialization.
        /// </summary>
        /// <returns>The effective <see cref="IDateTimeSerialization"/> instance.</returns>
        protected virtual IDateTimeSerialization EffectiveDateTimeSerialization()
        {
            return (SerializationManager ?? ISerializableObjectManager.Instance).DateTimeManager;
        }
    }
}
