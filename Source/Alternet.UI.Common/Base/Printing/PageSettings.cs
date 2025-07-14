using System;
using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies settings that apply to a single, printed page.
    /// </summary>
    /// <remarks>
    /// The <see cref="PageSettings"/> class is used to specify settings that modify the way
    /// a page will be printed.
    /// Typically, you set default settings for all pages to be printed through the
    /// <see cref="PrintDocument.PageSettings"/> property. To specify settings on a page-by-page
    /// basis, handle the
    /// <see cref="PrintDocument.PrintPage"/> event and modify the <see cref="PageSettings"/>
    /// argument included in the
    /// <see cref="PrintPageEventArgs"/>.
    /// </remarks>
    public class PageSettings : HandledObject<IPageSettingsHandler>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageSettings"/> class.
        /// </summary>
        public PageSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageSettings"/> class.
        /// </summary>
        public PageSettings(IPageSettingsHandler nativePageSettings)
        {
            Handler = nativePageSettings;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page should be printed in color.
        /// </summary>
        /// <value><see langword="true"/> if the page should be printed in color; otherwise,
        /// <see langword="false"/>.</value>
        public virtual bool Color
        {
            get
            {
                return Handler.Color;
            }

            set
            {
                Handler.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page is printed in landscape or portrait
        /// orientation.
        /// </summary>
        /// <value><see langword="true"/> if the page should be printed in landscape orientation;
        /// otherwise, <see langword="false"/>.</value>
        public virtual bool Landscape
        {
            get
            {
                return Handler.Landscape;
            }

            set
            {
                Handler.Landscape = value;
            }
        }

        /// <summary>
        /// Gets or sets the margins for this page.
        /// </summary>
        /// <value>A <see cref="Thickness"/> value that represents the margins,
        /// in millimeters, for the page.</value>
        public virtual Thickness Margins
        {
            get
            {
                return (Handler.MarginLeft, Handler.MarginTop, Handler.MarginRight, Handler.MarginBottom);
            }

            set
            {
                Handler.MarginLeft = value.Left;
                Handler.MarginTop = value.Top;
                Handler.MarginRight = value.Right;
                Handler.MarginBottom = value.Bottom;
            }
        }

        /// <summary>
        /// Gets or sets the paper size for the page.
        /// </summary>
        /// <value>A <see cref="PaperSize"/> that represents the size of the paper.</value>
        public virtual PaperSize PaperSize
        {
            get
            {
                if (Handler.UseCustomPaperSize)
                    return new PaperSize(Handler.CustomPaperSize);

                return new PaperSize((PaperKind)Handler.PaperSize);
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(PaperSize));

                Handler.UseCustomPaperSize = value.IsCustom;

                if (value.IsCustom)
                {
                    Handler.CustomPaperSize = value.CustomSize;
                }
                else
                {
                    Handler.PaperSize = value.Kind;
                }
            }
        }

        /// <summary>
        /// Gets or sets the printer resolution for the page.
        /// </summary>
        /// <value>
        /// A <see cref="PrinterResolution"/> that specifies the printer resolution for the page.
        /// </value>
        /// <remarks>
        /// A <see cref="PrinterResolution"/> represents the printer resolution of through the
        /// <see cref="PrinterResolution.Kind"/> property, which contains one of
        /// the <see cref="PrinterResolutionKind"/>
        /// values.
        /// </remarks>
        public virtual PrinterResolution PrinterResolution
        {
            get
            {
                return new PrinterResolution((PrinterResolutionKind)Handler.PrinterResolution);
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(PrinterResolution));

                Handler.PrinterResolution = value.Kind;
            }
        }

        /// <inheritdoc/>
        protected override IPageSettingsHandler CreateHandler()
        {
            return PrintingFactory.Handler.CreatePageSettingsHandler();
        }
    }
}