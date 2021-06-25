using System;
using System.Globalization;

namespace Alternet.UI
{
    public abstract class ListControl : Control
    {
        [Content]
        public Collection<object> Items { get; } = new Collection<object> { ThrowOnNullItemAddition = true };

        // todo: copy DataSource, DisplayMember, ValueMember etc implementation from WinForms

        public string GetItemText(object item)
        {
            return item switch
            {
                null => string.Empty,
                string s => s,
                _ => Convert.ToString(item, CultureInfo.CurrentCulture) ?? string.Empty
            };
        }
    }
}