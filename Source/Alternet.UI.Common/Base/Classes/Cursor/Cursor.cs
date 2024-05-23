using System;
using System.Collections.Generic;
using System.IO;
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
    public class Cursor : HandledObject<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        public Cursor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        public Cursor(object nativeCursor)
        {
            Handler = nativeCursor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class
        /// using a cursor identifier.
        /// </summary>
        /// <param name="cursor">Built in cursor type.</param>
        public Cursor(CursorType cursor)
        {
            Handler = NativeDrawing.Default.CreateCursor(cursor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class
        /// by passing a string resource name or filename.
        /// </summary>
        /// <param name="cursorName"></param>
        /// <param name="type"></param>
        /// <param name="hotSpotX"></param>
        /// <param name="hotSpotY"></param>
        public Cursor(
            string cursorName,
            BitmapType type,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            Handler = NativeDrawing.Default.CreateCursor(
                    cursorName,
                    type,
                    hotSpotX,
                    hotSpotY);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class from a stream.
        /// </summary>
        /// <param name="stream">Stream with cursor.</param>
        /// <param name="bitmapType">Type of the cursor.</param>
        public Cursor(Stream stream, BitmapType bitmapType = BitmapType.Any)
            : this(new Image(stream, bitmapType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class from an image.
        /// </summary>
        /// <param name="image">Image with cursor.</param>
        public Cursor(Image image)
        {
            Handler = NativeDrawing.Default.CreateCursor(image);
        }

        /// <summary>
        /// Returns true if cursor data is present.
        /// </summary>
        public bool IsOk
        {
            get
            {
                return NativeDrawing.Default.CursorIsOk(this);
            }
        }

        /// <summary>
        /// Gets hot spot of the cursor.
        /// </summary>
        public PointI HotSpot
        {
            get
            {
                return NativeDrawing.Default.CursorGetHotSpot(this);
            }
        }

        /// <summary>
        /// Sets global cursor for the application.
        /// </summary>
        /// <param name="cursor">Cursor.</param>
        /// <remarks>
        /// Pass <c>null</c> to reset global cursor. Use <see cref="Control.Cursor"/>
        /// to set cursor for the control.
        /// </remarks>
        public static void SetGlobal(Cursor? cursor = null)
        {
            NativeDrawing.Default.CursorSetGlobal(cursor);
        }

        /// <summary>
        /// Gets whether the specified cursor is Windows standard cursor
        /// (is provided by the OS).
        /// </summary>
        /// <param name="cursor">Cursor identifier.</param>
        public static bool IsStandardCursorWindows(CursorType cursor)
        {
#pragma warning disable
            switch (cursor)
            {
                case CursorType.None:
                case CursorType.Arrow:
                case CursorType.Char:
                case CursorType.Cross:
                case CursorType.Hand:
                case CursorType.IBeam:
                case CursorType.LeftButton:
                case CursorType.MiddleButton:
                case CursorType.NoEntry:
                case CursorType.QuestionArrow:
                case CursorType.RightButton:
                case CursorType.SizeNESW:
                case CursorType.SizeNS:
                case CursorType.SizeNWSE:
                case CursorType.SizeWE:
                case CursorType.Sizing:
                case CursorType.Wait:
                case CursorType.Watch:
                    return true;
                default:
                    return false;
            }
#pragma warning restore
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            NativeDrawing.Default.DisposeCursor(this);
        }

        /// <inheritdoc/>
        protected override object CreateHandler()
        {
            return NativeDrawing.Default.CreateCursor();
        }
    }
}