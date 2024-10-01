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
            platformView.KeyPress -= HandleKeyPress;
            platformView.UnhandledKeyEvent -= HandleUnhandledKeyEvent;
            platformView.CapturedPointer -= HandleCapturedPointer;
            platformView.GenericMotion -= HandleGenericMotion;
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            var platformView = GetPlatformView(Handler);
            if (platformView is null)
                return;

            platformView.NotifyKeyMultiple = HandleKeyMultiple;
            platformView.NotifyKeyDown = HandleKeyDown;
            platformView.NotifyKeyUp = HandleKeyUp;
            platformView.NotifyKeyLongPress = HandleKeyLongPress;
            platformView.NotifyFocusChanged = HandleFocusChanged;
            platformView.KeyPress += HandleKeyPress;
            platformView.UnhandledKeyEvent += HandleUnhandledKeyEvent;
            platformView.CapturedPointer += HandleCapturedPointer;
            platformView.GenericMotion += HandleGenericMotion;
        }

        protected virtual void HandleGenericMotion(
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
                    Control.RaiseMouseEnter();
                    break;
                case MotionEventActions.HoverExit:
                    Debug.WriteLineIf(false, "Mouse hover exit");
                    Control.RaiseMouseLeave();
                    break;
                case MotionEventActions.HoverMove:
                    long timestamp = DateUtils.GetNowInMilliseconds();
                    Control.BubbleMouseMove(
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

        protected virtual void HandleCapturedPointer(
            object? sender,
            Android.Views.View.CapturedPointerEventArgs e)
        {
        }

        protected virtual void HandleUnhandledKeyEvent(
            object? sender,
            Android.Views.View.UnhandledKeyEventEventArgs e)
        {
            e.Handled = false;
        }

        protected virtual void HandleKeyPress(object? sender, Android.Views.View.KeyEventArgs e)
        {
            e.Handled = false;
        }

        protected virtual void HandleFocusChanged(PlatformView sender, bool gainFocus)
        {
            if (gainFocus)
                Control?.RaiseGotFocus();
            else
                Control?.RaiseLostFocus();
        }

        /// <summary>
        /// Handles 'OnKeyLongPress' event on the Android platform.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="keyCode">Key code.</param>
        /// <param name="e">Event parameters.</param>
        /// <returns></returns>
        protected virtual bool HandleKeyLongPress(
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
        protected virtual bool HandleKeyUp(
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
        protected virtual bool HandleKeyDown(
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
        protected virtual bool HandleKeyMultiple(
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
