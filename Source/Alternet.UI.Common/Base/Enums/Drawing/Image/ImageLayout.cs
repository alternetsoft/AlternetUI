namespace Alternet.Drawing;

/// <summary>
///  Specifies the position of the image on the drawing surface.
/// </summary>
public enum ImageLayout
{
    /// <summary>
    ///  The image is left-aligned at the top across the drawing surface's bounds.
    /// </summary>
    None,

    /// <summary>
    ///  The image is tiled across the drawing surface's bounds.
    /// </summary>
    Tile,

    /// <summary>
    ///  The image is centered within the drawing surface's bounds.
    /// </summary>
    Center,

    /// <summary>
    ///  The image is stretched across the drawing surface's bounds.
    /// </summary>
    Stretch,

    /// <summary>
    ///  The image is enlarged within the drawing surface's bounds.
    /// </summary>
    Zoom,
}