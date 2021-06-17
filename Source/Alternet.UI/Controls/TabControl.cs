using System;
using System.Collections.Generic;

namespace Alternet.UI
{
    public class TabControl : Control
    {
        public new Collection<Control> Children => base.Children;

        [Content]
        public Collection<TabPage> Pages { get; } = new Collection<TabPage>();

        protected override IEnumerable<Control> LogicalChildren => Pages;
    }
}