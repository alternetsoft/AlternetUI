#nullable disable
using System.Collections.Generic;

namespace Avalonia.Markup.Xaml.XamlIl.Runtime
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public interface IAvaloniaXamlIlXmlNamespaceInfoProvider
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyList<AvaloniaXamlIlXmlNamespaceInfo>> XmlNamespaces { get; }
    }

    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public class AvaloniaXamlIlXmlNamespaceInfo
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
