using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>Specifies the built in cursor types.</summary>
    public enum CursorType
    {
        /// <summary>
        /// A value indicating that no cursor should be displayed.
        /// </summary>
        None = 0,

        /// <summary>
        /// A standard arrow cursor.
        /// </summary>
        Arrow,

        /// <summary>
        /// A standard arrow cursor pointing to the right.
        /// </summary>
        RightArrow,

        /// <summary>
        /// Bullseye cursor
        /// </summary>
        Bullseye,

        /// <summary>
        /// Rectangular character cursor.
        /// </summary>
        Char,

        /// <summary>
        /// A crosshair cursor.
        /// </summary>
        Cross,

        /// <summary>
        /// A hand cursor.
        /// </summary>
        Hand,

        /// <summary>
        /// A text I-Beam cursor (vertical line).
        /// </summary>
        IBeam,

        /// <summary>
        /// Represents a mouse with the left button depressed.
        /// </summary>
        LeftButton,

        /// <summary>
        /// A magnifier icon.
        /// </summary>
        Magnifier,

        /// <summary>
        /// Represents a mouse with the middle button depressed.
        /// </summary>
        MiddleButton,

        /// <summary>
        /// A no-entry sign cursor.
        /// </summary>
        NoEntry,

        /// <summary>
        /// A paintbrush cursor.
        /// </summary>
        PaintBrush,

        /// <summary>
        /// A pencil cursor.
        /// </summary>
        Pencil,

        /// <summary>
        /// A cursor that points left.
        /// </summary>
        PointLeft,

        /// <summary>
        /// A cursor that points right.
        /// </summary>
        PointRight,

        /// <summary>
        /// An arrow and question mark.
        /// </summary>
        QuestionArrow,

        /// <summary>
        /// Represents a mouse with the right button depressed.
        /// </summary>
        RightButton,

        /// <summary>
        /// A cursor with arrows pointing northeast and southwest.
        /// </summary>
        SizeNESW,

        /// <summary>
        /// A cursor with arrows pointing north and south.
        /// </summary>
        SizeNS,

        /// <summary>
        /// A cursor with arrows pointing northwest and southeast.
        /// </summary>
        SizeNWSE,

        /// <summary>
        /// A cursor with arrows pointing west and east.
        /// </summary>
        SizeWE,

        /// <summary>
        /// A general sizing cursor.
        /// </summary>
        Sizing,

        /// <summary>
        /// A spraycan cursor.
        /// </summary>
        SprayCan,

        /// <summary>
        /// An hourglass cursor.
        /// </summary>
        Wait,

        /// <summary>
        /// A watch cursor.
        /// </summary>
        Watch,

        /// <summary>
        /// A transparent cursor.
        /// </summary>
        Blank,
    }
}