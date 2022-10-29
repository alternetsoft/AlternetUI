using System;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Specifies settings that apply to a single, printed page.
    /// </summary>
    /// <remarks>
    /// The <see cref="PageSettings"/> class is used to specify settings that modify the way a page will be printed.
    /// Typically, you set default settings for all pages to be printed through the <see
    /// cref="PrintDocument.DefaultPageSettings"/> property. To specify settings on a page-by-page basis, handle the
    /// <see cref="PrintDocument.PrintPage"/> event and modify the <see cref="PageSettings"/> argument included in the
    /// <see cref="PrintPageEventArgs"/>.
    /// </remarks>
    public class PageSettings : IDisposable
    {
        private bool isDisposed;

        internal PageSettings(UI.Native.PageSettings nativePageSettings)
        {
            NativePageSettings = nativePageSettings;
        }

        internal UI.Native.PageSettings NativePageSettings { get; private set; }

        /// <summary>
        /// Gets the size of the page, taking into account the page orientation specified by the <see cref="Landscape"/> property.
        /// </summary>
        /// <value>
        /// A <see cref="Rect"/> that represents the length and width, in millimeters, of the page.
        /// </value>
        /// <remarks>
        /// Use the <see cref="Bounds"/> property along with the <see cref="Margins"/> property to calculate the printing area for the page.
        /// </remarks>
        public Rect Bounds { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the page should be printed in color.
        /// </summary>
        /// <value><see langword="true"/> if the page should be printed in color; otherwise, <see langword="false"/>.</value>
        public bool Color { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the page is printed in landscape or portrait orientation.
        /// </summary>
        /// <value><see langword="true"/> if the page should be printed in landscape orientation; otherwise, <see langword="false"/>.</value>
        public bool Landscape { get; set; }

        /// <summary>
        /// Gets or sets the margins for this page.
        /// </summary>
        /// <value>A <see cref="Printing.Margins"/> value that represents the margins, in millimeters, for the page.</value>
        /// <remarks>When handling the <see cref="PrintDocument.PrintPage"/> event, you can use this property along with
        /// the <see cref="Bounds"/> property to calculate the printing area for the page.</remarks>
        public Margins Margins { get; set; }

        /// <summary>
        /// Gets or sets the printer settings associated with the page.
        /// </summary>
        /// <value>
        /// A <see cref="PrinterSettings"/> that represents the printer settings associated with the page.
        /// </value>
        /// <remarks>
        /// You can use the printer settings to find default values for properties of the page that are not set.
        /// </remarks>
        public PrinterSettings PrinterSettings { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the paper size for the page.
        /// </summary>
        /// <value>A <see cref="PaperSize"/> that represents the size of the paper.</value>
        public PaperSize PaperSize { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the printer resolution for the page.
        /// </summary>
        /// <value>
        /// A <see cref="PrinterResolution"/> that specifies the printer resolution for the page.
        /// </value>
        /// <remarks>
        /// A <see cref="PrinterResolution"/> represents the printer resolution of through the
        /// <see cref="PrinterResolution.Kind"/> property, which contains one of the <see cref="PrinterResolutionKind"/>
        /// values.
        /// </remarks>
        public PrinterResolution PrinterResolution { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Releases all resources used by the object.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    NativePageSettings.Dispose();
                    NativePageSettings = null!;
                }

                isDisposed = true;
            }
        }
    }
}