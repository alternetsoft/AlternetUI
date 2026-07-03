using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class KnownPopupControls
    {
        private readonly List<InnerPopupTextBox> popupTextBoxes = new();

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
        public virtual InnerPopupTextBox? GetOrCreatePopupTextBox(bool hidePopups = true)
        {
            if (hidePopups)
                CloseAllPopupTextBoxes();

            foreach (var popupTextBox in popupTextBoxes)
            {
                if (popupTextBox.Parent is null)
                    return popupTextBox;
            }

            var result = CreateInnerPopupTextBox();
            popupTextBoxes.Add(result);
            return result;
        }

        /// <summary>
        /// Closes all popup text boxes created by this instance of <see cref="KnownPopupControls"/>.
        /// </summary>
        public virtual void CloseAllPopupTextBoxes()
        {
            foreach (var popupTextBox in popupTextBoxes)
            {
                if (popupTextBox.Parent is null)
                    continue;
                popupTextBox.Close(ModalResult.Canceled, new(PopupControl.CloseReason.Other));
            }
        }

        /// <summary>
        /// Closes popup text box which is currently used for editing
        /// the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control.</param>
        /// <returns><c>true</c> if the popup text box was closed; otherwise, <c>false</c>.</returns>
        public virtual bool CloseActivePopupTextBox(ObjectUniqueId id)
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
        /// Gets the active instance of <see cref="InnerPopupTextBox"/> control
        /// which is currently used for editing. If no instance is active, returns <c>null</c>.
        /// </summary>
        /// <param name="id">The unique identifier of the control for which popup is used.</param>
        /// <returns>The active instance of <see cref="InnerPopupTextBox"/> control
        /// or <c>null</c> if no instance is active.</returns>
        public virtual InnerPopupTextBox? GetActivePopupTextBox(ObjectUniqueId id)
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
        /// Gets a value indicating whether the <see cref="InnerPopupTextBox"/> is currently being used for editing
        /// by the control with the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the control.</param>
        /// <returns><c>true</c> if the <see cref="InnerPopupTextBox"/> is currently
        /// being used for editing by the control with the specified unique identifier;
        /// otherwise, <c>false</c>.</returns>
        public virtual bool HasActivePopupTextBox(ObjectUniqueId id)
        {
            return GetActivePopupTextBox(id) != null;
        }

        /// <summary>
        /// Creates a new instance of <see cref="InnerPopupTextBox"/> control.
        /// Override this method to provide a custom implementation of <see cref="InnerPopupTextBox"/>.
        /// </summary>
        /// <returns>A new instance of the <see cref="InnerPopupTextBox"/> control.</returns>
        public virtual InnerPopupTextBox CreateInnerPopupTextBox()
        {
            var result = new InnerPopupTextBox();
            return result;
        }
    }
}
