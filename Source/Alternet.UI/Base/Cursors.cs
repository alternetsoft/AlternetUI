using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides a collection of <see cref="Cursor"/> objects for use by an application.
    /// </summary>
    public static class Cursors
    {
        private static Cursor? noneCursor;
        private static Cursor? arrowCursor;
        private static Cursor? rightArrowCursor;
        private static Cursor? bullseyeCursor;
        private static Cursor? charCursor;
        private static Cursor? crossCursor;
        private static Cursor? handCursor;
        private static Cursor? iBeamCursor;
        private static Cursor? leftButtonCursor;
        private static Cursor? magnifierCursor;
        private static Cursor? middleButtonCursor;
        private static Cursor? noEntryCursor;
        private static Cursor? paintBrushCursor;
        private static Cursor? pencilCursor;
        private static Cursor? pointLeftCursor;
        private static Cursor? pointRightCursor;
        private static Cursor? questionArrowCursor;
        private static Cursor? rightButtonCursor;
        private static Cursor? sizeNESWCursor;
        private static Cursor? sizeNSCursor;
        private static Cursor? sizeNWSECursor;
        private static Cursor? sizeWECursor;
        private static Cursor? sizingCursor;
        private static Cursor? sprayCanCursor;
        private static Cursor? waitCursor;
        private static Cursor? watchCursor;
        private static Cursor? blankCursor;

        /// <summary>
        /// A value indicating that no cursor should be displayed.
        /// </summary>
        public static Cursor None
        {
            get
            {
                return noneCursor ??= new(CursorType.None);
            }

            set
            {
                noneCursor = value;
            }
        }

        /// <summary>
        /// A standard arrow cursor.
        /// </summary>
        public static Cursor Arrow
        {
            get
            {
                return arrowCursor ??= new(CursorType.Arrow);
            }

            set
            {
                arrowCursor = value;
            }
        }

        /// <summary>
        /// A standard arrow cursor pointing to the right.
        /// </summary>
        public static Cursor RightArrow
        {
            get
            {
                return rightArrowCursor ??= new(CursorType.RightArrow);
            }

            set
            {
                rightArrowCursor = value;
            }
        }

        /// <summary>
        /// Bullseye cursor
        /// </summary>
        public static Cursor Bullseye
        {
            get
            {
                return bullseyeCursor ??= new(CursorType.Bullseye);
            }

            set
            {
                bullseyeCursor = value;
            }
        }

        /// <summary>
        /// Rectangular character cursor.
        /// </summary>
        public static Cursor Char
        {
            get
            {
                return charCursor ??= new(CursorType.Char);
            }

            set
            {
                charCursor = value;
            }
        }

        /// <summary>
        /// A crosshair cursor.
        /// </summary>
        public static Cursor Cross
        {
            get
            {
                return crossCursor ??= new(CursorType.Cross);
            }

            set
            {
                crossCursor = value;
            }
        }

        /// <summary>
        /// A hand cursor.
        /// </summary>
        public static Cursor Hand
        {
            get
            {
                return handCursor ??= new(CursorType.Hand);
            }

            set
            {
                handCursor = value;
            }
        }

        /// <summary>
        /// A text I-Beam cursor (vertical line).
        /// </summary>
        public static Cursor IBeam
        {
            get
            {
                return iBeamCursor ??= new(CursorType.IBeam);
            }

            set
            {
                iBeamCursor = value;
            }
        }

        /// <summary>
        /// Represents a mouse with the left button depressed.
        /// </summary>
        public static Cursor LeftButton
        {
            get
            {
                return leftButtonCursor ??= new(CursorType.LeftButton);
            }

            set
            {
                leftButtonCursor = value;
            }
        }

        /// <summary>
        /// A magnifier icon.
        /// </summary>
        public static Cursor Magnifier
        {
            get
            {
                return magnifierCursor ??= new(CursorType.Magnifier);
            }

            set
            {
                magnifierCursor = value;
            }
        }

        /// <summary>
        /// Represents a mouse with the middle button depressed.
        /// </summary>
        public static Cursor MiddleButton
        {
            get
            {
                return middleButtonCursor ??= new(CursorType.MiddleButton);
            }

            set
            {
                middleButtonCursor = value;
            }
        }

        /// <summary>
        /// A no-entry sign cursor.
        /// </summary>
        public static Cursor NoEntry
        {
            get
            {
                return noEntryCursor ??= new(CursorType.NoEntry);
            }

            set
            {
                noEntryCursor = value;
            }
        }

        /// <summary>
        /// A paintbrush cursor.
        /// </summary>
        public static Cursor PaintBrush
        {
            get
            {
                return paintBrushCursor ??= new(CursorType.PaintBrush);
            }

            set
            {
                paintBrushCursor = value;
            }
        }

        /// <summary>
        /// A pencil cursor.
        /// </summary>
        public static Cursor Pencil
        {
            get
            {
                return pencilCursor ??= new(CursorType.Pencil);
            }

            set
            {
                pencilCursor = value;
            }
        }

        /// <summary>
        /// A cursor that points left.
        /// </summary>
        public static Cursor PointLeft
        {
            get
            {
                return pointLeftCursor ??= new(CursorType.PointLeft);
            }

            set
            {
                pointLeftCursor = value;
            }
        }

        /// <summary>
        /// A cursor that points right.
        /// </summary>
        public static Cursor PointRight
        {
            get
            {
                return pointRightCursor ??= new(CursorType.PointRight);
            }

            set
            {
                pointRightCursor = value;
            }
        }

        /// <summary>
        /// An arrow and question mark.
        /// </summary>
        public static Cursor QuestionArrow
        {
            get
            {
                return questionArrowCursor ??= new(CursorType.QuestionArrow);
            }

            set
            {
                questionArrowCursor = value;
            }
        }

        /// <summary>
        /// Represents a mouse with the right button depressed.
        /// </summary>
        public static Cursor RightButton
        {
            get
            {
                return rightButtonCursor ??= new(CursorType.RightButton);
            }

            set
            {
                rightButtonCursor = value;
            }
        }

        /// <summary>
        /// A cursor with arrows pointing northeast and southwest.
        /// </summary>
        public static Cursor SizeNESW
        {
            get
            {
                return sizeNESWCursor ??= new(CursorType.SizeNESW);
            }

            set
            {
                sizeNESWCursor = value;
            }
        }

        /// <summary>
        /// A cursor with arrows pointing north and south.
        /// </summary>
        public static Cursor SizeNS
        {
            get
            {
                return sizeNSCursor ??= new(CursorType.SizeNS);
            }

            set
            {
                sizeNSCursor = value;
            }
        }

        /// <summary>
        /// A cursor with arrows pointing northwest and southeast.
        /// </summary>
        public static Cursor SizeNWSE
        {
            get
            {
                return sizeNWSECursor ??= new(CursorType.SizeNWSE);
            }

            set
            {
                sizeNWSECursor = value;
            }
        }

        /// <summary>
        /// A cursor with arrows pointing west and east.
        /// </summary>
        public static Cursor SizeWE
        {
            get
            {
                return sizeWECursor ??= new(CursorType.SizeWE);
            }

            set
            {
                sizeWECursor = value;
            }
        }

        /// <summary>
        /// A general sizing cursor.
        /// </summary>
        public static Cursor Sizing
        {
            get
            {
                return sizingCursor ??= new(CursorType.Sizing);
            }

            set
            {
                sizingCursor = value;
            }
        }

        /// <summary>
        /// A spraycan cursor.
        /// </summary>
        public static Cursor SprayCan
        {
            get
            {
                return sprayCanCursor ??= new(CursorType.SprayCan);
            }

            set
            {
                sprayCanCursor = value;
            }
        }

        /// <summary>
        /// An hourglass cursor.
        /// </summary>
        public static Cursor Wait
        {
            get
            {
                return waitCursor ??= new(CursorType.Wait);
            }

            set
            {
                waitCursor = value;
            }
        }

        /// <summary>
        /// A watch cursor.
        /// </summary>
        public static Cursor Watch
        {
            get
            {
                return watchCursor ??= new(CursorType.Watch);
            }

            set
            {
                watchCursor = value;
            }
        }

        /// <summary>
        /// A transparent cursor.
        /// </summary>
        public static Cursor Blank
        {
            get
            {
                return blankCursor ??= new(CursorType.Blank);
            }

            set
            {
                blankCursor = value;
            }
        }
    }
}