using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IToolTipFactoryHandler : IDisposable
    {
        IRichToolTipHandler CreateRichToolTipHandler(
            string title,
            string message,
            bool useGeneric);

        bool SetReshow(long msecs);

        bool SetEnabled(bool flag);

        bool SetAutoPop(long msecs);

        bool SetDelay(long msecs);

        bool SetMaxWidth(int width);
    }
}
