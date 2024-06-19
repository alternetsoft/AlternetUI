using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class SkiaContainer
    {
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
