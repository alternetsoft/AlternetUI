using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

#if WINDOWS

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
            platformView.PointerEntered -= HandleWinPlatformPointerEntered;
            platformView.PointerExited -= HandleWinPlatformPointerExited;
            platformView.PointerPressed -= HandleWinPlatformPointerPressed;
            platformView.PointerWheelChanged -= HandleWinPlatformPointerWheelChanged;
            platformView.PointerReleased -= HandleWinPlatformPointerReleased;
            platformView.KeyDown -= HandleWinPlatformKeyDown;
            platformView.KeyUp -= HandleWinPlatformKeyUp;
            platformView.CharacterReceived -= HandleWinPlatformCharacterReceived;
            platformView.GotFocus -= HandleWinPlatformGotFocus;
            platformView.LostFocus -= HandleWinPlatformLostFocus;
            platformView.DragEnter -= HandleWinPlatformDragEnter;
            platformView.DragLeave -= HandleWinPlatformDragLeave;
            platformView.DragOver -= HandleWinPlatformDragOver;
            platformView.DragStarting -= HandleWinPlatformDragStarting;
            platformView.Drop -= HandleWinPlatformDrop;
            platformView.DropCompleted -= HandleWinPlatformDropCompleted;
            platformView.Holding -= HandleWinPlatformHolding;
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            var platformView = GetPlatformView(Handler);
            if (platformView is null)
                return;

            platformView.AllowFocusOnInteraction = true;
            platformView.IsTabStop = true;

            /*
            platformView.IsHoldingEnabled = true;
            */

            platformView.PointerEntered += HandleWinPlatformPointerEntered;
            platformView.PointerExited += HandleWinPlatformPointerExited;
            platformView.PointerPressed += HandleWinPlatformPointerPressed;
            platformView.PointerWheelChanged += HandleWinPlatformPointerWheelChanged;
            platformView.PointerReleased += HandleWinPlatformPointerReleased;
            platformView.KeyDown += HandleWinPlatformKeyDown;
            platformView.KeyUp += HandleWinPlatformKeyUp;
            platformView.CharacterReceived += HandleWinPlatformCharacterReceived;
            platformView.GotFocus += HandleWinPlatformGotFocus;
            platformView.LostFocus += HandleWinPlatformLostFocus;
            platformView.DragEnter += HandleWinPlatformDragEnter;
            platformView.DragLeave += HandleWinPlatformDragLeave;
            platformView.DragOver += HandleWinPlatformDragOver;
            platformView.DragStarting += HandleWinPlatformDragStarting;
            platformView.Drop += HandleWinPlatformDrop;
            platformView.DropCompleted += HandleWinPlatformDropCompleted;
            platformView.Holding += HandleWinPlatformHolding;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformHolding(
            object sender,
            Microsoft.UI.Xaml.Input.HoldingRoutedEventArgs e)
        {
            /*
            We do not raise long tap event here as it doesn't work for mouse clicks
            and not it is raised via calculating time between MouseDown/Up events.

            var platformView = GetPlatformView();
            if (platformView is null)
                return;

            LongTapEventArgs eventArgs = MauiWindowsUtils.Convert(platformView, e);
            RaiseLongTap(eventArgs);
            e.Handled = eventArgs.Handled;
            */
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformDropCompleted(
            Microsoft.UI.Xaml.UIElement sender,
            Microsoft.UI.Xaml.DropCompletedEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformDrop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformDragStarting(
            Microsoft.UI.Xaml.UIElement sender,
            Microsoft.UI.Xaml.DragStartingEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformDragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformDragLeave(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformDragEnter(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformLostFocus(
            object sender,
            Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseLostFocus(LostFocusEventArgs.Empty);
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformGotFocus(
            object sender,
            Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseGotFocus(GotFocusEventArgs.Empty);
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformCharacterReceived(
            Microsoft.UI.Xaml.UIElement sender,
            Microsoft.UI.Xaml.Input.CharacterReceivedRoutedEventArgs e)
        {
            if (Control is null)
                return;
            var args = Alternet.UI.MauiKeyboardHandler.Default.Convert(Control, e);
            Control.BubbleKeyPress(args);
            e.Handled = args.Handled;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformKeyUp(
            object sender,
            Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (Control is null)
                return;
            var args = Alternet.UI.MauiKeyboardHandler.Default.Convert(Control, e);
            Control.BubbleKeyUp(args);
            e.Handled = args.Handled;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformKeyDown(
            object sender,
            Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (Control is null)
                return;
            var args = Alternet.UI.MauiKeyboardHandler.Default.Convert(Control, e);
            Control.BubbleKeyDown(args);
            e.Handled = args.Handled;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformPointerReleased(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformPointerWheelChanged(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformPointerPressed(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformPointerExited(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseMouseLeave(EventArgs.Empty);
        }

        /// <summary>
        /// Handles platform event.
        /// </summary>
        /// <param name="sender">Platform view.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void HandleWinPlatformPointerEntered(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseMouseEnter(EventArgs.Empty);
        }
    }
}

#endif