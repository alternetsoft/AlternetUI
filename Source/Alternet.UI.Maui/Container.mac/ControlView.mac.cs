using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

#if IOS || MACCATALYST

using SkiaSharp.Views.iOS;

using UIKit;

namespace Alternet.UI
{
    public partial class ControlView
    {
        /// <inheritdoc/>
        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

            var platformView = GetPlatformView(args.OldHandler);
            if (platformView is null)
                return;

            platformView.OnPressesBegan = null;
            platformView.OnPressesCancelled = null;
            platformView.OnPressesChanged = null;
            platformView.OnPressesEnded = null;
            platformView.OnShouldUpdateFocus = null;
            platformView.OnDidUpdateFocus = null;
            platformView.OnResignFirstResponder = null;
            platformView.OnBecomeFirstResponder = null;
            platformView.OnHoverGestureRecognizer = null;
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            var platformView = GetPlatformView(Handler);
            if (platformView is null)
                return;

            platformView.OnPressesBegan = HandleMacPlatformPressesBegan;
            platformView.OnPressesCancelled = HandleMacPlatformPressesCancelled;
            platformView.OnPressesChanged = HandleMacPlatformPressesChanged;
            platformView.OnPressesEnded = HandleMacPlatformPressesEnded;
            platformView.OnShouldUpdateFocus = HandleMacPlatformShouldUpdateFocus;
            platformView.OnDidUpdateFocus = HandleMacPlatformDidUpdateFocus;
            platformView.OnResignFirstResponder = HandleMacPlatformResignFirstResponder;
            platformView.OnBecomeFirstResponder = HandleMacPlatformBecomeFirstResponder;
            platformView.OnHoverGestureRecognizer = HandleMacPlatformHoverGestureRecognizer;
        }

        /// <summary>
        /// Handles 'HoverGestureRecognizer' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="recognizer">Gesture recognizer.</param>
        protected virtual void HandleMacPlatformHoverGestureRecognizer(
            PlatformView sender,
            UIHoverGestureRecognizer recognizer)
        {
            App.DebugLogIf($"HoverGestureRecognizer: {recognizer.State}", false);
            if (Control is null)
                return;

            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Possible:
                    break;
                case UIGestureRecognizerState.Began:
                    Control.RaiseMouseEnter(EventArgs.Empty);
                    break;
                case UIGestureRecognizerState.Changed:
                    long timestamp = DateUtils.GetNowInMilliseconds();
                    var locationInView = recognizer.LocationInView(sender);
                    PointD position = locationInView.ToSKPoint();
                    AbstractControl.BubbleMouseMove(
                                Control,
                                timestamp,
                                position,
                                out _);
                    break;
                case UIGestureRecognizerState.Ended:
                    Control.RaiseMouseLeave(EventArgs.Empty);
                    break;
                case UIGestureRecognizerState.Cancelled:
                    break;
                case UIGestureRecognizerState.Failed:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles 'ResignFirstResponder' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        protected virtual void HandleMacPlatformResignFirstResponder(PlatformView sender)
        {
            App.DebugLogIf($"ResignFirstResponder", false);
            Control?.RaiseLostFocus(LostFocusEventArgs.Empty);
        }

        /// <summary>
        /// Handles 'BecomeFirstResponder' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        protected virtual void HandleMacPlatformBecomeFirstResponder(PlatformView sender)
        {
            App.DebugLogIf($"BecomeFirstResponder", false);
            Control?.RaiseGotFocus(GotFocusEventArgs.Empty);
        }

        /// <summary>
        /// Handles 'ShouldUpdateFocus' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="context">Event context.</param>
        protected virtual bool HandleMacPlatformShouldUpdateFocus(
            PlatformView sender,
            UIFocusUpdateContext context)
        {
            return true;
        }

        /// <summary>
        /// Handles 'DidUpdateFocus' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="context">Event context.</param>
        protected virtual void HandleMacPlatformDidUpdateFocus(
            PlatformView sender,
            UIFocusUpdateContext context)
        {
            App.DebugLogIf($"DidUpdateFocus", false);
            if (context.NextFocusedView == sender)
            {
                Control?.RaiseGotFocus(GotFocusEventArgs.Empty);
            }
            else
            if (context.PreviouslyFocusedView == sender)
            {
                Control?.RaiseLostFocus(LostFocusEventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles 'PressesEnded' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesEnded(
            PlatformView sender,
            PlatformView.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesEnded: {e}", false);
        }

        /// <summary>
        /// Handles 'PressesChanged' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesChanged(
            PlatformView sender,
            PlatformView.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesChanged: {e}", false);
        }

        /// <summary>
        /// Handles 'PressesCancelled' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesCancelled(
            PlatformView sender,
            PlatformView.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesCancelled: {e}", false);
            RaiseUpOrDown(sender, e, true);
        }

        /// <summary>
        /// Handles 'PressesBegan' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesBegan(
            PlatformView sender,
            PlatformView.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesBegan: {e}", false);
            RaiseUpOrDown(sender, e, false);
        }

        private void RaiseUpOrDown(
            PlatformView sender,
            PlatformView.PressesEventArgs e,
            bool raiseUpEvent)
        {
            if (Control is null)
                return;

            KeyStates keyStates = raiseUpEvent ? KeyStates.None : KeyStates.Down;

            foreach (var press in e.Presses)
            {
                var evt = MauiKeyboardHandler.Default.ToKeyEventArgs(Control, press, keyStates);
                if (evt is null)
                    return;
                Control.BubbleKeyUpOrDown(evt, raiseUpEvent);

                if (evt.Handled)
                    e.Handled = true;
                else
                if(!raiseUpEvent)
                {
                    var evtKeyPresses = MauiKeyboardHandler.Default.ToKeyPressEventArgs(Control, press);
                    if (evtKeyPresses is null)
                        continue;
                    foreach(var item in evtKeyPresses)
                    {
                        Control.BubbleKeyPress(item);
                        if (item.Handled)
                            e.Handled = true;
                    }
                }
            }
        }
    }
}
#endif
