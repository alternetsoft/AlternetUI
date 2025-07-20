using System;
using System.Runtime.CompilerServices;

namespace Alternet.UI.Port
{
    /// <summary>
    /// Provides services to help resolve 'nsPrefix:LocalName' into the appropriate type.
    /// </summary>
    internal interface IXamlTypeResolver
    {
        /// <summary>
        /// Resolves 'nsPrefix:LocalName' into the appropriate Type.
        /// </summary>
        /// <param name="qualifiedTypeName">TypeName that appears in xaml
        /// 'nsPrefix:LocalName' or 'LocalName'.</param>
        /// <returns>
        /// The type that the qualifiedTypeName represents.
        /// </returns>
        Type Resolve(string qualifiedTypeName);
    }
}
