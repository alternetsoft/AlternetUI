namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies the size of a piece of paper.
    /// </summary>
    /// <remarks>
    /// This class is used by the <see cref="PageSettings.PaperSize"/> property to get the paper to set the paper size
    /// for a page. You can use the <see cref="PaperSize"/> constructor to specify a custom paper size. The <see cref="SizeD"/> property can be set only for
    /// custom <see cref="PaperSize"/> objects.
    /// </remarks>
    public class PaperSize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaperSize"/> class with the specified custom size, in millimeters.
        /// </summary>
        /// <param name="customSize">The custom size of the paper, in millimeters.</param>
        public PaperSize(SizeD customSize)
        {
            IsCustom = true;
            CustomSize = customSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperSize"/> class with the specified paper kind.
        /// </summary>
        /// <param name="paperKind">The type of paper.</param>
        public PaperSize(PaperKind paperKind)
        {
            IsCustom = false;
            Kind = paperKind;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PaperSize"/> is custom.
        /// </summary>
        public bool IsCustom { get; private set; }

        /// <summary>
        /// Gets the custom size of the paper, in millimeters.
        /// </summary>
        public SizeD CustomSize { get; private set; }

        /// <summary>
        /// Gets the type of paper.
        /// </summary>
        public PaperKind Kind { get; private set; }
    }
}