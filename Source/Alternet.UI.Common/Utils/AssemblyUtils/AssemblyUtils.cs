﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Assembly"/>, <see cref="PropertyInfo"/> and <see cref="Type"/>
    /// related static methods.
    /// </summary>
    public static partial class AssemblyUtils
    {
        private static AdvDictionary<string, Assembly>? resNameToAssembly;
        private static int resNameToAssemblySavedLength = 0;
        private static SortedList<string, EventInfo>? allControlEvents;
        private static SortedList<string, Type>? allControlDescendants;

        /// <summary>
        /// Gets or sets list of all <see cref="AbstractControl"/> descendants.
        /// </summary>
        public static SortedList<string, Type> AllControlDescendants
        {
            get
            {
                allControlDescendants ??= GetSortedTypeDescendantsWithFullNames(typeof(AbstractControl));
                return allControlDescendants;
            }

            set
            {
                allControlDescendants = value;
            }
        }

        /// <summary>
        /// Gets or sets list of events for all <see cref="AbstractControl"/>
        /// descendants.
        /// </summary>
        public static SortedList<string, EventInfo> AllControlEvents
        {
            get
            {
                allControlEvents ??= AssemblyUtils.GetAllDescendantsEvents(typeof(AbstractControl));
                return allControlEvents;
            }

            set
            {
                allControlEvents = value;
            }
        }

        /// <summary>
        /// Creates the specified type via default parameterless constructor.
        /// </summary>
        /// <typeparam name="T">Type of the object to create.</typeparam>
        /// <returns></returns>
        /// <exception cref="MissingMemberException">Raised when type
        /// does not have a public, parameterless constructor.</exception>
        public static T CreateViaDefaultConstructor<T>()
        {
            try
            {
                return Activator.CreateInstance<T>();
            }
            catch (MissingMethodException)
            {
                throw new MissingMemberException(
                    $"Type '{typeof(T)}' does not have a public, parameterless constructor.");
            }
        }

        /// <summary>
        /// Gets whether specified string is name of the event declared in
        /// any descendant of the <see cref="AbstractControl"/>.
        /// </summary>
        /// <param name="name">Property or event name.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsControlDescendantEventName(string name)
        {
            return AllControlEvents.ContainsKey(name);
        }

        /// <summary>
        /// Gets type of the second parameter for the event. This is usually an
        /// <see cref="EventArgs"/> descendant.
        /// </summary>
        /// <param name="ev">Event information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type? GetEventArgsType(EventInfo? ev)
        {
            var eventArgsType
                = ev?.EventHandlerType.GetMethod("Invoke")?.GetParameters()[1]?.ParameterType;
            return eventArgsType;
        }

        /// <summary>
        /// Gets image url for loading image resource from the specified assembly using
        /// "embres" protocol.
        /// </summary>
        /// <param name="asm">Assembly to load image from.</param>
        /// <param name="name">Image name or relative path.
        /// Slash characters will be changed to '.'.
        /// Example: "ToolBarPng/Large\Calendar32.png" -> "ToolBarPng.Large.Calendar32.png".
        /// </param>
        /// <returns></returns>
        public static string GetImageUrlInAssembly(Assembly asm, string? name = null)
        {
            name ??= string.Empty;
            name = name.Replace('/', '.');
            name = name.Replace('\\', '.');
            var resName = AssemblyUtils.GetAssemblyResPrefix(asm) + name;
            var result = $"embres:{resName}";
            return result;
        }

        /// <summary>
        /// Gets prefix string for the embedded resource.
        /// </summary>
        /// <param name="asm">Assembly.</param>
        /// <returns></returns>
        public static string GetAssemblyResPrefix(Assembly asm)
        {
            var path = asm.Location;
            var name = Path.GetFileNameWithoutExtension(path);
            var result = name + ".";
            return result;
        }

        /// <summary>
        /// Gets <see cref="Assembly"/> with the specified name
        /// searching it through all assemblies of the current domain.
        /// </summary>
        /// <param name="name">Name of the assembly.</param>
        /// <returns></returns>
        public static Assembly? GetAssemblyByName(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var asmName = assembly.GetName().Name;
                if (asmName == name)
                    return assembly;
            }

            return null;
        }

        /// <summary>
        /// Enumerates members of all types in the loaded assemblies.
        /// </summary>
        /// <param name="bindingAttr">The bindings constraint. Optional.
        /// Default is Null. If not specified, members are not filtered by the binding constraint.</param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo[]> GetAllPublicMembers(BindingFlags? bindingAttr = null)
        {
            Func<Type, MemberInfo[]> getMembers;

            if (bindingAttr is null)
            {
                getMembers = (type) =>
                {
                    return type.GetMembers();
                };
            }
            else
            {
                getMembers = (type) =>
                {
                    return type.GetMembers(bindingAttr.Value);
                };
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetExportedTypes();
                foreach (var type in types)
                {
                    var members = getMembers(type);
                    yield return members;
                }
            }
        }

        /// <summary>
        /// Enumerates members of all types in the loaded assemblies filtered
        /// by name and binding constraint.
        /// </summary>
        /// <param name="bindingAttr">The bindings constraint. Optional.
        /// Default is Null. If not specified, members are not filtered by the binding constraint.</param>
        /// <returns></returns>
        /// <param name="nameContainsText">The string which specifies member name constraint.</param>
        public static IEnumerable<MemberInfo> GetAllPublicMembers(
            string nameContainsText,
            BindingFlags? bindingAttr = null)
        {
            SortedList<string, int> addedMethods = new();
            var allMembers = GetAllPublicMembers();

            foreach (var memberArray in allMembers)
            {
                addedMethods.Clear();

                foreach (var member in memberArray)
                {
                    var type = member.DeclaringType;
                    var name = member.Name;

                    if (member is MethodInfo method)
                    {
                        if (method.IsSpecialName)
                            continue;
                        if (addedMethods.ContainsKey(name))
                            continue;
                        addedMethods.Add(name, 0);
                    }

                    var fullName = $"{type.Name}.{name}";

                    if (fullName.IndexOf(
                        nameContainsText,
                        StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        yield return member;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="System.Type"/> object with the specified name
        /// searching it through all assemblies of the current domain.
        /// </summary>
        /// <param name="name">Name of the type in the same format as passed to
        /// <see cref="Assembly.GetType(string)"/>.</param>
        /// <param name="reverse">if <c>true</c> gives priority to most recently loaded types.
        /// Optional. Default is <c>false</c>.</param>
        /// <returns></returns>
        public static Type? GetTypeByName(string name, bool reverse = false)
        {
            IEnumerable<Assembly> assemblies;

            if (reverse)
                assemblies = AppDomain.CurrentDomain.GetAssemblies().Reverse();
            else
                assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var tt = assembly.GetType(name);
                if (tt != null)
                {
                    return tt;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates <see cref="Action"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="instance">Object which contains the method.</param>
        /// <param name="method">Method which needs to be converted to <see cref="Action"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                return addedNames.Values;
            else
                return result;
        }

        /// <summary>
        /// Compares two specified <see cref="EventInfo"/> objects by their names
        /// and returns an
        /// integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">First item to compare.</param>
        /// <param name="y">Second item to compare.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relationship between the two
        /// comparands. Result value less than 0 means that <paramref name="x"/> precedes
        /// <paramref name="y"/> in the sort order.
        /// Result value equal to 0 means <paramref name="x"/> occurs in the same
        /// position as <paramref name="y"/>
        /// in the sort order. Result value greater than 0 means that <paramref name="x"/> follows
        /// <paramref name="y"/> in the sort order.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareByName(EventInfo x, EventInfo y)
        {
            return string.Compare(x.Name, y.Name);
        }

        /// <summary>
        /// Compares two specified <see cref="PropertyInfo"/> objects by their names
        /// and returns an
        /// integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">First item to compare.</param>
        /// <param name="y">Second item to compare.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relationship between the two
        /// comparands. Result value less than 0 means that <paramref name="x"/> precedes
        /// <paramref name="y"/> in the sort order.
        /// Result value equal to 0 means <paramref name="x"/> occurs in the same
        /// position as <paramref name="y"/>
        /// in the sort order. Result value greater than 0 means that <paramref name="x"/> follows
        /// <paramref name="y"/> in the sort order.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CompareByName(PropertyInfo x, PropertyInfo y)
        {
            return string.Compare(x.Name, y.Name);
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
                return addedNames.Values;
            else
                return result;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> has a public instance constructor
        /// whose parameters match the types in the specified array.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="paramTypes">Array of constructor parameter types.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasConstructorWithParams(Type type, Type[] paramTypes)
        {
            var result = type.GetConstructor(paramTypes);
            return result != null;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> has a public instance constructor
        /// with single parameter which has the specified type.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="paramType">Constructor parameter type.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasConstructorWithParam(Type type, Type paramType)
        {
            var result = type.GetConstructor([paramType]);
            return result != null;
        }

        /// <summary>
        /// Gets whether <see cref="Type"/> has constructor without parameters.
        /// </summary>
        /// <param name="type">Object type.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ControlCategoryAttribute? GetControlCategory(Type type)
        {
            var attr = type.GetCustomAttribute(typeof(ControlCategoryAttribute))
                as ControlCategoryAttribute;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTypeCodeNumber(TypeCode typeCode)
        {
            return typeCode >= TypeCode.SByte && typeCode <= TypeCode.Decimal;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// Returns <c>true</c> if specified type equals any type from <paramref name="baseTypes"/>
        /// or is a descendant of that type.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Type to check.</param>
        /// <param name="baseTypes">Base types array.</param>
        public static bool TypeEqualsOrDescendant(Type type, Type[] baseTypes)
        {
            foreach (var item in baseTypes)
            {
                if (type == item)
                    return true;
            }

            foreach (var item in baseTypes)
            {
                if (TypeIsDescendant(type, item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns <c>true</c> if specified type equals <paramref name="baseType"/>
        /// or is a descendant of that type.
        /// </summary>
        /// <remarks>This method checks all base types recursively not only
        /// the first <see cref="Type.BaseType"/> value.</remarks>
        /// <param name="type">Type to check.</param>
        /// <param name="baseType">Base type.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TypeEqualsOrDescendant(Type type, Type baseType)
        {
            var result = type == baseType || TypeIsDescendant(type, baseType);
            return result;
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
        /// Gets whether property is readonly.
        /// </summary>
        /// <param name="prop">Property information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsReadOnly(PropertyInfo prop)
        {
            var result = prop.SetMethod == null || !prop.SetMethod.IsPublic;
            return result;
        }

        /// <summary>
        /// Returns list of types which descend from specified type.
        /// </summary>
        /// <param name="type">Base type.</param>
        public static IEnumerable<Type> GetTypeDescendants(Type type)
        {
            List<Type> result = new();

            Assembly asm = type.Assembly;

            var definedTypes = asm.DefinedTypes;

            foreach (TypeInfo typeInfo in definedTypes)
            {
                var resultType = typeInfo.AsType();
                if (resultType.IsAbstract || !resultType.IsPublic)
                    continue;
                if (TypeIsDescendant(resultType, type))
                    result.Add(resultType);
            }

            return result;
        }

        /// <summary>
        /// Returns list of types which descend from specified type.
        /// Result is sorted by <see cref="Type.FullName"/> and uses it as keys.
        /// </summary>
        /// <param name="type">Base type.</param>
        /// <returns></returns>
        public static SortedList<string, Type> GetSortedTypeDescendantsWithFullNames(Type type)
        {
            SortedList<string, Type> result = new();
            var types = GetTypeDescendants(type);
            foreach(var item in types)
            {
                result.Add(item.FullName, item);
            }

            return result;
        }

        /// <summary>
        /// Finds assembly for the specified resource name.
        /// </summary>
        /// <param name="resName">Resource name.</param>
        /// <returns></returns>
        public static Assembly? FindAssemblyForResource(string resName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (resNameToAssembly is null || assemblies.Length != resNameToAssemblySavedLength)
            {
                resNameToAssembly ??= new();
                resNameToAssemblySavedLength = assemblies.Length;

                foreach (var assembly in assemblies)
                {
                    if (assembly.IsDynamic)
                        continue;

                    var resources = assembly.GetManifestResourceNames();

                    if (resources.Length == 0)
                        continue;

                    foreach (var resource in resources)
                    {
                        resNameToAssembly.TryAdd(resource, assembly);
                    }
                }
            }

            if (resNameToAssembly.TryGetValue(resName, out var result))
                return result;
            return null;
        }

        /// <summary>
        /// Enumerates events in all descendants of the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="bindingFlags">Binding flags.</param>
        /// <returns></returns>
        public static SortedList<string, EventInfo> GetAllDescendantsEvents(
            Type type,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
        {
            IEnumerable<Type> types = GetTypeDescendants(type);

            var result = new SortedList<string, EventInfo>();

            foreach(var item in types)
            {
                IEnumerable<EventInfo> events = EnumEvents(item, false, bindingFlags);
                foreach(var ev in events)
                {
                    var name = ev.Name;
                    if (result.ContainsKey(name))
                        continue;
                    result.Add(name, ev);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets device the app is running on, such as a desktop computer or a tablet.
        /// Uses invoke of 'MauiUtils.GetDeviceType'.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GenericDeviceType InvokeMauiUtilsGetDeviceType()
        {
            var result = InvokeMethodWithResult(KnownTypes.MauiUtils.Value, "GetDeviceType");
            if (result is null)
                return GenericDeviceType.Unknown;
            return (GenericDeviceType)result;
        }

        /// <summary>
        /// Gets device platform using invoke of 'MauiUtils.GetDevicePlatform'.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object? InvokeMauiUtilsGetDevicePlatform()
        {
            var result = InvokeMethodWithResult(KnownTypes.MauiUtils.Value, "GetDevicePlatform");
            return result;
        }

        /// <summary>
        /// Gets whether device platform is MacCatalyst using invoke of 'MauiUtils.IsMacCatalyst'.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? InvokeMauiUtilsIsMacCatalyst()
        {
            var result = InvokeBoolMethod(KnownTypes.MauiUtils.Value, "IsMacCatalyst");
            return result;
        }

        /// <summary>
        /// Invokes 'OperatingSystem.IsAndroid' through the reflection.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? InvokeIsAndroid()
        {
            var result = InvokeBoolMethod<OperatingSystem>("IsAndroid");
            return result;
        }

        /// <summary>
        /// Invokes 'OperatingSystem.IsAndroid' through the reflection.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? InvokeIsIOS()
        {
            var result = InvokeBoolMethod<OperatingSystem>("IsIOS");
            return result;
        }

        /// <summary>
        /// Invokes method with boolean result through the reflection.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="methodName">Method name.</param>
        /// <param name="obj">Object instance. Optional. Default is null.</param>
        /// <param name="param">Method parameters. Optional. Default is null.</param>
        /// <returns>
        /// <c>null</c> if method not found; otherwise result of the method invoke.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool? InvokeBoolMethod<T>(
            string methodName,
            object? obj = null,
            object[]? param = null)
        {
            return InvokeBoolMethod(typeof(T), methodName, obj, param);
        }

        /// <summary>
        /// Invokes method with boolean result through the reflection.
        /// </summary>
        /// <param name="type">Type where to search the method.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="obj">Object instance. Optional. Default is null.</param>
        /// <param name="param">Method parameters. Optional. Default is null.</param>
        /// <returns>
        /// <c>null</c> if method not found; otherwise result of the method invoke.
        /// </returns>
        public static bool? InvokeBoolMethod(
            Type? type,
            string methodName,
            object? obj = null,
            object[]? param = null)
        {
            try
            {
                var method = type?.GetMethod(methodName);

                if (method is not null)
                {
                    var result = (bool)method.Invoke(obj, param);
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Invokes method with object result through the reflection.
        /// </summary>
        /// <param name="type">Type where to search the method.</param>
        /// <param name="methodName">Method name</param>
        /// <param name="obj">Object instance. Optional. Default is null.</param>
        /// <param name="param">Method parameters. Optional. Default is null.</param>
        /// <returns>
        /// <c>null</c> if method not found; otherwise result of the method invoke.
        /// </returns>
        public static object? InvokeMethodWithResult(
            Type? type,
            string methodName,
            object? obj = null,
            object[]? param = null)
        {
            try
            {
                var method = type?.GetMethod(methodName);

                if (method is not null)
                {
                    var result = method.Invoke(obj, param);
                    return result;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all static properties of the specified type in the specifed container type.
        /// Returned only not null properties of the exact type <typeparamref name="TProperty"/>.
        /// </summary>
        /// <typeparam name="TProperty">Type of the properties.</typeparam>
        /// <returns></returns>
        public static IEnumerable<TProperty> GetStaticProperties<TProperty>(Type containerType)
        {
            List<TProperty> result = new();

            var props = containerType.GetProperties(
                BindingFlags.Static | BindingFlags.Public);

            foreach (var p in props)
            {
                if (p.PropertyType != typeof(TProperty))
                    continue;

                if (p.GetValue(null) is not TProperty value)
                    continue;
                result.Add(value);
            }

            return result;
        }

        /// <summary>
        /// Enumerates public objects declared in the specified namespace.
        /// </summary>
        /// <param name="asm">Assembly.</param>
        /// <param name="namesp">Namespace.</param>
        /// <returns></returns>
        public static IEnumerable<Type> EnumPublicObjectsForNamespace(Assembly asm, string namesp)
        {
            var types = asm.ExportedTypes;

            foreach(var t in types)
            {
                if (t.Namespace == namesp)
                    yield return t;
            }
        }
    }
}
