using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains common methods and properties for all property grid controls.
    /// </summary>
    public class BasePropertyGrid : Control
    {
        public static readonly AdvDictionaryCached<Type, IPropertyGridTypeRegistry>
            TypeRegistry = new();

        private static AdvDictionary<Type, IPropertyGridChoices>? choicesCache = null;

        /// <summary>
        /// Gets <see cref="IPropertyGridTypeRegistry"/> for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type value.</param>
        public static IPropertyGridTypeRegistry GetTypeRegistry(Type type)
        {
            return TypeRegistry.GetOrCreateCached(type, () =>
            {
                return new PropertyGridTypeRegistry(type);
            });
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridTypeRegistry"/> for the given <see cref="Type"/>
        /// if its available, othewise returns <c>null</c>.
        /// </summary>
        /// <param name="type">Type value.</param>
        public static IPropertyGridTypeRegistry? GetTypeRegistryOrNull(Type type)
        {
            return TypeRegistry.GetValueOrDefaultCached(type);
        }

        /// <summary>
        /// Gets "constructed" <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        /// <remarks>
        /// See <see cref="IPropertyGridNewItemParams.Constructed"/> for the details.
        /// </remarks>
        public static IPropertyGridNewItemParams ConstructNewItemParams(
            Type type,
            PropertyInfo propInfo)
        {
            var prm = GetNewItemParams(type, propInfo);
            return prm.Constructed;
        }

        /// Gets "constructed" <see cref="IPropertyGridNewItemParams"/> for the given
        /// object instance and <see cref="PropertyInfo"/>.
        /// <param name="instance">Object instance.</param>
        /// <param name="propInfo">Property information.</param>
        /// <remarks>
        /// See <see cref="IPropertyGridNewItemParams.Constructed"/> for the details.
        /// </remarks>
        public static IPropertyGridNewItemParams ConstructNewItemParams(
            object instance,
            PropertyInfo propInfo)
        {
            if (instance == null)
                return PropertyGridNewItemParams.Default;
            var type = instance.GetType();
            return ConstructNewItemParams(type, propInfo);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams GetNewItemParams(
            Type type,
            PropertyInfo propInfo)
        {
            var registry = GetTypeRegistry(type);
            var propRegistry = registry.GetPropRegistry(propInfo);
            return propRegistry.NewItemParams;
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and property name.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propName">Property name.</param>
        public static IPropertyGridNewItemParams GetNewItemParams(Type type, string propName)
        {
            var registry = GetTypeRegistry(type);
            var propRegistry = registry.GetPropRegistry(propName);
            return propRegistry.NewItemParams;
        }

        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// object instance and <see cref="PropertyInfo"/>.
        /// <param name="instance">Object instance.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams GetNewItemParams(
            object instance,
            PropertyInfo propInfo)
        {
            if (instance == null)
                return PropertyGridNewItemParams.Default;
            var type = instance.GetType();
            return GetNewItemParams(type, propInfo);
        }

        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/> if its available,
        /// otherwise returns <c>null</c>.
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams? GetNewItemParamsOrNull(
            Type type,
            PropertyInfo propInfo)
        {
            var registry = GetTypeRegistryOrNull(type);
            if (registry == null)
                return null;
            var propRegistry = registry.GetPropRegistryOrNull(propInfo);
            if (propRegistry == null)
                return null;
            if (propRegistry.HasNewItemParams)
                return propRegistry.NewItemParams;
            return null;
        }

        /// Gets <see cref="IPropertyGridNewItemParams"/> for the given
        /// object instance and <see cref="PropertyInfo"/> if its available,
        /// otherwise returns <c>null</c>.
        /// <param name="instance">Object instance.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridNewItemParams? GetNewItemParamsOrNull(
            object instance,
            PropertyInfo propInfo)
        {
            if (instance == null)
                return null;
            var type = instance.GetType();
            return GetNewItemParamsOrNull(type, propInfo);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridPropInfoRegistry"/> for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="type">Object type.</param>
        /// <param name="propInfo">Property information.</param>
        public static IPropertyGridPropInfoRegistry GetPropRegistry(Type type, PropertyInfo propInfo)
        {
            var registry = GetTypeRegistry(type);
            var propRegistry = registry.GetPropRegistry(propInfo);
            return propRegistry;
        }

        /// <summary>
        /// Gets custom label for the given
        /// <see cref="Type"/> and <see cref="PropertyInfo"/>.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="propName">Property name.</param>
        public static string? GetCustomLabel<T>(string propName)
            where T : class
        {
            var propInfo = typeof(T).GetProperty(propName);
            if (propInfo == null)
                return null;

            var propRegistry = GetPropRegistry(typeof(T), propInfo);
            return propRegistry.NewItemParams.Label;
        }

        /// <summary>
        /// Returns <see cref="IPropertyGridChoices"/> for the specified <paramref name="instance"/>
        /// and <paramref name="propName"/>.
        /// </summary>
        /// <param name="instance">Object.</param>
        /// <param name="propName">Property name.</param>
        /// <returns></returns>
        public static IPropertyGridChoices? GetPropChoices(object instance, string propName)
        {
            var propInfo = AssemblyUtils.GetPropInfo(instance, propName);
            if (propInfo is null)
                return null;
            var propType = propInfo.PropertyType;
            var prm = ConstructNewItemParams(instance, propInfo);
            var choices = prm.Choices;
            var realType = AssemblyUtils.GetRealType(propType);
            choices ??= CreateChoicesOnce(realType);
            return choices;
        }

        /// <summary>
        /// Creates property choices list for use with <see cref="CreateFlagsItem"/> and
        /// <see cref="CreateChoicesItem"/>.
        /// </summary>
        public static IPropertyGridChoices CreateChoices()
        {
            return NativePlatform.Default.CreateChoices();
        }

        /// <summary>
        /// Returns <see cref="IPropertyGridChoices"/> for the given enumeration type.
        /// </summary>
        /// <typeparam name="T">Type of the enumeration.</typeparam>
        public static IPropertyGridChoices GetChoices<T>()
            where T : Enum
        {
            return CreateChoicesOnce(typeof(T));
        }

        /// <summary>
        /// Creates property choices list for the given enumeration type or returns it from
        /// the internal cache if it was previously created.
        /// </summary>
        /// <remarks>
        /// Result can be used in <see cref="CreateFlagsItem"/> and
        /// <see cref="CreateChoicesItem"/>.
        /// </remarks>
        public static IPropertyGridChoices CreateChoicesOnce(Type enumType)
        {
            choicesCache ??= new();
            if (choicesCache.TryGetValue(enumType, out IPropertyGridChoices? result))
                return result;
            result = CreateChoices(enumType);
            choicesCache.Add(enumType, result);
            return result;
        }

        /// <summary>
        /// Creates property choices list for the given enumeration type.
        /// </summary>
        /// <remarks>
        /// Result can be used in <see cref="CreateFlagsItem"/> and
        /// <see cref="CreateChoicesItem"/>.
        /// </remarks>
        public static IPropertyGridChoices CreateChoices(Type enumType)
        {
            var result = CreateChoices();

            if (!enumType.IsEnum)
                return result;

            var values = Enum.GetValues(enumType);
            var names = Enum.GetNames(enumType);

            bool isFlags = AssemblyUtils.EnumIsFlags(enumType);

            for (int i = 0; i < values.Length; i++)
            {
                var value = (int)values.GetValue(i)!;
                if (isFlags && value == 0)
                    continue;
                result.Add(names[i], value);
            }

            return result;
        }

        public static IPropertyGridNewItemParams CreateNewItemParams(
           IPropertyGridPropInfoRegistry? owner, PropertyInfo? propInfo = null)
        {
            return new PropertyGridNewItemParams(owner, propInfo);
        }

        /// <summary>
        /// Gets <see cref="IPropertyGridPropInfoRegistry"/> item for the specified
        /// <paramref name="type"/> and <paramref name="propInfo"/>. Uses validator
        /// functions to check whether results is ok.
        /// </summary>
        /// <param name="type">Type which contains the property.</param>
        /// <param name="propInfo">Property information.</param>
        /// <param name="validatorFunc">Validator function.</param>
        /// <remarks>
        /// This method also searches for the result in all base types of
        /// the <paramref name="type"/>.
        /// </remarks>
        public static IPropertyGridPropInfoRegistry? GetValidBasePropRegistry(
            Type? type,
            PropertyInfo? propInfo,
            Func<IPropertyGridPropInfoRegistry, bool> validatorFunc)
        {
            if (type == null || propInfo == null)
                return null;
            var registry = GetTypeRegistry(type);

            while (true)
            {
                if (registry == null)
                    return null;
                var propRegistry = registry.GetPropRegistryOrNull(propInfo.Name);
                if (propRegistry == null)
                {
                    registry = registry.BaseTypeRegistry;
                    continue;
                }

                var isOk = validatorFunc(propRegistry);
                if (!isOk)
                {
                    registry = registry.BaseTypeRegistry;
                    continue;
                }

                return propRegistry;
            }
        }

        /// <summary>
        /// Creates new <see cref="IPropertyGridNewItemParams"/> instance.
        /// </summary>
        public static IPropertyGridNewItemParams CreateNewItemParams(
            PropertyInfo? propInfo = null)
        {
            return new PropertyGridNewItemParams(null, propInfo);
        }
    }
}
