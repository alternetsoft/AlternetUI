using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal delegate TProp? BindablePropertyGetValueDelegate<TElement, TProp>(TElement element);

    internal delegate void BindablePropertySetValueDelegate<TElement, TProp>(TElement element, TProp? value);

    internal class BindableProperty<TElement, TProp> : ImmutableObject
    {
        public BindableProperty()
        {
            PropertyType = typeof(TProp);
            ElementType = typeof(TElement);
        }

        public string? PropertyName { get; set; }

        public Type? PropertyType { get; }

        public Type? ElementType { get; }

        public BindablePropertyGetValueDelegate<TElement, TProp>? GetValue { get; set; }

        public BindablePropertySetValueDelegate<TElement, TProp>? SetValue { get; set; }
    }
}
