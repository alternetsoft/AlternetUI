using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to create and manage cursors.
    /// </summary>
    public interface ICursorFactoryHandler : IDisposable
    {
        /// <inheritdoc cref="App.IsBusyCursor"/>
        bool IsBusyCursor();

        /// <inheritdoc cref="App.BeginBusyCursor"/>
        void BeginBusyCursor();

        /// <inheritdoc cref="App.EndBusyCursor"/>
        void EndBusyCursor();

        /// <summary>
        /// Creates cursor handler.
        /// </summary>
        /// <returns></returns>
        ICursorHandler CreateCursorHandler();

        /// <summary>
        /// Creates cursor handler with the specified cursor type.
        /// </summary>
        /// <returns></returns>
        ICursorHandler CreateCursorHandler(CursorType cursor);

        /// <summary>
        /// Creates cursor handler by passing a string resource name or filename.
        /// </summary>
        /// <param name="cursorName">The name of the resource or the image file to load.</param>
        /// <param name="type">Icon type to load.</param>
        /// <param name="hotSpotX">Hotspot x coordinate (relative to the top left of the image).</param>
        /// <param name="hotSpotY">Hotspot y coordinate (relative to the top left of the image).</param>
        /// <returns></returns>
        /// <remarks>
        /// The arguments hotSpotX and hotSpotY are only used when there's no hotspot info
        /// in the resource/image-file to load (e.g. when using icon under Windows or xpm under Linux).
        /// </remarks>
        ICursorHandler CreateCursorHandler(
            string cursorName,
            BitmapType type,
            int hotSpotX,
            int hotSpotY);

        /// <summary>
        /// Create cursor handler for the specified image.
        /// </summary>
        /// <param name="image">Image for the cursor.</param>
        /// <returns></returns>
        /// <remarks>
        /// If cursor are monochrome on the current platform, colors with the RGB elements all greater
        /// than 127 will be foreground, colors less than this background. The mask (if any) will be
        /// used to specify the transparent area.
        /// </remarks>
        /// <remarks>
        /// On Windows the foreground will be white and the background black. If the cursor is
        /// larger than 32x32 it is resized.
        /// </remarks>
        /// <remarks>
        /// On Linux, color cursors and alpha channel are
        /// supported, the cursor will be displayed at the size of the image.
        /// Under MacOs, large cursors are supported.
        /// </remarks>
        /// <param name="hotSpotX">Hotspot x coordinate (relative to the top left of the image).</param>
        /// <param name="hotSpotY">Hotspot y coordinate (relative to the top left of the image).</param>
        ICursorHandler CreateCursorHandler(
            Image image,
            int hotSpotX,
            int hotSpotY);

        /// <summary>
        /// Create cursor handler for the specified image.
        /// </summary>
        /// <param name="image">Image for the cursor.</param>
        /// <param name="hotSpotX">Hotspot x coordinate (relative to the top left of the image).</param>
        /// <param name="hotSpotY">Hotspot y coordinate (relative to the top left of the image).</param>
        ICursorHandler CreateCursorHandler(
            GenericImage image,
            int hotSpotX,
            int hotSpotY);
    }
}
