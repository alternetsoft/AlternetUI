using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Calculates horizontal <see cref="AlignedPosition"/> using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="childControl">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        public static AlignedPosition AlignHorizontal(
                    RectD layoutBounds,
                    AbstractControl childControl,
                    SizeD childPreferredSize,
                    HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return new AlignedPosition(
                        layoutBounds.Left + childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Left +
                        ((layoutBounds.Width
                        - (childPreferredSize.Width + childControl.Margin.Horizontal)) / 2) +
                        childControl.Margin.Left,
                        childPreferredSize.Width);
                case HorizontalAlignment.Right:
                    return new AlignedPosition(
                        layoutBounds.Right -
                        childControl.Margin.Right - childPreferredSize.Width,
                        childPreferredSize.Width);
                case HorizontalAlignment.Stretch:
                default:
                    return new AlignedPosition(
                        layoutBounds.Left + childControl.Margin.Left,
                        layoutBounds.Width - childControl.Margin.Horizontal);
            }
        }

        /// <summary>
        /// Calculates vertical <see cref="AlignedPosition"/> using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="control">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        public static AlignedPosition AlignVertical(
            RectD layoutBounds,
            AbstractControl control,
            SizeD childPreferredSize,
            VerticalAlignment alignment)
        {
            switch (alignment)
            {
                case VerticalAlignment.Top:
                    return new AlignedPosition(
                        layoutBounds.Top + control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Center:
                    return new AlignedPosition(
                        layoutBounds.Top +
                        ((layoutBounds.Height
                        - (childPreferredSize.Height + control.Margin.Vertical)) / 2) +
                        control.Margin.Top,
                        childPreferredSize.Height);
                case VerticalAlignment.Bottom:
                    return new AlignedPosition(
                        layoutBounds.Bottom - control.Margin.Bottom - childPreferredSize.Height,
                        childPreferredSize.Height);
                case VerticalAlignment.Stretch:
                default:
                    return new AlignedPosition(
                        layoutBounds.Top + control.Margin.Top,
                        layoutBounds.Height - control.Margin.Vertical);
            }
        }

        internal static void PerformDefaultLayout(
            AbstractControl container,
            RectD childrenLayoutBounds,
            IReadOnlyList<AbstractControl> childControls)
        {
            foreach (var control in childControls)
            {
                if (control.Dock != DockStyle.None || control.IgnoreLayout)
                    continue;

                var preferredSize = control.GetPreferredSizeLimited(
                    new PreferredSizeContext(childrenLayoutBounds.Size - control.Margin.Size));

                var horizontalPosition =
                    AlignHorizontal(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.HorizontalAlignment);
                var verticalPosition =
                    AlignVertical(
                        childrenLayoutBounds,
                        control,
                        preferredSize,
                        control.VerticalAlignment);

                control.Bounds = new RectD(
                    horizontalPosition.Origin,
                    verticalPosition.Origin,
                    horizontalPosition.Size,
                    verticalPosition.Size);
            }
        }

        internal static SizeD GetPreferredSizeDockLayout(
            AbstractControl container,
            PreferredSizeContext context)
        {
            if (container.HasChildren)
                return container.GetBestSizeWithChildren(context);
            return container.GetBestSizeWithPadding(context);
        }

        internal static SizeD GetPreferredSizeDefaultLayout(
            AbstractControl container,
            PreferredSizeContext context)
        {
            if (container.HasChildren)
                return container.GetBestSizeWithChildren(context);
            return container.GetBestSizeWithPadding(context);
        }

        internal static Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesBgColor(controlType, renderSize);
        }

        internal static Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesFgColor(controlType, renderSize);
        }

        internal static Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return SystemSettings.Handler.GetClassDefaultAttributesFont(controlType, renderSize);
        }

        internal void RaiseProcessException(ThrowExceptionEventArgs e)
        {
            OnProcessException(e);
            ProcessException?.Invoke(this, e);
        }

        internal void SetParentInternal(AbstractControl? value)
        {
            parent = value;
            LogicalParent = value;
        }

        internal virtual SizeD GetPreferredSizeLimited(PreferredSizeContext context)
        {
            var preferredSize = GetPreferredSize(context);
            var result = GetSizeLimited(preferredSize);
            return result;
        }

        internal virtual SizeD GetSizeLimited(SizeD size)
        {
            var minSize = MinimumSize;
            var maxSize = MaximumSize;
            var result = size.ApplyMinMax(minSize, maxSize);
            return result;
        }

        /// <summary>
        /// Contains location and size calculated by the align method.
        /// </summary>
        public class AlignedPosition
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AlignedPosition"/> class.
            /// </summary>
            /// <param name="origin">Location.</param>
            /// <param name="size">Size.</param>
            public AlignedPosition(Coord origin, Coord size)
            {
                Origin = origin;
                Size = size;
            }

            /// <summary>
            /// Gets location.
            /// </summary>
            public Coord Origin { get; }

            /// <summary>
            /// Gets size.
            /// </summary>
            public Coord Size { get; }
        }
    }
}
