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
        /// <summary>
        /// Gets or sets maximal phone screen diagonal size.
        /// This value is used in <see cref="IsPhoneScreen"/> and other methods.
        /// </summary>
        internal static Coord MaxPhoneScreenDiagonalInch = 7;

        /// <summary>
        /// Gets or sets maximal tablet screen diagonal size.
        /// This value is used in <see cref="IsTabletScreen"/> and other methods.
        /// </summary>
        internal static Coord MaxTabletScreenDiagonalInch = 14;

        private static IDisplayFactoryHandler? factory;
        private static Coord? maxScaleFactor;
        private static Coord? minScaleFactor;
        private static SizeI? baseDPI;
        private static Display? primary;

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
            if (index >= 0 && index < Count)
                Handler = Factory.CreateDisplay(index);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Display"/> class.
        /// </summary>
        /// <param name="control">Control for which <see cref="Display"/> is created.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Display(AbstractControl control)
            : this(GetFromControl(control))
        {
        }

        /// <summary>
        /// Gets whether it is suggested to use large images.
        /// </summary>
        public static bool SuggestedLargeImages
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MaxScaleFactor > SizeD.One.Width;
            }
        }

        /// <summary>
        /// Maximal scale factor value for all displays.
        /// </summary>
        public static Coord MaxScaleFactor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if(maxScaleFactor is null)
                {
                    var value = MathUtils.Max(AllScaleFactorsUnsafe());

                    if (value >= 1)
                        maxScaleFactor = value;
                }

                return maxScaleFactor ?? 1;
            }
        }

        /// <summary>
        /// Minimal scale factor value for all displays.
        /// </summary>
        public static Coord MinScaleFactor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (minScaleFactor is null)
                {
                    var value = MathUtils.Min(AllScaleFactorsUnsafe());

                    if (value >= 1)
                        minScaleFactor = value;
                }

                return minScaleFactor ?? 1;
            }
        }

        /// <summary>
        /// Gets whether display factory is initialized and <see cref="Display"/> class
        /// can be used. Normally this returns False when <see cref="SystemSettings.Handler"/>
        /// is not yet assigned.
        /// </summary>
        public static bool HasFactory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return App.Handler is not null && Primary.HasValidScaleFactor;
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                factory = value;
            }
        }

        /// <summary>
        /// Gets the number of connected displays.
        /// </summary>
        public static int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Factory.GetCount();
            }
        }

        /// <summary>
        /// Gets base display resolution for the current platform in pixels per inch.
        /// </summary>
        /// <remarks>
        /// Currently the base DPI is the same in both horizontal and vertical
        /// directions on all platforms and its value is 96 everywhere except under
        /// Apple devices (those running macOS, iOS, watchOS etc), where it is 72.
        /// </remarks>
        public static int BaseDPIValue
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return BaseDPI.Width;
            }
        }

        /// <summary>
        ///  Gets an array of all of the displays on the system.
        /// </summary>
        public static Display[] AllScreens
        {
            get
            {
                if (Count == 1)
                {
                    return [Primary];
                }
                else
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
        }

        /// <summary>
        /// Gets array of scale factors for all displays on the system.
        /// </summary>
        public static Coord[] AllScaleFactors
        {
            get
            {
                if(Count == 1)
                {
                    return [Primary.ScaleFactor];
                }
                else
                {
                    var screens = AllScreens;
                    var length = screens.Length;
                    var result = new Coord[length];

                    for (int i = 0; i < length; i++)
                        result[i] = screens[i].ScaleFactor;

                    return result;
                }
            }
        }

        /// <summary>
        /// Gets array of DPI values for all displays on the system.
        /// </summary>
        public static int[] AllDPI
        {
            get
            {
                try
                {
                    if (Count == 1)
                    {
                        return [Primary.DPI.Height];
                    }
                    else
                    {
                        var screens = AllScreens;
                        var length = screens.Length;
                        var result = new int[length];

                        for (int i = 0; i < length; i++)
                            result[i] = screens[i].DPI.Height;

                        return result;
                    }
                }
                catch
                {
                    return [96];
                }
            }
        }

        /// <summary>
        /// Minimal DPI value for all displays.
        /// </summary>
        public static int MinDPI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MathUtils.Max(AllDPI);
            }
        }

        /// <summary>
        /// Gets whether system has displays with different DPI values.
        /// </summary>
        public static bool HasDifferentDPI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return MaxDPI != MinDPI;
            }
        }

        /// <summary>
        /// Gets primary display.
        /// </summary>
        public static Display Primary
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return primary ??= new Display();
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
        public static SizeI BaseDPI
        {
            get
            {
                return baseDPI ??= Factory.GetDefaultDPI();
            }
        }

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        /// <remarks>Same as <see cref="Name"/></remarks>
        public string DeviceName
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Name;
            }
        }

        /// <summary>
        /// Gets the display's name.
        /// </summary>
        /// <remarks>Same as <see cref="DeviceName"/></remarks>
        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Handler.GetName();
            }
        }

        /// <summary>
        /// Gets whether this <see cref="Display"/> object is ok.
        /// </summary>
        public bool IsOk
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Handler.IsOk;
            }
        }

        /// <summary>
        /// Gets display resolution in pixels per inch.
        /// </summary>
        public SizeI DPI
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GraphicsFactory.ScaleFactorToDpi(ScaleFactor);
            }
        }

        /// <summary>
        /// Gets scaling factor value returned by the platform.
        /// Please use <see cref="ScaleFactor"/> instead of this property.
        /// </summary>
        public Coord UnsafeScaleFactor
        {
            get
            {
                return Handler.GetScaleFactor();
            }
        }

        /// <summary>
        /// Gets whether scale factor information reported
        /// by <see cref="UnsafeScaleFactor"/> is valid.
        /// </summary>
        public bool HasValidScaleFactor
        {
            get
            {
                return UnsafeScaleFactor >= 1;
            }
        }

        /// <summary>
        /// Gets scaling factor used by this display.
        /// </summary>
        public Coord ScaleFactor
        {
            get
            {
                if (scaleFactor >= 1)
                    return scaleFactor.Value;

                var newScaleFactor = UnsafeScaleFactor;

                if (newScaleFactor >= 1)
                    scaleFactor = newScaleFactor;

                return Math.Max(1, newScaleFactor);
            }
        }

        /// <summary>
        /// Gets <c>true</c> if the display is the primary display.
        /// </summary>
        public bool IsPrimary
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Handler.IsPrimary();
            }
        }

        /// <summary>
        /// Gets the client area of the display.
        /// </summary>
        public RectI ClientArea
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Handler.GetClientArea();
            }
        }

        /// <summary>
        /// Returns the bounding rectangle of the display in pixels.
        /// </summary>
        /// <remarks>Same as <see cref="Geometry"/></remarks>
        public RectI Bounds
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Geometry;
            }
        }

        /// <summary>
        /// Returns the bounding rectangle of the display in dips.
        /// </summary>
        /// <remarks>Same as <see cref="GeometryDip"/></remarks>
        public RectD BoundsDip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return GeometryDip;
            }
        }

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
        public RectI Geometry
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return Handler.GetGeometry();
            }
        }

        /// <summary>
        /// Returns the bounding rectangle of the display in the
        /// device-independent units.
        /// </summary>
        /// <remarks>Same as <see cref="BoundsDip"/></remarks>
        public RectD GeometryDip
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return PixelToDip(Geometry);
            }
        }

        /// <summary>
        /// Returns the bounding rectangle of the display in inches.
        /// </summary>
        internal SizeD SizeInch
        {
            get
            {
                return GraphicsFactory.PixelToInch(Bounds.Size);
            }
        }

        /// <summary>
        /// Gets display diagonal (inches).
        /// </summary>
        internal Coord DiagonalSizeInInches
        {
            get
            {
                return SizeInch.Diagonal;
            }
        }

        /// <summary>
        /// Gets whether this display has diagonal less or
        /// equal to <see cref="MaxPhoneScreenDiagonalInch"/>.
        /// </summary>
        internal bool IsPhoneScreen
        {
            get
            {
                return DiagonalSizeInInches <= MaxPhoneScreenDiagonalInch;
            }
        }

        /// <summary>
        /// Gets whether this display has diagonal
        /// greater than <see cref="MaxPhoneScreenDiagonalInch"/>
        /// and less or equal to <see cref="MaxTabletScreenDiagonalInch"/>.
        /// </summary>
        internal bool IsTabletScreen
        {
            get
            {
                var diagonal = DiagonalSizeInInches;

                return diagonal <= MaxTabletScreenDiagonalInch && !IsPhoneScreen;
            }
        }

        /// <summary>
        /// Gets whether this display has diagonal
        /// greater than <see cref="MaxTabletScreenDiagonalInch"/>.
        /// </summary>
        internal bool IsDesktopScreen
        {
            get
            {
                return DiagonalSizeInInches > MaxTabletScreenDiagonalInch;
            }
        }

        /// <summary>
        /// Gets display with the specified index.
        /// </summary>
        /// <param name="index">Index of the display.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Display GetDisplay(int index)
        {
            return new Display(index);
        }

        /// <summary>
        /// Gets the specified display if it is not null; otherwise returns
        /// display of the <see cref="App.SafeWindow"/>.
        /// </summary>
        /// <param name="display">Display to return if it is not null.</param>
        /// <returns></returns>
        public static Display SafeDisplay(Display? display)
        {
            if (display is null)
            {
                var mainWindow = App.SafeWindow;

                if (mainWindow is null)
                    display = Display.Primary;
                else
                    display = new Display(mainWindow);
            }

            return display;
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
        public static int GetFromControl(AbstractControl control)
        {
            return Factory.GetFromControl(control);
        }

        /// <summary>
        /// Resets all internal structures.
        /// </summary>
        public static void Reset()
        {
            baseDPI = null;
            maxScaleFactor = null;
            minScaleFactor = null;
            primary = null;
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
            method($"DeviceType: {App.DeviceType}");
            method($"IsOpenGLAvailable: {GraphicsFactory.IsOpenGLAvailable}");

            for (int i = 0; i < Display.AllScreens.Length; i++)
            {
                method(StringUtils.OneSpace);
                var display = Display.AllScreens[i];
                method($"Index: {i}");
                method($"Name: {display.DeviceName}");
                method($"DPI: {display.DPI}");
                method($"ScaleFactor: {display.ScaleFactor}");
                method($"IsPrimary: {display.IsPrimary}");
                method($"IsVertical: {display.IsVertical}");
                method($"ClientArea: {display.ClientArea}");
                method($"Diagonal (inch): {display.SizeInch.Diagonal}");
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

        private static Coord[] AllScaleFactorsUnsafe()
        {
            if (Count == 1)
            {
                return [Primary.UnsafeScaleFactor];
            }
            else
            {
                var screens = AllScreens;
                var length = screens.Length;
                var result = new Coord[length];

                for (int i = 0; i < length; i++)
                    result[i] = screens[i].UnsafeScaleFactor;

                return result;
            }
        }
    }
}