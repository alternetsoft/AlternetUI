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
    public class Cursor : HandledObject<ICursorHandler>
    {
        private static ICursorFactoryHandler? factory;

        private readonly CursorType? cursorType;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        public Cursor()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class.
        /// </summary>
        public Cursor(ICursorHandler handler)
        {
            Handler = handler;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class
        /// using a cursor identifier.
        /// </summary>
        /// <param name="cursor">Built in cursor type.</param>
        public Cursor(CursorType cursor)
        {
            cursorType = cursor;
            Handler = Factory.CreateCursorHandler(cursor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class
        /// by passing a filename.
        /// </summary>
        /// <param name="cursorName">Cursor filename.</param>
        /// <param name="type">Type of the bitmap.</param>
        /// <param name="hotSpotX">Hot spot X.</param>
        /// <param name="hotSpotY">Hot spot Y.</param>
        public Cursor(
            string cursorName,
            BitmapType type = BitmapType.CursorDefaultType,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            if (GetAllowCustomCursors())
                Handler = Factory.CreateCursorHandler(cursorName, type, hotSpotX, hotSpotY);
            else
                Handler = Factory.CreateCursorHandler(CursorType.Arrow);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class from a stream.
        /// </summary>
        /// <param name="stream">Stream with cursor.</param>
        /// <param name="bitmapType">Type of the cursor.</param>
        public Cursor(Stream stream, BitmapType bitmapType = BitmapType.Any)
            : this(new Bitmap(stream, bitmapType))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class from an image.
        /// </summary>
        /// <param name="image">Image with cursor.</param>
        /// <param name="hotSpotX">Hot spot X.</param>
        /// <param name="hotSpotY">Hot spot Y.</param>
        public Cursor(
            Image image,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            if(GetAllowCustomCursors())
                Handler = Factory.CreateCursorHandler(image, hotSpotX, hotSpotY);
            else
                Handler = Factory.CreateCursorHandler(CursorType.Arrow);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Cursor"/> class from an image.
        /// </summary>
        /// <param name="image">Image with cursor.</param>
        /// <param name="hotSpotX">Hot spot X.</param>
        /// <param name="hotSpotY">Hot spot Y.</param>
        public Cursor(
            GenericImage image,
            int hotSpotX = 0,
            int hotSpotY = 0)
        {
            if (GetAllowCustomCursors())
                Handler = Factory.CreateCursorHandler(image, hotSpotX, hotSpotY);
            else
                Handler = Factory.CreateCursorHandler(CursorType.Arrow);
        }

        /// <summary>
        /// Gets or sets whether custom cursors are allowed. Default is <c>false</c>.
        /// </summary>
        /// <remarks>
        /// When this property is <c>false</c>, all cursors which are not of <see cref="CursorType"/>
        /// will be created as <see cref="CursorType.Arrow"/>.
        /// </remarks>
        public static bool AllowCustomCursors { get; set; } = false;

        /// <summary>
        /// Gets or sets factory handler.
        /// </summary>
        public static ICursorFactoryHandler Factory
        {
            get
            {
                return factory ??= Mouse.Handler.CreateCursorFactoryHandler();
            }

            set => factory = value;
        }

        /// <summary>
        /// Gets type of the cursor if it is known.
        /// </summary>
        public virtual CursorType? KnownCursorType => cursorType;

        /// <summary>
        /// Returns true if cursor data is present.
        /// </summary>
        public virtual bool IsOk
        {
            get
            {
                return Handler.IsOk;
            }
        }

        /// <summary>
        /// Gets hot spot of the cursor.
        /// </summary>
        public virtual PointI HotSpot
        {
            get
            {
                return Handler.GetHotSpot();
            }
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
        public override string? ToString()
        {
            if(cursorType is not null)
                return Enum.GetName(typeof(Cursor), cursorType);
            return base.ToString();
        }

        /// <inheritdoc/>
        protected override ICursorHandler CreateHandler()
        {
            return Factory.CreateCursorHandler();
        }

        private bool GetAllowCustomCursors()
        {
            return AllowCustomCursors && Factory.AllowCustomCursors;
        }
    }
}