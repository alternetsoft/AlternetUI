using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Alternet.Maui.Extensions
{
    /// <summary>
    /// Provides extension methods for .NET MAUI components.
    /// </summary>
    public static class MauiExtensions
    {
        /// <summary>
        /// Converts an <see cref="Alternet.Drawing.Color"/> to a .NET MAUI color.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <returns>A <see cref="Microsoft.Maui.Graphics.Color"/> representing the converted color.</returns>
        public static Microsoft.Maui.Graphics.Color ToMaui(this Alternet.Drawing.Color color)
        {
            return Alternet.UI.MauiUtils.Convert(color);
        }

        /// <summary>
        /// Attempts to set focus on the specified <see cref="GraphicsView"/>.
        /// </summary>
        /// <param name="graphicsView">The <see cref="GraphicsView"/> to focus.</param>
        /// <param name="focusState">The desired focus state.</param>
        /// <returns><c>true</c> if focus was successfully set; otherwise, <c>false</c>.</returns>
        public static bool Focus(this GraphicsView graphicsView, Alternet.UI.FocusState focusState)
        {
#if ANDROID
            return false;
#elif IOS || MACCATALYST
            return false;
#elif WINDOWS
            var platformView = graphicsView.GetPlatformView();
            return platformView?.Focus((Microsoft.UI.Xaml.FocusState)focusState) ?? false;
#endif
        }

        /// <summary>
        /// Retrieves the platform-specific view for the specified <see cref="GraphicsView"/>.
        /// </summary>
        /// <param name="graphicsView">The <see cref="GraphicsView"/> to retrieve the platform view for.</param>
        /// <returns>The platform-specific view, or <c>null</c> if not available.</returns>
        public static Microsoft.Maui.Platform.PlatformTouchGraphicsView? GetPlatformView(
            this GraphicsView graphicsView)
        {
            var result = graphicsView.Handler?.PlatformView
                as Microsoft.Maui.Platform.PlatformTouchGraphicsView;
            return result;
        }

        /// <summary>
        /// Converts a <see cref="Rect"/> to a <see cref="RectD"/>.
        /// </summary>
        /// <param name="value">The <see cref="Rect"/> to convert.</param>
        /// <returns>A <see cref="RectD"/> representation of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD ToRectD(this Rect value)
        {
            return new((Coord)value.X, (Coord)value.Y, (Coord)value.Width, (Coord)value.Height);
        }

        /// <summary>
        /// Converts an <see cref="Alternet.Drawing.Font"/> to a MAUI-compatible font.
        /// </summary>
        /// <param name="font">The <see cref="Alternet.Drawing.Font"/> to convert.</param>
        /// <returns>A MAUI-compatible <see cref="Microsoft.Maui.Graphics.IFont"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Microsoft.Maui.Graphics.IFont ToMaui(this Alternet.Drawing.Font font)
        {
            return (Microsoft.Maui.Graphics.IFont)font.Handler;
        }

        /// <summary>
        /// Converts a <see cref="SKMouseButton"/> to an <see cref="MouseButton"/>.
        /// </summary>
        /// <param name="value">The <see cref="SKMouseButton"/> to convert.</param>
        /// <returns>The corresponding <see cref="MouseButton"/> value.</returns>
        public static MouseButton ToAlternet(this SKMouseButton value)
        {
            switch (value)
            {
                case SKMouseButton.Unknown:
                default:
                    return MouseButton.Unknown;
                case SKMouseButton.Left:
                    return MouseButton.Left;
                case SKMouseButton.Middle:
                    return MouseButton.Middle;
                case SKMouseButton.Right:
                    return MouseButton.Right;
            }
        }

        /// <summary>
        /// Converts a <see cref="Microsoft.Maui.Graphics.RectF"/> to a <see cref="RectD"/>.
        /// </summary>
        /// <param name="value">The <see cref="Microsoft.Maui.Graphics.RectF"/> to convert.</param>
        /// <returns>A <see cref="RectD"/> representation of the input.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD ToRectD(this Microsoft.Maui.Graphics.RectF value)
        {
            return new(value.X, value.Y, value.Width, value.Height);
        }
    }
}