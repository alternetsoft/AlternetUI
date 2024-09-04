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

namespace Alternet.UI.Extensions
{
    internal static class MauiExtensionsPrivate
    {
#if WINDOWS
        public static bool Focus(this GraphicsView graphicsView, Alternet.UI.FocusState focusState)
        {
            var platformView = graphicsView.GetPlatformView();
            return platformView?.Focus((Microsoft.UI.Xaml.FocusState)focusState) ?? false;
        }
#else
        public static bool Focus(this GraphicsView graphicsView, Alternet.UI.FocusState focusState)
        {
            return false;
        }
#endif

        public static Microsoft.Maui.Platform.PlatformTouchGraphicsView? GetPlatformView(
            this GraphicsView graphicsView)
        {
            var result = graphicsView.Handler?.PlatformView
                as Microsoft.Maui.Platform.PlatformTouchGraphicsView;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD ToRectD(this Rect value)
        {
            return new(value.X, value.Y, value.Width, value.Height);
        }

        public static PointD GetAbsolutePosition(this VisualElement visualElement)
        {
            var ancestors = AllParents(visualElement);

            var x = visualElement.X;
            var y = visualElement.Y;

            foreach (var item in ancestors)
            {
                x += item.X;
                y += item.Y;
            }

            return new(x, y);
        }

        public static IEnumerable<VisualElement> AllParents(this VisualElement? element)
        {
            element = element?.Parent as VisualElement;

            while (element != null)
            {
                yield return element;
                element = element.Parent as VisualElement;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Microsoft.Maui.Graphics.IFont ToMaui(this Alternet.Drawing.Font font)
        {
            return (Microsoft.Maui.Graphics.IFont)font.Handler;
        }

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RectD ToRectD(this Microsoft.Maui.Graphics.RectF value)
        {
            return new(value.X, value.Y, value.Width, value.Height);
        }
    }
}