using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Determines the sizes and locations of displays connected to the system.
    /// </summary>
    public class Display : DisposableObject
    {
        private static Display? primary;
        private static Display[]? allScreens;

        private readonly Control? control;

        static Display()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// This constructor creates primary display.
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
        /// <remarks>
        /// Currently the default DPI is the same in both horizontal and vertical
        /// directions on all platforms and its value is 96 everywhere except under
        /// Apple devices (those running macOS, iOS, watchOS etc), where it is 72.
        /// </remarks>
        public static int DefaultDPIValue => Native.WxOtherFactory.DisplayGetStdPPIValue();

        /// <summary>
        ///  Gets an array of all of the displays on the system.
        /// </summary>
        public static unsafe Display[] AllScreens
        {
            get
            {
                if (allScreens is not null)
                    return allScreens;
                var count = Count;
                allScreens = new Display[count];
                for(int i = 0; i < count; i++)
                {
                    allScreens[i] = new Display(i);
                }

                return allScreens;
            }
        }

        /// <summary>
        /// Gets primary display.
        /// </summary>
        public static Display Primary
        {
            get
            {
                return primary ??= new Display();
            }
        }

        /// <summary>
        /// Gets default display resolution for the current platform as <see cref="SizeI"/>.
        /// </summary>
        /// <remarks>
        /// Currently the default DPI is the same in both horizontal and vertical
        /// directions on all platforms and its value is 96 everywhere except under
        /// Apple devices (those running macOS, iOS, watchOS etc), where it is 72.
        /// </remarks>
        public static SizeI DefaultDPI => Native.WxOtherFactory.DisplayGetStdPPI();

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        /// <remarks>Same as <see cref="Name"/></remarks>
        public string DeviceName => Name;

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        /// <remarks>Same as <see cref="DeviceName"/></remarks>
        public string Name => Native.WxOtherFactory.DisplayGetName(Handle);

        /// <summary>
        /// Gets display resolution in pixels per inch.
        /// </summary>
        public SizeI DPI => Native.WxOtherFactory.DisplayGetPPI(Handle);

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
        public RectI ClientArea => Native.WxOtherFactory.DisplayGetClientArea(Handle);

        /// <summary>
        /// Returns the bounding rectangle of the display in pixels.
        /// </summary>
        /// <remarks>Same as <see cref="Geometry"/></remarks>
        public RectI Bounds => Geometry;

        /// <summary>
        /// Returns the bounding rectangle of the display in dips.
        /// </summary>
        /// <remarks>Same as <see cref="GeometryDip"/></remarks>
        public RectD BoundsDip => GeometryDip;

        /// <summary>
        /// Gets whether display height is bigger than width.
        /// </summary>
        public bool IsVertical
        {
            get
            {
                var clientArea = ClientArea;
                return clientArea.Height > clientArea.Width;
            }
        }

        /// <summary>
        /// Gets the client area of the display in the
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        public RectD ClientAreaDip
        {
            get
            {
                return PixelToDip(ClientArea);
            }
        }

        /// <summary>
        /// Returns the bounding rectangle of the display in pixels.
        /// </summary>
        public RectI Geometry => Native.WxOtherFactory.DisplayGetGeometry(Handle);

        /// <summary>
        /// Returns the bounding rectangle of the display in the
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        public RectD GeometryDip
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
        public static int GetFromPoint(PointI pt) =>
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

            for (int i = 0; i < Display.AllScreens.Length; i++)
            {
                method(" ");
                var display = Display.AllScreens[i];
                method($"Index: {i}");
                method($"Name: {display.DeviceName}");
                method($"DPI: {display.DPI}");
                method($"ScaleFactor: {display.ScaleFactor}");
                method($"IsPrimary: {display.IsPrimary}");
                method($"IsVertical: {display.IsVertical}");
                method($"ClientArea: {display.ClientArea}");
                method($"Bounds: {display.Bounds}");
                method($"BoundsDip: {display.BoundsDip}");
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
                var scaleFactor = ScaleFactor;
                if (scaleFactor == 1)
                    return value;
                else
                    return value / scaleFactor;
            }
            else
                return control.PixelToDip(value);
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public int PixelFromDip(double value)
        {
            if (control is null)
            {
                var scaleFactor = ScaleFactor;
                if (scaleFactor == 1)
                    return (int)value;
                return (int)Math.Round(value * scaleFactor);
            }
            else
                return control.PixelFromDip(value);
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public SizeI PixelFromDip(SizeD value)
        {
            return new(PixelFromDip(value.Width), PixelFromDip(value.Height));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public PointI PixelFromDip(PointD value)
        {
            return new(PixelFromDip(value.X), PixelFromDip(value.Y));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public RectI PixelFromDip(RectD value)
        {
            return new(PixelFromDip(value.Location), PixelFromDip(value.Size));
        }

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        public SizeD PixelToDip(SizeI value)
        {
            return new(PixelToDip(value.Width), PixelToDip(value.Height));
        }

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        public PointD PixelToDip(PointI value)
        {
            return new(PixelToDip(value.X), PixelToDip(value.Y));
        }

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        public RectD PixelToDip(RectI value)
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