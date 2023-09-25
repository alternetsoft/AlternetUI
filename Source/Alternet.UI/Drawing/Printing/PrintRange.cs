namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies the printer's duplex setting.
    /// </summary>
    public enum PrintRange
    {
        /// <summary>
        /// All pages are printed.
        /// </summary>
        AllPages,

        /// <summary>
        /// The selected pages are printed.
        /// </summary>
        Selection,

        /// <summary>
        /// The pages between <see cref="PrinterSettings.FromPage"/>
        /// and <see cref="PrinterSettings.ToPage"/> are printed.
        /// </summary>
        SomePages,
    }
}