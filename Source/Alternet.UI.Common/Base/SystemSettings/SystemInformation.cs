using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides information about the current system environment.
    /// </summary>
    public static class SystemInformation
    {
        /// <summary>
        /// Specifies the default pixel size used to determine whether two consecutive mouse clicks are considered a
        /// double-click in Windows Forms applications.
        /// </summary>
        /// <remarks>This value represents the maximum distance, in pixels, that the mouse pointer can
        /// move between clicks for the clicks to be recognized as a double-click. It is typically used when
        /// implementing custom mouse handling logic that needs to match Windows Forms double-click behavior.</remarks>
        public const int WinFormsDoubleClickSize = 4;
        
        /// <summary>
        /// Specifies the default maximum interval, in milliseconds, that distinguishes a double-click from two
        /// consecutive single clicks in Windows Forms applications.
        /// </summary>
        /// <remarks>This value is typically used to determine whether two mouse clicks should be
        /// interpreted as a double-click. The default value of 500 milliseconds matches the standard double-click time
        /// used by Windows Forms controls.</remarks>
        public const int WinFormsDoubleClickTime = 500;

        private static int? doubleClickTime;
        private static SizeI? doubleClickSize;

        /// <summary>
        /// Gets or sets the maximum number of milliseconds that can elapse between a first click and
        /// a second click for the OS to consider the mouse action a double-click.
        /// </summary>
        /// <returns>
        /// The maximum amount of time, in milliseconds, that can elapse between a first click
        /// and a second click for the OS to consider the mouse action a double-click.
        /// </returns>
        public static int DoubleClickTime
        {
            get => doubleClickTime ??= SystemSettings.GetMetric(SystemSettingsMetric.DClickMSec);

            set => doubleClickTime = value;
        }

        /// <summary>
        /// Gets the maximum interval, in ticks, that is recognized as a double-click by the system.
        /// </summary>
        /// <remarks>This value corresponds to the system's double-click time setting, converted from
        /// milliseconds to ticks. It can be used to determine whether two input events should be interpreted as a
        /// double-click based on their timing.</remarks>
        public static long DoubleClickTimeInTicks
        {
            get
            {
                return DateUtils.TicksFromMilliseconds(DoubleClickTime);
            }
        }

        /// <summary>
        /// Size of rectangle within which two successive mouse clicks must fall
        /// to generate a double-click. This property returns size in pixels.
        /// In order to get size in dips, use <see cref="DoubleClickSizeForControl"/>
        /// or <see cref="DoubleClickSizeDips"/>.
        /// </summary>
        public static SizeI DoubleClickSize
        {
            get
            {
                if (doubleClickSize is null)
                {
                    doubleClickSize = new SizeI(
                        SystemSettings.GetMetric(SystemSettingsMetric.DClickX),
                        SystemSettings.GetMetric(SystemSettingsMetric.DClickY));
                }

                return doubleClickSize.Value;
            }

            set
            {
                doubleClickSize = value;
            }
        }

        /// <summary>
        /// Gets the number of lines to scroll when the mouse wheel is rotated.
        /// </summary>
        /// <returns>
        /// The number of lines to scroll on a mouse wheel rotation, or -1 if the
        /// "One screen at a time" mouse option is selected.</returns>
        public static int MouseWheelScrollLines { get; set; } = 3;

        /// <summary>
        /// Gets the size, in dips, of the working area of the screen.
        /// Do not use this property if there are multiple displays with different DPI settings.
        /// </summary>
        /// <returns>A <see cref="RectD" /> that represents the size, in dips, of
        /// the working area of the screen.</returns>
        public static RectD WorkingArea
        {
            get
            {
                return Display.Primary.ClientAreaDip;
            }
        }

        /// <summary>
        /// Logs the current system settings values to the application log.
        /// </summary>
        /// <remarks>This method records the values for diagnostic or informational purposes. It is typically used to
        /// assist with debugging or to capture the current system settings.</remarks>
        public static void Log()
        {
            App.LogBeginSection("System Information");
            App.Log($"DoubleClickTime: {DoubleClickTime}");
            App.Log($"DoubleClickSize: {DoubleClickSize}");
            App.Log($"MouseWheelScrollLines: {MouseWheelScrollLines}");
            App.Log($"WorkingArea: {WorkingArea}");
            App.LogEndSection();
        }

        /// <summary>
        /// Determines whether the specified points are within
        /// the system-defined double-click area for the given control.
        /// </summary>
        /// <remarks>The double-click area is based on the system settings for the specified control,
        /// which may vary depending on user configuration or control type.</remarks>
        /// <param name="first">The first point to compare, typically representing the location of the initial click.</param>
        /// <param name="second">The second point to compare, typically representing the location of a subsequent click.</param>
        /// <param name="control">The control for which the double-click area is determined. Used to retrieve the appropriate double-click
        /// size.</param>
        /// <returns>true if the second point is within the double-click area centered on the first point for the specified
        /// control; otherwise, false.</returns>
        public static bool IsWithinDoubleClickSize(PointD first, PointD second, AbstractControl control)
        {
            var dd = SystemInformation.DoubleClickSizeForControl(control);
            var rect = new RectD(
                first.X - dd.Width / 2,
                first.Y - dd.Height / 2,
                dd.Width,
                dd.Height);
            var result = rect.Contains(second);
            return result;
        }

        /// <summary>
        /// Returns the recommended double-click hit test size for the specified control,
        /// adjusted for its current scale
        /// factor.
        /// </summary>
        /// <param name="control">The control for which to calculate the double-click hit test size. Cannot be null.</param>
        /// <returns>A <see cref="SizeD"/> representing the width and height, in device-independent pixels, of the double-click
        /// hit test area for the specified control.</returns>
        public static SizeD DoubleClickSizeForControl(AbstractControl control)
        {
            var result = DoubleClickSizeDips(control.ScaleFactor);
            return result;
        }

        /// <summary>
        /// Gets the system double-click size converted to device-independent pixels (DIPs).
        /// </summary>
        /// <param name="scaleFactor">An optional scale factor to use for the conversion.
        /// If null, the system's current scale factor is used.</param>
        /// <returns>A <see cref="SizeD"/> representing the double-click size in device-independent pixels.</returns>
        public static SizeD DoubleClickSizeDips(float? scaleFactor = null)
        {
            var size = DoubleClickSize;
            var result = GraphicsFactory.PixelToDip(size, scaleFactor);
            return result;
        }
    }
}
