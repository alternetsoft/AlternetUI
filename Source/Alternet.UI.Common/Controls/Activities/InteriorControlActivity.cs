using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements mouse handling for the <see cref="InteriorDrawable"/>.
    /// </summary>
    public class InteriorControlActivity : BaseControlActivity
    {
        private readonly InteriorDrawable interior;

        private AbstractControl? control;
        private bool subscribedClickRepeated;
        private bool isDragging = false;
        private PointD clickOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteriorControlActivity"/> class.
        /// </summary>
        /// <param name="interior">Interior drawable.</param>
        public InteriorControlActivity(InteriorDrawable interior)
        {
            this.interior = interior;
        }

        /// <summary>
        /// Gets or sets whether to send scroll events to the attached control. Default is <c>true</c>.
        /// </summary>
        public virtual bool SendScrollToControl { get; set; } = true;

        /// <inheritdoc/>
        public override void AfterMouseMove(AbstractControl sender, MouseEventArgs e)
        {
            DebugUtils.DebugCallIf(false, () =>
            {
                var hitTests = interior.HitTests(sender, e.Location);

                App.LogIf(hitTests.ToString(), false);
            });

            if (isDragging)
            {
                var hitTests = interior.HitTests(sender, e.Location);

                var beforeThumb = hitTests.ScrollRectangles[ScrollBarDrawable.HitTestResult.BeforeThumb];
                var afterThumb = hitTests.ScrollRectangles[ScrollBarDrawable.HitTestResult.AfterThumb];
                var thumb = hitTests.ScrollRectangles[ScrollBarDrawable.HitTestResult.Thumb];

                var oldValue = hitTests.ScrollPosition.Position;
                var maxValue = hitTests.ScrollPosition.Range;

                ScrollEventType evType = ScrollEventType.ThumbTrack;

                if (hitTests.IsHorzScrollBar)
                {
                    var maxX = afterThumb.Right - beforeThumb.X;
                    var newX = Math.Max(e.X - beforeThumb.X - clickOffset.X, 0);
                    var newValue = (int)((maxValue * newX) / maxX);
                    if (newValue != oldValue)
                        RaiseScroll(sender, ScrollBarOrientation.Horizontal, evType, oldValue, newValue);
                }
                else
                if (hitTests.IsVertScrollBar)
                {
                    var maxY = afterThumb.Bottom - beforeThumb.Y;
                    var newY = Math.Max(e.Y - beforeThumb.Y - clickOffset.Y, 0);
                    var newValue = (int)((maxValue * newY) / maxY);
                    if(newValue != oldValue)
                        RaiseScroll(sender, ScrollBarOrientation.Vertical, evType, oldValue, newValue);
                }
            }
        }

        /// <inheritdoc/>
        public override void AfterSetScrollBarInfo(
            AbstractControl sender,
            bool isVertical,
            ScrollBarInfo value)
        {
            DebugUtils.DebugCallIf(false, () =>
            {
                if (isVertical)
                    return;
                var prefix = isVertical ? "V: " : "H: ";
                var s = $"{prefix}{value}";
                LogUtils.LogAndToFile(s);
            });
        }

        /// <inheritdoc/>
        public override void AfterScroll(AbstractControl sender, ScrollEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseLeave(AbstractControl sender, EventArgs e)
        {
            UnsubscribeClickRepeated();
            isDragging = false;
        }

        /// <inheritdoc/>
        public override void AfterVisualStateChanged(AbstractControl sender, EventArgs e)
        {
            if (sender.VisualState != VisualControlState.Pressed)
                UnsubscribeClickRepeated();
            isDragging = false;
        }

        /// <inheritdoc/>
        public override void AfterVisibleChanged(AbstractControl sender, EventArgs e)
        {
            isDragging = false;
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterLostFocus(AbstractControl sender, LostFocusEventArgs e)
        {
            isDragging = false;
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void BeforeMouseDown(AbstractControl sender, MouseEventArgs e)
        {
            if(e.Button != MouseButtons.Left)
                return;

            var hitTests = OnClickElement(sender);

            if (hitTests.NeedRepeatedClick)
            {
                SubscribeClickRepeated(sender);
            }

            if(hitTests.IsThumb)
            {
                isDragging = true;
                var thumb = hitTests.ScrollRectangles[ScrollBarDrawable.HitTestResult.Thumb];
                clickOffset = e.Location - thumb.Location;
            }

            if (hitTests.IsScrollBar)
            {
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonUp(AbstractControl sender, MouseEventArgs e)
        {
            isDragging = false;
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterClick(AbstractControl sender, EventArgs e)
        {
            var mouseLocation = Mouse.GetPosition(sender);
            var hitTests = interior.HitTests(sender, mouseLocation);

            if (!hitTests.IsNone)
            {
                interior.RaiseElementClick(sender, hitTests);

                if (hitTests.IsCorner)
                {
                    interior.RaiseCornerClick(sender);
                }
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnsubscribeClickRepeated();
            base.DisposeManaged();
        }

        private void RaiseScroll(
            AbstractControl sender,
            ScrollBarOrientation orientation,
            ScrollEventType evType,
            int oldValue = 0,
            int newValue = 0)
        {
            ScrollEventArgs scrollArgs = new();

            scrollArgs.ScrollOrientation = orientation;
            scrollArgs.Type = evType;
            scrollArgs.NewValue = newValue;
            scrollArgs.OldValue = oldValue;

            interior.RaiseScroll(sender, scrollArgs);

            if (SendScrollToControl)
                sender.RaiseScroll(scrollArgs);
        }

        private InteriorDrawable.HitTestsResult OnClickElement(AbstractControl sender)
        {
            var mouseLocation = Mouse.GetPosition(sender);

            var hitTests = interior.HitTests(sender, mouseLocation);

            if (!hitTests.IsScrollBar)
                return hitTests;

            ScrollEventType evType;

            switch (hitTests.ScrollBar)
            {
                case ScrollBarDrawable.HitTestResult.None:
                    return hitTests;
                case ScrollBarDrawable.HitTestResult.Thumb:
                    return hitTests;
                case ScrollBarDrawable.HitTestResult.StartButton:
                    evType = ScrollEventType.SmallDecrement;
                    break;
                case ScrollBarDrawable.HitTestResult.EndButton:
                    evType = ScrollEventType.SmallIncrement;
                    break;
                case ScrollBarDrawable.HitTestResult.BeforeThumb:
                    evType = ScrollEventType.LargeDecrement;
                    break;
                case ScrollBarDrawable.HitTestResult.AfterThumb:
                    evType = ScrollEventType.LargeIncrement;
                    break;
                default:
                    return hitTests;
            }

            RaiseScroll(sender, hitTests.Orientation, evType);

            return hitTests;
        }

        private void OnClickRepeatTimerEvent(object sender, EventArgs e)
        {
            if (control is null)
                return;

            if (TimerUtils.LastClickLessThanRepeatInterval(control))
                return;
            OnClickElement(control);
        }

        private void UnsubscribeClickRepeated()
        {
            this.control = null;
            if (subscribedClickRepeated)
            {
                TimerUtils.ClickRepeated -= OnClickRepeatTimerEvent;
                subscribedClickRepeated = false;
            }
        }

        private void SubscribeClickRepeated(AbstractControl control)
        {
            if (!subscribedClickRepeated)
            {
                this.control = control;
                TimerUtils.ClickRepeated += OnClickRepeatTimerEvent;
                subscribedClickRepeated = true;
            }
        }
    }
}
