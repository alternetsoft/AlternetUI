using System;
using System.Drawing.Printing;

namespace Alternet.UI
{
    /// <summary>
    /// Lets users select a printer and choose which sections of the document to print from an AlterNET UI application.
    /// </summary>
    /// <remarks>
    /// When you create an instance of <see cref="PrintDialog"/>, the read/write properties are set to initial values.
    /// To get printer settings that are modified by the user with the
    /// <see cref="PrintDialog"/>, use the <see cref="PrinterSettings"/> property.
    /// </remarks>
    public sealed class PrintDialog : CommonDialog
    {
        /// <summary>
        /// Gets or sets a value indicating whether the <b>Pages</b> option button is enabled.
        /// </summary>
        public bool AllowSomePages { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Selection</b> option button is enabled.
        /// </summary>
        public bool AllowSelection { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the <b>Print to file</b> option button is enabled.
        /// </summary>
        public bool AllowPrintToFile { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating whether the the <b>Help</b> button is displayed.
        /// </summary>
        public bool ShowHelp { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets a value indicating the PrintDocument used to obtain <see cref="PrinterSettings"/>.
        /// </summary>
        /// <value>The <see cref="PrintDocument"/> used to obtain <see cref="PrinterSettings"/>. The default is <see langword="null"/>.</value>
        public PrintDocument? Document { get => throw new Exception(); set => throw new Exception(); }

        /// <summary>
        /// Gets or sets the printer settings the dialog box modifies.
        /// </summary>
        public PrinterSettings PrinterSettings { get => throw new Exception(); set => throw new Exception(); }

        private Native.PrintDialog nativeDialog;

        /// <summary>
        /// Initializes a new instance of <see cref="PrintDialog"/>.
        /// </summary>
        public PrintDialog()
        {
            nativeDialog = new Native.PrintDialog();
        }

        private protected override ModalResult ShowModalCore(Window? owner)
        {
            CheckDisposed();
            return ModalResult.Canceled;
            //var nativeOwner = owner == null ? null : ((NativeWindowHandler)owner.Handler).NativeControl;
            //return (ModalResult)nativeDialog.ShowModal(nativeOwner);
        }

        private protected override string? TitleCore
        {
            get
            {
                CheckDisposed();
                return null;
                //return nativeDialog.Title;
            }

            set
            {
                CheckDisposed();
                //nativeDialog.Title = value;
            }
        }
    }
}