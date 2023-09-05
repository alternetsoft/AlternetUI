using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Assembly"/> related static methods.
    /// </summary>
    public static class AssemblyUtils
    {
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
        /// Gets property info.
        /// </summary>
        /// <param name="instance">Object instance.</param>
        /// <param name="name">Property name.</param>
        public static PropertyInfo? GetPropInfo(object instance, string name)
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
                TypeCode.SByte => sbyte.MinValue,
                TypeCode.Byte => byte.MinValue,
                TypeCode.Int16 => short.MinValue,
                TypeCode.UInt16 => ushort.MinValue,
                TypeCode.Int32 => int.MinValue,
                TypeCode.UInt32 => uint.MinValue,
                TypeCode.Int64 => long.MinValue,
                TypeCode.UInt64 => ulong.MinValue,
                TypeCode.Single => float.MinValue,
                TypeCode.Double => double.MinValue,
                TypeCode.Decimal => decimal.MinValue,
                TypeCode.DateTime => DateTime.MinValue,
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
                TypeCode.SByte => sbyte.MaxValue,
                TypeCode.Byte => byte.MaxValue,
                TypeCode.Int16 => short.MaxValue,
                TypeCode.UInt16 => ushort.MaxValue,
                TypeCode.Int32 => int.MaxValue,
                TypeCode.UInt32 => uint.MaxValue,
                TypeCode.Int64 => long.MaxValue,
                TypeCode.UInt64 => ulong.MaxValue,
                TypeCode.Single => float.MaxValue,
                TypeCode.Double => double.MaxValue,
                TypeCode.Decimal => decimal.MaxValue,
                TypeCode.DateTime => DateTime.MaxValue,
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
        /// Returns list of types which descend from specified type.
        /// </summary>
        /// <param name="type">Base type.</param>
        /// <param name="ascending">Sort result ascending by type name.</param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypeDescendants(Type type, bool ascending = true)
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

            if (!ascending)
                return result;

            result.Sort(StringUtils.ComparerObjectUsingToString);

            return result;
        }
    }
}
