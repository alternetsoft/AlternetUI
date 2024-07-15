using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    /// <summary>
    /// Contains static methods and properties which allow to create print dialogs.
    /// </summary>
    public static class PrintingFactory
    {
        private static IPrintingHandler? handler;

        /// <summary>
        /// Gets or sets <see cref="IPrintingHandler"/> provider which is used to create
        /// print dialogs.
        /// </summary>
        public static IPrintingHandler Handler
        {
            get
            {
                return handler ??= App.Handler.CreatePrintingHandler();
            }

            set
            {
                handler = value;
            }
        }
    }
}
