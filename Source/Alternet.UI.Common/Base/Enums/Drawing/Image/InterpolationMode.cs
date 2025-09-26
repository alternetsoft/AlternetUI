namespace Alternet.Drawing
{
    /// <summary>
    /// The <see cref="InterpolationMode"/> enumeration specifies the algorithm that is
    /// used when images are scaled.
    /// </summary>
    public enum InterpolationMode
    {
        /// <summary>
        /// Specifies no interpolation (also known as "nearest-neighbor" interpolation).
        /// </summary>
        None,

        /// <summary>
        /// Specifies low quality interpolation.
        /// </summary>
        LowQuality,

        /// <summary>
        /// Specifies medium quality interpolation.
        /// </summary>
        MediumQuality,

        /// <summary>
        /// Specifies high quality interpolation.
        /// </summary>
        HighQuality,

        /// <summary>
        /// Same as <see cref="LowQuality"/>. Added for compatibility with GDI+.
        /// </summary>
        Low = LowQuality,

        /// <summary>
        /// Same as <see cref="HighQuality"/>. Added for compatibility with GDI+.
        /// </summary>
        High = HighQuality,

        /// <summary>
        /// Same as <see cref="None"/>. Added for compatibility with GDI+.
        /// This is "nearest-neighbor" interpolation.
        /// </summary>
        NearestNeighbor = None,
    }
}