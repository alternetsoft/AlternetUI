using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.Diagnostics;


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

            Debug.WriteLine($"HandleGenericMotion: {e.Event.Action}");

            switch (e.Event.Action)
            {
                case MotionEventActions.HoverEnter:
                    Debug.WriteLine("Mouse hover enter");
                    Control.RaiseMouseEnter();
                    break;
                case MotionEventActions.HoverExit:
                    Debug.WriteLine("Mouse hover exit");
                    Control.RaiseMouseLeave();
                    break;
                case MotionEventActions.HoverMove:
                    var x = e.Event.GetX();
                    var y = e.Event.GetY();
                    long timestamp = DateUtils.GetNowInMilliseconds();
                    PointD position = (x, y);
                    Control.BubbleMouseMove(
                                Control,
                                timestamp,
                                position,
                                out _);
                    break;
                case MotionEventActions.Scroll:
                    var scrollX = e.Event.GetAxisValue(Axis.Hscroll);
                    var scrollY = e.Event.GetAxisValue(Axis.Vscroll);
                    Debug.WriteLine("Mouse scrolled " + scrollX + ", " + scrollY);
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
            return false;
        }

        private bool RaiseUpOrDown(
            PlatformView sender,
            Keycode keyCode,
            KeyEvent? e,
            bool raiseUpEvent)
        {
            if (Control is null)
                return false;

            KeyStates keyStates = raiseUpEvent ? KeyStates.None : KeyStates.Down;

            var evt = MauiKeyboardHandler.Default.ToKeyEventArgs(Control, keyStates, keyCode, e);
            if (evt is null)
                return false;

            Control.BubbleKeyUpOrDown(evt, raiseUpEvent);

            if (evt.Handled)
                return true;
            else
            {
                if (!raiseUpEvent)
                {
                }

                return false;
            }
        }
    }
}
#endif
