using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies the size of a piece of paper.
    /// </summary>
    /// <remarks>
    /// This class is used by the <see cref="PageSettings.PaperSize"/> property to get
    /// the paper to set the paper size
    /// for a page. You can use the <see cref="PaperSize"/> constructor to specify a
    /// custom paper size. The <see cref="SizeD"/> property can be set only for
    /// custom <see cref="PaperSize"/> objects.
    /// </remarks>
    public partial class PaperSize : BaseObject
    {
        private SizeD customSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperSize"/> class with the
        /// specified custom size, in millimeters.
        /// </summary>
        /// <param name="customSize">The custom size of the paper, in millimeters.</param>
        public PaperSize(SizeD customSize)
        {
            IsCustom = true;
            CustomSize = customSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaperSize"/> class with the
        /// specified paper kind.
        /// </summary>
        /// <param name="paperKind">The type of paper.</param>
        public PaperSize(PaperKind paperKind)
        {
            IsCustom = false;
            Kind = paperKind;
        }

        /// <summary>
        /// Initializes a new instance of the PaperSize class with the specified paper kind or a custom size.
        /// </summary>
        /// <remarks>If paperKind is specified, the instance represents a standard paper size. If
        /// customSize is provided and paperKind is null, the instance represents a custom paper size.</remarks>
        /// <param name="paperKind">The standard paper kind to use for the paper size, or null to specify a custom size.</param>
        /// <param name="customSize">The custom paper size to use if paperKind is null. If null, the size is set to an empty value.</param>
        public PaperSize(PaperKind? paperKind, SizeD? customSize)
        {
            IsCustom = customSize.HasValue;
            Kind = paperKind ?? PaperKind.A4;
            CustomSize = customSize ?? SizeD.Empty;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="PaperSize"/> is custom.
        /// </summary>
        public virtual bool IsCustom { get; private set; }

        /// <summary>
        /// Gets the custom size of the paper, in millimeters.
        /// This property is valid only if <see cref="IsCustom"/> is <see langword="true"/>.
        /// </summary>
        public virtual SizeD CustomSize
        {
            get
            {
                return customSize;
            }

            private set
            {
                customSize = value;
            }
        }

        /// <summary>
        /// Gets the type of paper.
        /// This property is valid only if <see cref="IsCustom"/> is <see langword="false"/>.
        /// </summary>
        public virtual PaperKind Kind { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return IsCustom ? $"Custom Size: {CustomSize}" : $"Paper Kind: {Kind}";
        }
    }
}