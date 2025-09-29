using System.Runtime.InteropServices;

using SkiaSharp;

namespace Alternet.UI.WinForms
{
    public static class WinFormsUtils
    {
#if NET9_0_OR_GREATER
#else
        private static Alternet.UI.SystemColorModeType applicationSystemColorMode
            = SystemColorModeType.Classic;
        private static Alternet.UI.SystemColorModeType applicationColorMode
            = Alternet.UI.SystemColorModeType.Classic;
        private static bool isDarkModeEnabled = false;
#endif

        /// <summary>
        /// Determines whether the specified <see cref="Alternet.Drawing.Font"/>
        /// and <see cref="System.Drawing.Font"/>
        /// instances represent the same font.
        /// </summary>
        /// <remarks>The comparison checks the font name, size (in points),
        /// and style to determine
        /// equality.</remarks>
        /// <param name="uiFont">The <see cref="Alternet.Drawing.Font"/> instance to compare.
        /// Can be <see langword="null"/>.</param>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> instance to compare.
        /// Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if both fonts are <see langword="null"/>
        /// or if they represent the same font;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool Equals(
            Alternet.Drawing.Font? uiFont,
            System.Drawing.Font? winFont,
            float scale)
        {
            if (uiFont == null && winFont == null)
                return true;
            if (uiFont == null || winFont == null)
                return false;

            var size = FontSizeToAlternet(winFont.SizeInPoints, scale);

            return uiFont.Equals(
                winFont.Name,
                size,
                (Alternet.Drawing.FontStyle)winFont.Style);
        }

        public static float FontSizeToAlternet(float sizeInPoints, float scale)
        {
            var size = sizeInPoints * (96 / 72);
            size *= scale;
            return size;
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Font"/> object to an
        /// <see cref="Alternet.Drawing.Font"/> object.
        /// </summary>
        /// <remarks>The conversion maps the font name, size (in points),
        /// and style from the <see cref="System.Drawing.Font"/> to the corresponding
        /// properties of the <see cref="Alternet.Drawing.Font"/>.</remarks>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> to convert.
        /// Can be <see langword="null"/>.</param>
        /// <returns>An <see cref="Alternet.Drawing.Font"/> object that represents
        /// the converted font, or <see langword="null"/>
        /// if <paramref name="winFont"/> is <see langword="null"/>.</returns>
        public static Alternet.Drawing.Font? ToAlternet(this System.Drawing.Font? winFont, float scale)
        {
            if (winFont is null)
                return null;
            var size = FontSizeToAlternet(winFont.SizeInPoints, scale);
            return new Alternet.Drawing.Font(
                winFont.Name,
                size,
                (Alternet.Drawing.FontStyle)winFont.Style);
        }

        /// <summary>
        /// Converts an array of <see cref="System.Drawing.Point"/> objects to an array
        /// of  <see cref="Drawing.PointD"/>
        /// objects.
        /// </summary>
        /// <param name="points">The array of <see cref="System.Drawing.Point"/> objects to convert.
        /// This parameter cannot be <see
        /// langword="null"/>.</param>
        /// <returns>An array of <see cref="Drawing.PointD"/> objects representing the converted points.
        /// The array will be empty
        /// if the input array is empty.</returns>
        public static Drawing.PointD[] ToAlternet(this System.Drawing.Point[] points)
        {
            return points.Select(p => new Drawing.PointD(p.X, p.Y)).ToArray();
        }

        /// <summary>
        /// Ensures that the specified <see cref="Alternet.Drawing.Font"/> instance corresponds
        /// to the given <see cref="System.Drawing.Font"/> instance.
        /// </summary>
        /// <remarks>This method ensures that the <paramref name="uiFont"/> parameter
        /// represents the same font as <paramref name="winFont"/>. If <paramref name="uiFont"/> is
        /// already equivalent to <paramref name="winFont"/>, no changes are made.</remarks>
        /// <param name="uiFont">A reference to an <see cref="Alternet.Drawing.Font"/> instance.
        /// If <paramref name="winFont"/> is <see langword="null"/>,
        /// this will be set to <see langword="null"/>. Otherwise, it will be updated to match
        /// <paramref name="winFont"/> if they are not already equivalent.</param>
        /// <param name="winFont">The <see cref="System.Drawing.Font"/> instance to synchronize with.
        /// If <see langword="null"/>, <paramref name="uiFont"/>
        /// will be set to <see langword="null"/>.</param>
        public static void EnsureFont(
            ref Alternet.Drawing.Font? uiFont,
            System.Drawing.Font? winFont,
            float scale = 1)
        {
            if (winFont == null)
            {
                uiFont = null;
                return;
            }

            if(uiFont is null)
            {
                uiFont = winFont.ToAlternet(scale);
            }
            else
            {
                if (!Equals(uiFont, winFont, scale))
                    uiFont = winFont.ToAlternet(scale);
            }
        }

        /// <summary>
        /// Gets or sets the color mode for the application.
        /// </summary>
        /// <remarks>Use this property to retrieve or modify the application's color mode, which
        /// determines the visual appearance of the application.
        /// Setting this property updates the color mode
        /// globally for the application.</remarks>
        public static Alternet.UI.SystemColorModeType ApplicationColorMode
        {
            get
            {
#if NET9_0_OR_GREATER
                return (Alternet.UI.SystemColorModeType)Application.ColorMode;
#else
                return applicationColorMode;
#endif
            }

            set
            {
#if NET9_0_OR_GREATER
                Application.SetColorMode((System.Windows.Forms.SystemColorMode)value);
#else
                applicationColorMode = value;
#endif
            }
        }

        /// <summary>
        /// Gets the current system color mode used by the application.
        /// </summary>
        /// <remarks>This property reflects the application's system color mode, which determines the
        /// color scheme (e.g., light or dark mode) used by the application.
        /// The value is derived from the underlying
        /// system settings.</remarks>
        public static Alternet.UI.SystemColorModeType ApplicationSystemColorMode
        {
            get
            {
#if NET9_0_OR_GREATER
                return (Alternet.UI.SystemColorModeType)Application.SystemColorMode;
#else
                return applicationSystemColorMode;
#endif
            }

#if NET9_0_OR_GREATER
#else
            set
            {
                applicationSystemColorMode = value;
            }
#endif
        }

        /// <summary>
        /// Gets a value indicating whether the application is currently in dark mode.
        /// </summary>
        public static bool ApplicationIsDarkMode
        {
            get
            {
#if NET9_0_OR_GREATER
                return Application.IsDarkModeEnabled;
#else
                return isDarkModeEnabled;
#endif
            }

#if NET9_0_OR_GREATER
#else
            set
            {
                isDarkModeEnabled = value;
            }
#endif
        }

        /// <summary>
        /// Determines whether the specified <see cref="Bitmap"/> is compatible with SkiaSharp.
        /// </summary>
        /// <param name="bitmap">The <see cref="Bitmap"/> to evaluate.
        /// Must not be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the <paramref name="bitmap"/>
        /// has a non-zero width and height  and uses a pixel
        /// format compatible with SkiaSharp
        /// (either <see cref="System.Drawing.Imaging.PixelFormat.Format32bppPArgb"/> 
        /// or <see cref="System.Drawing.Imaging.PixelFormat.Format32bppArgb"/>); otherwise,
        /// <see langword="false"/>.</returns>
        public static bool IsSkiaCompatibleBitmap(Bitmap bitmap)
        {
            if (bitmap == null || bitmap.Width == 0 || bitmap.Height == 0)
                return false;

            if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppPArgb
                || bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb)
                return true;

            return false;
        }

