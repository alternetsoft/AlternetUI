#nullable disable
using System;

namespace Avalonia.Markup.Xaml
{
    internal abstract class MarkupExtension
    {
        public abstract object ProvideValue(IServiceProvider serviceProvider);
    }
}
