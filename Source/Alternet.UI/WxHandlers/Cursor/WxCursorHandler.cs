using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxCursorHandler : DisposableObject<IntPtr>, ICursorHandler
    {
        public WxCursorHandler(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public WxCursorHandler()
            : base(UI.Native.WxOtherFactory.CreateCursor(), true)
        {
        }

        public WxCursorHandler(CursorType cursor)
            : base(UI.Native.WxOtherFactory.CreateCursor2((int)cursor), true)
        {
        }

        public WxCursorHandler(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
            : base(UI.Native.WxOtherFactory
                  .CreateCursor3(cursorName, (int)type, hotSpotX, hotSpotY), true)
        {
        }

        public WxCursorHandler(
            Image image,
            int hotSpotX = 0,
            int hotSpotY = 0)
            : base(
                  UI.Native.WxOtherFactory.CreateCursor4((UI.Native.Image)image.Handler, hotSpotX, hotSpotY),
                  true)
        {
        }

        public WxCursorHandler(
            GenericImage image,
            int hotSpotX = 0,
            int hotSpotY = 0)
            : base(
                  UI.Native.WxOtherFactory.CreateCursor5(
                      ((WxGenericImageHandler)image.Handler).Handle,
                      hotSpotX,
                      hotSpotY),
                  true)
        {
        }

        public bool IsOk
        {
            get => UI.Native.WxOtherFactory.CursorIsOk(Handle);
        }

        public static IntPtr CursorToPtr(Cursor? cursor)
        {
            if (cursor is null)
                return default;
            var handler = (WxCursorHandler)cursor.Handler;
            return handler.Handle;
        }

        public PointI GetHotSpot()
        {
            return UI.Native.WxOtherFactory.CursorGetHotSpot(Handle);
        }

        protected override void DisposeUnmanaged()
        {
            if (Handle == default)
                return;
            UI.Native.WxOtherFactory.DeleteCursor(Handle);
            Handle = default;
        }
    }
}