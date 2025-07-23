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
        }

        /// <inheritdoc/>
        public override void AfterVisualStateChanged(AbstractControl sender, EventArgs e)
        {
            if (sender.VisualState != VisualControlState.Pressed)
                UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterVisibleChanged(AbstractControl sender, EventArgs e)
        {
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterLostFocus(AbstractControl sender, LostFocusEventArgs e)
        {
            UnsubscribeClickRepeated();
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonDown(AbstractControl sender, MouseEventArgs e)
        {
            var hitTests = OnClickElement(sender);

            if (hitTests.NeedRepeatedClick)
            {
                SubscribeClickRepeated(sender);
            }
        }

        /// <inheritdoc/>
        public override void AfterMouseLeftButtonUp(AbstractControl sender, MouseEventArgs e)
        {
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

            ScrollEventArgs scrollArgs = new();

            scrollArgs.ScrollOrientation = hitTests.Orientation;
            scrollArgs.Type = evType;

            interior.RaiseScroll(sender, scrollArgs);

            if (SendScrollToControl)
                sender.RaiseScroll(scrollArgs);

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
