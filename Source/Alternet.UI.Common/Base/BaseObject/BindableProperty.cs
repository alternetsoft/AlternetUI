using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal delegate object? BindablePropertyGetValueDelegate(object? element);

    internal delegate void BindablePropertySetValueDelegate(object? element, object? value);

    internal class BindableProperty : ImmutableObject
    {
        public BindableProperty()
        {
        }

        public virtual string? PropertyName { get; set; }

        public virtual Type? PropertyType { get; set; }

        public virtual Type? ElementType { get; set; }

        public virtual BindablePropertyGetValueDelegate? GetValue { get; set; }

        public virtual BindablePropertySetValueDelegate? SetValue { get; set; }
    }
}
