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
