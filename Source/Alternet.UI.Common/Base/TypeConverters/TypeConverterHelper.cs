// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Alternet.UI.Port
{
    /// <summary>
    /// Class that provides functionality to obtain a TypeConverter from a property or the
    /// type of the property, based on logic similar to TypeDescriptor.GetConverter.
    /// </summary>
    internal static class TypeConverterHelper
    {
        private static CultureInfo invariantEnglishUS = CultureInfo.InvariantCulture;

        internal static CultureInfo InvariantEnglishUS
        {
            get
            {
                return invariantEnglishUS;
            }
        }

        internal static Type? GetConverterType(Type type)
        {
            Debug.Assert(type != null, "Null passed for type to GetConverterType");

            Type? converterType = null;

            // Try looking for the TypeConverter for the type using reflection.
            string? converterName
                = AssemblyUtils.GetTypeConverterAttributeData(type, out converterType);

            if (converterType == null)
            {
                converterType = GetConverterTypeFromName(converterName);
            }

            return converterType;
        }

        internal static Type? GetConverterTypeFromName(string? converterName)
        {
            Type? GetQualifiedType()
            {
                return Type.GetType(converterName);
            }

            Type? converterType = null;

            if (!string.IsNullOrEmpty(converterName))
            {
                converterType = GetQualifiedType();

                if (converterType != null)
                {
                    // Validate that this is an accessible type converter.
                    if (!AssemblyUtils.IsPublicType(converterType))
                    {
                        converterType = null;
                    }
                }
            }

            return converterType;
        }

        internal static Type? GetCoreConverterTypeFromCustomType(Type type)
        {
            Type? converterType = null;
            if (type.IsEnum)
            {
                // Need to handle Enums types specially as they require a ctor that
                // takes the underlying type, but at compile time we only need to know
                // the Type of the Converter and not an actual instance of it.
                converterType = typeof(EnumConverter);
            }
            else if (typeof(int).IsAssignableFrom(type))
            {
                converterType = typeof(Int32Converter);
            }
            else if (typeof(short).IsAssignableFrom(type))
            {
                converterType = typeof(Int16Converter);
            }
            else if (typeof(long).IsAssignableFrom(type))
            {
                converterType = typeof(Int64Converter);
            }
            else if (typeof(uint).IsAssignableFrom(type))
            {
                converterType = typeof(UInt32Converter);
            }
            else if (typeof(ushort).IsAssignableFrom(type))
            {
                converterType = typeof(UInt16Converter);
            }
            else if (typeof(ulong).IsAssignableFrom(type))
            {
                converterType = typeof(UInt64Converter);
            }
            else if (typeof(bool).IsAssignableFrom(type))
            {
                converterType = typeof(BooleanConverter);
            }
            else if (typeof(double).IsAssignableFrom(type))
            {
                converterType = typeof(DoubleConverter);
            }
            else if (typeof(float).IsAssignableFrom(type))
            {
                converterType = typeof(SingleConverter);
            }
            else if (typeof(byte).IsAssignableFrom(type))
            {
                converterType = typeof(ByteConverter);
            }
            else if (typeof(sbyte).IsAssignableFrom(type))
            {
                converterType = typeof(SByteConverter);
            }
            else if (typeof(char).IsAssignableFrom(type))
            {
                converterType = typeof(CharConverter);
            }
            else if (typeof(decimal).IsAssignableFrom(type))
            {
                converterType = typeof(DecimalConverter);
            }
            else if (typeof(TimeSpan).IsAssignableFrom(type))
            {
                converterType = typeof(TimeSpanConverter);
            }
            else if (typeof(Guid).IsAssignableFrom(type))
            {
                converterType = typeof(GuidConverter);
            }
            else if (typeof(string).IsAssignableFrom(type))
            {
                converterType = typeof(StringConverter);
            }
            else if (typeof(CultureInfo).IsAssignableFrom(type))
            {
                converterType = typeof(CultureInfoConverter);
            }
            else if (typeof(Type).IsAssignableFrom(type))
            {
                converterType = typeof(TypeTypeConverter);
            }
            else if (typeof(DateTime).IsAssignableFrom(type))
            {
                converterType = typeof(DateTimeConverter2);
            }

            return converterType;
        }

        internal static TypeConverter? GetCoreConverterFromCustomType(Type type)
        {
            TypeConverter? typeConverter = null;
            if (type.IsEnum)
            {
                // Need to handle Enums types specially as they require a ctor that
                // takes the underlying type.
                typeConverter = new System.ComponentModel.EnumConverter(type);
            }
            else if (typeof(int).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.Int32Converter();
            }
            else if (typeof(short).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.Int16Converter();
            }
            else if (typeof(long).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.Int64Converter();
            }
            else if (typeof(uint).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.UInt32Converter();
            }
            else if (typeof(ushort).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.UInt16Converter();
            }
            else if (typeof(ulong).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.UInt64Converter();
            }
            else if (typeof(bool).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.BooleanConverter();
            }
            else if (typeof(double).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.DoubleConverter();
            }
            else if (typeof(float).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.SingleConverter();
            }
            else if (typeof(byte).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.ByteConverter();
            }
            else if (typeof(sbyte).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.SByteConverter();
            }
            else if (typeof(char).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.CharConverter();
            }
            else if (typeof(decimal).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.DecimalConverter();
            }
            else if (typeof(TimeSpan).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.TimeSpanConverter();
            }
            else if (typeof(Guid).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.GuidConverter();
            }
            else if (typeof(string).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.StringConverter();
            }
            else if (typeof(CultureInfo).IsAssignableFrom(type))
            {
                typeConverter = new System.ComponentModel.CultureInfoConverter();
            }
            else if (typeof(Type).IsAssignableFrom(type))
            {
                typeConverter = new TypeTypeConverter();
            }
            else if (typeof(DateTime).IsAssignableFrom(type))
            {
                typeConverter = new DateTimeConverter2();
            }
            else if (typeof(Uri).IsAssignableFrom(type))
            {
                typeConverter = new TypeUriConverter();
            }

            return typeConverter;
        }

        /// <summary>
        /// Returns a TypeConverter for the given target Type, otherwise null if not found.
        /// First, if the type is one of the known system types, it lookups
        /// a table to determine the TypeConverter.
        /// Next, it tries to find a TypeConverterAttribute on the type using reflection.
        /// Finally, it looks up the table of known typeConverters again if
        /// the given type derives from one of the
        /// known system types.
        /// </summary>
        /// <param name="type">The target Type for which to find a TypeConverter.</param>
        /// <returns>A TypeConverter for the Type type if found. Null otherwise.</returns>
        internal static TypeConverter? GetTypeConverter(Type type)
        {
            if (type == null)
                return null;

            TypeConverter? typeConverter = GetCoreConverterFromCoreType(type);

            if (typeConverter == null)
            {
                Type? converterType = GetConverterType(type);
                if (converterType != null)
                {
                    typeConverter = Activator.CreateInstance(
                        converterType,
                        BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public,
                        null,
                        null,
                        InvariantEnglishUS) as TypeConverter;
                }
                else
                {
                    typeConverter = GetCoreConverterFromCustomType(type);
                }

                if (typeConverter == null)
                {
                    typeConverter = new TypeConverter();
                }
            }

            return typeConverter;
        }

        private static TypeConverter? GetCoreConverterFromCoreType(Type type)
        {
            TypeConverter? typeConverter = null;
            if (type == typeof(int))
            {
                typeConverter = new System.ComponentModel.Int32Converter();
            }
            else if (type == typeof(short))
            {
                typeConverter = new System.ComponentModel.Int16Converter();
            }
            else if (type == typeof(long))
            {
                typeConverter = new System.ComponentModel.Int64Converter();
            }
            else if (type == typeof(uint))
            {
                typeConverter = new System.ComponentModel.UInt32Converter();
            }
            else if (type == typeof(ushort))
            {
                typeConverter = new System.ComponentModel.UInt16Converter();
            }
            else if (type == typeof(ulong))
            {
                typeConverter = new System.ComponentModel.UInt64Converter();
            }
            else if (type == typeof(bool))
            {
                typeConverter = new System.ComponentModel.BooleanConverter();
            }
            else if (type == typeof(double))
            {
                typeConverter = new System.ComponentModel.DoubleConverter();
            }
            else if (type == typeof(float))
            {
                typeConverter = new System.ComponentModel.SingleConverter();
            }
            else if (type == typeof(byte))
            {
                typeConverter = new System.ComponentModel.ByteConverter();
            }
            else if (type == typeof(sbyte))
            {
                typeConverter = new System.ComponentModel.SByteConverter();
            }
            else if (type == typeof(char))
            {
                typeConverter = new System.ComponentModel.CharConverter();
            }
            else if (type == typeof(decimal))
            {
                typeConverter = new System.ComponentModel.DecimalConverter();
            }
            else if (type == typeof(TimeSpan))
            {
                typeConverter = new System.ComponentModel.TimeSpanConverter();
            }
            else if (type == typeof(Guid))
            {
                typeConverter = new System.ComponentModel.GuidConverter();
            }
            else if (type == typeof(string))
            {
                typeConverter = new System.ComponentModel.StringConverter();
            }
            else if (type == typeof(CultureInfo))
            {
                typeConverter = new System.ComponentModel.CultureInfoConverter();
            }
            else if (type == typeof(Type))
            {
                typeConverter = new TypeTypeConverter();
            }
            else if (type == typeof(DateTime))
            {
                typeConverter = new DateTimeConverter2();
            }
            else if (AssemblyUtils.IsNullableType(type))
            {
                typeConverter = new System.ComponentModel.NullableConverter(type);
            }

            return typeConverter;
        }
    }
}