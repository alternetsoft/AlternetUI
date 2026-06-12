namespace Alternet.UI;

using Alternet.Drawing;

#if WINDOWS

using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;

using Windows.UI.ViewManagement;

/// <summary>
/// Contains static methods and properties related to the Windows platform.
/// </summary>
public static class MauiWindowsUtils
{
    private static EnumArray<InputSystemCursorShape, InputSystemCursor?> systemCursors = new();

    /// <summary>
    /// Determines whether the device is in tablet mode.
    /// </summary>
    /// <returns></returns>
    public static bool IsTabletMode()
    {
        try
        {
            var uiViewSettings = UIViewSettings.GetForCurrentView();
            return uiViewSettings.UserInteractionMode == UserInteractionMode.Touch;
        }
        catch
        {
            return false;
        }
    }

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
    /// Converts <see cref="CursorType"/> to <see cref="InputSystemCursorShape"/>.
    /// </summary>
    /// <param name="cursor">The cursor type to convert.</param>
    /// <returns>The corresponding <see cref="InputSystemCursorShape"/> or null if none.</returns>
    public static InputSystemCursorShape? FromAlternet(CursorType cursor)
    {
        switch (cursor)
        {
            default:
            case CursorType.None:
                return null;
            case CursorType.Arrow:
                return InputSystemCursorShape.Arrow;
            case CursorType.Cross:
                return InputSystemCursorShape.Cross;
            case CursorType.Hand:
                return InputSystemCursorShape.Hand;
            case CursorType.IBeam:
                return InputSystemCursorShape.IBeam;
            case CursorType.NoEntry:
                return InputSystemCursorShape.UniversalNo;
            case CursorType.SizeNESW:
                return InputSystemCursorShape.SizeNortheastSouthwest;
            case CursorType.SizeNS:
                return InputSystemCursorShape.SizeNorthSouth;
            case CursorType.SizeNWSE:
                return InputSystemCursorShape.SizeNorthwestSoutheast;
            case CursorType.SizeWE:
                return InputSystemCursorShape.SizeWestEast;
            case CursorType.Sizing:
                return InputSystemCursorShape.SizeAll;
            case CursorType.Wait:
                return InputSystemCursorShape.Wait;
        }
    }

    /// <summary>
    /// Gets the system cursor for the specified cursor type.
    /// If the cursor does not already exist, it is created.
    /// All continuous calls to this method with the same shape will return the same cursor instance.
    /// </summary>
    /// <param name="cursor">The type of the system cursor.</param>
    /// <returns>The system cursor for the specified type.</returns>
    public static InputSystemCursor? GetOrCreateSystemCursor(CursorType cursor)
    {
        var cursorShape = FromAlternet(cursor);
        return GetOrCreateSystemCursor(cursorShape);
    }

    /// <summary>
    /// Gets the system cursor for the specified shape.
    /// If the cursor does not already exist, it is created.
    /// All continuous calls to this method with the same shape will return the same cursor instance.
    /// </summary>
    /// <param name="shape">The shape of the system cursor.</param>
    /// <returns>The system cursor for the specified shape.</returns>
    public static InputSystemCursor? GetOrCreateSystemCursor(InputSystemCursorShape? shape)
    {
        if (shape is null)
            return null;

        if (systemCursors[shape.Value] == null)
        {
            systemCursors[shape.Value] = InputSystemCursor.Create(shape.Value);
        }

        return systemCursors[shape.Value]!;
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