using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to commonly used icons.
    /// </summary>
    public static class KnownIcons
    {
        private static IconSet? defaultIcon;
        private static IconSet? calendarIcon;

        /// <summary>
        /// Gets template for the resource URLs.
        /// </summary>
        public static string ResTemplate { get; } =
            "embres:Alternet.UI.Common.Resources.Ico.{0}.ico?assembly=Alternet.UI.Common";

        /// <summary>
        /// Gets or sets url used to load default icon.
        /// </summary>
        public static string UrlDefault { get; set; } = GetImageUrl("Sample");

        /// <summary>
        /// Gets or sets the default <see cref="IconSet"/> instance.
        /// If not assigned, it will be loaded from <see cref="UrlDefault"/>.
        /// </summary>
        public static IconSet? Default
        {
            get
            {
                return defaultIcon ??= IconSet.FromUrlOrNull(UrlDefault);
            }

            set => defaultIcon = value;
        }

        /// <summary>
        /// Gets or sets the calendar <see cref="IconSet"/> instance.
        /// This icon contains a number of the current day on it.
        /// If not assigned, it will be created with the default settings.
        /// </summary>
        public static IconSet DayNumber
        {
            get
            {
                return calendarIcon ??= CreateDayNumberIcon();
            }

            set
            {
                calendarIcon = value;
            }
        }

        /// <summary>
        /// Creates a day number icon with the specified background and foreground colors,
        /// and an number to display a day number on the icon.
        /// </summary>
        /// <param name="backColor">The background color of the icon.</param>
        /// <param name="foreColor">The foreground color of the icon.</param>
        /// <param name="number">An optional number to display on the icon.
        /// If <c>null</c>, the current day number is used.</param>
        /// <returns>The created <see cref="IconSet"/>.</returns>
        public static IconSet CreateDayNumberIcon(
            SKColor? backColor = null,
            SKColor? foreColor = null,
            int? number = null)
        {
            var image16 = CreateDayNumberIcon(16, backColor, foreColor, number);
            var image32 = CreateDayNumberIcon(32, backColor, foreColor, number);
            var image48 = CreateDayNumberIcon(48, backColor, foreColor, number);
            var image64 = CreateDayNumberIcon(64, backColor, foreColor, number);

            Bitmap i16 = (Bitmap)image16;
            Bitmap i32 = (Bitmap)image32;
            Bitmap i48 = (Bitmap)image48;
            Bitmap i64 = (Bitmap)image64;

            IconSet result = new (i16, i32, i48, i64);
            return result;
        }

        /// <summary>
        /// Creates a day number icon with the specified size, background and foreground colors,
        /// and an optional number to display a day number on the icon.
        /// </summary>
        /// <param name="size">The size of the icon.</param>
        /// <param name="backColor">The background color of the icon.</param>
        /// <param name="foreColor">The foreground color of the icon.</param>
        /// <param name="number">An optional number to display on the icon.
        /// If <c>null</c>, the current day number is used.</param>
        /// <returns>The created <see cref="SKBitmap"/>.</returns>
        public static SKBitmap CreateDayNumberIcon(
            int size,
            SKColor? backColor = null,
            SKColor? foreColor = null,
            int? number = null)
        {
            var bitmap = new SKBitmap(size, size);

            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.Transparent);

            float padding = size * 0.1f;

            var rect = new SKRect(
                padding,
                padding,
                size - padding,
                size - padding);

            float radius = size * 0.15f;

            using var paint = new SKPaint
            {
                Color = backColor ?? DefaultColors.GetWindowBackColor(!App.IsDark),
                IsAntialias = true
            };

            canvas.DrawRoundRect(rect, radius, radius, paint);

            paint.Color = DefaultColors.BorderColor.GetColor(App.IsDark);
            paint.IsStroke = true;
            canvas.DrawRoundRect(rect, radius, radius, paint);

            string text = number?.ToString() ?? DateTime.Now.Day.ToString();

            using var textPaint = new SKPaint
            {
                Color = foreColor ?? DefaultColors.GetWindowForeColor(!App.IsDark),
                IsAntialias = true,
            };

            using var font = new SKFont(Font.Default.AsBold.FontFamily.SkiaTypeface, size * 0.55f);

            SKRect bounds = new();
            font.MeasureText(text, out bounds);

            float x = rect.MidX - bounds.MidX;
            float y = rect.MidY - bounds.MidY;

            canvas.DrawText(text, x, y, SKTextAlign.Left, font, textPaint);

            return bitmap;
        }

        /// <summary>
        /// Gets the stream for the default icon.
        /// </summary>
        /// <returns>The stream for the default icon, or <c>null</c> if the icon cannot be loaded.</returns>
        public static Stream? GetDefaultIconStream()
        {
            var stream = ResourceLoader.StreamFromUrlOrDefault(UrlDefault);
            return stream;
        }

        private static string GetImageUrl(string name) => string.Format(ResTemplate, name);
    }
}