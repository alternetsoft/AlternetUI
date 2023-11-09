using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// A cursor is a small bitmap usually used for denoting where the mouse pointer is, with
    /// a picture that might indicate the interpretation of a mouse click.
    /// </summary>
    public class Cursor : DisposableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        public Cursor()
            : base(Native.WxOtherFactory.CreateCursor(), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class
        /// using a cursor identifier.
        /// </summary>
        /// <param name="cursor">Built in cursor type.</param>
        public Cursor(CursorType cursor)
            : base(Native.WxOtherFactory.CreateCursor2((int)cursor), true)
        {
        }

        internal Cursor(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class
        /// by passing a string resource name or filename.
        /// </summary>
        /// <param name="cursorName"></param>
        /// <param name="type"></param>
        /// <param name="hotSpotX"></param>
        /// <param name="hotSpotY"></param>
        internal Cursor(
            string cursorName,
            int type,
            int hotSpotX = 0,
            int hotSpotY = 0)
            : base(
                  Native.WxOtherFactory.CreateCursor3(
                    cursorName,
                    (int)type,
                    hotSpotX,
                    hotSpotY), true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class from an image.
        /// </summary>
        /// <param name="image">Image with cursor.</param>
        internal Cursor(Image image)
            : base(Native.WxOtherFactory.CreateCursor4(image.NativeImage), true)
        {
        }

        /// <summary>
        /// Returns true if cursor data is present.
        /// </summary>
        public bool IsOk
        {
            get
            {
                return Native.WxOtherFactory.CursorIsOk(Handle);
            }
        }

        /// <summary>
        /// Gets hot spot of the cursor.
        /// </summary>
        public Int32Point HotSpot
        {
            get
            {
                return Native.WxOtherFactory.CursorGetHotSpot(Handle);
            }
        }

        /// <inheritdoc/>
        protected override void DisposeUnmanagedResources()
        {
            Native.WxOtherFactory.DeleteCursor(Handle);
        }
    }
}