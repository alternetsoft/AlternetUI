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
    public class PageSettings : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageSettings"/> class.
        /// </summary>
        public PageSettings()
            : this(new UI.Native.PageSettings())
        {
        }

        internal PageSettings(UI.Native.PageSettings nativePageSettings)
        {
            NativePageSettings = nativePageSettings;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page should be printed in color.
        /// </summary>
        /// <value><see langword="true"/> if the page should be printed in color; otherwise,
        /// <see langword="false"/>.</value>
        public bool Color
        {
            get
            {
                return NativePageSettings.Color;
            }

            set
            {
                NativePageSettings.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the page is printed in landscape or portrait
        /// orientation.
        /// </summary>
        /// <value><see langword="true"/> if the page should be printed in landscape orientation;
        /// otherwise, <see langword="false"/>.</value>
        public bool Landscape
        {
            get
            {
                return NativePageSettings.Landscape;
            }

            set
            {
                NativePageSettings.Landscape = value;
            }
        }

        /// <summary>
        /// Gets or sets the margins for this page.
        /// </summary>
        /// <value>A <see cref="Thickness"/> value that represents the margins,
        /// in millimeters, for the page.</value>
        public Thickness Margins
        {
            get
            {
                return NativePageSettings.Margins;
            }

            set
            {
                NativePageSettings.Margins = value;
            }
        }

        /// <summary>
        /// Gets or sets the paper size for the page.
        /// </summary>
        /// <value>A <see cref="PaperSize"/> that represents the size of the paper.</value>
        public PaperSize PaperSize
        {
            get
            {
                if (NativePageSettings.UseCustomPaperSize)
                    return new PaperSize(NativePageSettings.CustomPaperSize);

                return new PaperSize((PaperKind)NativePageSettings.PaperSize);
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                NativePageSettings.UseCustomPaperSize = value.IsCustom;

                if (value.IsCustom)
                {
                    NativePageSettings.CustomPaperSize = value.CustomSize;
                }
                else
                {
                    NativePageSettings.PaperSize = (UI.Native.PaperKind)value.Kind;
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
        /// <see cref="PrinterResolution.Kind"/> property, which contains one of the <see cref="PrinterResolutionKind"/>
        /// values.
        /// </remarks>
        public PrinterResolution PrinterResolution
        {
            get
            {
                return new PrinterResolution((PrinterResolutionKind)NativePageSettings.PrinterResolution);
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                NativePageSettings.PrinterResolution = (UI.Native.PrinterResolutionKind)value.Kind;
            }
        }

        internal UI.Native.PageSettings NativePageSettings { get; private set; }

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