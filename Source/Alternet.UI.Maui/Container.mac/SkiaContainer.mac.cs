using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

#if IOS || MACCATALYST

using UIKit;

namespace Alternet.UI
{
    public partial class SkiaContainer
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
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            var platformView = GetPlatformView(Handler);
            if (platformView is null)
                return;

            platformView.OnPressesBegan = HandlePressesBegan;
            platformView.OnPressesCancelled = HandlePressesCancelled;
            platformView.OnPressesChanged = HandlePressesChanged;
            platformView.OnPressesEnded = HandlePressesEnded;
            platformView.OnShouldUpdateFocus = HandleShouldUpdateFocus;
            platformView.OnDidUpdateFocus = HandleDidUpdateFocus;
        }

        /// <summary>
        /// Handles 'HandleShouldUpdateFocus' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="context">Event context.</param>
        protected virtual bool HandleShouldUpdateFocus(SKCanvasViewAdv sender, UIFocusUpdateContext context)
        {
            return true;
        }

        /// <summary>
        /// Handles 'HandleDidUpdateFocus' event of the platform view on mac platform.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="context">Event context.</param>
        protected virtual void HandleDidUpdateFocus(SKCanvasViewAdv sender, UIFocusUpdateContext context)
        {
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
        protected virtual void HandlePressesEnded(
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
        protected virtual void HandlePressesChanged(
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
        protected virtual void HandlePressesCancelled(
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
        protected virtual void HandlePressesBegan(
            SKCanvasViewAdv sender,
            SKCanvasViewAdv.PressesEventArgs e)
        {
            App.DebugLogIf($"PressesBegan: {e}", true);
        }
    }
}
#endif
