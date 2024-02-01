using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Assembly"/>, <see cref="PropertyInfo"/> and <see cref="Type"/>
    /// related static methods.
    /// </summary>
    public static class AssemblyUtils
    {
        /// <summary>
        /// Gets <c>true</c> value as <see cref="object"/>.
        /// </summary>
        public static readonly object True = true;

        /// <summary>
        /// Gets <c>false</c> value as <see cref="object"/>.
        /// </summary>
        public static readonly object False = false;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="sbyte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueSByte = sbyte.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="byte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueByte = byte.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="short"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueInt16 = short.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="ushort"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueUInt16 = ushort.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="int"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueInt32 = int.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="uint"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueUInt32 = uint.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="long"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueInt64 = long.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="ulong"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueUInt64 = ulong.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="float"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueSingle = float.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="double"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueDouble = double.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="decimal"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueDecimal = decimal.MaxValue;

        /// <summary>
        /// Gets maximal possible variable value for the <see cref="DateTime"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MaxValueDateTime = DateTime.MaxValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="sbyte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueSByte = sbyte.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="byte"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueByte = byte.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="short"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueInt16 = short.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="ushort"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueUInt16 = ushort.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="int"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueInt32 = int.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="uint"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueUInt32 = uint.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="long"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueInt64 = long.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="ulong"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueUInt64 = ulong.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="float"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueSingle = float.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="double"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueDouble = double.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="decimal"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueDecimal = decimal.MinValue;

        /// <summary>
        /// Gets minimal possible variable value for the <see cref="DateTime"/> type
        /// as <see cref="object"/>.
        /// </summary>
        public static readonly object MinValueDateTime = DateTime.MinValue;

        /// <summary>
        /// Gets default value for the <see cref="sbyte"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultSByte = default(sbyte);

        /// <summary>
        /// Gets default value for the <see cref="byte"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultByte = default(byte);

        /// <summary>
        /// Gets default value for the <see cref="short"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultInt16 = default(short);

        /// <summary>
        /// Gets default value for the <see cref="ushort"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultUInt16 = default(ushort);

        /// <summary>
        /// Gets default value for the <see cref="int"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultInt32 = default(int);

        /// <summary>
        /// Gets default value for the <see cref="uint"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultUInt32 = default(uint);

        /// <summary>
        /// Gets default value for the <see cref="long"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultInt64 = default(long);

        /// <summary>
        /// Gets default value for the <see cref="ulong"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultUInt64 = default(ulong);

        /// <summary>
        /// Gets default value for the <see cref="float"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultSingle = default(float);

        /// <summary>
        /// Gets default value for the <see cref="double"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultDouble = default(double);

        /// <summary>
        /// Gets default value for the <see cref="decimal"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultDecimal = default(decimal);

        /// <summary>
        /// Gets default value for the <see cref="DateTime"/> type as <see cref="object"/>.
        /// </summary>
        public static readonly object DefaultDateTime = default(DateTime);

        /// <summary>
        /// Creates <see cref="Action"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="instance">Object which contains the method.</param>
        /// <param name="method">Method which needs to be converted to <see cref="Action"/>.</param>
        public static Action CreateAction(object instance, MethodInfo method)
        {
            Action action = (Action)Delegate.CreateDelegate(typeof(Action), instance, method);
            return action;
        }

        /// <summary>
        /// Gets reset method for the specified property.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static MethodInfo? GetResetPropMethod(object instance, string propName)
        {
            var methodName = "Reset" + propName;
            var type = instance.GetType();
            var result = type.GetMethod(methodName, Array.Empty<Type>());
            return result;
        }

        /// <summary>
        /// Gets whether specified type has declared events (events declared in the parent
        /// types are not counted).
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns></returns>
        public static bool HasOwnEvents(Type? type)
        {
            if (type == null)
                return false;
            var events = AssemblyUtils.EnumEvents(type, true);

            foreach (var item in events)
            {
                if (item.DeclaringType != type)
                    continue;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Enumerates event information for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type which events are enumerated.</param>
        /// <param name="sort">Defines whether to sort result events by name.</param>
        /// <param name="bindingFlags">Specifies flags that control the way in which
        /// the search for members is conducted.</param>
        /// <returns></returns>
        public static IEnumerable<EventInfo> EnumEvents(
            Type type,
            bool sort = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            List<EventInfo> result = new();

            IList<EventInfo> props =
                new List<EventInfo>(type.GetEvents(bindingFlags));

            SortedList<string, EventInfo> addedNames = new();

            foreach (var p in props)
            {
                var propName = p.Name;
                if (addedNames.ContainsKey(propName))
                    continue;
                result.Add(p);
                addedNames.Add(propName, p);
            }

            if (sort)
                result.Sort(PropertyGridItem.CompareByName);
            return result;
        }

        /// <summary>
        /// Enumerates property information for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type which events are enumerated.</param>
        /// <param name="sort">Defines whether to sort result events by name.</param>
        /// <param name="bindingFlags">Specifies flags that control the way in which
        /// the search for members is conducted.</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> EnumProps(
            Type type,
            bool sort = false,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            List<PropertyInfo> result = new();

            IList<PropertyInfo> props =
                new List<PropertyInfo>(type.GetProperties(bindingFlags));

            SortedList<string, PropertyInfo> addedNames = new();

            foreach (var p in props)
            {
                var propName = p.Name;
                if (addedNames.ContainsKey(propName))
                    continue;
                result.Add(p);
                addedNames.Add(propName, p);
            }

            if (sort)
                result.Sort(PropertyGridItem.CompareByName);
            return result;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> has constructor without parameters.
        /// </summary>
        /// <param name="type">Object type.</param>
        public static bool HasConstructorNoParams(Type type)
        {
            var result = type.GetConstructor(Type.EmptyTypes);
            return result != null;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> is struct.
        /// </summary>
        /// <param name="type">Object type.</param>
        public static bool IsStruct(Type? type)
        {
            if (type is null)
                return false;
            var realType = AssemblyUtils.GetRealType(type);
            TypeCode typeCode = Type.GetTypeCode(realType);
            var result = (typeCode == TypeCode.Object) && realType.IsValueType && !realType.IsEnum;
            return result;
        }

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="instance">Instance which contains the property.</param>
        /// <param name="propInfo">Property info.</param>
        /// <param name="defValue">Default property value (used if property value is null).</param>
        /// <typeparam name="T">Type of result.</typeparam>
        public static T GetPropValue<T>(object? instance, PropertyInfo propInfo, T defValue)
        {
            object? result = propInfo.GetValue(instance, null);
            if (result == null)
                return defValue;
            else
                return (T)result;
        }

        /// <summary>
        /// Gets real type, using <see cref="Nullable.GetUnderlyingType"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns><see cref="Nullable.GetUnderlyingType"/> if its not null or
        /// <paramref name="type"/> value.</returns>
        public static Type GetRealType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            var realType = underlyingType ?? type;
            return realType;
        }

        /// <summary>
        /// Gets <see cref="TypeCode"/> of the real type using <see cref="GetRealType"/>
        /// and <see cref="Type.GetTypeCode"/>.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns></returns>
        public static TypeCode GetRealTypeCode(Type type)
        {
            var realType = GetRealType(type);
            var typeCode = Type.GetTypeCode(realType);
            return typeCode;
        }

        /// <summary>
        /// Gets property info.
        /// </summary>
        /// <param name="instance">Object instance.</param>
        /// <param name="name">Property name.</param>
        public static PropertyInfo? GetPropInfo(object? instance, string? name)
        {
            if (instance == null || string.IsNullOrEmpty(name))
                return null;
            var type = instance.GetType();
            var propInfo = type.GetProperty(name);
            return propInfo;
        }

        /// <summary>
        /// Gets minimal possible variable value for the given <see cref="TypeCode"/>.
        /// </summary>
        public static object? GetMinValue(TypeCode code)
        {
            return code switch
            {
                TypeCode.SByte => MinValueSByte,
                TypeCode.Byte => MinValueByte,
                TypeCode.Int16 => MinValueInt16,
                TypeCode.UInt16 => MinValueUInt16,
                TypeCode.Int32 => MinValueInt32,
                TypeCode.UInt32 => MinValueUInt32,
                TypeCode.Int64 => MinValueInt64,
                TypeCode.UInt64 => MinValueUInt64,
                TypeCode.Single => MinValueSingle,
                TypeCode.Double => MinValueDouble,
                TypeCode.Decimal => MinValueDecimal,
                TypeCode.DateTime => MinValueDateTime,
                _ => null,
            };
        }

        /// <summary>
        /// Returns minimal and maximal possible values for the given <see cref="TypeCode"/>
        /// as a range string using specified <paramref name="format"/>.
        /// </summary>
        /// <remarks>
        /// For example, if format is "{0}..{1}", result is "0..255".
        /// </remarks>
        public static string? GetMinMaxRangeStr(
            TypeCode code,
            string? format = null,
            object? minValue = null,
            object? maxValue = null)
        {
            var minValueT = MathUtils.Max(GetMinValue(code), minValue);
            var maxValueT = MathUtils.Min(GetMaxValue(code), maxValue);
            if (minValueT is null || maxValueT is null)
                return null;
            format ??= "{0}..{1}";
            return string.Format(format, minValueT, maxValueT);
        }

        /// <summary>
        /// Upgrades signed inegers to <see cref="long"/>, unsigned integers to
        /// <see cref="ulong"/> and returns all other types as is.
        /// </summary>
        /// <param name="value">Value of any type.</param>
        public static object UpgradeNumberType(object value)
        {
            var typeCode = AssemblyUtils.GetRealTypeCode(value.GetType());
            return typeCode switch
            {
                TypeCode.SByte => LongOrULong((long)((sbyte)value)),
                TypeCode.Int16 => LongOrULong((long)((short)value)),
                TypeCode.Int32 => LongOrULong((long)((int)value)),
                TypeCode.Byte => (ulong)((byte)value),
                TypeCode.UInt16 => (ulong)((ushort)value),
                TypeCode.UInt32 => (ulong)((uint)value),
                _ => value,
            };

            static object LongOrULong(long longValue)
            {
                if (longValue < 0)
                    return longValue;
                return (ulong)longValue;
            }
        }

        /// <summary>
        /// Gets default <see cref="NumberStyles"/> for the specified <paramref name="typeCode"/>.
        /// </summary>
        public static NumberStyles GetDefaultNumberStyles(TypeCode typeCode)
        {
            const NumberStyles NumberStylesInt = NumberStyles.Integer;
            const NumberStyles NumberStylesFloat = NumberStyles.Float | NumberStyles.AllowThousands;

            return typeCode switch
            {
                TypeCode.SByte => NumberStylesInt,
                TypeCode.Byte => NumberStylesInt,
                TypeCode.Int16 => NumberStylesInt,
                TypeCode.UInt16 => NumberStylesInt,
                TypeCode.Int32 => NumberStylesInt,
                TypeCode.UInt32 => NumberStylesInt,
                TypeCode.Int64 => NumberStylesInt,
                TypeCode.UInt64 => NumberStylesInt,

                TypeCode.Single => NumberStylesFloat,
                TypeCode.Double => NumberStylesFloat,

                TypeCode.Decimal => NumberStyles.Number,

                _ => NumberStyles.None,
            };
        }

        /// <summary>
        /// Gets default value for the given <see cref="TypeCode"/>.
        /// </summary>
        public static object? GetDefaultValue(TypeCode code)
        {
            return code switch
            {
                TypeCode.SByte => DefaultSByte,
                TypeCode.Byte => DefaultByte,
                TypeCode.Int16 => DefaultInt16,
                TypeCode.UInt16 => DefaultUInt16,
                TypeCode.Int32 => DefaultInt32,
                TypeCode.UInt32 => DefaultUInt32,
                TypeCode.Int64 => DefaultInt64,
                TypeCode.UInt64 => DefaultUInt64,
                TypeCode.Single => DefaultSingle,
                TypeCode.Double => DefaultDouble,
                TypeCode.Decimal => DefaultDecimal,
                TypeCode.DateTime => DefaultDateTime,
                _ => null,
            };
        }

        /// <summary>
        /// Gets maximal possible variable value for the given <see cref="TypeCode"/>.
        /// </summary>
        public static object? GetMaxValue(TypeCode code)
        {
            return code switch
            {
                TypeCode.SByte => MaxValueSByte,
                TypeCode.Byte => MaxValueByte,
                TypeCode.Int16 => MaxValueInt16,
                TypeCode.UInt16 => MaxValueUInt16,
                TypeCode.Int32 => MaxValueInt32,
                TypeCode.UInt32 => MaxValueUInt32,
                TypeCode.Int64 => MaxValueInt64,
                TypeCode.UInt64 => MaxValueUInt64,
                TypeCode.Single => MaxValueSingle,
                TypeCode.Double => MaxValueDouble,
                TypeCode.Decimal => MaxValueDecimal,
                TypeCode.DateTime => MaxValueDateTime,
                _ => null,
            };
        }

        /// <summary>
        /// Returns whether property is nullable (for example byte?).
        /// </summary>
        /// <param name="p">Property info.</param>
        /// <returns><c>true</c> if property is nullable, <c>false</c> otherwise.</returns>
        public static bool GetNullable(PropertyInfo p)
        {
            var propType = p.PropertyType;
            var underlyingType = Nullable.GetUnderlyingType(propType);
            var isNullable = underlyingType != null;
            return isNullable;
        }

        /// <summary>
        /// Returns whether property is browsable.
        /// </summary>
        /// <param name="p">Property info.</param>
        /// <returns><c>true</c> if property is browsable, <c>false</c> otherwise.</returns>
        public static bool GetBrowsable(PropertyInfo p)
        {
            var browsable = p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute;
            if (browsable is not null)
                return browsable.Browsable;
            return true;
        }

        /// <summary>
        /// Returns toolbox category of the control.
        /// </summary>
        /// <param name="type">Type of the control.</param>
        /// <returns></returns>
        public static ControlCategoryAttribute? GetControlCategory(Type type)
        {
            var attr = type.GetCustomAttribute(typeof(ControlCategoryAttribute)) as ControlCategoryAttribute;
            return attr;
        }

        /// <summary>
        /// Gets default value of the property using <see cref="DefaultValueAttribute"/>.
        /// </summary>
        /// <param name="p">Property information.</param>
        /// <param name="defValue">Default value of the property.</param>
        /// <returns></returns>
        public static bool GetDefaultValue(PropertyInfo p, out object? defValue)
        {
            var attr = p.GetCustomAttribute(typeof(DefaultValueAttribute));
            if (attr is not DefaultValueAttribute defaultValueAttr)
            {
                defValue = null;
                return false;
            }

            defValue = defaultValueAttr.Value;
            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is number type.
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static bool IsTypeCodeNumber(TypeCode typeCode)
        {
            switch (typeCode)
            {
                default:
                    return false;
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is an unsigned integer number type
        /// (byte, ushort, uint, ulong).
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static bool IsTypeCodeUnsignedInt(TypeCode typeCode)
        {
            switch (typeCode)
            {
                default:
                    return false;
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is a signed integer number type
        /// (sbyte, short, int, long).
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static bool IsTypeCodeSignedInt(TypeCode typeCode)
        {
            switch (typeCode)
            {
                default:
                    return false;
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="typeCode"/> is a floating point number type
        /// (float, double or decimal).
        /// </summary>
        /// <param name="typeCode">Type code.</param>
        public static bool IsTypeCodeFloat(TypeCode typeCode)
        {
            switch (typeCode)
            {
                default:
                    return false;
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
            }
        }

        /// <summary>
        /// Returns whether enumeration has <see cref="FlagsAttribute"/>.
        /// </summary>
        /// <param name="type">Type of enumeration.</param>
        /// <returns></returns>
        public static bool EnumIsFlags(Type type)
        {
            var flags = type.GetCustomAttribute(typeof(FlagsAttribute)) as FlagsAttribute;
            if (flags is not null)
                return true;
            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if <paramref name="type"/> is a descendant of any type
        /// in the array of types.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Type to check.</param>
        /// <param name="baseTypes">Base types array.</param>
        public static bool TypeIsDescendant(Type type, Type[] baseTypes)
        {
            foreach(var item in baseTypes)
            {
                if (TypeIsDescendant(type, item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if specified type is a descendant of another type.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Descendant type.</param>
        /// <param name="baseTypeToCheck">Base type.</param>
        /// <returns></returns>
        public static bool TypeIsDescendant(Type type, Type baseTypeToCheck)
        {
            while (true)
            {
                if (type == null)
                    return false;

                var baseType = type.BaseType;

                if (baseType == baseTypeToCheck)
                    return true;

                if (type.IsInterface)
                {
                    if (baseType == null)
                        return false;
                }

                if (type.IsClass)
                {
                    if (baseType == typeof(object))
                        return false;
                }

                type = baseType!;
            }
        }

        /// <summary>
        /// Outputs all <see cref="Control"/> descendants to the debug console.
        /// </summary>
        public static void ControlsToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                GetTypeDescendants(typeof(Control)),
                (t) => Debug.WriteLine(t.Name));
        }

        /// <summary>
        /// Outputs all <see cref="Native.NativeObject"/> descendants to the debug console.
        /// </summary>
        public static void NativeObjectToConsole()
        {
            EnumerableUtils.ForEach<Type>(
                GetTypeDescendants(typeof(Native.NativeObject), true, false),
                (t) => Debug.WriteLine(t.Name));
        }

        /// <summary>
        /// Gets whether property is readonly.
        /// </summary>
        /// <param name="prop">Property information.</param>
        /// <returns></returns>
        public static bool IsReadOnly(PropertyInfo prop)
        {
            var result = prop.SetMethod == null || !prop.SetMethod.IsPublic;
            return result;
        }

        /// <summary>
        /// Returns list of types which descend from specified type.
        /// </summary>
        /// <param name="type">Base type.</param>
        /// <param name="ascending">Sort result ascending by type name.</param>
        /// <param name="isPublic">Whether to return only public types.</param>
        public static IEnumerable<Type> GetTypeDescendants(
            Type type,
            bool ascending = true,
            bool isPublic = true)
        {
            List<Type> result = new();

            Assembly asm = type.Assembly;

            var definedTypes = asm.DefinedTypes;

            foreach (TypeInfo typeInfo in definedTypes)
            {
                var resultType = typeInfo.AsType();
                if (resultType.IsAbstract || (resultType.IsPublic != isPublic))
                    continue;
                if (TypeIsDescendant(resultType, type))
                    result.Add(resultType);
            }

            if (!ascending)
                return result;

            result.Sort(StringUtils.ComparerObjectUsingToString);

            return result;
        }
    }
}
