using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

#if ANDROID
using WindowSoftInputModeAdjust
    = Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.WindowSoftInputModeAdjust;
#endif

/*
    Do not change namespace of the MauiUtils as it is used in App static constructor and other places
    in order to determine device platform and get other MAUI related settings.
*/

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties related to MAUI platform.
    /// </summary>
    public static partial class MauiUtils
    {
        private static Alternet.UI.GenericDeviceType? deviceType;
        private static PlatformApplication? platformApplication;

        /// <summary>
        /// Gets collection of all windows in the application. This is safe property
        /// and it returns an empty collection if application is not yet created.
        /// </summary>
        public static IEnumerable<Microsoft.Maui.Controls.Window> Windows
        {
            get
            {
                if (Application.Current is not null)
                {
                    foreach (var window in Application.Current.Windows)
                    {
                        yield return window;
                    }
                }
            }
        }

        /// <summary>
        /// Gets collection of <see cref="Alternet.UI.Control"/>
        /// added to pages of the application inside <see cref="Alternet.UI.ControlView"/>
        /// elements.
        /// </summary>
        public static IEnumerable<Alternet.UI.Control> Controls
        {
            get
            {
                foreach(var view in ControlViews)
                {
                    if (view.Control is not null)
                        yield return view.Control;
                }
            }
        }

        /// <summary>
        /// Gets collection of <see cref="Alternet.UI.ControlView"/> elements
        /// added to pages of the application.
        /// </summary>
        public static IEnumerable<Alternet.UI.ControlView> ControlViews
        {
            get
            {
                foreach(var page in Pages)
                {
                    var elements = GetChildren<Alternet.UI.ControlView>(page, true);

                    foreach (var element in elements)
                        yield return element;
                }
            }
        }

        /// <summary>
        /// Gets collection of all pages in the application. If application is not yet created,
        /// returns an empty collection. Uses <see cref="Windows"/>,
        /// <see cref="Microsoft.Maui.Controls.Window.Page"/>,
        /// <see cref="INavigation.ModalStack"/>, <see cref="INavigation.NavigationStack"/>
        /// in order to get the result.
        /// </summary>
        public static IEnumerable<Page> Pages
        {
            get
            {
                foreach(var window in Windows)
                {
                    var page = window.Page;
                    if (page is not null)
                    {
                        var navigation = page.Navigation;

                        foreach (var subpage in navigation.NavigationStack)
                        {
                            yield return subpage;
                        }

                        foreach (var subpage in navigation.ModalStack)
                        {
                            yield return subpage;
                        }

                        yield return page;
                    }
                }
            }
        }

        /// <summary>
        /// Gets whether current application theme is dark.
        /// </summary>
        public static bool? IsDarkTheme
        {
            get
            {
                var currentTheme = Application.Current?.RequestedTheme;
                if (currentTheme == AppTheme.Dark)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets <see cref="PlatformApplication"/>.
        /// </summary>
        public static PlatformApplication PlatformApplication
        {
            get
            {
#if ANDROID
                platformApplication ??= (Android.App.Application)Android.App.Application.Context;
#elif IOS || MACCATALYST
                platformApplication ??= UIKit.UIApplication.SharedApplication.Delegate;
#elif WINDOWS
                platformApplication ??= Microsoft.UI.Xaml.Application.Current;
#endif
                return platformApplication;
            }
        }

#if WINDOWS
#endif

        /// <summary>
        /// Debug related method. Do not use directly.
        /// </summary>
        public static void AddAllViewsToParent(Layout parent)
        {
            var types = Alternet.UI.AssemblyUtils.GetTypeDescendants(typeof(View));

            foreach(var type in types)
            {
                if (type == typeof(WebView) || type == typeof(VerticalStackLayout)
                    || type == typeof(GroupableItemsView))
                    continue;

                View? instance;

                try
                {
                    if (!Alternet.UI.AssemblyUtils.HasConstructorNoParams(type))
                        continue;
                    instance = (View?)Activator.CreateInstance(type);
                }
                catch (Exception)
                {
                    instance = null;
                }

                if (instance is null)
                    continue;

                try
                {
                    parent.Children.Add(instance);
                    var nativeType = instance.Handler?.PlatformView?.GetType().FullName;
                    Alternet.UI.LogUtils.LogToFile($"{instance.GetType().FullName} uses {nativeType}");
                    parent.Children.Remove(instance);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Checks whether device platform is Android.
        /// </summary>
        /// <returns></returns>
        public static bool IsAndroid() => DeviceInfo.Current.Platform == DevicePlatform.Android;

        /// <summary>
        /// Checks whether device platform is iOS.
        /// </summary>
        /// <returns></returns>
        public static bool IsIOS() => DeviceInfo.Current.Platform == DevicePlatform.iOS;

        /// <summary>
        /// Checks whether device platform is macOS.
        /// </summary>
        /// <returns></returns>
        public static bool IsMacOS() => DeviceInfo.Current.Platform == DevicePlatform.macOS;

        /// <summary>
        /// Checks whether device platform is MacCatalyst.
        /// </summary>
        /// <returns></returns>
        public static bool IsMacCatalyst()
        {
            return DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst;
        }

        /// <summary>
        /// Sets soft input mode for the window.
        /// </summary>
        /// <param name="window">Window to apply the setting.</param>
        /// <param name="isResize">Whether to resize the window when soft keyboard is shown.</param>
        public static void SetWindowSoftInputModeAdjust(
            Microsoft.Maui.Controls.Window? window,
            bool isResize = true)
        {
            if (window is null)
                return;

#if ANDROID
            WindowSoftInputModeAdjust value = isResize
                ? WindowSoftInputModeAdjust.Resize : WindowSoftInputModeAdjust.Pan;
            Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific
                .Application.SetWindowSoftInputModeAdjust(window, value);
#endif
        }

        /// <summary>
        /// Gets device the app is running on, such as a desktop computer or a tablet.
        /// </summary>
        /// <returns></returns>
        public static Alternet.UI.GenericDeviceType GetDeviceType()
        {
            deviceType ??= Fn();
            return deviceType.Value;

            Alternet.UI.GenericDeviceType Fn()
            {
                var idiom = DeviceInfo.Current.Idiom;

                if (idiom == DeviceIdiom.Desktop)
                    return Alternet.UI.GenericDeviceType.Desktop;
                if (idiom == DeviceIdiom.Phone)
                    return Alternet.UI.GenericDeviceType.Phone;
                if (idiom == DeviceIdiom.Tablet)
                    return Alternet.UI.GenericDeviceType.Tablet;
                if (idiom == DeviceIdiom.TV)
                    return Alternet.UI.GenericDeviceType.TV;
                if (idiom == DeviceIdiom.Watch)
                    return Alternet.UI.GenericDeviceType.Watch;

                return Alternet.UI.GenericDeviceType.Unknown;
            }
        }

        /// <summary>
        /// Converts <see cref="SKTouchEventArgs"/> to <see cref="TouchEventArgs"/>.
        /// </summary>
        /// <param name="e">Value to convert.</param>
        /// <param name="control">Control where touch event occured.</param>
        /// <returns></returns>
        public static Alternet.UI.TouchEventArgs Convert(
            SKTouchEventArgs e,
            Alternet.UI.AbstractControl? control)
        {
            Alternet.UI.TouchEventArgs result = new();

            result.Id = e.Id;
            result.ActionType = (Alternet.UI.TouchAction)e.ActionType;
            result.DeviceType = (Alternet.UI.TouchDeviceType)e.DeviceType;
            result.Location = GraphicsFactory.PixelToDip(((PointD)e.Location).ToPoint());
            result.InContact = e.InContact;
            result.WheelDelta = e.WheelDelta;
            result.Pressure = e.Pressure;
            result.MouseButton = e.MouseButton.ToAlternet();
            return result;
        }

        /// <summary>
        /// Creates <see cref="ImageSource"/> from the specified <see cref="SvgImage"/>.
        /// Default normal color is used for the single color svg images.
        /// </summary>
        /// <param name="svgImage">Svg image.</param>
        /// <param name="sizeInPixels">Image size in pixels.</param>
        /// <param name="isDark">Whether background is dark.</param>
        /// <returns></returns>
        public static ImageSource? ImageSourceFromSvg(
            SvgImage? svgImage,
            int sizeInPixels,
            bool isDark)
        {
            var img = svgImage?.AsNormalImage(sizeInPixels, isDark);
            if (img is not null)
            {
                var skiaImg = (SKBitmap)img;
                var imageSource = new SKBitmapImageSource();
                imageSource.Bitmap = skiaImg;
                return imageSource;
            }

            return null;
        }

        /// <summary>
        /// Gets device platform.
        /// </summary>
        /// <returns></returns>
        public static DevicePlatform GetDevicePlatform()
        {
            var result = DeviceInfo.Current.Platform;
            return result;
        }

        /// <summary>
        /// Debug related method. Do not use directly.
        /// </summary>
        public static void EnumViewsToLog(Layout parent)
        {
            foreach (var item in parent.Children)
            {
                var nativeType = item.Handler?.PlatformView?.GetType().Name;

                Alternet.UI.LogUtils.LogToFile($"{item.GetType().Name} uses {nativeType}");
            }
        }

        /// <summary>
        /// Gets collection of the child elements in the specified container.
        /// </summary>
        /// <typeparam name="T">Type of the child element.</typeparam>
        /// <param name="container">Elements container.</param>
        /// <param name="recursive">Whether to get elements recursively.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetChildren<T>(object? container, bool recursive)
            where T : View
        {
            if (container is IElementController controller)
            {
                foreach (var element in controller.LogicalChildren)
                {
                    if (element is T result)
                        yield return result;
                    if (recursive)
                    {
                        var elements = GetChildren<T>(element, true);

                        foreach (var item in elements)
                            yield return item;
                    }
                }
            }
        }
    }
}
