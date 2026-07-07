using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class KnownPopupControls : IPopupEntryHandler
    {
        private readonly List<InnerPopupTextBox> popupTextBoxes = new();

        /// <summary>
        /// Shows the popup entry.
        /// </summary>
        /// <param name="prm">The parameters for the popup entry.</param>
        /// <returns><c>true</c> if the popup entry was shown; otherwise, <c>false</c>.</returns>
        public virtual bool ShowPopupEntry(PopupEntryParams prm)
        {
            var popup = KnownPopupControls.Default.GetOrCreatePopupTextBox();

            if (popup is null)
                return false;

            return popup.ShowAsItemEditor(prm);
        }

        /// <summary>
        /// Closes all popup entries created by this instance of <see cref="KnownPopupControls"/>.
        /// </summary>
        public virtual void CloseAllPopupEntries()
        {
            foreach (var popupTextBox in popupTextBoxes)
            {
                if (popupTextBox.Parent is null)
                    continue;
                popupTextBox.Close(ModalResult.Canceled, new(PopupControl.CloseReason.Other));
            }
        }

        /// <summary>
        /// Closes popup entry which is currently used for editing
        /// the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control.</param>
        /// <returns><c>true</c> if the popup entry was closed; otherwise, <c>false</c>.</returns>
        public virtual bool CloseActivePopupEntry(ObjectUniqueId id)
        {
            var activePopup = GetActivePopupTextBox(id);
            if (activePopup != null)
            {
                activePopup.Close(ModalResult.Canceled, new(PopupControl.CloseReason.Other));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the popup entry is currently being used for editing
        /// by the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control.</param>
        /// <returns><c>true</c> if the popup entry is currently
        /// being used for editing by the control with the specified unique identifier;
        /// otherwise, <c>false</c>.</returns>
        public virtual bool HasActivePopupEntry(ObjectUniqueId id)
        {
            return GetActivePopupTextBox(id) != null;
        }

        /// <summary>
        /// Gets the active instance of <see cref="InnerPopupTextBox"/> control
        /// which is currently used for editing. If no instance is active, returns <c>null</c>.
        /// </summary>
        /// <param name="id">The unique identifier of the control for which popup is used.</param>
        /// <returns>The active instance of <see cref="InnerPopupTextBox"/> control
        /// or <c>null</c> if no instance is active.</returns>
        protected virtual InnerPopupTextBox? GetActivePopupTextBox(ObjectUniqueId id)
        {
            foreach (var popup in popupTextBoxes)
            {
                if (!popup.IsVisible || popup.Parent is null)
                    continue;
                if (popup.TargetControlUniqueId == id)
                    return popup;
            }

            return null;
        }

        /// <summary>
        /// Creates a new instance of <see cref="InnerPopupTextBox"/> control.
        /// Override this method to provide a custom implementation of <see cref="InnerPopupTextBox"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="InnerPopupTextBox"/> control.</returns>
        protected virtual InnerPopupTextBox CreateInnerPopupTextBox()
        {
            var result = new InnerPopupTextBox();
            return result;
        }

        /// <summary>
        /// Gets instance of <see cref="InnerPopupTextBox"/> control.
        /// If it is not created yet, it will be created using <see cref="CreateInnerPopupTextBox"/> method.
        /// A single instance of <see cref="InnerPopupTextBox"/> returned by
        /// this method can be reused for multiple editing operations. For example different
        /// <see cref="VirtualListBox"/> controls share
        /// the same instance of <see cref="InnerPopupTextBox"/> for editing their items.
        /// </summary>
        /// <returns>The instance of <see cref="InnerPopupTextBox"/> control or <c>null</c>
        /// if it cannot be created.</returns>
        protected virtual InnerPopupTextBox? GetOrCreatePopupTextBox(bool hidePopups = true)
        {
            if (hidePopups)
                CloseAllPopupEntries();

            foreach (var popupTextBox in popupTextBoxes)
            {
                if (popupTextBox.Parent is null)
                    return popupTextBox;
            }

            var result = CreateInnerPopupTextBox();
            popupTextBoxes.Add(result);
            return result;
        }
    }
}
