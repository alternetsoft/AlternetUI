using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PlessCursorFactoryHandler : DisposableObject, ICursorFactoryHandler
    {
        private int busyCursorCounter;

        public virtual void BeginBusyCursor()
        {
            busyCursorCounter++;
        }

        public virtual ICursorHandler CreateCursorHandler()
        {
            return new PlessCursorHandler();
        }

        public virtual ICursorHandler CreateCursorHandler(CursorType cursor)
        {
            return new PlessCursorHandler();
        }

        public virtual ICursorHandler CreateCursorHandler(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return new PlessCursorHandler();
        }

        public virtual ICursorHandler CreateCursorHandler(Image image)
        {
            return new PlessCursorHandler();
        }

        public virtual void EndBusyCursor()
        {
            busyCursorCounter--;
        }

        public virtual bool IsBusyCursor()
        {
            return busyCursorCounter > 0;
        }

        public virtual bool SetGlobal(Cursor? cursor)
        {
            return false;
        }
    }
}
