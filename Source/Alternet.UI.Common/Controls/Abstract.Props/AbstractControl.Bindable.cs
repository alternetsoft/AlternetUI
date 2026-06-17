using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        internal static readonly BindableProperty<AbstractControl, string> TextProperty = new()
        {
            PropertyName = nameof(Text),
            GetValue = (instance) =>
            {
                return instance.Text;
            },
            SetValue = (instance, value) =>
            {
                instance.Text = value ?? string.Empty;
            },
        };
    }
}
