using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Determines the sizes and locations of displays connected to the system.
    /// </summary>
    public class Display : DisposableObject
    {
        private readonly Control? control;

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// </summary>
        public Display()
            : base(Native.WxOtherFactory.CreateDisplay(), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// </summary>
        /// <param name="index">Display index.</param>
        public Display(int index)
            : base(Native.WxOtherFactory.CreateDisplay2((uint)index), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// </summary>
        /// <param name="control">Control for which <see cref="Display"/> is created.</param>
        public Display(Control control)
            : base(Native.WxOtherFactory.CreateDisplay3(control.WxWidget), true)
        {
            this.control = control;
        }

        /// <summary>
        /// Gets the number of connected displays.
        /// </summary>
        public static int Count => (int)Native.WxOtherFactory.DisplayGetCount();

        /// <summary>
        /// Gets default display resolution for the current platform in pixels per inch.
        /// </summary>
        public static int DefaultDPIValue => Native.WxOtherFactory.DisplayGetStdPPIValue();

        /// <summary>
        /// Gets default display resolution for the current platform as <see cref="Int32Size"/>.
        /// </summary>
        public static Int32Size DefaultDPI => Native.WxOtherFactory.DisplayGetStdPPI();

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        public string Name => Native.WxOtherFactory.DisplayGetName(Handle);

        /// <summary>
        /// Gets display resolution in pixels per inch.
        /// </summary>
        public Int32Size DPI => Native.WxOtherFactory.DisplayGetPPI(Handle);

        /// <summary>
        /// Gets scaling factor used by this display.
        /// </summary>
        public double ScaleFactor => Native.WxOtherFactory.DisplayGetScaleFactor(Handle);

        /// <summary>
        /// Gets <c>true</c> if the display is the primary display.
        /// </summary>
        public bool IsPrimary => Native.WxOtherFactory.DisplayIsPrimary(Handle);

        /// <summary>
        /// Gets the client area of the display.
        /// </summary>
        public Int32Rect ClientArea => Native.WxOtherFactory.DisplayGetClientArea(Handle);

        /// <summary>
        /// Gets the client area of the display in the
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        public Rect ClientAreaDip
        {
            get
            {
                return PixelToDip(ClientArea);
            }
        }

        /// <summary>
        /// Returns the bounding rectangle of the display.
        /// </summary>
        public Int32Rect Geometry => Native.WxOtherFactory.DisplayGetGeometry(Handle);

        /// <summary>
        /// Returns the bounding rectangle of the display in the
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        public Rect GeometryDip
        {
            get
            {
                return PixelToDip(Geometry);
            }
        }

        /// <summary>
        /// Returns the index of the display on which the given point lies,
        /// or -1 if the point is not on any connected display.
        /// </summary>
        /// <param name="pt">Point for which index of display is returned.</param>
        public static int GetFromPoint(Int32Point pt) =>
            Native.WxOtherFactory.DisplayGetFromPoint(pt);

        /// <summary>
        /// Returns the index of the display on which the given control lies.
        /// </summary>
        /// <param name="control">Control for which index of display is returned.</param>
        public static int GetFromControl(Control control) =>
            Native.WxOtherFactory.DisplayGetFromWindow(control.WxWidget);

        /// <summary>
        /// Logs display related information.
        /// </summary>
        public static void Log()
        {
            var method = LogUtils.GetLogMethod(false);

            method(LogUtils.SectionSeparator);
            method("Display:");
            method($"Count: {Count}");
            method($"DefaultDPI: {DefaultDPI}");

            for (int i = 0; i < Count; i++)
            {
                method(" ");
                var display = new Display(i);
                method($"Index: {i}");
                method($"Name: {display.Name}");
                method($"DPI: {display.DPI}");
                method($"ScaleFactor: {display.ScaleFactor}");
                method($"IsPrimary: {display.IsPrimary}");
                method($"ClientArea: {display.ClientArea}");
                method($"Geometry: {display.Geometry}");
                method($"PixelToDip(100): {display.PixelToDip(100)}");
            }

            method(LogUtils.SectionSeparator);
        }

        /// <summary>
        /// Converts pixels to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <returns></returns>
        public double PixelToDip(int value)
        {
            if (control is null)
            {
                return value / ScaleFactor;
            }
            else
                return control.PixelToDip(value);
        }

        /// <summary>
        /// Converts <see cref="Int32Size"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="Int32Size"/> in pixels.</param>
        /// <returns></returns>
        public Size PixelToDip(Int32Size value)
        {
            return new(PixelToDip(value.Width), PixelToDip(value.Height));
        }

        /// <summary>
        /// Converts <see cref="Int32Point"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="Int32Point"/> in pixels.</param>
        /// <returns></returns>
        public Point PixelToDip(Int32Point value)
        {
            return new(PixelToDip(value.X), PixelToDip(value.Y));
        }

        /// <summary>
        /// Converts <see cref="Int32Rect"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="Int32Rect"/> in pixels.</param>
        /// <returns></returns>
        public Rect PixelToDip(Int32Rect value)
        {
            return new(PixelToDip(value.Location), PixelToDip(value.Size));
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteDisplay(Handle);
        }
    }
}