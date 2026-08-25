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
    /// A <see cref="XScrollBar"/> is a control that represents a horizontal or vertical scrollbar.
    /// </summary>
    [ControlCategory(KnownControlCategory.Common)]
    public partial class XScrollBar : ScrollableGenericControl, IScrollEventRouter
    {
        private readonly AltScrollBarPositionInfo pos = new();

        private ScrollBarMetricsInfo? metrics;
        private bool isVertical;

        /// <summary>
        /// Initializes a new instance of the <see cref="XScrollBar"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public XScrollBar(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XScrollBar"/> class.
        /// </summary>
        public XScrollBar()
        {
            TabStop = false;
            CanSelect = false;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            HasBorder = false;
            pos.PropertyChanged += OnPositionPropertyChanged;
            Interior.Border = null;
            Interior.Background = null;
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
        public virtual ScrollBarMetricsInfo? Metrics
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
        public new Font Font
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
        /// Gets or sets whether <see cref="XScrollBar"/> is vertical.
        /// </summary>
        public virtual bool IsVertical
        {
            get
            {
                return isVertical;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (IsVertical == value)
                    return;
                isVertical = value;
                UpdateScrollBars(refresh: true);
                IsVerticalChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets scrollbar position as <see cref="AltScrollBarPositionInfo"/>.
        /// </summary>
        [Browsable(false)]
        public virtual AltScrollBarPositionInfo AltPosInfo
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

        /// <inheritdoc/>
        public override IScrollEventRouter ScrollEventRouter
        {
            get
            {
                return this;
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
        /// Returns a string that represents the <see cref="XScrollBar" /> control.
        /// </summary>
        /// <returns>A string that represents the current <see cref="XScrollBar" />.</returns>
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
        /// This method should not be used in the scrollbar.
        /// </summary>
        public override ScrollBarInfo GetScrollBarInfo(bool isVertical)
        {
            if (isVertical == IsVertical)
                return PosInfo;
            return ScrollBarInfo.Hidden;
        }

        /// <summary>
        /// This method should not be used in the scrollbar.
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
        public override void DefaultPaint(PaintEventArgs e)
        {
            if (DisposingOrDisposed)
                return;

            e.Graphics.DoInsideClipped(ClientRectangle, DoDefaultPaint);

            void DoDefaultPaint()
            {
                var dc = e.Graphics;

                UpdateInteriorProperties();

                DrawInterior(dc);
            }
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

        /// <inheritdoc/>
        protected override SizeD GetPreferredSizeInternal(PreferredSizeContext context)
        {
            var scaleFactor = ScaleFactor;
            var suggested = SuggestedSize;

            var isNanSuggestedWidth = suggested.IsNanWidth;
            var isNanSuggestedHeight = suggested.IsNanHeight;

            var containerSize = suggested;

            if (isNanSuggestedWidth)
                containerSize.Width = context.AvailableSize.Width;

            if (isNanSuggestedHeight)
                containerSize.Height = context.AvailableSize.Height;

            var measured = containerSize;

            var metrics = Interior.GetRealMetrics(this);

            if (IsVertical)
            {
                var vertWidth = metrics.GetPreferredSize(isVertical: true, scaleFactor).Width;
                measured.Width = vertWidth;
            }
            else
            {
                var horzHeight = metrics.GetPreferredSize(isVertical: false, scaleFactor).Height;
                measured.Height = horzHeight;
            }

            return measured;
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
        /// <see cref="ScrollBarMetricsInfo.DefaultMetrics"/>.
        /// </summary>
        /// <returns></returns>
        protected virtual ScrollBarMetricsInfo GetRealMetrics()
        {
            return metrics ?? ScrollBarMetricsInfo.DefaultMetrics(this);
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

        /// <summary>
        /// Called when any member of <see cref="AltPosInfo"/> property is changed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        private void OnPositionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            Refresh();
            if (!e.HasPropertyName())
                RaiseValueChanged();
        }

        void IScrollEventRouter.CalcScrollBarInfo(out ScrollBarInfo horzScrollbar, out ScrollBarInfo vertScrollbar)
        {
            horzScrollbar = GetScrollBarInfo(isVertical: false);
            vertScrollbar = GetScrollBarInfo(isVertical: true);
        }

        void IScrollEventRouter.DoActionScrollCharLeft()
        {
            pos.Value -= SmallChange;
        }

        void IScrollEventRouter.DoActionScrollCharRight()
        {
            pos.Value += SmallChange;
        }

        void IScrollEventRouter.DoActionScrollToFirstChar()
        {
            pos.Value = Minimum;
        }

        void IScrollEventRouter.DoActionScrollPageLeft()
        {
            pos.Value -= LargeChange;
        }

        void IScrollEventRouter.DoActionScrollPageRight()
        {
            pos.Value += LargeChange;
        }

        void IScrollEventRouter.DoActionScrollPageUp()
        {
            pos.Value -= LargeChange;
        }

        void IScrollEventRouter.DoActionScrollPageDown()
        {
            pos.Value += LargeChange;
        }

        void IScrollEventRouter.DoActionScrollLineUp()
        {
            pos.Value -= SmallChange;
        }

        void IScrollEventRouter.DoActionScrollLineDown()
        {
            pos.Value += SmallChange;
        }

        void IScrollEventRouter.DoActionScrollToFirstLine()
        {
            pos.Value = Minimum;
        }

        void IScrollEventRouter.DoActionScrollToLastLine()
        {
            pos.Value = Maximum;
        }

        void IScrollEventRouter.DoActionScrollToVertPos(int value)
        {
            pos.Value = value;
        }

        void IScrollEventRouter.DoActionScrollToHorzPos(int value)
        {
            pos.Value = value;
        }
    }

    /// <summary>
    /// A <see cref="XVertScrollBar"/> is a control that represents a vertical scrollbar.
    /// </summary>
    public partial class XVertScrollBar : XScrollBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XVertScrollBar"/> class.
        /// </summary>
        public XVertScrollBar()
            : base()
        {
            IsVertical = true;
        }
    }

    /// <summary>
    /// A <see cref="XHorzScrollBar"/> is a control that represents a horizontal scrollbar.
    /// </summary>
    public partial class XHorzScrollBar : XScrollBar
    {
    }
}