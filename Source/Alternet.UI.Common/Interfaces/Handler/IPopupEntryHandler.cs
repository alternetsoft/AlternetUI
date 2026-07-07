using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
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
        /// Closes popup entry which is currently used for editing
        /// the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control for which the popup is used.</param>
        /// <returns><c>true</c> if the popup entry was closed; otherwise, <c>false</c>.</returns>
        bool CloseActivePopupEntry(ObjectUniqueId id);

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
    }
}