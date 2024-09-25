using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID

using Android.Content;
using Android.Util;

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
    }
}
#endif
