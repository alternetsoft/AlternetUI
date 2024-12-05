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
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ScrollViewer;

        /// <inheritdoc/>
        public override void OnLayout()
        {
            OldLayout.LayoutWhenScroll(this, ChildrenLayoutBounds, AllChildrenInLayout, true);
        }

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            return base.GetDefaultLayout();
        }

        /// <inheritdoc/>
        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);

            var offset = GetScrollBarInfo(e.IsVertical).Position;

            if (e.IsVertical)
                LayoutOffset = new PointD(LayoutOffset.X, -offset);
            else
                LayoutOffset = new PointD(-offset, LayoutOffset.Y);
        }
    }
}