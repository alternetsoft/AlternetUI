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
