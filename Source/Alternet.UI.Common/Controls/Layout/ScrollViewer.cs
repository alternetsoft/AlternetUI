using System;
using System.Collections.Generic;
using System.ComponentModel;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    [ControlCategory("Containers")]
    public partial class ScrollViewer : ContainerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ScrollViewer(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        public ScrollViewer()
        {
            CanSelect = false;
            TabStop = false;
            IsScrollable = true;
            ParentBackColor = true;
            ParentForeColor = true;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ScrollViewer;

        /// <summary>
        /// Creates <see cref="ScrollViewer"/> with the specified child control.
        /// </summary>
        /// <param name="child">Child control.</param>
        /// <returns></returns>
        public static ScrollViewer CreateWithChild(AbstractControl? child)
        {
            ScrollViewer result = new();

            if(child is not null)
            {
                child.Parent = result;
                child.Visible = true;
            }

            return result;
        }

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Scroll;
        }
    }
}