namespace Alternet.UI;

using Alternet.Drawing;

#if WINDOWS

using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;

/// <summary>
/// Contains static methods and properties related to the Windows platform.
/// </summary>
public static class MauiWindowsUtils
{
    /// <summary>
    /// Converts <see cref="HoldingRoutedEventArgs"/> to <see cref="LongTapEventArgs"/>.
    /// </summary>
    /// <param name="platformView">View for which event is created.</param>
    /// <param name="e">Value to convert.</param>
    /// <returns></returns>
    public static LongTapEventArgs Convert(
        PlatformView platformView,
        HoldingRoutedEventArgs e)
    {
        var position = e.GetPosition(platformView);
        var device = e.PointerDeviceType;

        var touchDevice = ToTouchDeviceType(device);

        LongTapEventArgs result = new(touchDevice, new PointD((float)position.X, (float)position.Y));
        return result;
    }

    /// <summary>
    /// Converts <see cref="HoldingState"/> to <see cref="TouchAction"/>.
    /// </summary>
    /// <param name="state">Value to convert.</param>
    /// <returns></returns>
    public static TouchAction ToTouchAction(HoldingState state)
    {
        switch (state)
        {
            case HoldingState.Started:
                return TouchAction.Pressed;
            case HoldingState.Completed:
                return TouchAction.Released;
            case HoldingState.Canceled:
                return TouchAction.Cancelled;
            default:
                return TouchAction.Cancelled;
        }
    }

    /// <summary>
    /// Converts <see cref="PointerDeviceType"/> to <see cref="TouchDeviceType"/>.
    /// </summary>
    /// <param name="device">Value to convert.</param>
    /// <returns></returns>
    public static TouchDeviceType ToTouchDeviceType(PointerDeviceType device)
    {
        switch (device)
        {
            case PointerDeviceType.Touch:
                return TouchDeviceType.Touch;
            case PointerDeviceType.Pen:
                return TouchDeviceType.Pen;
            case PointerDeviceType.Mouse:
                return TouchDeviceType.Mouse;
            case PointerDeviceType.Touchpad:
                return TouchDeviceType.Touchpad;
            default:
                return TouchDeviceType.Touch;
        }
    }
}

#endif