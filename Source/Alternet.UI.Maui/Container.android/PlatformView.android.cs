using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID

using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;

using SkiaSharp;
using SkiaSharp.Views.Android;

namespace Alternet.UI
{
    /// <summary>
    /// Adds additional functionality to the <see cref="SKCanvasView"/> control.
    /// </summary>
    public partial class PlatformView : SKCanvasView
    {
        public PlatformView(Context context)
            : base(context)
        {
        }

        public PlatformView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public PlatformView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            App.DebugLogIf($"OnKeyDown", true);
            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            App.DebugLogIf($"OnKeyUp", true);
            return base.OnKeyUp(keyCode, e);
        }

        public override bool OnKeyLongPress([GeneratedEnum] Keycode keyCode, KeyEvent? e)
        {
            App.DebugLogIf($"OnKeyLongPress", true);
            return base.OnKeyLongPress(keyCode, e);
        }
    }
}
#endif
