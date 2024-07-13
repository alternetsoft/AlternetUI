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

        /// <inheritdoc/>
        public virtual void BeginBusyCursor()
        {
            busyCursorCounter++;
        }

        /// <inheritdoc/>
        public virtual ICursorHandler CreateCursorHandler()
        {
            return new PlessCursorHandler();
        }

        /// <inheritdoc/>
        public virtual ICursorHandler CreateCursorHandler(CursorType cursor)
        {
            return new PlessCursorHandler();
        }

        /// <inheritdoc/>
        public virtual ICursorHandler CreateCursorHandler(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return new PlessCursorHandler();
        }

        /// <inheritdoc/>
        public virtual ICursorHandler CreateCursorHandler(
            Image image,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return new PlessCursorHandler();
        }

        /// <inheritdoc/>
        public virtual ICursorHandler CreateCursorHandler(
            GenericImage image,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return new PlessCursorHandler();
        }

        /// <inheritdoc/>
        public virtual void EndBusyCursor()
        {
            busyCursorCounter--;
        }

        /// <inheritdoc/>
        public virtual bool IsBusyCursor()
        {
            return busyCursorCounter > 0;
        }
    }
}
