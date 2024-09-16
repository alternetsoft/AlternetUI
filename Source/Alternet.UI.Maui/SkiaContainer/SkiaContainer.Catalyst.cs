using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.UI
{
    public partial class SkiaContainer
    {
#if MACCATALYST
        internal SkiaSharp.Views.iOS.SKCanvasView? GetPlatformView(IElementHandler? handler = null)
        {
            handler ??= Handler;
            var platformView = handler?.PlatformView as SkiaSharp.Views.iOS.SKCanvasView;
            return platformView;
        }
#endif

#if MACCATALYST
        /// <inheritdoc/>
        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

            var platformView = GetPlatformView(args.OldHandler);
            if (platformView is null)
                return;
        }
#endif

#if MACCATALYST
        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            var platformView = GetPlatformView(Handler);
            if (platformView is null)
                return;
        }
#endif
}
}
