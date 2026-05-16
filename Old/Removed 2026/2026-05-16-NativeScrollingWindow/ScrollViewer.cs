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
        /// Gets or sets default multiplier used in calculation
        /// of total size of the scroll area in the <see cref="ScrollViewer"/>.
        /// This value is assigned to the scrollbar's total size.
        /// </summary>
        public static SizeD DefaultScrollBarTotalSizeMultiplier = 1;

        /// <summary>
        /// Gets or sets default mouse wheel scroll factor. This value is multiplied
        /// with line height and used as an offset when control is scrolled using mouse wheel.
        /// </summary>
        public static SizeD DefaultMouseWheelScrollFactor = (2, 2);

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ScrollViewer(AbstractControl parent)
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
        /// Gets default value used to offset scrollbar position when scroll
        /// wheel event is handled.
        /// </summary>
        /// <param name="measureCanvas">The canvas used for text measuring.</param>
        /// <param name="font">The font used for text measuring.</param>
        /// <param name="isVert">Whether to get value for
        /// the vertical of horizontal scrollbar.</param>
        /// <returns></returns>
        public static int GetDefaultScrollWheelDelta(Graphics measureCanvas, Font font, bool isVert)
        {
            if (isVert)
            {
                var h = measureCanvas.GetTextExtent("Wg", font).Height;
                h *= DefaultMouseWheelScrollFactor.Height;
                return (int)h;
            }
            else
            {
                var w = measureCanvas.GetTextExtent("W", font).Width;
                w *= DefaultMouseWheelScrollFactor.Width;
                return (int)w;
            }
        }

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
            return false;
        }

        /// <inheritdoc/>
        protected override void OnBeforeChildMouseWheel(object? sender, MouseEventArgs e)
        {
            /*

            if (e.Handled || IgnoreChildMouseWheel(sender as AbstractControl)
                || !IsScrolledWithMouseWheel)
                return;

            var sign = Math.Sign(e.Delta);
            var isVert = !Keyboard.IsShiftPressed;
            var delta = GetDefaultScrollWheelDelta(MeasureCanvas, RealFont, isVert);

            if (isVert)
            {
                IncVerticalLayoutOffset(sign * delta);
            }
            else
            {
                IncHorizontalLayoutOffset(sign * delta);
            }

            e.Handled = true;
            */
        }

        /// <inheritdoc/>
        protected override LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Scroll;
        }
    }
}