using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        internal static readonly BindableProperty TextProperty = new()
        {
            PropertyName = nameof(Text),
            PropertyType = typeof(string),
            ElementType = typeof(AbstractControl),
            GetValue = (instance) =>
            {
                return ((AbstractControl?)instance)?.Text;
            },
            SetValue = (instance, value) =>
            {
                if (instance is AbstractControl control)
                    control.Text = value?.ToString() ?? string.Empty;
            },
        };
    }
}
