namespace Alternet.UI;

using Microsoft.Maui.Devices;
using Microsoft.Maui.Platform;

using Alternet.Drawing;

#if ANDROID
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Views;

public static class AndroidUtils
{
    public static Android.Views.View? GetDecorView(Android.Views.View view)
    {
        if (view == null || view.Context?.GetActivity() is not Activity nativeActivity)
            return null;

        var decorView = nativeActivity.Window?.DecorView;

        return decorView;
    }

    public static RectI ToRectI(this Android.Graphics.Rect rect)
    {
        return new(rect.Left, rect.Top, rect.Right, rect.Bottom);
    }

    public static PointI GetLocationInWindow(PlatformView view)
    {
        int[] outLocation = new int[2];
        view.GetLocationInWindow(outLocation);
        return (outLocation[0], outLocation[1]);
    }

    public static PointI GetLocationOnScreen(PlatformView view)
    {
        int[] outLocation = new int[2];
        view.GetLocationOnScreen(outLocation);
        return (outLocation[0], outLocation[1]);
    }

    public static RectI GetDrawingRect(Android.Views.View view)
    {
        Rect rect = new();
        view.GetDrawingRect(rect);
        RectI result = rect.ToRectI();
        return result;
    }

    public static RectI? GetDecorViewVisibleDisplayFrame(Android.Views.View view)
    {
        var decorView = GetDecorView(view);
        if (decorView is null)
            return null;
        var result = GetWindowVisibleDisplayFrame(decorView);
        return result;
    }

    public static RectI GetWindowVisibleDisplayFrame(Android.Views.View view)
    {
        Rect visibleRect = new();
        view.GetWindowVisibleDisplayFrame(visibleRect);
        RectI rect = visibleRect.ToRectI();
        return rect;
    }

    public static Android.App.Activity? GetActivity(this Context context)
    {
        if (context == null)
            return null;

        if (context is Android.App.Activity activity)
            return activity;

        if (context is ContextWrapper contextWrapper)
            return contextWrapper.BaseContext?.GetActivity();

        return null;
    }

    public static void UpdateWindowSoftInputModeAdjust(Android.App.Activity? activity, SoftInput inputMode)
    {
        activity?.Window?.SetSoftInputMode(inputMode);
    }
}

/*

Rect rectangle = new Rect();
Window window = getWindow();
window.getDecorView().getWindowVisibleDisplayFrame(rectangle);
int statusBarHeight = rectangle.top;
int contentViewTop =
window.findViewById(Window.ID_ANDROID_CONTENT).getTop();
int titleBarHeight = contentViewTop - statusBarHeight;
*/

#endif