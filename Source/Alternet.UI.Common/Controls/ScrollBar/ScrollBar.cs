using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// A <see cref="ScrollBar"/> is a control that represents a horizontal or vertical scrollbar.
    /// </summary>
    /// <remarks>
    /// A scrollbar has the following main attributes: range, thumb size,
    /// page size, and position. The range is the total number of units
    /// associated with the view represented by the scrollbar. For a table
    /// with 15 columns, the range would be 15. The thumb size is the number of units
    /// that are currently visible. For the table example, the window might be sized
    /// so that only 5 columns are currently visible, in which case the application
    /// would set the thumb size to 5. When the thumb size becomes the same as or
    /// greater than the range, the scrollbar will be automatically hidden on
    /// most platforms. The page size is the number of units that the scrollbar
    /// should scroll by, when 'paging' through the data. This value is normally the
    /// same as the thumb size length, because it is natural to assume that the visible
    /// window size defines a page. The scrollbar position is the current thumb position.
    /// Most applications will find it convenient to provide a function called
    /// AdjustScrollbars() which can be called initially, from an OnSize event
    /// handler, and whenever the application data changes in size. It will adjust
    /// the view, object and page size according to the size of the window and the size of the data.
    /// </remarks>
    [ControlCategory("Common")]
    public partial class ScrollBar : Control
    {
        private static MetricsInfo? defaultMetrics;

        private readonly AltPositionInfo pos = new();

        private MetricsInfo? metrics;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        public ScrollBar()
        {
            pos.PropertyChanged += OnPositionPropertyChanged;
        }

        /// <summary>
        /// Occurs when the <see cref="Value" /> property is changed, either
        /// by a <see cref="Control.Scroll" /> event or programmatically.
        /// </summary>
        [Category("Action")]
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs when the <see cref="IsVertical" /> property is changed.
        /// </summary>
        [Category("Action")]
        public event EventHandler? IsVerticalChanged;

        /// <summary>
        /// Gets or sets default metrics used to paint non-system scrollbars.
        /// </summary>
        public static MetricsInfo DefaultMetrics
        {
            get
            {
                return defaultMetrics ??= new MetricsInfo();
            }

            set
            {
                defaultMetrics = value;
            }
        }

        /// <summary>
        /// Gets or sets metrics used to paint this scrollbar when its style is non-system.
        /// </summary>
        [Browsable(false)]
        public virtual MetricsInfo? Metrics
        {
            get
            {
                return metrics;
            }

            set
            {
                metrics = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value to be added to or subtracted from the
        /// <see cref="Value" /> property when the scroll box is moved a large distance.
        /// </summary>
        /// <returns>A numeric value. The default value is 10.</returns>
        [Category("Behavior")]
        [DefaultValue(10)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int LargeChange
        {
            get
            {
                return pos.LargeChange;
            }

            set
            {
                pos.LargeChange = value;
            }
        }

        /// <summary>
        /// Gets or sets the upper limit of values of the scrollable range.
        /// </summary>
        /// <returns>
        /// A numeric value. The default value is 100.
        /// </returns>
        [Category("Behavior")]
        [DefaultValue(100)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int Maximum
        {
            get
            {
                return pos.Maximum;
            }

            set
            {
                pos.Maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the lower limit of values of the scrollable range.
        /// </summary>
        /// <returns>
        /// A numeric value. The default value is 0.
        /// </returns>
        [Category("Behavior")]
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public virtual int Minimum
        {
            get
            {
                return pos.Minimum;
            }

            set
            {
                pos.Minimum = value;
            }
        }

        /// <summary>
        /// Gets or sets the value to be added to or subtracted from
        /// the <see cref="Value" /> property when the scroll thumb is moved
        /// a small distance.
        /// </summary>
        /// <returns>A numeric value. The default value is 1.</returns>
        [Category("Behavior")]
        [DefaultValue(1)]
        public virtual int SmallChange
        {
            get
            {
                return pos.SmallChange;
            }

            set
            {
                pos.SmallChange = value;
            }
        }

        /// <inheritdoc cref="Control.IsBold"/>
        [Browsable(false)]
        public new bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        /// <inheritdoc cref="Control.Font"/>
        [Browsable(false)]
        public new Font? Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        /// <inheritdoc cref="Control.BackgroundColor"/>
        [Browsable(false)]
        public new Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set => base.BackgroundColor = value;
        }

        /// <inheritdoc cref="Control.ForegroundColor"/>
        [Browsable(false)]
        public new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set => base.ForegroundColor = value;
        }

        /// <inheritdoc cref="Control.Padding"/>
        [Browsable(false)]
        public new Thickness Padding
        {
            get => base.Padding;
            set => base.Padding = value;
        }

        /// <summary>
        /// Gets or sets a numeric value that represents the current position of the
        /// scroll thumb on the scroll bar control.
        /// </summary>
        /// <returns>A numeric value that is within the <see cref="Minimum" /> and
        /// <see cref="Maximum" /> range. The default value is 0.</returns>
        [Category("Behavior")]
        [DefaultValue(0)]
        [Bindable(true)]
        public virtual int Value
        {
            get
            {
                return pos.Value;
            }

            set
            {
                pos.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="ScrollBar"/> is vertical.
        /// </summary>
        public virtual bool IsVertical
        {
            get
            {
                return Handler.IsVertical;
            }

            set
            {
                if (IsVertical == value)
                    return;
                Handler.IsVertical = value;
                IsVerticalChanged?.Invoke(this, EventArgs.Empty);
                UpdateScrollInfo();
            }
        }

        /// <summary>
        /// Gets scrollbar position as <see cref="AltPositionInfo"/>.
        /// </summary>
        [Browsable(false)]
        public AltPositionInfo AltPosInfo => pos;

        /// <summary>
        /// Gets scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        [Browsable(false)]
        public ScrollBarInfo PosInfo => pos.AsPositionInfo();

        /// <summary>
        /// Gets control handler.
        /// </summary>
        [Browsable(false)]
        public new IScrollBarHandler Handler => (IScrollBarHandler)base.Handler;

        [Browsable(false)]
        internal new string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        [Browsable(false)]
        internal new bool ParentFont
        {
            get => base.ParentFont;
            set => base.ParentFont = value;
        }

        [Browsable(false)]
        internal new string Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        [Browsable(false)]
        internal new bool ParentForeColor
        {
            get => base.ParentForeColor;
            set => base.ParentForeColor = value;
        }

        [Browsable(false)]
        internal new bool ParentBackColor
        {
            get => base.ParentBackColor;
            set => base.ParentBackColor = value;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="ScrollBar" /> control.
        /// </summary>
        /// <returns>A string that represents the current <see cref="ScrollBar" />.</returns>
        public override string ToString()
        {
            string? text = base.ToString();
            return text +
                ", Minimum: " + Minimum.ToString(CultureInfo.CurrentCulture) +
                ", Maximum: " + Maximum.ToString(CultureInfo.CurrentCulture) +
                ", Value: " + Value.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Logs scrollbar info.
        /// </summary>
        public virtual void LogInfo()
        {
            App.Log(ToString());
            var position = $"Position: {Handler.ThumbPosition}";
            var range = $"Range: {Handler.Range}";
            var pageSize = $"PageSize: {Handler.PageSize}";
            App.Log($"Native: {position}, {range}, {pageSize}");
        }

        /// <summary>
        /// Updates scroll info and calls <see cref="SetScrollbar"/> to update native control.
        /// </summary>
        public virtual void UpdateScrollInfo()
        {
            var posInfo = pos.AsPositionInfo();
            SetScrollbar(posInfo.Position, posInfo.Range, posInfo.PageSize);
        }

        /// <summary>
        /// Raises scroll events.
        /// </summary>
        public virtual void RaiseScroll()
        {
            var newPos = (Handler.EventNewPos / SmallChange) + Minimum;
            var oldPos = Value;
            newPos = MathUtils.ApplyMinMax(newPos, Minimum, Maximum);
            if (newPos == oldPos)
                return;
            pos.Value = newPos;
            var eventType = Handler.EventTypeID;
            var orientation = Handler.IsVertical ? ScrollBarOrientation.Vertical
                : ScrollBarOrientation.Horizontal;
            RaiseScroll(new ScrollEventArgs(eventType, oldPos, newPos, orientation));
            OnValueChanged(EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateScrollBarHandler(this);
        }

        /// <summary>
        /// Sets the native scrollbar properties.
        /// </summary>
        /// <param name="position">The position of the scrollbar in scroll units.</param>
        /// <param name="range">The maximum position of the scrollbar.</param>
        /// <param name="pageSize">The size of the page size in scroll units. This is the
        /// number of units the scrollbar will scroll when it is paged up or down.
        /// Often it is the same as the thumb size.</param>
        /// <param name="refresh"><c>true</c> to redraw the scrollbar, <c>false</c> otherwise.</param>
        /// <remarks>
        /// Let's say you wish to display 50 lines of text, using the same font.
        /// The window is sized so that you can only see 16 lines at a time. You would use:
        /// scrollbar.SetScrollbar(0, 16, 50, 15);
        /// The page size is 1 less than the thumb size so that the last line of the previous
        /// page will be visible on the next page, to help orient the user. Note that with the
        /// window at this size, the thumb position can never go above 50 minus 16, or 34.
        /// You can determine how many lines are currently visible by dividing the
        /// current view size by the character height in pixels. When defining your own
        /// scrollbar behaviour, you will always need to recalculate the scrollbar settings
        /// when the window size changes. You could therefore put your scrollbar calculations
        /// and SetScrollbar() call into a function named AdjustScrollbars, which can
        /// be called initially and also from a size event handler function.
        /// </remarks>
        protected virtual void SetScrollbar(
            int? position,
            int? range,
            int? pageSize,
            bool refresh = true)
        {
            Handler.SetScrollbar(
                position,
                range,
                pageSize,
                refresh);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged" /> event.</summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void BindHandlerEvents()
        {
            base.BindHandlerEvents();
            UpdateScrollInfo();
            Handler.Scroll = RaiseScroll;
        }

        /// <inheritdoc/>
        protected override void UnbindHandlerEvents()
        {
            base.UnbindHandlerEvents();
            Handler.Scroll = null;
        }

        /// <summary>
        /// Gets size of the scrollbar from the <see cref="Metrics"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeD SizeFromMetrics()
        {
            var result = GetRealMetrics().GetPreferredSize(IsVertical, ScaleFactor);
            return result;
        }

        /// <summary>
        /// Gets size of the arrow bitmap from the <see cref="Metrics"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeD ArrowBitmapSizeFromMetrics()
        {
            var result = GetRealMetrics().GetArrowBitmapSize(IsVertical, ScaleFactor);
            return result;
        }

        /// <summary>
        /// Gets real scroll bar metrics. If <see cref="Metrics"/> is not specified, returns
        /// <see cref="ScrollBar.DefaultMetrics"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual ScrollBar.MetricsInfo GetRealMetrics()
        {
            return metrics ?? ScrollBar.DefaultMetrics;
        }

        /// <summary>
        /// Gets size of the scroll thumb from the <see cref="Metrics"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual SizeD ThumbSizeFromMetrics()
        {
            var result = GetRealMetrics().GetThumbSize(IsVertical, ClientSize, ScaleFactor);
            return result;
        }

        private void OnPositionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateScrollInfo();
            if (!e.HasPropertyName())
                OnValueChanged(EventArgs.Empty);
        }
    }
}