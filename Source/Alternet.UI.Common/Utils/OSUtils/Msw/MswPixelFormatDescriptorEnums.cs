using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Flags used in the Win32 PIXELFORMATDESCRIPTOR structure for OpenGL and GDI rendering.
    /// </summary>
    public static class MswPixelFormatDescriptorEnums
    {
        /// <summary>
        /// Specifies a set of flags that define the capabilities and behaviors of a pixel format.
        /// </summary>
        /// <remarks>This enumeration is decorated with the <see cref="FlagsAttribute"/>, allowing a
        /// bitwise combination of its member values. It is commonly used to describe the features supported by a
        /// rendering context or a pixel format in graphics programming.</remarks>
        [Flags]
        public enum Flags : uint
        {
            /// <summary>Enables double buffering.</summary>
            DoubleBuffer = 0x00000001,

            /// <summary>Enables stereo buffering.</summary>
            Stereo = 0x00000002,

            /// <summary>Indicates the format can be drawn to a window (HWND).</summary>
            DrawToWindow = 0x00000004,

            /// <summary>Indicates the format can be drawn to a memory bitmap.</summary>
            DrawToBitmap = 0x00000008,

            /// <summary>Supports GDI drawing.</summary>
            SupportGdi = 0x00000010,

            /// <summary>Supports OpenGL rendering.</summary>
            SupportOpenGl = 0x00000020,

            /// <summary>Indicates a generic pixel format not accelerated by hardware.</summary>
            GenericFormat = 0x00000040,

            /// <summary>Requires a palette for color management.</summary>
            NeedPalette = 0x00000080,

            /// <summary>Requires the system palette.</summary>
            NeedSystemPalette = 0x00000100,

            /// <summary>Uses swap exchange method for buffer swapping.</summary>
            SwapExchange = 0x00000200,

            /// <summary>Uses swap copy method for buffer swapping.</summary>
            SwapCopy = 0x00000400,

            /// <summary>Supports swapping of layer buffers.</summary>
            SwapLayerBuffers = 0x00000800,

            /// <summary>Indicates hardware acceleration via a generic driver.</summary>
            GenericAccelerated = 0x00001000,

            /// <summary>Supports DirectDraw acceleration (legacy).</summary>
            SupportDirectDraw = 0x00002000,

            /// <summary>Supports composition under DWM (Windows Vista+).</summary>
            SupportComposition = 0x00008000,
        }

        /// <summary>
        /// Specifies the pixel format type used in a PIXELFORMATDESCRIPTOR.
        /// </summary>
        public enum PixelType : byte
        {
            /// <summary>
            /// RGBA format. Each pixel contains red, green, blue, and alpha components.
            /// </summary>
            Rgba = 0,

            /// <summary>
            /// Color-index format. Each pixel is an index into a palette of colors.
            /// </summary>
            ColorIndex = 1,
        }

        /// <summary>
        /// Specifies the layer type for a PIXELFORMATDESCRIPTOR.
        /// </summary>
        public enum PixelLayerType : byte
        {
            /// <summary>
            /// The main drawing plane. This is the default and most commonly used layer.
            /// </summary>
            MainPlane = 0,

            /// <summary>
            /// An overlay plane that appears above the main framebuffer. Used for layered rendering.
            /// </summary>
            OverlayPlane = 1,

            /// <summary>
            /// An underlay plane that appears beneath the main framebuffer. Rarely used.
            /// </summary>
            UnderlayPlane = 255,
        }
    }
}
