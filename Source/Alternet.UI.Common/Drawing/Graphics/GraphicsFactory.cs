using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public static class GraphicsFactory
    {
        private static IGraphicsFactoryHandler? handler;

        public static IGraphicsFactoryHandler Handler
        {
            get => handler ??= App.Handler.CreateGraphicsFactoryHandler();

            set => handler = value;
        }
    }
}
