using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;


#if ANDROID

using Android.App;
using Android.Graphics;
using Android.Views;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui;
using Microsoft.Maui.Platform;

namespace Alternet.Maui
{
    /*
        Newer APIs allow WindowInsets and IME insets; the GlobalLayout approach works broadly and is simple.
        If you need more accurate insets on Android 11+, consider WindowInsetsCompat/OnApplyWindowInsets.    
    */

    public partial class KeyboardVisibilityService : DisposableObject, IKeyboardVisibilityService, IDisposable
    {
        /// <inheritdoc/>
        public event EventHandler<KeyboardVisibleChangedEventArgs>? KeyboardVisibleChanged;

        /// <inheritdoc/>
        public bool IsVisible { get; private set; }

        /// <inheritdoc/>
        public double Height { get; private set; }

        private View? rootView;
        private ViewTreeObserver.IOnGlobalLayoutListener? layoutListener;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardVisibilityService"/> class.
        /// </summary>
        public KeyboardVisibilityService()
        {
            var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            if (activity == null) return;
            
            rootView = activity.Window?.DecorView.FindViewById(Android.Resource.Id.Content);

            if (rootView == null) return;

            layoutListener = new GlobalLayoutListener(this, rootView);
            rootView?.ViewTreeObserver?.AddOnGlobalLayoutListener(layoutListener);
        }

        /// <summary>
        /// Raises the <see cref="KeyboardVisibleChanged"/> event with the specified event arguments.
        /// </summary>
        /// <param name="e">The event arguments. Optional. If not specified,
        /// defaults to the current keyboard visibility state.</param>
        public virtual void RaiseKeyboardVisibleChanged(KeyboardVisibleChangedEventArgs? e = null)
        {
            KeyboardVisibleChanged?.Invoke(this, e ?? new KeyboardVisibleChangedEventArgs(IsVisible, Height));
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (rootView != null && layoutListener != null)
            {
                rootView.ViewTreeObserver?.RemoveOnGlobalLayoutListener(layoutListener);
                layoutListener = null;
                rootView = null;
            }

            base.DisposeManaged();
        }

        private class GlobalLayoutListener : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
        {
            private readonly KeyboardVisibilityService owner;
            private readonly View? view;
            private int lastHeight = 0;

            public GlobalLayoutListener(KeyboardVisibilityService owner, View? view) { this.owner = owner; this.view = view; }

            public int GetKeyboardHeight()
            {
                var activity = Platform.CurrentActivity;

                if (activity is null)
                    return 0;

                var rootView = activity.Window?.DecorView?.RootView;

                if (rootView is null)
                    return 0;

                var visibleRect = new Rect();
                rootView.GetWindowVisibleDisplayFrame(visibleRect);

                int screenHeight = rootView.Height;
                int visibleHeight = visibleRect.Height();

                int heightDiff = screenHeight - visibleHeight;

                // Heuristic: only treat as keyboard if diff is significant
                // Adjust threshold as needed
                if (heightDiff > screenHeight * 0.15)
                {
                    // Subtract navigation bar height if needed
                    int navBarHeight = 0;
                    var resourceId = activity.Resources?.GetIdentifier("navigation_bar_height", "dimen", "android");
                    if (resourceId > 0)
                        navBarHeight = activity.Resources?.GetDimensionPixelSize(resourceId.Value) ?? 0;

                    // Only subtract if navigation bar is at bottom (portrait)
                    if (heightDiff > navBarHeight)
                        heightDiff -= navBarHeight;

                    return heightDiff;
                }

                // Keyboard not visible
                return 0;
            }

            public bool IsKeyboardVisible()
            {
                return GetKeyboardHeight() > 0;
            }

            public void OnGlobalLayout()
            {
                owner.Height = GetKeyboardHeight();
                owner.IsVisible = owner.Height > 0;
                owner.RaiseKeyboardVisibleChanged();
            }

            public void OnGlobalLayoutOld()
            {
                if (view == null) return;
                var r = new Rect();
                view.GetWindowVisibleDisplayFrame(r);
                int visibleHeight = r.Height();
                int totalHeight = view.RootView?.Height ?? 0;
                int heightDiff = totalHeight - visibleHeight;

                // threshold — treat as keyboard if more than 100px (or 1/4 of screen)
                int threshold = (int)(totalHeight * 0.15);
                bool isVisible = heightDiff > Math.Max(100, threshold);

                if (isVisible)
                {
                    if (heightDiff != lastHeight)
                    {
                        lastHeight = heightDiff;
                        // convert pixels to DIPs
                        double density = view.Context?.Resources?.DisplayMetrics?.Density ?? 1;
                        double heightDips = heightDiff / density;
                        owner.IsVisible = true;
                        owner.Height = heightDips;
                        owner.RaiseKeyboardVisibleChanged(new KeyboardVisibleChangedEventArgs(true, heightDips));
                    }
                }
                else
                {
                    if (owner.IsVisible)
                    {
                        lastHeight = 0;
                        owner.IsVisible = false;
                        owner.Height = 0;
                        owner.RaiseKeyboardVisibleChanged(new KeyboardVisibleChangedEventArgs(false, 0));
                    }
                }
            }
        }
    }
}
#endif