namespace Alternet.Drawing
{
    /// <summary>
    /// Specifies a generic <see cref="FontFamily"/> object.
    /// </summary>
    public enum GenericFontFamily
    {
        /// <summary>
        /// Not specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// A generic 'Sans Serif' (Swiss) <see cref="FontFamily"/> object.
        /// </summary>
        SansSerif = 1,

        /// <summary>
        /// A generic 'Serif' (Roman) <see cref="FontFamily"/> object.
        /// </summary>
        Serif = 2,

        /// <summary>
        /// A generic 'Monospace' (Teletype, fixed pitch font) <see cref="FontFamily"/> object.
        /// </summary>
        Monospace = 3,

        /// <summary>
        /// Default <see cref="FontFamily"/> object.
        /// </summary>
        Default = 4,
    }
}