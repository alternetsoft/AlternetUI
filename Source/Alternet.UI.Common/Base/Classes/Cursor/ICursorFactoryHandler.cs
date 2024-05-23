using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface ICursorFactoryHandler : IDisposable
    {
        bool IsBusyCursor();

        void BeginBusyCursor();

        void EndBusyCursor();

        ICursorHandler CreateCursorHandler();

        ICursorHandler CreateCursorHandler(CursorType cursor);

        ICursorHandler CreateCursorHandler(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0);

        ICursorHandler CreateCursorHandler(Image image);

        bool SetGlobal(Cursor? cursor);
    }
}
