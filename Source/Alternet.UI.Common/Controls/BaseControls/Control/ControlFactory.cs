using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public static class ControlFactory
    {
        private static IControlFactoryHandler? handler;

        public static IControlFactoryHandler Handler
        {
            get
            {
                return handler ??= App.Handler.CreateControlFactoryHandler();
            }

            set
            {
                handler = value;
            }
        }
    }
}
