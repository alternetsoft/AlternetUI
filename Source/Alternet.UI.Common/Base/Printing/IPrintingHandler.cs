using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Contains methods which create printing related interface providers.
    /// </summary>
    public interface IPrintingHandler : IDisposable
    {
        /// <summary>
        /// Creates <see cref="IPrintDocumentHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPrintDocumentHandler CreatePrintDocumentHandler();

        /// <summary>
        /// Creates <see cref="IPrinterSettingsHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPrinterSettingsHandler CreatePrinterSettingsHandler();

        /// <summary>
        /// Creates <see cref="IPrintDialogHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPrintDialogHandler CreatePrintDialogHandler();

        /// <summary>
        /// Creates <see cref="IPageSettingsHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPageSettingsHandler CreatePageSettingsHandler();

        /// <summary>
        /// Creates <see cref="IPageSetupDialogHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPageSetupDialogHandler CreatePageSetupDialogHandler();

        /// <summary>
        /// Creates <see cref="IPrintPreviewDialogHandler"/> interface provider.
        /// </summary>
        /// <returns></returns>
        IPrintPreviewDialogHandler CreatePrintPreviewDialogHandler();
    }
}
