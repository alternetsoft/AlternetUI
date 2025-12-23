namespace Alternet.UI;

#if ANDROID

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alternet.Drawing;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Views;

using Microsoft.Maui;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Platform;

public static class AndroidUtils
{
    public static void DefaultMainActivityOnCreate(MauiAppCompatActivity activity, Bundle? savedInstanceState)
    {
        activity.Window?.SetSoftInputMode(SoftInput.AdjustResize);
    }

    /// <summary>
    /// Gets application's private storage folder.
    /// </summary>
    /// <returns></returns>
    public static string? GetPrivateStoragePath()
    {
        // Get the application context
        Context context = Android.App.Application.Context;

        // Get the private storage path
        var privateStoragePath = context?.FilesDir?.AbsolutePath;

        return privateStoragePath;
    }

    public static void LogDirs()
    {
        // Get the application context
        Context context = Android.App.Application.Context;

        // Return the primary external storage directory where this application's OBB files
        // (if there are any) can be found.
        LogDir("ObbDir", context.ObbDir?.AbsolutePath);

        // Returns the absolute path to the directory on the
        // filesystem similar to Android.Content.Context.FilesDir.
        LogDir("NoBackupFilesDir", context.NoBackupFilesDir?.AbsolutePath);

        // Returns the absolute path to the directory on the filesystem where files created
        // with Android.Content.Context.OpenFileOutput
        // (System.String,Android.Content.FileCreationMode)
        // are stored.
        LogDir("FilesDir", context.FilesDir?.AbsolutePath);

        var packagename = GetPackageName();

        // The.NET libraries are usually stored
        // in the app's private storage directory, which is located at
        // /data/data/[your.package.name]/files/. This directory contains the
        // app's files, including the .NET runtime and libraries.
        LogDir(
            "/data/data/[your.package.name]/files/",
            $"/data/data/{packagename}/files/");

        // Native Libraries: If your app uses native libraries, they are typically
        // located in the lib directory within the app's APK. When the app
        // is installed, these libraries are extracted
        // to the /data/app/[your.package.name]/lib/ directory.
        LogDir(
            "/data/data/[your.package.name]/lib/",
            $"/data/data/{packagename}/lib/");

        // Temporary Storage: Temporary files and libraries might
        // be stored in the app's cache directory,
        // located at /data/data/[your.package.name]/cache/.
        LogDir(
            "/data/data/[your.package.name]/cache/",
            $"/data/data/{packagename}/cache/");

        // External Storage: If your app has permissions to
        // access external storage, you can also store and access
        // libraries from there. However, this is less common
        // for .NET libraries and more for user-generated content or large data files.

        // Returns the absolute path to the directory on the primary external filesystem
        // (that is somewhere on Android.OS.Environment.ExternalStorageDirectory where the
        // application can place cache files it owns.
        LogDir("FilesDir", context.ExternalCacheDir?.AbsolutePath);

        void LogDir(string title, string? path)
        {
            Alternet.UI.App.LogBeginSection();

            if (path is null)
            {
                Alternet.UI.App.LogNameValue(title, "null");
                return;
            }

            Alternet.UI.App.Log(title);
            Alternet.UI.App.Log(path);

            var hasDll = Alternet.UI.FileUtils.HasFiles(path, "*.dll");

            Alternet.UI.App.LogNameValue("Has Dll", hasDll);

            Alternet.UI.App.LogEndSection();
        }
    }

    public static string? GetPackageName()
    {
        return Android.App.Application.Context.PackageName;
    }

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