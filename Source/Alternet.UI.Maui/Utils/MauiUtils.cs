﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.Maui.Extensions;

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
                if (Microsoft.Maui.Controls.Application.Current is not null)
                {
                    foreach (var window in Microsoft.Maui.Controls.Application.Current.Windows)
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
                foreach (var view in ControlViews)
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
                foreach (var page in Pages)
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
                foreach (var window in Windows)
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
        public static bool IsDarkTheme
        {
            get
            {
                var currentTheme = Microsoft.Maui.Controls.Application.Current?.RequestedTheme;
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

        /// <summary>
        /// Gets the absolute position of a <see cref="VisualElement"/> relative to its ancestors.
        /// </summary>
        /// <param name="visualElement">The <see cref="VisualElement"/> to
        /// calculate the position for.</param>
        /// <returns>A <see cref="PointD"/> representing the absolute position.</returns>
        public static PointD GetAbsolutePosition(VisualElement visualElement)
        {
            var ancestors = AllParents(visualElement);

            var x = visualElement.X;
            var y = visualElement.Y;

            foreach (var item in ancestors)
            {
                if(item is VisualElement visualItem)
                {
                    x += visualItem.X;
                    y += visualItem.Y;
                }
            }

            return new(x, y);
        }

        /// <summary>
        /// Converts an <see cref="Alternet.Drawing.Color"/> to
        /// a <see cref="Microsoft.Maui.Graphics.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="Alternet.Drawing.Color"/> to convert.</param>
        /// <returns>A <see cref="Microsoft.Maui.Graphics.Color"/> representation
        /// of the input color.</returns>
        public static Microsoft.Maui.Graphics.Color Convert(Alternet.Drawing.Color color)
        {
            return Microsoft.Maui.Graphics.Color.FromUint(color.AsUInt());
        }

        /// <summary>
        /// Enumerates all parent elements of the specified <see cref="VisualElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="VisualElement"/> to retrieve parents for.</param>
        /// <returns>An enumerable collection of parent <see cref="VisualElement"/> instances.</returns>
        public static IEnumerable<Element> AllParents(Element? element)
        {
            element = element?.Parent;

            while (element != null)
            {
                yield return element;
                element = element.Parent;
            }
        }

        /// <summary>
        /// Debug related method. Do not use directly.
        /// </summary>
        public static void AddAllViewsToParent(Layout parent)
        {
            var types = Alternet.UI.AssemblyUtils.GetTypeDescendants(typeof(View));

            foreach (var type in types)
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
#elif IOS || MACCATALYST
#elif WINDOWS
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
        /// <param name="control">Control where touch event occurred.</param>
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
        /// Converts an <see cref="Drawing.Image"/> to an <see cref="SKBitmapImageSource"/>.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns>An <see cref="SKBitmapImageSource"/> if the image is not null;
        /// otherwise, null.</returns>
        public static SKBitmapImageSource? ImageSourceFromImage(Drawing.Image? image)
        {
            if (image is not null)
            {
                var skiaImg = (SKBitmap)image;
                var imageSource = new SKBitmapImageSource();
                imageSource.Bitmap = skiaImg;
                return imageSource;
            }

            return null;
        }

        /// <summary>
        /// Creates <see cref="ImageSource"/> from the specified <see cref="SvgImage"/>.
        /// </summary>
        /// <param name="svgImage">Svg image.</param>
        /// <param name="sizeInPixels">Image size in pixels.</param>
        /// <param name="isDark">Whether background is dark.</param>
        /// <param name="isDisabled">Whether to use disabled or normal color for the single
        /// color SVG images.</param>
        /// <returns></returns>
        public static SKBitmapImageSource? ImageSourceFromSvg(
            SvgImage? svgImage,
            int sizeInPixels,
            bool isDark,
            bool isDisabled = false)
        {
            Drawing.Image? img;

            if (isDisabled)
                img = svgImage?.AsDisabledImage(sizeInPixels, isDark);
            else
                img = svgImage?.AsNormalImage(sizeInPixels, isDark);
            return ImageSourceFromImage(img);
        }

        /// <summary>
        /// Sets the image for a <see cref="Button"/> using the specified SVG image and size.
        /// </summary>
        /// <param name="button">The button to set the image for.</param>
        /// <param name="svg">The SVG image to use.</param>
        /// <param name="size">The size of the image.</param>
        /// <param name="disabled">Whether to use the disabled image.</param>
        public static void SetButtonImage(
            Microsoft.Maui.Controls.Button button,
            Drawing.SvgImage? svg,
            int size,
            bool disabled = false)
        {
            if (svg is null)
            {
                button.RemoveBinding(Microsoft.Maui.Controls.Button.ImageSourceProperty);
                return;
            }

            var images = Alternet.UI.MauiUtils.ImageSourceFromSvg(svg, size, disabled);

            if (images is not null)
            {
                button.SetAppTheme<ImageSource>(
                    Microsoft.Maui.Controls.Button.ImageSourceProperty,
                    images.Value.Light,
                    images.Value.Dark);
            }
        }

        /// <summary>
        /// Sets the image for a <see cref="ImageButton"/> using the specified SVG image and size.
        /// </summary>
        /// <param name="button">The button to set the image for.</param>
        /// <param name="svg">The SVG image to use.</param>
        /// <param name="size">The size of the image.</param>
        public static void SetButtonImage(ImageButton button, Drawing.SvgImage? svg, int size)
        {
            if (svg is null)
            {
                button.RemoveBinding(Microsoft.Maui.Controls.ImageButton.SourceProperty);
                return;
            }

            var images = Alternet.UI.MauiUtils.ImageSourceFromSvg(svg, size);

            if (images is not null)
            {
                button.SetAppTheme<ImageSource>(
                    Microsoft.Maui.Controls.ImageButton.SourceProperty,
                    images.Value.Light,
                    images.Value.Dark);
            }
        }

        /// <summary>
        /// Creates <see cref="ImageSource"/> with disabled images
        /// from the specified <see cref="SvgImage"/>.
        /// </summary>
        /// <param name="svgImage">The SVG image to convert.</param>
        /// <param name="sizeInPixels">The size of the image in pixels.</param>
        /// <returns>A tuple containing the light and
        /// dark <see cref="SKBitmapImageSource"/> with disabled images.</returns>
        /// <param name="isDisabled">Whether to use disabled or normal color for the single
        /// color SVG images.</param>
        public static (SKBitmapImageSource Light, SKBitmapImageSource Dark)? ImageSourceFromSvg(
            SvgImage? svgImage,
            int sizeInPixels,
            bool isDisabled = false)
        {
            if (svgImage is null)
                return null;

            var darkImage = ImageSourceFromSvg(
                svgImage,
                sizeInPixels,
                isDark: true,
                isDisabled);
            var lightImage = ImageSourceFromSvg(
                svgImage,
                sizeInPixels,
                isDark: false,
                isDisabled);

            if (darkImage is null || lightImage is null)
                return null;

            return (lightImage, darkImage);
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
        /// Gets collection of <see cref="View"/> elements
        /// added to pages of the application.
        /// </summary>
        public static IEnumerable<T> GetAllChildren<T>()
        {
            foreach (var page in Pages)
            {
                var elements = GetChildren<T>(page, true);

                foreach (var element in elements)
                    yield return element;
            }
        }

        /// <summary>
        /// Gets the current page of the first window in the application.
        /// </summary>
        /// <returns>The current page of the first window, or null if no
        /// windows are available.</returns>
        public static Page? GetFirstWindowPage()
        {
            var windows = Application.Current?.Windows;

            if (windows is null || windows.Count == 0)
                return null;

            return windows[0].Page;
        }

        /// <summary>
        /// Retrieves the nearest parent <see cref="AbsoluteLayout"/>
        /// that contains the specified view.
        /// </summary>
        /// <param name="view">The view whose parent <see cref="AbsoluteLayout"/>
        /// is to be found.</param>
        /// <returns>The closest parent <see cref="AbsoluteLayout"/>,
        /// or <c>null</c> if none is found.</returns>
        public static AbsoluteLayout? GetParentAbsoluteLayout(Element? view)
        {
            return GetSpecialParent<AbsoluteLayout>(view);
        }

        /// <summary>
        /// Searches for the closest parent of type <typeparamref name="T"/> in the view hierarchy.
        /// </summary>
        /// <typeparam name="T">The type of the parent view to find.</typeparam>
        /// <param name="view">The view whose parent is to be found.</param>
        /// <returns>The closest parent of type <typeparamref name="T"/>, or null if
        /// none is found.</returns>
        public static T? GetSpecialParent<T>(Element? view)
            where T : Element
        {
            var parent = view?.Parent;
            while (parent is not null && parent is not T)
            {
                parent = parent.Parent;
            }

            var page = parent as T;

            return page;
        }

        /// <summary>
        /// Retrieves the highest-level parent <c>AbsoluteLayout</c> that
        /// contains the specified element.
        /// </summary>
        /// <param name="view">The element whose topmost <c>AbsoluteLayout</c> parent
        /// is to be found.</param>
        /// <returns>The topmost <c>AbsoluteLayout</c>, or null if none is found.</returns>
        public static AbsoluteLayout? GetTopAbsoluteLayout(Element? view)
        {
            return GetTopSpecialParent<AbsoluteLayout>(view);
        }

        /// <summary>
        /// Searches for the highest-level parent of type
        /// <typeparamref name="T"/> in the element hierarchy.
        /// </summary>
        /// <typeparam name="T">The type of the parent element to find.</typeparam>
        /// <param name="view">The element whose topmost parent is to be found.</param>
        /// <returns>The topmost parent of type <typeparamref name="T"/>,
        /// or null if none is found.</returns>
        public static T? GetTopSpecialParent<T>(Element? view)
            where T : Element
        {
            T? result = null;

            foreach(var parent in AllParents(view))
            {
                if (parent is T typedParent)
                    result = typedParent;
            }

            return result;
        }

        /// <summary>
        /// Gets the page that contains the specified view.
        /// </summary>
        /// <param name="view">The view to find the containing page for.</param>
        /// <returns>The page that contains the specified view, or the first
        /// window's current page if no containing page is found.</returns>
        public static Page? GetPage(VisualElement? view)
        {
            var parent = view?.Parent;
            while (parent is not null && parent is not Page)
            {
                parent = parent.Parent;
            }

            var page = parent as Page;

            if (page is not null)
                return page;

            page = GetFirstWindowPage();
            return page;
        }

        /// <summary>
        /// Gets collection of the child elements in the specified container.
        /// </summary>
        /// <typeparam name="T">Type of the child element.</typeparam>
        /// <param name="container">Elements container.</param>
        /// <param name="recursive">Whether to get elements recursively.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetChildren<T>(object? container, bool recursive)
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

        /// <summary>
        /// Closes the application.
        /// </summary>
        public static bool CloseApplication()
        {
            /*
                Environment.Exit(0);
                Microsoft.Maui.Controls.Application.Current?.Quit();
            */

            try
            {
                var windows = Application.Current?.Windows;

                if (windows is null || windows.Count == 0)
                    return false;

                var window = windows[0];
                Application.Current?.CloseWindow(window);
                return true;
            }
            catch
            {
                return false;
            }
#if ANDROID
#elif IOS || MACCATALYST
#elif WINDOWS
#endif
        }
    }
}