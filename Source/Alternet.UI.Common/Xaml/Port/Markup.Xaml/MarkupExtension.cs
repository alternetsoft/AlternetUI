#nullable disable
using System;

namespace Alternet.UI.Markup.Xaml
{
    internal abstract class MarkupExtension
    {
        public abstract object ProvideValue(IServiceProvider serviceProvider);
    }
}
