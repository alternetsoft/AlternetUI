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

        /// <summary>
        /// Gets or sets a value indicating whether the scroll
        /// viewer is scrolled with the mouse wheel.
        /// </summary>
        public virtual bool IsScrolledWithMouseWheel { get; set; } = true;

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

        /// <summary>
        /// Determines whether the mouse wheel event should be ignored for
        /// the specified child control.
        /// </summary>
        /// <param name="child">The child control to check.</param>
        /// <returns>True if the mouse wheel event should be ignored; otherwise, false.</returns>
        protected virtual bool IgnoreChildMouseWheel(AbstractControl? child)
        {
            return child is ComboBox;
        }

        /// <inheritdoc/>
        protected override void OnBeforeChildMouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Handled || IgnoreChildMouseWheel(sender as AbstractControl)
                || !IsScrolledWithMouseWheel)
                return;

            var sign = Math.Sign(e.Delta);

            if (Keyboard.IsShiftPressed)
            {
                var w = (int)MeasureCanvas.GetTextExtent("W", RealFont).Width;
                IncHorizontalLayoutOffset(sign * w);
            }
            else
            {
                var h = (int)MeasureCanvas.GetTextExtent("Wg", RealFont).Height;
                IncVerticalLayoutOffset(sign * h);
            }

            e.Handled = true;
        }

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Scroll;
        }
    }
}