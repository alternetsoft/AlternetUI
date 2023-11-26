using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Control state flags used in <see cref="NativeControlPainter"/>.
    /// </summary>
    [Flags]
    internal enum NativeControlPainterFlags
    {
        None = 0x00000000,  // absence of any other flags
        Disabled = 0x00000001,  // control is disabled
        Focused = 0x00000002,  // currently has keyboard focus
        Pressed = 0x00000004,  // (button) is pressed
        Special = 0x00000008,  // control-specific bit:
        IsDefault = Special, // only for the buttons
        IsSubMenu = Special, // only for the menu items
        EXPANDED = Special, // only for the tree items
        SizeGrip = Special, // only for the status bar panes
        Flat = Special, // checkboxes only: flat border
        Cell = Special, // only for item selection rect
        Current = 0x00000010,  // mouse is currently over the control
        Selected = 0x00000020,  // selected item in e.g. listbox
        Checked = 0x00000040,  // (check/radio button) is checked
        Checkable = 0x00000080,  // (menu) item can be checked
        Undetermined = Checkable, // (check) undetermined state
    }
}