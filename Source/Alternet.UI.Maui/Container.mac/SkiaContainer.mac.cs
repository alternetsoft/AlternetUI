using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.UI
{
#if MACCATALYST
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
        }

        private void HandlePressesEnded(SKCanvasViewAdv sender, SKCanvasViewAdv.PressesEventArgs e)
        {
            LogUtils.DebugLogToFileIf($"PressesEnded: {e}", true);
        }

        private void HandlePressesChanged(SKCanvasViewAdv sender, SKCanvasViewAdv.PressesEventArgs e)
        {
            LogUtils.DebugLogToFileIf($"PressesChanged: {e}", true);
        }

        private void HandlePressesCancelled(SKCanvasViewAdv sender, SKCanvasViewAdv.PressesEventArgs e)
        {
            LogUtils.DebugLogToFileIf($"PressesCancelled: {e}", true);
        }

        private void HandlePressesBegan(SKCanvasViewAdv sender, SKCanvasViewAdv.PressesEventArgs e)
        {
            LogUtils.DebugLogToFileIf($"PressesBegan: {e}", true);
        }
    }
#endif
}
