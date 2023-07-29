namespace Alternet.Drawing
{
    /// <summary>
    /// The <see cref="InterpolationMode"/> enumeration specifies the algorithm that is used when images are scaled.
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
    }
}