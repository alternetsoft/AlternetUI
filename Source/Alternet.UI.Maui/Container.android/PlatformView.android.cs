using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if ANDROID

using Android.Content;
using Android.Graphics;
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
        public Action<PlatformView, bool>? NotifyFocusChanged;

        public Func<PlatformView, Keycode, int, KeyEvent?, bool>? NotifyKeyMultiple;

        public Func<PlatformView, Keycode, KeyEvent?, bool>? NotifyKeyDown;

        public Func<PlatformView, Keycode, KeyEvent?, bool>? NotifyKeyUp;

        public Func<PlatformView, Keycode, KeyEvent?, bool>? NotifyKeyLongPress;

        public PlatformView(Context context)
            : base(context)
        {
            Initialize();
        }

        public PlatformView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public PlatformView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public override bool IsInEditMode => base.IsInEditMode;

        public override bool IsInTouchMode => base.IsInTouchMode;

        public override bool OnKeyMultiple(Keycode keyCode, int count, KeyEvent? e)
        {
            LogEvent("OnKeyMultiple", keyCode, count, e, true);

            if (NotifyKeyMultiple is not null)
            {
                var result = NotifyKeyMultiple(this, keyCode, count, e);
                if (result)
                    return false;
            }

            return base.OnKeyMultiple(keyCode, count, e);
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent? e)
        {
            LogEvent("OnKeyDown", keyCode, 0, e, true);

            if (NotifyKeyDown is not null)
            {
                var result = NotifyKeyDown(this, keyCode, e);
                if (result)
                    return false;
            }

            return base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp(Keycode keyCode, KeyEvent? e)
        {
            LogEvent("OnKeyUp", keyCode, 0, e, true);
            if (NotifyKeyUp is not null)
            {
                var result = NotifyKeyUp(this, keyCode, e);
                if (result)
                    return false;
            }

            return base.OnKeyUp(keyCode, e);
        }

        public override bool OnCheckIsTextEditor()
        {
            return true;
        }

        public override bool OnKeyLongPress(Keycode keyCode, KeyEvent? e)
        {
            LogEvent("OnKeyLongPress", keyCode, 0, e, true);
            if (NotifyKeyLongPress is not null)
            {
                var result = NotifyKeyLongPress(this, keyCode, e);
                if (result)
                    return false;
            }

            return base.OnKeyLongPress(keyCode, e);
        }

        protected override void OnFocusChanged(
            bool gainFocus,
            [GeneratedEnum] FocusSearchDirection direction,
            Rect? previouslyFocusedRect)
        {
            NotifyFocusChanged?.Invoke(this, gainFocus);
            base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
        }

        protected virtual void Initialize()
        {
            Focusable = true;
            FocusableInTouchMode = true;
            Clickable = true;

            /*
            KeyboardNavigationCluster = true;
            */
        }

        [Conditional("DEBUG")]
        private void LogEvent(string name, Keycode keyCode, int count, KeyEvent? e, bool log)
        {
            App.DebugLogIf($"{name}: {keyCode}, {e?.Modifiers}", log);
        }
    }
}
#endif
