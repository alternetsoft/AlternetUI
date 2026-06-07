using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /*
        Do not change the order of the items in this enum or element indexes.
        Do not remove or add items.
        This enum is mapped to the native cursor types and is used in interop calls, so changing it may break functionality.
    */

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
        Arrow = 1,

        /// <summary>
        /// Rectangular character cursor.
        /// </summary>
        Char = 4,

        /// <summary>
        /// A crosshair cursor.
        /// </summary>
        Cross = 5,

        /// <summary>
        /// A hand cursor.
        /// </summary>
        Hand = 6,

        /// <summary>
        /// A text I-Beam cursor (vertical line).
        /// </summary>
        IBeam = 7,

        /// <summary>
        /// Represents a mouse with the left button depressed.
        /// </summary>
        LeftButton = 8,

        /// <summary>
        /// Represents a mouse with the middle button depressed.
        /// </summary>
        MiddleButton = 10,

        /// <summary>
        /// A no-entry sign cursor.
        /// </summary>
        NoEntry = 11,

        /// <summary>
        /// An arrow and question mark.
        /// </summary>
        QuestionArrow = 16,

        /// <summary>
        /// Represents a mouse with the right button depressed.
        /// </summary>
        RightButton = 17,

        /// <summary>
        /// A cursor with arrows pointing northeast and southwest.
        /// </summary>
        SizeNESW = 18,

        /// <summary>
        /// A cursor with arrows pointing north and south.
        /// </summary>
        SizeNS = 19,

        /// <summary>
        /// A cursor with arrows pointing northwest and southeast.
        /// </summary>
        SizeNWSE = 20,

        /// <summary>
        /// A cursor with arrows pointing west and east.
        /// </summary>
        SizeWE = 21,

        /// <summary>
        /// A general sizing cursor.
        /// </summary>
        Sizing = 22,

        /// <summary>
        /// An hourglass cursor.
        /// </summary>
        Wait = 24,

        /// <summary>
        /// A watch cursor.
        /// </summary>
        Watch = 25,
    }
}