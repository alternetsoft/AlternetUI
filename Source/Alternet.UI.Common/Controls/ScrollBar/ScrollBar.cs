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
        private readonly AltPositionInfo pos = new();

        private MetricsInfo? metrics;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ScrollBar(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollBar"/> class.
        /// </summary>
        public ScrollBar()
        {
            pos.PropertyChanged += OnPositionPropertyChanged;
        }

        /// <summary>
        /// Occurs when the <see cref="Value" /> property is changed, either
        /// by a <see cref="AbstractControl.Scroll" /> event or programmatically.
        /// </summary>
        [Category("Action")]
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs when the <see cref="IsVertical" /> property is changed.
        /// </summary>
        [Category("Action")]
        public event EventHandler? IsVerticalChanged;

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

        /// <inheritdoc cref="AbstractControl.IsBold"/>
        [Browsable(false)]
        public new bool IsBold
        {
            get => base.IsBold;
            set => base.IsBold = value;
        }

        /// <inheritdoc cref="AbstractControl.Font"/>
        [Browsable(false)]
        public new Font? Font
        {
            get => base.Font;
            set => base.Font = value;
        }

        /// <inheritdoc cref="AbstractControl.BackgroundColor"/>
        [Browsable(false)]
        public new Color? BackgroundColor
        {
            get => base.BackgroundColor;
            set => base.BackgroundColor = value;
        }

        /// <inheritdoc cref="AbstractControl.ForegroundColor"/>
        [Browsable(false)]
        public new Color? ForegroundColor
        {
            get => base.ForegroundColor;
            set => base.ForegroundColor = value;
        }

        /// <inheritdoc cref="AbstractControl.Padding"/>
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
                if (DisposingOrDisposed)
                    return default;
                return PlatformControl.IsVertical;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (IsVertical == value)
                    return;
                PlatformControl.IsVertical = value;
                IsVerticalChanged?.Invoke(this, EventArgs.Empty);
                UpdateScrollInfo();
            }
        }

        /// <summary>
        /// Gets scrollbar position as <see cref="AltPositionInfo"/>.
        /// </summary>
        [Browsable(false)]
        public virtual AltPositionInfo AltPosInfo
        {
            get
            {
                return pos;
            }
        }

        /// <summary>
        /// Gets scrollbar position as <see cref="ScrollBarInfo"/>.
        /// </summary>
        [Browsable(false)]
        public virtual ScrollBarInfo PosInfo
        {
            get
            {
                var result = pos.AsPositionInfo();
                return result;
            }

            set
            {
                pos.Assign(value);
            }
        }

        internal IScrollBarHandler PlatformControl
        {
            get
            {
                CheckDisposed();
                return (IScrollBarHandler)Handler;
            }
        }

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
        /// Gets or sets default metrics used to paint non-system scrollbars.
        /// </summary>
        public static MetricsInfo DefaultMetrics(AbstractControl control)
        {
            return new MetricsInfo(control);
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
        /// Raises the <see cref="ValueChanged" /> event and
        /// <see cref="OnValueChanged"/> method.
        /// </summary>
        public void RaiseValueChanged()
        {
            OnValueChanged(EventArgs.Empty);
            ValueChanged?.Invoke(this, EventArgs.Empty);
            Designer?.RaisePropertyChanged(this, nameof(Value));
        }

        /// <summary>
        /// Logs scrollbar info.
        /// </summary>
        public virtual void LogInfo()
        {
            App.Log(ToString());
            var position = $"Position: {PlatformControl.ThumbPosition}";
            var range = $"Range: {PlatformControl.Range}";
            var pageSize = $"PageSize: {PlatformControl.PageSize}";
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
        /// This method should not be used in the <see cref="ScrollBar"/>.
        /// </summary>
        public override ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            if (isVertical == IsVertical)
                return PosInfo;
            return ScrollBarInfo.Default;
        }

        /// <summary>
        /// This method should not be used in the <see cref="ScrollBar"/>.
        /// </summary>
        public override void SetScrollBarInfo(bool isVertical, ScrollBarInfo value)
        {
        }

        /// <summary>
        /// Raises scroll event.
        /// </summary>
        public virtual void RaiseHandlerScroll(ScrollEventType eventType, int newPosFromZero)
        {
            if (DisposingOrDisposed)
                return;
            var newPos = (newPosFromZero / SmallChange) + Minimum;
            var oldPos = Value;
            newPos = MathUtils.ApplyMinMax(newPos, Minimum, Maximum);
            if (newPos == oldPos)
                return;
            pos.Value = newPos;
            var orientation = IsVertical ? ScrollBarOrientation.Vertical
                : ScrollBarOrientation.Horizontal;
            RaiseScroll(new ScrollEventArgs(eventType, oldPos, newPos, orientation));
            RaiseValueChanged();
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return ControlFactory.Handler.CreateScrollBarHandler(this);
        }

        /// <inheritdoc/>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
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
        /// scrollbar behavior, you will always need to recalculate the scrollbar settings
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
            if (DisposingOrDisposed)
                return;
            PlatformControl.SetScrollbar(
                position,
                range,
                pageSize,
                refresh);
        }

        /// <summary>
        /// Called when <see cref="ValueChanged"/> event is raised.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs" /> that contains the event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
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
            return metrics ?? ScrollBar.DefaultMetrics(this);
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
            if (DisposingOrDisposed)
                return;
            UpdateScrollInfo();
            if (!e.HasPropertyName())
                RaiseValueChanged();
        }
    }
}