using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the event that is raised when
    /// a property of a <see cref="TreeViewItem"/> changes.
    /// </summary>
    public class TreeViewItemPropChangedEventArgs : ObjectPropertyChangedEventArgs<TreeViewItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewItemPropChangedEventArgs"/> class.
        /// </summary>
        /// <param name="instance">The <see cref="TreeViewItem"/> instance
        /// whose property was changed.</param>
        /// <param name="propName">
        /// The name of the property that was changed. If <c>null</c>,
        /// more than one property was changed.
        /// </param>
        public TreeViewItemPropChangedEventArgs(TreeViewItem instance, string? propName)
            : base(instance, propName)
        {
        }
    }
}
