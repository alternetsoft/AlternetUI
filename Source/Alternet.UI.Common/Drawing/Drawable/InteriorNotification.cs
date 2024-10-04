using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements mouse handling for the <see cref="InteriorDrawable"/>.
    /// </summary>
    public class InteriorNotification : ControlNotification
    {
        private readonly InteriorDrawable interior;

        private Control? control;
        private bool subscribedClickRepeated;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteriorNotification"/> class.
        /// </summary>
        /// <param name="interior">Interior drawable.</param>
        public InteriorNotification(InteriorDrawable interior)
        {
            this.interior = interior;
        }

        /// <summary>
        /// Gets or sets whether to send scroll events to the attached control. Default is <c>true</c>.
        /// </summary>
        public virtual bool SendScrollToControl { get; set; } = true;

        /// <inheritdoc/>
        public override void AfterMouseMove(Control sender, MouseEventArgs e)
        {
            DebugUtils.DebugCallIf(false, () =>
            {
                var hitTests = interior.HitTests(sender.ScaleFactor, e.Location);
                App.LogIf(hitTests.ToString(), false);
            });
        }

        /// <inheritdoc/>
        public override void AfterSetScrollBarInfo(
            Control sender,
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
        public override void AfterScroll(Control sender, ScrollEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override void AfterMouseLeave(Control sender)
        {
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterVisualStateChanged(Control sender)
        {
            if (sender.VisualState != VisualControlState.Pressed)
                UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterLostFocus(Control sender)
        {
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonDown(Control sender, MouseEventArgs e)
        {
            OnClickElement(sender);
            SubscribeClickRepeated(sender);
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonUp(Control sender, MouseEventArgs e)
        {
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterClick(Control sender)
        {
            var mouseLocation = Mouse.GetPosition(sender);
            var hitTests = interior.HitTests(sender.ScaleFactor, mouseLocation);

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

        private void OnClickElement(Control sender)
        {
            var mouseLocation = Mouse.GetPosition(sender);

            var hitTests = interior.HitTests(sender.ScaleFactor, mouseLocation);

            if (!hitTests.IsScrollBar)
                return;

            ScrollEventType evType;

            switch (hitTests.ScrollBar)
            {
                case ScrollBarDrawable.HitTestResult.None:
                    return;
                case ScrollBarDrawable.HitTestResult.Thumb:
                    return;
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
                    return;
            }

            ScrollEventArgs scrollArgs = new();

            scrollArgs.ScrollOrientation = hitTests.Orientation;
            scrollArgs.Type = evType;

            interior.RaiseScroll(sender, scrollArgs);

            if (SendScrollToControl)
                sender.RaiseScroll(scrollArgs);
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

        private void SubscribeClickRepeated(Control control)
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
