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
        /// A generic Sans Serif <see cref="FontFamily"/> object.
        /// </summary>        
        SansSerif = 1, // wxFONTFAMILY_SWISS

        /// <summary>
        /// A generic Serif <see cref="FontFamily"/> object.
        /// </summary>
        Serif = 2, // wxFONTFAMILY_ROMAN

        /// <summary>
        /// A generic Monospace <see cref="FontFamily"/> object.
        /// </summary>
        Monospace = 3, // wxFONTFAMILY_TELETYPE

        /// <summary>
        /// Default <see cref="FontFamily"/> object.
        /// </summary>
        Default = 4, // wxFONTFAMILY_DEFAULT
    }
}