using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

#if ANDROID

using Android.Views;

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

            platformView.NotifyKeyMultiple = null;
            platformView.NotifyKeyDown = null;
            platformView.NotifyKeyUp = null;
            platformView.NotifyKeyLongPress = null;
            platformView.NotifyFocusChanged = null;
            platformView.KeyPress -= HandleAndroidPlatformKeyPress;
            platformView.UnhandledKeyEvent -= HandleAndroidPlatformUnhandledKeyEvent;
            platformView.CapturedPointer -= HandleAndroidPlatformCapturedPointer;
            platformView.GenericMotion -= HandleAndroidPlatformGenericMotion;
            platformView.LongClick -= HandleAndroidPlatformLongClick;
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            var platformView = GetPlatformView(Handler);
            if (platformView is null)
                return;

            platformView.NotifyKeyMultiple = HandleAndroidPlatformKeyMultiple;
            platformView.NotifyKeyDown = HandleAndroidPlatformKeyDown;
            platformView.NotifyKeyUp = HandleAndroidPlatformKeyUp;
            platformView.NotifyKeyLongPress = HandleAndroidPlatformKeyLongPress;
            platformView.NotifyFocusChanged = HandleAndroidPlatformFocusChanged;
            platformView.KeyPress += HandleAndroidPlatformKeyPress;
            platformView.UnhandledKeyEvent += HandleAndroidPlatformUnhandledKeyEvent;
            platformView.CapturedPointer += HandleAndroidPlatformCapturedPointer;
            platformView.GenericMotion += HandleAndroidPlatformGenericMotion;
            platformView.LongClick += HandleAndroidPlatformLongClick;
        }

        protected virtual void HandleAndroidPlatformLongClick(
            object? sender,
            Android.Views.View.LongClickEventArgs e)
        {
            /*

            Long tap events are not called from here as they are handlerd by PlessMouse.StartLongTapTimer.

            if (Control is null)
                return;

            var position = PlessMouse.LastMousePosition;
            if (position.Control != Control || position.Position is null)
                return;

            LongTapEventArgs args = new(
                TouchAction.Released,
                TouchDeviceType.Touch,
                position.Position.Value);

            RaiseLongTap(args);
            */
        }

        protected virtual void HandleAndroidPlatformGenericMotion(
            object? sender,
            Android.Views.View.GenericMotionEventArgs e)
        {
            if (Control is null || e.Event is null)
                return;

            if (!e.Event.IsFromSource(InputSourceType.ClassPointer))
                return;

            PointD GetEventPoint(MotionEvent motionEvent)
            {
                var x = motionEvent.GetX();
                var y = motionEvent.GetY();
                PointI pt = ((int)x, (int)y);
                PointD position = GraphicsFactory.PixelToDip(pt, Control!.ScaleFactor);
                return position;
            }

            Debug.WriteLineIf(false, $"HandleGenericMotion: {e.Event.Action}");

            switch (e.Event.Action)
            {
                case MotionEventActions.HoverEnter:
                    Debug.WriteLineIf(false, "Mouse hover enter");
                    Control.RaiseMouseEnter(EventArgs.Empty);
                    break;
                case MotionEventActions.HoverExit:
                    Debug.WriteLineIf(false, "Mouse hover exit");
                    Control.RaiseMouseLeave(EventArgs.Empty);
                    break;
                case MotionEventActions.HoverMove:
                    long timestamp = DateUtils.GetNowInMilliseconds();
                    AbstractControl.BubbleMouseMove(
                                Control,
                                timestamp,
                                GetEventPoint(e.Event),
                                out _);
                    break;
                case MotionEventActions.Scroll:
                    var scrollX = (int)e.Event.GetAxisValue(Axis.Hscroll);
                    var scrollY = (int)e.Event.GetAxisValue(Axis.Vscroll);
                    Debug.WriteLineIf(false, "Mouse scrolled " + scrollX + ", " + scrollY);
                    TouchEventArgs touchArgs = new();
                    touchArgs.DeviceType = TouchDeviceType.Mouse;
                    touchArgs.Location = GetEventPoint(e.Event);
                    touchArgs.ActionType = TouchAction.WheelChanged;
                    touchArgs.WheelDelta = scrollX == 0 ? scrollY : scrollX;
                    Control?.RaiseTouch(touchArgs);
                    break;
            }
        }

        protected virtual void HandleAndroidPlatformCapturedPointer(
            object? sender,
            Android.Views.View.CapturedPointerEventArgs e)
        {
        }

        protected virtual void HandleAndroidPlatformUnhandledKeyEvent(
            object? sender,
            Android.Views.View.UnhandledKeyEventEventArgs e)
        {
            e.Handled = false;
        }

        protected virtual void HandleAndroidPlatformKeyPress(
            object? sender,
            Android.Views.View.KeyEventArgs e)
        {
            e.Handled = false;
        }

        protected virtual void HandleAndroidPlatformFocusChanged(PlatformView sender, bool gainFocus)
        {
            if (gainFocus)
                Control?.RaiseGotFocus(GotFocusEventArgs.Empty);
            else
                Control?.RaiseLostFocus(LostFocusEventArgs.Empty);
        }

        /// <summary>
        /// Handles 'OnKeyLongPress' event on the Android platform.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="keyCode">Key code.</param>
        /// <param name="e">Event parameters.</param>
        /// <returns></returns>
        protected virtual bool HandleAndroidPlatformKeyLongPress(
            PlatformView sender,
            Keycode keyCode,
            KeyEvent? e)
        {
            return false;
        }

        /// <summary>
        /// Handles 'OnKeyUp' event on the Android platform.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="keyCode">Key code.</param>
        /// <param name="e">Event parameters.</param>
        /// <returns></returns>
        protected virtual bool HandleAndroidPlatformKeyUp(
            PlatformView sender,
            Keycode keyCode,
            KeyEvent? e)
        {
            return RaiseUpOrDown(sender, keyCode, e, true);
        }

        /// <summary>
        /// Handles 'OnKeyDown' event on the Android platform.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="keyCode">Key code.</param>
        /// <param name="e">Event parameters.</param>
        /// <returns></returns>
        protected virtual bool HandleAndroidPlatformKeyDown(
            PlatformView sender,
            Keycode keyCode,
            KeyEvent? e)
        {
            return RaiseUpOrDown(sender, keyCode, e, false);
        }

        /// <summary>
        /// Handles 'OnKeyMultiple' event on the Android platform.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="keyCode">Key code.</param>
        /// <param name="repeatCount">Key repeat count.</param>
        /// <param name="e">Event parameters.</param>
        /// <returns></returns>
        protected virtual bool HandleAndroidPlatformKeyMultiple(
            PlatformView sender,
            Keycode keyCode,
            int repeatCount,
            KeyEvent? e)
        {
            if (Control is null || e is null)
                return false;

            if (keyCode == Keycode.Unknown)
            {
                var ch = e?.Characters;
                if (ch is null || ch.Length == 0)
                    return false;

                var handled = false;

                for(int i = 0; i < ch.Length; i++)
                {
                    var keyPressEvt = new KeyPressEventArgs(Control, ch[i]);
                    Control.BubbleKeyPress(keyPressEvt);
                    if (keyPressEvt.Handled)
                        handled = true;
                }

                return handled;
            }

            return RaiseUpOrDown(sender, keyCode, e, false);
        }

        private bool RaiseUpOrDown(
            PlatformView sender,
            Keycode keyCode,
            KeyEvent? e,
            bool raiseUpEvent)
        {
            if (Control is null || e is null)
                return false;

            KeyStates keyStates = raiseUpEvent ? KeyStates.None : KeyStates.Down;

            bool handled = false;

            if(keyCode != Keycode.Unknown)
            {
                var evt = MauiKeyboardHandler.Default.ToKeyEventArgs(Control, keyStates, keyCode, e);
                Control.BubbleKeyUpOrDown(evt, raiseUpEvent);
                handled = evt.Handled;
            }

            if (handled)
                return true;
            else
            {
                if (!raiseUpEvent)
                {
                    char ch = (char)e.UnicodeChar;
                    if (ch == 0)
                        return false;
                    var keyPressEvt = new KeyPressEventArgs(Control, ch);
                    Control.BubbleKeyPress(keyPressEvt);
                    if (keyPressEvt.Handled)
                        return true;
                }

                return false;
            }
        }
    }
}
#endif
