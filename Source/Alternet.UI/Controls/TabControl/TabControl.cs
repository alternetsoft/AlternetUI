using System.Collections.Generic;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that manages a related set of tab pages.
    /// </summary>
    public class TabControl : Control
    {
        /// <summary>
        /// Gets the collection of tab pages in this tab control.
        /// </summary>
        /// <value>A <see cref="Collection{TabPage}"/> that contains the <see cref="TabPage"/> objects in this <see cref="TabControl"/>.</value>
        /// <remarks>The order of tab pages in this collection reflects the order the tabs appear in the control.</remarks>
        [Content]
        public Collection<TabPage> Pages { get; } = new Collection<TabPage>();

        /// <inheritdoc />
        protected override IEnumerable<Control> LogicalChildren => Pages;
    }
}