        /// <summary>
        /// Executes the specified action with a locked <see cref="SKImage"/> representation
        /// of the provided <see cref="Bitmap"/>.
        /// </summary>
        /// <remarks>This method locks the provided <see cref="Bitmap"/> in read-only mode,
        /// creates an <see cref="SKImage"/> from its pixel data,
        /// and ensures that the bitmap is unlocked after the action
        /// completes. The <see cref="SKImage"/> is disposed automatically
        /// after the action is executed.</remarks>
        /// <param name="bitmap">The <see cref="Bitmap"/> to lock and convert
        /// into an <see cref="SKImage"/>.</param>
        /// <param name="action">The action to perform on
        /// the <see cref="SKImage"/>. The <see cref="SKImage"/> is valid only within the scope
        /// of this action.</param>
        public static bool WithLockedImage(Bitmap bitmap, Action<SKImage> action)
        {
            if(!IsSkiaCompatibleBitmap(bitmap))
                return false;

            var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            System.Drawing.Imaging.BitmapData bmpData
                = bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                var info = new SKImageInfo(
                    bitmap.Width,
                    bitmap.Height,
                    SKColorType.Bgra8888, SKAlphaType.Premul);
                var pixMap = new SKPixmap(info, bmpData.Scan0);
                using var skImage = SKImage.FromPixels(pixMap);
                action(skImage);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                bitmap.UnlockBits(bmpData);
            }
        }

        /// <summary>
        /// Draws a bitmap onto the specified <see cref="SKCanvas"/> at the given position.
        /// </summary>
        /// <param name="canvas">The <see cref="SKCanvas"/> on which the bitmap will be drawn.
        /// Cannot be <c>null</c>.</param>
        /// <param name="bitmap">The <see cref="Bitmap"/> to draw. Cannot be <c>null</c>.</param>
        /// <param name="point">The <see cref="SKPoint"/> specifying the position on the
        /// canvas where the bitmap will be drawn.</param>
        /// <returns><see langword="true"/> if the bitmap was successfully drawn;
        /// otherwise, <see langword="false"/>.</returns>
        public static bool DrawBitmapOnCanvas(SKCanvas canvas, Bitmap bitmap, float x, float y)
        {
            var result = WithLockedImage(bitmap, (image) => {
                canvas.DrawImage(image, x, y);
            });
            
            return result;
        }
    }
}
