using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.UI
{
    public partial class SkiaContainer
    {
        internal SkiaSharp.Views.Windows.SKXamlCanvas? GetPlatformView(IElementHandler? handler = null)
        {
            handler ??= Handler;
            var platformView = handler?.PlatformView as SkiaSharp.Views.Windows.SKXamlCanvas;
            return platformView;
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

            var platformView = GetPlatformView(args.OldHandler);
            if (platformView is null)
                return;
            platformView.PointerEntered -= PlatformView_PointerEntered;
            platformView.PointerExited -= PlatformView_PointerExited;
            platformView.PointerPressed -= PlatformView_PointerPressed;
            platformView.PointerWheelChanged -= PlatformView_PointerWheelChanged;
            platformView.PointerReleased -= PlatformView_PointerReleased;
            platformView.KeyDown -= PlatformView_KeyDown;
            platformView.KeyUp -= PlatformView_KeyUp;
            platformView.CharacterReceived -= PlatformView_CharacterReceived;
            platformView.GotFocus -= PlatformView_GotFocus;
            platformView.LostFocus -= PlatformView_LostFocus;
            platformView.DragEnter -= PlatformView_DragEnter;
            platformView.DragLeave -= PlatformView_DragLeave;
            platformView.DragOver -= PlatformView_DragOver;
            platformView.DragStarting -= PlatformView_DragStarting;
            platformView.Drop -= PlatformView_Drop;
            platformView.DropCompleted -= PlatformView_DropCompleted;
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

            platformView.PointerEntered += PlatformView_PointerEntered;
            platformView.PointerExited += PlatformView_PointerExited;
            platformView.PointerPressed += PlatformView_PointerPressed;
            platformView.PointerWheelChanged += PlatformView_PointerWheelChanged;
            platformView.PointerReleased += PlatformView_PointerReleased;
            platformView.KeyDown += PlatformView_KeyDown;
            platformView.KeyUp += PlatformView_KeyUp;
            platformView.CharacterReceived += PlatformView_CharacterReceived;
            platformView.GotFocus += PlatformView_GotFocus;
            platformView.LostFocus += PlatformView_LostFocus;
            platformView.DragEnter += PlatformView_DragEnter;
            platformView.DragLeave += PlatformView_DragLeave;
            platformView.DragOver += PlatformView_DragOver;
            platformView.DragStarting += PlatformView_DragStarting;
            platformView.Drop += PlatformView_Drop;
            platformView.DropCompleted += PlatformView_DropCompleted;
        }

        private void PlatformView_DropCompleted(
            Microsoft.UI.Xaml.UIElement sender,
            Microsoft.UI.Xaml.DropCompletedEventArgs args)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_DragStarting(
            Microsoft.UI.Xaml.UIElement sender,
            Microsoft.UI.Xaml.DragStartingEventArgs args)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_DragLeave(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_DragEnter(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_LostFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseLostFocus();
        }

        private void PlatformView_GotFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseGotFocus();
        }

        private void PlatformView_CharacterReceived(
            Microsoft.UI.Xaml.UIElement sender,
            Microsoft.UI.Xaml.Input.CharacterReceivedRoutedEventArgs e)
        {
            if (Control is null)
                return;
            var args = Alternet.UI.MauiKeyboardHandler.Convert(Control, e);
            Control.BubbleKeyPress(args);
            e.Handled = args.Handled;
        }

        private void PlatformView_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (Control is null)
                return;
            var args = Alternet.UI.MauiKeyboardHandler.Convert(Control, e);
            Control.BubbleKeyUp(args);
            e.Handled = args.Handled;
        }

        private void PlatformView_KeyDown(
            object sender,
            Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (Control is null)
                return;
            var args = Alternet.UI.MauiKeyboardHandler.Convert(Control, e);
            Control.BubbleKeyDown(args);
            e.Handled = args.Handled;
        }

        private void PlatformView_PointerReleased(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_PointerWheelChanged(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_PointerPressed(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
        }

        private void PlatformView_PointerExited(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseMouseLeave();
        }

        private void PlatformView_PointerEntered(
            object sender,
            Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (Control is null)
                return;
            Control.RaiseMouseEnter();
        }
    }
}
