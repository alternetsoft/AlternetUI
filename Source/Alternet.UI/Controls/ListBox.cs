using System;

namespace Alternet.UI
{
    public class ListBox : Control
    {
        [Content]
        public Collection<object> Items { get; } = new Collection<object>();
    }
}