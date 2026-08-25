using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a scrollable area that can contain other visible elements.
    /// </summary>
    [ControlCategory(KnownControlCategory.Containers)]
    public partial class ScrollViewer : ScrollableCanvasControl
    {
        internal static bool LogDebugInfo = true;

        private readonly ScrollContainer scrollContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollViewer"/> class.
        /// </summary>
        public ScrollViewer()
        {
            scrollContainer = new ScrollContainer();
            scrollContainer.Parent = this;

            SizeChanged += (s, e) =>
            {
                UpdateContentViewportRect();
            };

            scrollContainer.ChildBoundsChanged += (s, e) =>
            {
                UpdateContentViewportRect();
            };

            Interior?.Required();
        }

        /// <summary>
        /// Gets the content of the scroll viewer.
        /// </summary>
        [Browsable(false)]
        public ScrollContainer Content
        {
            get
            {
                return scrollContainer;
            }
        }

        /// <summary>
        /// Gets the collection of child controls within the scroll container,
        /// which represents the content of the scroll viewer.
        /// </summary>
        [Content]
        [Browsable(false)]
        public BaseCollection<AbstractControl> ContentChildren
        {
            get
            {
                return scrollContainer.Children;
            }
        }

        /// <summary>
        /// Gets the first child control within the scroll container, which can be used
        /// to determine the current scroll position and the layout of the content.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? FirstContentChild
        {
            get
            {
                return scrollContainer?.FirstChild;
            }
        }

        /// <inheritdoc/>
        public override PointD LayoutOffset
        {
            get
            {
                return base.LayoutOffset;
            }

            set
            {
                if (LayoutOffset == value)
                    return;

                SuspendUpdateInterior();

                try
                {
                    base.LayoutOffset = value;

                    scrollContainer.OnLayout();
                }
                finally
                {
                    ResumeUpdateInterior();
                }

            }
        }

        /// <summary>
        /// This is redeclared to hide the base class's Children property, which is not
        /// relevant for the ScrollViewer control,
        /// as the ScrollViewer uses a <see cref="ScrollContainer"/> to manage its content.
        /// </summary>
        [Browsable(false)]
        new protected ControlCollection Children
        {
            get
            {
                return base.Children;
            }
        }

        /// <summary>
        /// This is redeclared to hide the base class's Children property, which is
        /// not relevant for the ScrollViewer control,
        /// as the ScrollViewer uses a <see cref="ScrollContainer"/> to manage its content.
        /// </summary>
        [Browsable(false)]
        new protected ControlCollection Controls
        {
            get
            {
                return base.Children;
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

            if (child is not null)
            {
                child.Parent = result.Content;
                child.Visible = true;
            }

            return result;
        }

        /// <inheritdoc/>
        public override bool IsValidChild(AbstractControl control)
        {
            return control == scrollContainer;
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            var dc = e.Graphics;

            DrawDefaultBackground(e, DrawDefaultBackgroundFlags.DrawBorderAndBackground);

            UpdateInteriorProperties();

            DrawInterior(dc);
        }

        /// <inheritdoc/>
        protected override void OnBeforeChildMouseWheel(object? sender, MouseEventArgs e)
        {
            if (e.Handled || IgnoreChildMouseWheel(sender as AbstractControl)
                || !IsScrolledWithMouseWheel)
                return;

            var sign = Math.Sign(e.Delta);
            var isVert = !Keyboard.IsShiftPressed;
            var delta = GetDefaultScrollWheelDelta(MeasureCanvas, RealFont, isVert);
            var incValue1 = -(sign * delta);
            var incValue2 = 0;

            if (isVert)
            {
                incValue2 = incValue1;
                incValue1 = 0;
            };

            DoActionOffsetScroll(new SizeD(incValue1, incValue2));

            e.Handled = true;
        }

        /// <inheritdoc/>
        protected override SizeD GetContentPreferredSize()
        {
            var control = FirstContentChild;

            if (control is null || !control.Visible)
                return SizeD.Empty;

            var result = control.Bounds.Size;

            var contentSizeScale = GetEffectiveContentSizeScale();

            result.Width *= contentSizeScale.Width;
            result.Height *= contentSizeScale.Height;

            return result;
        }

        /// <inheritdoc/>
        public override void OnLayout()
        {
            // This method is supposed to do nothing.
        }

        /// <inheritdoc/>
        public override RectD GetGenericChildrenClipRect()
        {
            return scrollContainer.Bounds;
        }

        /// <inheritdoc/>
        protected override void OnScrolledVerticallyChanged()
        {
            UpdateContentViewportRect(refresh: true);
        }

        /// <inheritdoc/>
        protected override void OnScrolledHorizontallyChanged()
        {
            UpdateContentViewportRect(refresh: true);
        }

        /// <summary>
        /// Updates the viewport rectangle of the content area within the scroll viewer,
        /// ensuring that the visible portion of the content is correctly displayed based
        /// on the current scroll position and layout.
        /// </summary>
        /// <param name="refresh">Indicates whether to refresh the interior after updating the viewport rectangle.</param>
        protected virtual void UpdateContentViewportRect(bool refresh = true)
        {
            var viewportRect = GetPreferredContentViewportRect();
            scrollContainer.Bounds = viewportRect;
            UpdateInterior(refresh);
        }

        /// <summary>
        /// Represents a container control that is used to hold the child controls within
        /// the scrollable area of the <see cref="ScrollViewer"/>.
        /// </summary>
        public class ScrollContainer : UserControl
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ScrollContainer"/> class,
            /// which serves as the container for child controls within the
            /// scrollable area of the <see cref="ScrollViewer"/>.
            /// </summary>
            public ScrollContainer()
            {
                TabStop = false;
                CanSelect = false;
                ParentBackColor = true;
                ParentForeColor = true;
            }

            /// <inheritdoc/>
            public override void DefaultPaint(PaintEventArgs e)
            {
                DrawDefaultBackground(e, DrawDefaultBackgroundFlags.DrawBackground);
            }

            /// <inheritdoc/>
            public override void OnLayout()
            {
                var control = FirstChild;

                if (control is null)
                    return;

                var unboundedPreferredSize = control.GetPreferredSize();

                var p = Parent as ScrollViewer;

                var layoutOffset = p?.LayoutOffset ?? PointD.Empty;

                control.Bounds = new RectD(
                    - layoutOffset.X,
                    - layoutOffset.Y,
                    unboundedPreferredSize.Width,
                    unboundedPreferredSize.Height);
            }

            /// <inheritdoc/>
            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                Invalidate();
            }
        }
    }
}
