using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for a popup entry control.
    /// </summary>
    public interface IPopupEntry
    {
        /// <summary>
        /// Gets or sets the starting position of the text selection within the popup entry.
        /// </summary>
        int? SelectionStart { get; set; }

        /// <summary>
        /// Gets or sets the length of the text selection within the popup entry.
        /// </summary>
        int? SelectionLength { get; set; }

        /// <summary>
        /// Gets or sets the position of the caret (text cursor) within the popup entry.
        /// </summary>
        PointI? CaretPosition { get; set; }
    }

    /// <summary>
    /// Defines the interface for handling popup entry.
    /// It provides methods for showing and closing popup entry,
    /// as well as checking if a popup entry is currently active.
    /// Popup entry is a UI element that allows users to input or edit text in a popup window.
    /// </summary>
    public interface IPopupEntryHandler
    {
        /// <summary>
        /// Closes all popup entries created by this handler.
        /// </summary>
        void CloseAllPopupEntries();

        /// <summary>
        /// Gets height of the popup entry.
        /// </summary>
        /// <param name="control">The control for which the popup entry is used.</param>
        /// <param name="font">The font used in the popup entry.</param>
        /// <param name="hasBorder">Indicates whether the popup entry has a border.</param>
        /// <returns>The height of the popup entry.</returns>
        float GetPopupEntryHeight(AbstractControl? control, Font font, bool hasBorder);

        /// <summary>
        /// Closes popup entry which is currently used for editing
        /// the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control for which the popup is used.</param>
        /// <param name="result">The modal result to use when closing the popup entry.</param>
        /// <returns><c>true</c> if the popup entry was closed; otherwise, <c>false</c>.</returns>
        bool CloseActivePopupEntry(ObjectUniqueId id, ModalResult result);

        /// <summary>
        /// Gets a value indicating whether the popup entry is currently being used for editing
        /// by the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control.</param>
        /// <returns><c>true</c> if the popup entry is currently
        /// being used for editing by the control with the specified unique identifier;
        /// otherwise, <c>false</c>.</returns>
        bool HasActivePopupEntry(ObjectUniqueId id);

        /// <summary>
        /// Shows the popup entry.
        /// </summary>
        /// <param name="prm">The parameters for the popup entry.</param>
        /// <returns><c>true</c> if the popup entry was shown; otherwise, <c>false</c>.</returns>
        bool ShowPopupEntry(PopupEntryParams prm);

        /// <summary>
        /// Gets the popup entry which is currently used for editing
        /// by the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control which called the popup entry.</param>
        /// <returns>The popup entry used for the control, or <c>null</c> if no popup entry is found.</returns>
        IPopupEntry? GetPopupEntry(ObjectUniqueId id);
    }
}