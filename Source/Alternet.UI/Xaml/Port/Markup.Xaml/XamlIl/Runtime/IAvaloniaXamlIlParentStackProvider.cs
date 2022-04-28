#nullable disable
using System.Collections.Generic;

namespace Avalonia.Markup.Xaml.XamlIl.Runtime
{
    /// <summary>
    /// This item supports the framework infrastructure and is not intended to be used directly from your code.
    /// </summary>
    public interface IAvaloniaXamlIlParentStackProvider
    {
        /// <summary>
        /// This item supports the framework infrastructure and is not intended to be used directly from your code.
        /// </summary>
        IEnumerable<object> Parents { get; }
    }
}
