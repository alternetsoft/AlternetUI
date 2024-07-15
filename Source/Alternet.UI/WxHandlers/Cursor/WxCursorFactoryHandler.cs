using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxCursorFactoryHandler : DisposableObject, ICursorFactoryHandler
    {
        public bool SetGlobal(Cursor? cursor)
        {
            if (cursor is null)
                UI.Native.WxOtherFactory.SetCursor(default);
            else
            {
                var handler = (WxCursorHandler)cursor.Handler;
                UI.Native.WxOtherFactory.SetCursor(handler.Handle);
            }

            return true;
        }

        public bool IsBusyCursor() => Native.WxOtherFactory.IsBusyCursor();

        public void BeginBusyCursor() => Native.WxOtherFactory.BeginBusyCursor();

        public void EndBusyCursor() => Native.WxOtherFactory.EndBusyCursor();

        public ICursorHandler CreateCursorHandler(CursorType cursor)
        {
            return new WxCursorHandler(cursor);
        }

        public ICursorHandler CreateCursorHandler(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return new WxCursorHandler(cursorName, type, hotSpotX, hotSpotY);
        }

        public ICursorHandler CreateCursorHandler(
            Image image,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return new WxCursorHandler(image, hotSpotX, hotSpotY);
        }

        public ICursorHandler CreateCursorHandler(
            GenericImage image,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            return new WxCursorHandler(image, hotSpotX, hotSpotY);
        }

        public ICursorHandler CreateCursorHandler()
        {
            return new WxCursorHandler();
        }
    }
}
