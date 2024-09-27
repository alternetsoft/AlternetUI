using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if IOS || MACCATALYST
using Foundation;

using SkiaSharp;

using UIKit;

namespace Alternet.UI
{
    /// <summary>
    /// Adds additional functionality to the <see cref="SkiaSharp.Views.iOS.SKCanvasView"/> control.
    /// </summary>
    public partial class PlatformView : SkiaSharp.Views.iOS.SKCanvasView
    {
        /// <summary>
        /// Raised when <see cref="PressesBegan"/> is called.
        /// </summary>
        public Action<PlatformView, PressesEventArgs>? OnPressesBegan;

        /// <summary>
        /// Raised when <see cref="PressesCancelled"/> is called.
        /// </summary>
        public Action<PlatformView, PressesEventArgs>? OnPressesCancelled;

        /// <summary>
        /// Raised when <see cref="UIHoverGestureRecognizer"/> state is changed.
        /// </summary>
        public Action<PlatformView, UIHoverGestureRecognizer>? OnHoverGestureRecognizer;

        /// <summary>
        /// Raised when <see cref="ShouldUpdateFocus"/> is called.
        /// </summary>
        public Func<PlatformView, UIFocusUpdateContext, bool>? OnShouldUpdateFocus;

        /// <summary>
        /// Raised when <see cref="DidUpdateFocus"/> is called.
        /// </summary>
        public Action<PlatformView, UIFocusUpdateContext>? OnDidUpdateFocus;

        /// <summary>
        /// Raised when <see cref="PressesChanged"/> is called.
        /// </summary>
        public Action<PlatformView, PressesEventArgs>? OnPressesChanged;

        /// <summary>
        /// Raised when <see cref="BecomeFirstResponder"/> is called.
        /// </summary>
        public Action<PlatformView>? OnBecomeFirstResponder;

        /// <summary>
        /// Raised when <see cref="ResignFirstResponder"/> is called.
        /// </summary>
        public Action<PlatformView>? OnResignFirstResponder;

        /// <summary>
        /// Raised when <see cref="PressesEnded"/> is called.
        /// </summary>
        public Action<PlatformView, PressesEventArgs>? OnPressesEnded;

        private static readonly Lazy<bool> isValidEnvironment = new(() =>
        {
            try
            {
                SKPMColor.PreMultiply(SKColors.Black);
                return true;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        });

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformView"/> class.
        /// </summary>
        /// <param name="handle"></param>
        public PlatformView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformView"/> class.
        /// </summary>
        public PlatformView()
        {
            Initialize();
        }

        /// <inheritdoc/>
        public override bool CanResignFirstResponder => true;

        /// <inheritdoc/>
        public override bool CanBecomeFirstResponder => true;

        /// <inheritdoc/>
        public override bool CanBecomeFocused
        {
            get
            {
                return true;
            }
        }

        internal static bool IsValidEnvironment => isValidEnvironment.Value;

        /// <inheritdoc/>
        public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            CallAction(presses, evt, OnPressesBegan, base.PressesBegan);
        }

        /// <inheritdoc/>
        public override bool BecomeFirstResponder()
        {
            return base.BecomeFirstResponder();
        }

        /// <inheritdoc/>
        public override bool ResignFirstResponder()
        {
            return base.ResignFirstResponder();
        }

        /// <inheritdoc/>
        public override void DidUpdateFocus(
            UIFocusUpdateContext context,
            UIFocusAnimationCoordinator coordinator)
        {
            if (OnDidUpdateFocus is not null)
            {
                OnDidUpdateFocus(this, context);
            }

            base.DidUpdateFocus(context, coordinator);
        }

        /// <inheritdoc/>
        public override bool ShouldUpdateFocus(UIFocusUpdateContext context)
        {
            if (OnShouldUpdateFocus is not null)
            {
                return OnShouldUpdateFocus(this, context);
            }

            return true;
        }

        /// <inheritdoc/>
        public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            CallAction(presses, evt, OnPressesCancelled, base.PressesCancelled);
        }

        /// <inheritdoc/>
        public override void PressesChanged(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            CallAction(presses, evt, OnPressesChanged, base.PressesChanged);
        }

        /// <inheritdoc/>
        public override void PressesEnded(NSSet<UIPress> presses, UIPressesEvent evt)
        {
            CallAction(presses, evt, OnPressesEnded, base.PressesEnded);
        }

        /// <summary>
        /// Common initialization method which is called from the constructors.
        /// </summary>
        protected virtual void Initialize()
        {
            var recognizer = new UIHoverGestureRecognizer((g) =>
            {
                OnHoverGestureRecognizer?.Invoke(this, g);
            });

            AddGestureRecognizer(recognizer);
        }

        private void CallAction(
            NSSet<UIPress> presses,
            UIPressesEvent evt,
            Action<PlatformView, PressesEventArgs>? action,
            Action<NSSet<UIPress>, UIPressesEvent> baseAction)
        {
            var handled = false;

            if (action is not null)
            {
                PressesEventArgs e = new(presses, evt);
                action(this, e);
                handled = e.Handled;
            }

            if (!handled)
            {
                baseAction(presses, evt);
            }
        }

        /// <summary>
        /// Contains presses information.
        /// </summary>
        public class PressesEventArgs : HandledEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="PressesEventArgs"/> class.
            /// </summary>
            /// <param name="presses">Presses information.</param>
            /// <param name="evt"></param>
            public PressesEventArgs(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                Presses = presses;
                Event = evt;
            }

            /// <summary>
            /// Gets or sets presses information.
            /// </summary>
            public NSSet<UIPress> Presses { get; set; }

            /// <summary>
            /// Gets or sets event object.
            /// </summary>
            public UIPressesEvent Event { get; set; }

            /// <inheritdoc/>
            public override string? ToString()
            {
                string? result = null;

                foreach (UIPress press in Presses)
                {
                    if (press.Key is null)
                        continue;
                    var keyCode = press.Key.KeyCode;
                    var modifiers = press.Key.ModifierFlags;
                    var s = $"{keyCode}, modifiers: {modifiers}, chars: <{press.Key.Characters}>";

                    if (result is null)
                        result = s;
                    else
                        result = $"{result},{s}";
                }

                if (result is not null)
                {
                    result = $"({result})";
                    return result;
                }

                return base.ToString();
            }
        }

        internal class UIHoverGestureRecognizerAdv : UIHoverGestureRecognizer
        {
            public UIHoverGestureRecognizerAdv(Action<UIHoverGestureRecognizer> action)
                : base(action)
            {
            }

            public override void PressesCancelled(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                base.PressesCancelled(presses, evt);
            }

            public override void PressesBegan(NSSet<UIPress> presses, UIPressesEvent evt)
            {
                App.LogIf("UIHoverGestureRecognizerAdv", false);
                base.PressesBegan(presses, evt);
            }
        }
    }
}
#endif
