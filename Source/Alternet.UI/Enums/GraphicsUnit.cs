namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies the unit of measure for the data.
    /// </summary>
    public enum GraphicsUnit
    {
        /// <summary>
        /// Specifies the world coordinate system unit as the unit of measure.
        /// </summary>
        World, // World coordinate (non-physical unit).

        /// <summary>
        /// Specifies the unit of measure of the display device.
        /// Typically pixels for video displays, and 1/100 inch for printers.
        /// </summary>
        Display, // Variable - for PageTransform only.

        /// <summary>
        /// Specifies a device pixel as the unit of measure.
        /// </summary>
        Pixel, // Each unit is one device pixel.

        /// <summary>
        /// Specifies a printer's point (1/72 inch) as the unit of measure.
        /// </summary>
        Point, // Each unit is a printer's point, or 1/72 inch.

        /// <summary>
        /// Specifies the inch as the unit of measure.
        /// </summary>
        Inch, // Each unit is 1 inch.

        /// <summary>
        /// Specifies the document unit (1/300 inch) as the unit of measure.
        /// </summary>
        Document, // Each unit is 1/300 inch.

        /// <summary>
        /// Specifies the millimeter as the unit of measure.
        /// 1 inch = 25.4 millimeters. 1 inch = 2.54 centimeters. 1 centimeter = 10 millimeters.
        /// </summary>
        Millimeter, // Each unit is 1 millimeter.

        /// <summary>
        /// Specifies a device-independent point (1/96 inch) as the unit of measure.
        /// </summary>
        Dip, // Each unit is 1/96 inch.
    }
}