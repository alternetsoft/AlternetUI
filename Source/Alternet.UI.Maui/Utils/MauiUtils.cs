using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;

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
        private static PlatformApplication? platformApplication;

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
            var types = AssemblyUtils.GetTypeDescendants(typeof(View));

            foreach(var type in types)
            {
                if (type == typeof(WebView) || type == typeof(VerticalStackLayout) || type == typeof(GroupableItemsView))
                    continue;

                View? instance;

                try
                {
                    if (!AssemblyUtils.HasConstructorNoParams(type))
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
                    LogUtils.LogToFile($"{instance.GetType().FullName} uses {nativeType}");
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
        public static bool IsMacCatalyst() => DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst;

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

                LogUtils.LogToFile($"{item.GetType().Name} uses {nativeType}");
            }
        }
    }
}
