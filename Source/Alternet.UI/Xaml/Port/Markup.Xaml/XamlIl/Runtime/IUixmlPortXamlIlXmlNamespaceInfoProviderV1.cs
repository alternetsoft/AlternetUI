#nullable disable
using System.Collections.Generic;

namespace Alternet.UI.Markup
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public interface IUixmlPortXamlIlXmlNamespaceInfoProvider
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyList<UixmlPortXamlIlXmlNamespaceInfo>> XmlNamespaces { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public class UixmlPortXamlIlXmlNamespaceInfo
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public string ClrNamespace { get; set; }
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        public string ClrAssemblyName { get; set; }
    }
}
