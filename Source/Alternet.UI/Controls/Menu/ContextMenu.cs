using Alternet.Drawing;
using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the context menu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ContextMenu"/> class represents shortcut menus that can be displayed when the user clicks the right mouse
    /// button over a control or area of the form. Shortcut menus are typically used to combine different menu items
    /// from a <see cref="MainMenu"/> of a form that are useful for the user given the context of the application. For example, you
    /// can use a shortcut menu assigned to a <see cref="TextBox"/> control to provide menu items for changing the font of the text,
    /// finding text within the control, or <see cref="Clipboard"/> features for copying and pasting text. You can also display new
    /// <see cref="MenuItem"/> objects in a shortcut menu that are not located within a <see cref="MainMenu"/> to provide situation specific
    /// commands that are not appropriate for the <see cref="MainMenu"/> to display.
    /// </para>
    /// </remarks>
    public class ContextMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenu"/> class.
        /// </summary>
        public ContextMenu()
        {
        }

        /// <summary>
        /// Displays the shortcut menu at the specified position.
        /// </summary>
        /// <param name="control">A <see cref="Control"/> that specifies the control with which this shortcut menu is associated.</param>
        /// <param name="position">
        /// A <see cref="Point"/> that specifies the coordinates at which to display the menu. These coordinates are specified relative
        /// to the client coordinates of the control specified in the control parameter.</param>
        /// <remarks>
        /// Typically, a <see cref="ContextMenu"/> is displayed when the user clicks the right mouse button on a control
        /// or area of the form that the <see cref="ContextMenu"/> is bound to. You can use this method to manually display
        /// the shortcut menu at a specific location and bind it with a specific control. This method does not return until the menu is dismissed.
        /// </remarks>
        public void Show(Control control, Point position)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            ((NativeContextMenuHandler)Handler).Show(control, position);
        }
    }
}