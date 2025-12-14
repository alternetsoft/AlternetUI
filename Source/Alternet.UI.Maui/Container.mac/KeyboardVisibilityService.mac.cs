using System;
using System.Collections.Generic;
using System.Text;

#if IOS || MACCATALYST
using Foundation;
using UIKit;
using CoreGraphics;
#endif

using Microsoft.Maui;
using Microsoft.Maui.Platform;

using Alternet.UI;

namespace Alternet.Maui
{
#if IOS || MACCATALYST
    /// <summary>
    /// Provides functionality to monitor the visibility of the on-screen keyboard on iOS and Mac Catalyst platforms.
    /// </summary>
    public partial class KeyboardVisibilityService : DisposableObject, IKeyboardVisibilityService, IDisposable
    {
        private NSObject? showObserver;
        private NSObject? hideObserver;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardVisibilityService"/> class.
        /// </summary>
        public KeyboardVisibilityService()
        {
            showObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnWillShow);
            hideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnWillHide);
        }

        /// <inheritdoc/>
        public event EventHandler<KeyboardVisibleChangedEventArgs>? KeyboardVisibleChanged;

        /// <inheritdoc/>
        public bool IsVisible { get; private set; }

        /// <inheritdoc/>
        public double Height { get; private set; }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            if (showObserver != null) { NSNotificationCenter.DefaultCenter.RemoveObserver(showObserver); showObserver = null; }
            if (hideObserver != null) { NSNotificationCenter.DefaultCenter.RemoveObserver(hideObserver); hideObserver = null; }
            base.DisposeManaged();
        }

        private void OnWillShow(NSNotification n)
        {
            var frameValue = n.UserInfo?[UIKeyboard.FrameEndUserInfoKey] as NSValue;
            var rect = frameValue?.CGRectValue ?? CGRect.Empty;
            // iOS reports in points (which are DIPs for MAUI)
            var heightDips = rect.Height;
            IsVisible = true;
            Height = heightDips;
            KeyboardVisibleChanged?.Invoke(this, new KeyboardVisibleChangedEventArgs(true, heightDips));
        }

        private void OnWillHide(NSNotification n)
        {
            IsVisible = false;
            Height = 0;
            KeyboardVisibleChanged?.Invoke(this, new KeyboardVisibleChangedEventArgs(false, 0));
        }
    }
#endif
}