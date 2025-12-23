using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using Microsoft.Maui;
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
        /// Updates the specified view to reflect the current visibility and height of the keyboard as indicated by the
        /// event arguments.
        /// </summary>
        /// <remarks>When the keyboard is visible, the view's width is set to the keyboard's height and
        /// the view is made visible. When the keyboard is hidden, the view is hidden. This method is intended for use
        /// in scenarios where a UI element needs to track the keyboard's presence and size.</remarks>
        /// <param name="e">The event arguments containing information about the keyboard's visibility and height.</param>
        /// <param name="view">The view to update based on the keyboard's visibility state. The view's width and visibility will be
        /// modified.</param>
        public static void UpdateKeyboardPanel(this KeyboardVisibleChangedEventArgs e, View view)
        {
            if (e.IsVisible)
            {
                view.MinimumHeightRequest = e.Height;
                view.IsVisible = true;
            }
            else
            {
                view.IsVisible = false;
            }
        }

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

        /// <summary>
        /// Converts an <see cref="Alternet.UI.MenuItem"/> to a <see cref="MenuBarItem"/>.
        /// </summary>
        /// <param name="menuItem">The <see cref="Alternet.UI.MenuItem"/> to convert. Must not be <see langword="null"/>.</param>
        /// <returns>A <see cref="MenuBarItem"/> that represents the converted menu item, including its text, enabled state, and
        /// child items if present.</returns>
        public static MenuBarItem ToMenuBarItem(this Alternet.UI.MenuItem menuItem)
        {
            var result = new MenuBarItem
            {
                Text = menuItem.Text,
                IsEnabled = menuItem.Enabled,
            };

            if (menuItem.HasItems)
            {
                foreach (var child in menuItem.Items)
                {
                    result.Add(child.ToMenuFlyoutItem());
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a new <see cref="MenuFlyout"/> that contains the items from the specified <see cref="ContextMenu"/>.
        /// </summary>
        /// <remarks>
        /// This method copies the items from the <see cref="ContextMenu"/> to the <see cref="MenuFlyout"/>. Changes to the
        /// original <see cref="ContextMenu"/> after calling this method are not reflected in the returned <see cref="MenuFlyout"/>.
        /// </remarks>
        /// <param name="contextMenu">The <see cref="ContextMenu"/> whose items will be added to the new <see cref="MenuFlyout"/>.
        /// Cannot be null.</param>
        /// <returns>A <see cref="MenuFlyout"/> containing all items from
        /// the specified <see cref="ContextMenu"/>, in the same order.</returns>
        public static MenuFlyout ToMenuFlyout(this Alternet.UI.ContextMenu contextMenu)
        {
            var result = new MenuFlyout();
            foreach (var item in contextMenu.Items)
            {
                result.Add(item.ToMenuFlyoutItem());
            }
            return result;
        }

        /// <summary>
        /// Converts an <see cref="Alternet.UI.MenuItem"/> to a corresponding
        /// <see cref="Microsoft.Maui.Controls.MenuFlyoutItem"/>.
        /// </summary>
        /// <param name="menuItem">The <see cref="Alternet.UI.MenuItem"/> to convert. Cannot be <c>null</c>.</param>
        /// <returns>A <see cref="Microsoft.Maui.Controls.MenuFlyoutItem"/> that represents the converted menu item.</returns>
        public static MenuFlyoutItem ToMenuFlyoutItem(this Alternet.UI.MenuItem menuItem)
        {
            if (menuItem.IsSeparator)
            {
                var separator = new MenuFlyoutSeparator();
                return separator;
            }

            var result = new MenuFlyoutItem
            {
                Text = menuItem.Text,
            };

            if (menuItem.Command is not null)
            {
                result.Command = menuItem.Command.ToMaui();
                result.CommandParameter = menuItem.CommandParameter;
            }
            else
            {
                result.Clicked += (s, e) => menuItem.RaiseClick();
                result.IsEnabled = menuItem.Enabled;
            }

            return result;
        }

        /// <summary>
        /// Updates the execution state of the specified <see cref="MenuBarItem"/> and its child items.
        /// </summary>
        /// <remarks>This method recursively updates the execution state of all child items within the
        /// specified <see cref="MenuBarItem"/>. If a child item is a <see cref="MenuFlyoutItem"/>, its execution state
        /// is updated by calling its <c>UpdateCanExecute</c> method. If a child item is another <see
        /// cref="MenuBarItem"/>, this method is called recursively on that item.</remarks>
        /// <param name="menuItem">The <see cref="MenuBarItem"/> whose execution state is to be updated.</param>
        public static void UpdateCanExecute(this MenuBarItem menuItem)
        {
            for (int i = 0; i < menuItem.Count; i++)
            {
                var child = menuItem[i];
                if (child is MenuFlyoutItem menuFlyoutItem)
                    menuFlyoutItem.UpdateCanExecute();
                else
                if (child is MenuBarItem menuBarItem)
                    menuBarItem.UpdateCanExecute();
            }
        }

        /// <summary>
        /// Updates 'IsEnabled' property of <see cref="MenuFlyoutItem"/> based on the result of the
        /// <see cref="ICommand.CanExecute"/> method of the associated command.
        /// </summary>
        /// <param name="menuFlyoutItem">The <see cref="MenuFlyoutItem"/> whose state will be updated.
        /// The item must have an associated  <see cref="ICommand"/> for this method to take effect.</param>
        public static void UpdateCanExecute(this MenuFlyoutItem menuFlyoutItem)
        {
            if (menuFlyoutItem.Command is null)
                return;
            menuFlyoutItem.IsEnabled = menuFlyoutItem.Command.CanExecute(menuFlyoutItem.CommandParameter);
        }
    }
}