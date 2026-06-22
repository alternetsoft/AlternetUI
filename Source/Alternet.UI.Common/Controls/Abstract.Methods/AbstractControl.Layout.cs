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
        private LayoutManager? layoutManager;

        /// <summary>
        /// Gets or sets the layout manager responsible for arranging child elements within the container.
        /// Default is null, which means that the default layout manager will be used. Setting this property to a non-null
        /// value allows you to specify a custom layout manager for the control,
        /// which can provide specialized layout behavior for its child controls.
        /// </summary>
        /// <remarks>Setting this property updates the layout of the container. </remarks>
        [Browsable(false)]
        public virtual LayoutManager? LayoutManager
        {
            get => layoutManager;
            set
            {
                if (layoutManager == value)
                    return;
                layoutManager = value;
                PerformLayout();
            }
        }

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

            if (items.Count == 0)
                return;

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

            GetLayoutManager().OnLayout(this, layoutType, GetSpace, items);
        }

        /// <summary>
        /// Gets the layout manager which is used to perform layout and calculate preferred sizes.
        /// This method returns the value of the <see cref="LayoutManager"/> property if it is not null;
        /// otherwise, it returns the default layout manager instance which is specified
        /// by the <see cref="LayoutManager.Instance"/> property.
        /// </summary>
        /// <returns>The <see cref="ILayoutManager"/> instance used by this control.</returns>
        public virtual ILayoutManager GetLayoutManager()
        {
            return LayoutManager ?? LayoutManager.Instance;
        }

        /// <summary>
        /// Gets preferred size of the control based on its layout and child controls.
        /// If you need to change default behavior, override <see cref="GetPreferredSizeInternal"/>.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> providing
        /// the available size and other layout information.</param>
        /// <returns>A <see cref="SizeD"/> representing the preferred width and height of
        /// a rectangle, in device-independent units.</returns>
        public virtual SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var layoutType = Layout ?? GetDefaultLayout();
            var hasGlobal = StaticControlEvents.HasRequestPreferredSizeHandlers;
            var hasLocal = RequestPreferredSize != null;

            var defaultPreferredSize = GetPreferredSizeInternal(context);

            if (hasGlobal || hasLocal)
            {
                var e = new DefaultPreferredSizeEventArgs(layoutType, context);
                e.DefaultPreferredSize = defaultPreferredSize;

                if (hasLocal)
                {
                    RequestPreferredSize?.Invoke(this, e);
                    if (e.Handled && e.Result != SizeD.MinusOne)
                        return e.Result;
                }

                if (hasGlobal)
                {
                    StaticControlEvents.RaiseRequestPreferredSize(this, e);
                    if (e.Handled && e.Result != SizeD.MinusOne)
                        return e.Result;
                }
            }

            return defaultPreferredSize;
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can
        /// be fitted, in device-independent units.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> providing
        /// the available size and other layout information.</param>
        /// <returns>A <see cref="SizeD"/> representing the preferred width and height of
        /// a rectangle, in device-independent units.</returns>
        protected virtual SizeD GetPreferredSizeInternal(PreferredSizeContext context)
        {
            var layoutType = Layout ?? GetDefaultLayout();

            var defaultPreferredSize = GetLayoutManager().OnGetPreferredSize(this, context, layoutType);

            return defaultPreferredSize;
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

        /// <summary>
        /// Gets size of the native control without padding.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> representing
        /// context for the layout.</param>
        /// <returns></returns>
        public virtual SizeD GetBestSizeWithoutPadding(PreferredSizeContext context)
        {
            return SizeD.Empty;
        }

        /// <summary>
        /// Gets size of the native control based on the specified available size.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> representing
        /// context for the layout.</param>
        /// <returns></returns>
        public SizeD GetBestSizeWithPadding(PreferredSizeContext context)
        {
            if (IsDummy)
                return SizeD.Empty;
            var s = GetBestSizeWithoutPadding(context);
            s += Padding.Size;
            return new SizeD(
                Coord.IsNaN(SuggestedWidth) ? s.Width : SuggestedWidth,
                Coord.IsNaN(SuggestedHeight) ? s.Height : SuggestedHeight);
        }

        /// <summary>
        /// Gets the size of the area which is used for intersection of vertical
        /// and horizontal scrollbars, in device-independent units.
        /// </summary>
        /// <returns>The size of the scrollbar corner area.</returns>
        public virtual SizeD GetScrollBarCornerSize()
        {
            return ScrollBar.GetCornerSize(this);
        }

        /// <summary>
        /// Returns the size of the area which can fit all the children of this
        /// control, with an added padding.
        /// </summary>
        public virtual SizeD GetPaddedPreferredSize(SizeD preferredSize)
        {
            var padding = Padding;
            var intrinsicPadding = NativePadding;
            return preferredSize + padding.Size + intrinsicPadding.Size;
        }

        /// <summary>
        /// Calculates the maximum bottom-right coordinate among all child controls in the layout.
        /// </summary>
        /// <remarks>This method is useful for determining the overall extent required to contain all
        /// child controls within the layout. Margins are only included if the includeMargins parameter is set to
        /// true.</remarks>
        /// <param name="includeMargins">true to include each child's right
        /// and bottom margins in the calculation; otherwise, false.</param>
        /// <returns>A SizeD representing the maximum right and bottom coordinates of all child controls.
        /// The values include
        /// margins if specified.</returns>
        public virtual SizeD GetChildrenMaxRightBottom(bool includeMargins)
        {
            Coord maxRight = 0;
            Coord maxBottom = 0;
            foreach (var control in AllChildrenInLayout)
            {
                var margin = includeMargins ? control.Margin.RightBottom : SizeD.Empty;
                var controlBounds = control.Bounds;
                maxRight = Math.Max(controlBounds.Right + margin.Width, maxRight);
                maxBottom = Math.Max(controlBounds.Bottom + margin.Height, maxBottom);
            }

            return new SizeD(maxRight, maxBottom);
        }

        /// <summary>
        /// Forces the control to apply layout logic to child controls.
        /// </summary>
        /// <remarks>
        /// If the <see cref="SuspendLayout"/> method was called before calling
        /// the <see cref="PerformLayout"/> method,
        /// the layout is suppressed.
        /// </remarks>
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        /// <param name="layoutParams">The parameters for performing layout. Optional.</param>
        [Browsable(false)]
        public virtual void PerformLayout(bool layoutParent = true, PerformLayoutParams? layoutParams = null)
        {
            if (IsLayoutSuspended || DisposingOrDisposed)
            {
                return;
            }

            if (inLayout)
            {
                PerformLayoutIgnored?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (layoutParent)
            {
                if (!IsParentPerformLayoutCalled())
                    layoutParent = false;
            }

            inLayout = true;
            try
            {
                if (layoutParent)
                {
                    Parent?.PerformLayout();
                }

                OnLayout();
            }
            finally
            {
                inLayout = false;
            }

            RaiseLayoutUpdated(EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether the parent control's PerformLayout method should be called
        /// when this control's PerformLayout is called. By default this method takes into account
        /// the <see cref="LayoutFlags.NoParentPerformLayoutCalled"/> flag, the visibility of the control,
        /// and whether the control is ignoring layout.
        /// </summary>
        /// <returns><see langword="true"/> if the parent control's PerformLayout method should be called;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool IsParentPerformLayoutCalled(PerformLayoutParams? layoutParams = null)
        {
            if (LayoutFlags.HasFlag(LayoutFlags.NoParentPerformLayoutCalled) || !Visible || IgnoreLayout)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the size of the area which can fit all the children of this control.
        /// </summary>
        public virtual SizeD GetChildrenMaxPreferredSize(PreferredSizeContext context)
        {
            Coord maxWidth = 0;
            Coord maxHeight = 0;

            foreach (var control in AllChildrenInLayout)
            {
                var margin = control.Margin.Size;
                var controlContext = new PreferredSizeContext(context.AvailableSize, margin);
                var controlPreferredSize = control.GetPreferredSize(controlContext);
                var preferredSize = controlPreferredSize + margin;
                maxWidth = Math.Max(preferredSize.Width, maxWidth);
                maxHeight = Math.Max(preferredSize.Height, maxHeight);
            }

            return new SizeD(maxWidth, maxHeight);
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

        /// <summary>
        /// Gets default layout in case when <see cref="Layout"/> property is null.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This method returns <see cref="LayoutStyle.Basic"/> in <see cref="AbstractControl"/>.
        /// </remarks>
        protected virtual LayoutStyle GetDefaultLayout()
        {
            return LayoutStyle.Basic;
        }
    }
}