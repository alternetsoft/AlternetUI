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
            SKCanvasViewAdv sender,
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
                    Control.RaiseMouseEnter();
                    break;
                case UIGestureRecognizerState.Changed:
                    long timestamp = DateUtils.GetNowInMilliseconds();
                    var locationInView = recognizer.LocationInView(sender);
                    PointD position = locationInView.ToSKPoint();
                    Control.BubbleMouseMove(
                                Control,
                                timestamp,
                                position,
                                out _);
                    break;
                case UIGestureRecognizerState.Ended:
                    Control.RaiseMouseLeave();
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
        protected virtual void HandleMacPlatformResignFirstResponder(SKCanvasViewAdv sender)
        {
            App.DebugLogIf($"ResignFirstResponder", false);
            Control?.RaiseLostFocus();
        }

        /// <summary>
        /// Handles 'BecomeFirstResponder' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        protected virtual void HandleMacPlatformBecomeFirstResponder(SKCanvasViewAdv sender)
        {
            App.DebugLogIf($"BecomeFirstResponder", false);
            Control?.RaiseGotFocus();
        }

        /// <summary>
        /// Handles 'ShouldUpdateFocus' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="context">Event context.</param>
        protected virtual bool HandleMacPlatformShouldUpdateFocus(
            SKCanvasViewAdv sender,
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
            SKCanvasViewAdv sender,
            UIFocusUpdateContext context)
        {
            App.DebugLogIf($"DidUpdateFocus", false);
            if (context.NextFocusedView == sender)
            {
                Control?.RaiseGotFocus();
            }
            else
            if (context.PreviouslyFocusedView == sender)
            {
                Control?.RaiseLostFocus();
            }
        }

        /// <summary>
        /// Handles 'PressesEnded' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesEnded(
            SKCanvasViewAdv sender,
            SKCanvasViewAdv.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesEnded: {e}", true);
        }

        /// <summary>
        /// Handles 'PressesChanged' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesChanged(
            SKCanvasViewAdv sender,
            SKCanvasViewAdv.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesChanged: {e}", true);
        }

        /// <summary>
        /// Handles 'PressesCancelled' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesCancelled(
            SKCanvasViewAdv sender,
            SKCanvasViewAdv.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesCancelled: {e}", true);
        }

        /// <summary>
        /// Handles 'PressesBegan' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleMacPlatformPressesBegan(
            SKCanvasViewAdv sender,
            SKCanvasViewAdv.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesBegan: {e}", true);
        }
    }
}
#endif
