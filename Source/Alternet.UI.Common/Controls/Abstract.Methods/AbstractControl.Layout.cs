using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Gets real layout style of the child controls.
        /// </summary>
        [Browsable(false)]
        public LayoutStyle RealLayout
        {
            get
            {
                return Layout ?? GetDefaultLayout();
            }
        }

        /// <summary>
        /// Gets a rectangle which describes an area inside of the
        /// <see cref="AbstractControl"/> available
        /// for positioning (layout) of its child controls, in device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual RectD ChildrenLayoutBounds
        {
            get
            {
                var childrenBounds = ClientRectangle;
                var padding = Padding;
                var intrinsicPadding = NativePadding;
                var borderWidth = Border.SafeBorderWidth(this);

                var sz = childrenBounds.Size;
                SizeD size = sz - padding.Size - intrinsicPadding.Size - borderWidth.Size;

                if (size.AnyIsEmptyOrNegative)
                    return RectD.Empty;

                PointD location = new(
                        padding.Left + intrinsicPadding.Left + borderWidth.Left,
                        padding.Top + intrinsicPadding.Top + borderWidth.Top);

                return new RectD(location, size);
            }
        }

        /// <summary>
        /// Called when the control should reposition its child controls.
        /// </summary>
        [Browsable(false)]
        public virtual void OnLayout()
        {
            if (CustomLayout is not null)
            {
                var e = new HandledEventArgs();
                CustomLayout(this, e);
                if (e.Handled)
                    return;
            }

            var layoutType = RealLayout;

            RectD GetSpace()
            {
                return ChildrenLayoutBounds;
            }

            var items = AllChildrenInLayout;

            if (StaticControlEvents.HasLayoutHandlers)
            {
                var e = new DefaultLayoutEventArgs(this, layoutType, GetSpace(), items);
                StaticControlEvents.RaiseLayout(this, e);
                if (e.Handled)
                    return;
                else
                {
                    layoutType = e.Layout;
                    items = e.Children;
                }
            }

            GetLayoutManager().DefaultOnLayout(this, layoutType, GetSpace, items);
        }

        internal virtual LayoutManager GetLayoutManager()
        {
            return LayoutManager.Instance;
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can
        /// be fitted, in device-independent units.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> providing
        /// the available size and other layout information.</param>
        /// <returns>A <see cref="SizeD"/> representing the preferred width and height of
        /// a rectangle, in device-independent units.</returns>
        public virtual SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var layoutType = Layout ?? GetDefaultLayout();

            if (StaticControlEvents.HasRequestPreferredSizeHandlers)
            {
                var e = new DefaultPreferredSizeEventArgs(layoutType, context);
                StaticControlEvents.RaiseRequestPreferredSize(this, e);
                if (e.Handled && e.Result != SizeD.MinusOne)
                    return e.Result;
            }

            return GetLayoutManager().DefaultGetPreferredSize(this, context, layoutType);
        }

        /// <summary>
        /// Calls <see cref="GetPreferredSize(PreferredSizeContext)"/>
        /// with <see cref="PreferredSizeContext.PositiveInfinity"/> as a parameter value.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD GetPreferredSize() => GetPreferredSize(PreferredSizeContext.PositiveInfinity);

        /// <summary>
        /// Aligns control in the parent using horizontal and vertical
        /// alignment options.
        /// </summary>
        /// <param name="horz">Horizontal alignment.</param>
        /// <param name="vert">Vertical alignment.</param>
        /// <param name="shrinkSize">Whether to shrink size of the rectangle
        /// to fit in the container. Optional. Default is <c>true</c>.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using
        /// <see cref="IgnoreLayout"/> property
        /// of the control.
        /// </remarks>
        public virtual void AlignInParent(
            HorizontalAlignment? horz,
            VerticalAlignment? vert,
            bool shrinkSize = true)
        {
            if (Parent is null)
                return;
            AlignInRect(
                        Parent.ClientRectangle,
                        horz,
                        vert,
                        shrinkSize);
        }

        /// <summary>
        /// Aligns control in the parent using <see cref="DockStyle"/> alignment option.
        /// </summary>
        /// <param name="value"><see cref="DockStyle"/> value which specifies align option.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using
        /// <see cref="IgnoreLayout"/> property
        /// of the control.
        /// </remarks>
        public virtual RectD DockInParent(DockStyle value)
        {
            if (Parent is null)
                return RectD.Empty;
            var result = DockInRect(Parent.ClientRectangle, value);
            return result;
        }

        /// <summary>
        /// Aligns control in the specified container rectangle
        /// using <see cref="DockStyle"/> alignment option.
        /// </summary>
        /// <param name="value"><see cref="DockStyle"/> value which specifies align option.</param>
        /// <param name="container">Container rectangle.</param>
        /// <param name="layoutFlags">Layout flags. Optional. Default is <see cref="LayoutFlags.None"/>.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using
        /// <see cref="IgnoreLayout"/> property of the control.
        /// </remarks>
        public virtual RectD DockInRect(RectD container, DockStyle value, LayoutFlags layoutFlags = LayoutFlags.None)
        {
            GetLayoutManager().LayoutWhenDocked(ref container, this, value, layoutFlags);
            return container;
        }

        /// <summary>
        /// Aligns control in the specified container rectangle using horizontal and vertical
        /// alignment options.
        /// </summary>
        /// <param name="container">Container rectangle.</param>
        /// <param name="horz">Horizontal alignment.</param>
        /// <param name="vert">Vertical alignment.</param>
        /// <param name="shrinkSize">Whether to shrink size of the rectangle
        /// to fit in the container. Optional. Default is <c>true</c>.</param>
        /// <remarks>
        /// This method changes <see cref="Bounds"/> so default layout must be disabled
        /// before using it. You can disable default layout using
        /// <see cref="IgnoreLayout"/> property
        /// of the control.
        /// </remarks>
        public virtual void AlignInRect(
            RectD container,
            HorizontalAlignment? horz,
            VerticalAlignment? vert,
            bool shrinkSize = true)
        {
            var newBounds = AlignUtils.AlignRectInRect(
                Bounds,
                container,
                horz,
                vert,
                shrinkSize);
            Bounds = newBounds;
        }

        internal static SizeD GetMinStretchedSize(
            SizeD availableSize,
            IReadOnlyList<ILayoutItem> children)
        {
            SizeD result = 0;

            foreach (var child in children)
            {
                if (child.VerticalAlignment == VerticalAlignment.Stretch
                    || child.HorizontalAlignment == HorizontalAlignment.Stretch)
                {
                    var childMargin = child.Margin;
                    var childPreferredSize = child.GetPreferredSizeLimited(availableSize);
                    var childPreferredSizeWithMargin = childPreferredSize + childMargin.Size;
                    result = SizeD.Max(result, childPreferredSizeWithMargin);
                }
            }

            return result;
        }
    }
}