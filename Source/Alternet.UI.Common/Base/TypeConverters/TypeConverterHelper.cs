// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

using Alternet.UI.Port;

namespace Alternet.UI
{
    /// <summary>
    /// Contains helper methods and properties related to <see cref="TypeConverter"/>.
    /// Provides functionality to get a <see cref="TypeConverter"/>
    /// from a property or the type of the property, based on logic similar
    /// to <see cref="TypeDescriptor.GetConverter(Type)"/>.
    /// </summary>
    internal static class TypeConverterHelper
    {
        private static IndexedValues<Type, TypeConverter> convertersCached = new();

        static TypeConverterHelper()
        {
            TypeDescriptor.Refreshed += TypeDescriptorRefreshed;
        }

        /// <summary>
        /// Returns a <see cref="TypeConverter"/> for the given target Type,
        /// otherwise Null if not found.
        /// First, if the type is one of the known system types, it lookups
        /// a table to determine the <see cref="TypeConverter"/>.
        /// Next, it tries to find a <see cref="TypeConverterAttribute"/> on
        /// the type using reflection.
        /// Finally, it looks up the table of known typeConverters again if
        /// the given type derives from one of the known system types.
        /// </summary>
        /// <param name="type">The target <see cref="Type"/> for
        /// which to find a <see cref="TypeConverter"/>.</param>
        /// <returns>A <see cref="TypeConverter"/> for the <see cref="Type"/>
        /// type if found; Null otherwise.</returns>
        /// <param name="culture">Optional <see cref="CultureInfo"/> object used to create
        /// <see cref="TypeConverter"/> instance.</param>
        /// <returns></returns>
        public static TypeConverter? GetTypeConverter(Type? type, CultureInfo? culture = null)
        {
            if (type == null)
                return null;

            var result = convertersCached.GetValue(type, Internal);
            return result;

            TypeConverter Internal()
            {
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
                            culture ?? App.InvariantEnglishUS) as TypeConverter;
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
        }

        private static Type? GetConverterType(Type type)
        {
            Debug.Assert(type != null, "Null passed for type to GetConverterType");

            // Try looking for the TypeConverter for the type using reflection.
            string? converterName
                = AssemblyUtils.GetTypeConverterAttributeData(type, out Type? converterType);

            if (converterType == null)
            {
                converterType = GetConverterTypeFromName(converterName);
            }

            return converterType;
        }

        private static Type? GetConverterTypeFromName(string? converterName)
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

#pragma warning disable
        private static Type? GetCoreConverterTypeFromCustomType(Type type)
#pragma warning restore
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
                converterType = typeof(DateTimeConverterUixml);
            }

            return converterType;
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
                typeConverter = new DateTimeConverterUixml();
            }
            else if (AssemblyUtils.IsNullableType(type))
            {
                typeConverter = new System.ComponentModel.NullableConverter(type);
            }

            return typeConverter;
        }

        private static TypeConverter? GetCoreConverterFromCustomType(Type type)
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
                typeConverter = new DateTimeConverterUixml();
            }
            else if (typeof(Uri).IsAssignableFrom(type))
            {
                typeConverter = new TypeUriConverter();
            }

            return typeConverter;
        }

        private static void TypeDescriptorRefreshed(RefreshEventArgs args)
        {
            convertersCached = new();
        }
    }
}