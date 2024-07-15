using System;
using System.Runtime.CompilerServices;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Determines the sizes and locations of displays connected to the system.
    /// </summary>
    public class Display : HandledObject<IDisplayHandler>
    {
        private static IDisplayFactoryHandler? factory;
        private static Coord? maxScaleFactor;
        private static SizeI? baseDPI;

        private SizeI? dpi;
        private Coord? scaleFactor;

        static Display()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// This constructor creates primary display.
        /// </summary>
        public Display()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// </summary>
        /// <param name="index">Display index.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Display(int index)
        {
            Handler = Factory.CreateDisplay(index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// </summary>
        /// <param name="control">Control for which <see cref="Display"/> is created.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Display(Control control)
            : this(GetFromControl(control))
        {
        }

        /// <summary>
        /// Maximal scale factor value for all displays.
        /// </summary>
        public static Coord MaxScaleFactor
        {
            get
            {
                return maxScaleFactor ??= MathUtils.Max(AllScaleFactors);
            }
        }

        /// <summary>
        /// Minimal scale factor value for all displays.
        /// </summary>
        public static Coord MinScaleFactor
        {
            get
            {
                return maxScaleFactor ??= MathUtils.Min(AllScaleFactors);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="IDisplayFactoryHandler"/> object used to perform
        /// display related operations.
        /// </summary>
        public static IDisplayFactoryHandler Factory
        {
            get
            {
                return factory ??= SystemSettings.Handler.CreateDisplayFactoryHandler();
            }

            set
            {
                factory = value;
            }
        }

        /// <summary>
        /// Gets the number of connected displays.
        /// </summary>
        public static int Count => Factory.GetCount();

        /// <summary>
        /// Gets base display resolution for the current platform in pixels per inch.
        /// </summary>
        /// <remarks>
        /// Currently the base DPI is the same in both horizontal and vertical
        /// directions on all platforms and its value is 96 everywhere except under
        /// Apple devices (those running macOS, iOS, watchOS etc), where it is 72.
        /// </remarks>
        public static int BaseDPIValue => BaseDPI.Width;

        /// <summary>
        ///  Gets an array of all of the displays on the system.
        /// </summary>
        public static Display[] AllScreens
        {
            get
            {
                // Do not keep displays in memory,
                // otherwise when DPI is changed we will have an exception.
                var count = Count;
                var allScreens = new Display[count];
                for (int i = 0; i < count; i++)
                {
                    allScreens[i] = new Display(i);
                }

                return allScreens;
            }
        }

        /// <summary>
        /// Gets array of scale factors for all displays on the system.
        /// </summary>
        public static Coord[] AllScaleFactors
        {
            get
            {
                var screens = AllScreens;
                var length = screens.Length;
                var result = new Coord[length];

                for (int i = 0; i < length; i++)
                    result[i] = screens[i].ScaleFactor;

                return result;
            }
        }

        /// <summary>
        /// Gets array of DPI values for all displays on the system.
        /// </summary>
        public static int[] AllDPI
        {
            get
            {
                var screens = AllScreens;
                var length = screens.Length;
                var result = new int[length];

                for (int i = 0; i < length; i++)
                    result[i] = screens[i].DPI.Height;

                return result;
            }
        }

        /// <summary>
        /// Minimal DPI value for all displays.
        /// </summary>
        public static int MinDPI
        {
            get
            {
                return MathUtils.Min(AllDPI);
            }
        }

        /// <summary>
        /// Maximal DPI value for all displays.
        /// </summary>
        public static int MaxDPI
        {
            get
            {
                return MathUtils.Max(AllDPI);
            }
        }

        /// <summary>
        /// Gets whether system has displays with different DPI values.
        /// </summary>
        public static bool HasDifferentDPI => MaxDPI != MinDPI;

        /// <summary>
        /// Gets primary display.
        /// </summary>
        public static Display Primary
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Display();
            }
        }

        /// <summary>
        /// Gets default base display resolution for the current platform as <see cref="SizeI"/>.
        /// </summary>
        /// <remarks>
        /// Currently the base DPI is the same in both horizontal and vertical
        /// directions on all platforms and its value is 96 everywhere except under
        /// Apple devices (those running macOS, iOS, watchOS etc), where it is 72.
        /// </remarks>
        public static SizeI BaseDPI => baseDPI ??= Factory.GetDefaultDPI();

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        /// <remarks>Same as <see cref="Name"/></remarks>
        public string DeviceName => Name;

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        /// <remarks>Same as <see cref="DeviceName"/></remarks>
        public string Name => Handler.GetName();

        /// <summary>
        /// Gets whether this <see cref="Display"/> object is ok.
        /// </summary>
        public bool IsOk => Handler.IsOk;

        /// <summary>
        /// Gets display resolution in pixels per inch.
        /// </summary>
        public SizeI DPI
        {
            get
            {
                return dpi ??= GraphicsFactory.ScaleFactorToDpi(ScaleFactor);
            }
        }

        /// <summary>
        /// Gets scaling factor used by this display.
        /// </summary>
        public Coord ScaleFactor
        {
            get
            {
                return scaleFactor ??= Handler.GetScaleFactor();
            }
        }

        /// <summary>
        /// Gets <c>true</c> if the display is the primary display.
        /// </summary>
        public bool IsPrimary => Handler.IsPrimary();

        /// <summary>
        /// Gets the client area of the display.
        /// </summary>
        public RectI ClientArea => Handler.GetClientArea();

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
        /// Gets the client area of the display in the device-independent units.
        /// </summary>
        public RectD ClientAreaDip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return PixelToDip(ClientArea);
            }
        }

        /// <summary>
        /// Returns the bounding rectangle of the display in pixels.
        /// </summary>
        public RectI Geometry => Handler.GetGeometry();

        /// <summary>
        /// Returns the bounding rectangle of the display in the
        /// device-independent units.
        /// </summary>
        public RectD GeometryDip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return PixelToDip(Geometry);
            }
        }

        /// <summary>
        /// Gets display with the specified index.
        /// </summary>
        /// <param name="index">Index of the display.</param>
        /// <returns></returns>
        public static Display GetDisplay(int index)
        {
            return new Display(index);
        }

        /// <summary>
        /// Returns the index of the display on which the given point lies,
        /// or -1 if the point is not on any connected display.
        /// </summary>
        /// <param name="pt">Point for which index of display is returned.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetFromPoint(PointI pt) => Factory.GetFromPoint(pt);

        /// <summary>
        /// Returns the index of the display on which the given control lies.
        /// </summary>
        /// <param name="control">Control for which index of display is returned.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetFromControl(Control control)
        {
            return Factory.GetFromControl(control);
        }

        /// <summary>
        /// Resets all internal structures.
        /// </summary>
        public static void Reset()
        {
            maxScaleFactor = null;
        }

        /// <summary>
        /// Logs display related information.
        /// </summary>
        public static void Log()
        {
            var method = LogUtils.GetLogMethod(false);

            method(LogUtils.SectionSeparator);
            method("Display:");
            method($"Count: {Count}");
            method($"DefaultDPI: {GraphicsFactory.DefaultDPI}");

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
        /// Converts pixels to device-independent units.
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <returns></returns>
        public Coord PixelToDip(int value)
        {
            return GraphicsFactory.PixelToDip(value, ScaleFactor);
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public int PixelFromDip(Coord value)
        {
            return GraphicsFactory.PixelFromDip(value, ScaleFactor);
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public SizeI PixelFromDip(SizeD value)
        {
            return new(PixelFromDip(value.Width), PixelFromDip(value.Height));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointI PixelFromDip(PointD value)
        {
            return new(PixelFromDip(value.X), PixelFromDip(value.Y));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectI PixelFromDip(RectD value)
        {
            return new(PixelFromDip(value.Location), PixelFromDip(value.Size));
        }

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SizeD PixelToDip(SizeI value)
        {
            return new(PixelToDip(value.Width), PixelToDip(value.Height));
        }

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PointD PixelToDip(PointI value)
        {
            return new(PixelToDip(value.X), PixelToDip(value.Y));
        }

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectD PixelToDip(RectI value)
        {
            return new(PixelToDip(value.Location), PixelToDip(value.Size));
        }

        /// <inheritdoc/>
        protected override IDisplayHandler CreateHandler()
        {
            return Factory.CreateDisplay();
        }
    }
}