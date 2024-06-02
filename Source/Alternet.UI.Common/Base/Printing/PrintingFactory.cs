using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing.Printing
{
    public static class PrintingFactory
    {
        private static IPrintingHandler? handler;

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
