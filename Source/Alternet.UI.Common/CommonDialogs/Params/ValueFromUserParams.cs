using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties which allow to customize dialog box which asks a value from the user.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public class ValueFromUserParams<T>
    {
        /// <summary>
        /// Gets or sets virtual keyboard class used in the input dialog.
        /// </summary>
        /// <remarks>
        /// Not all platforms support this option.
        /// </remarks>
        public VirtualKeyboardClass? VirtualKeyboardClass;

        /// <summary>
        /// Gets or sets virtual keyboard flags used in the input dialog.
        /// </summary>
        /// <remarks>
        /// Not all platforms support this option.
        /// </remarks>
        public VirtualKeyboardFlags? VirtualKeyboardFlags;

        /// <summary>
        /// Gets or sets dialog message.
        /// </summary>
        public string? Message;

        /// <summary>
        /// Gets or sets dialog title. If not specified, default input dialog title is used.
        /// </summary>
        public string? Title;

        /// <summary>
        /// Gets or sets action which is fired when dialog is closed.
        /// </summary>
        public Action<T?>? OnClose;

        /// <summary>
        /// Gets or sets action which is fired when 'Ok' is pressed in the dialog.
        /// </summary>
        public Action<T?>? OnApply;

        /// <summary>
        /// Gets or sets action which is fired when 'Cancel' is pressed in the dialog.
        /// </summary>
        public Action? OnCancel;

        /// <summary>
        /// Gets or sets dialog parent.
        /// </summary>
        public Control? Parent;

        /// <summary>
        /// Gets or sets default value for the input text.
        /// </summary>
        public T? DefaultValue;

        /// <summary>
        /// Gets or sets max length of the input value.
        /// </summary>
        /// <remarks>
        /// Not used on some platforms.
        /// </remarks>
        public int MaxLength = -1;

        /// <summary>
        /// Gets or sets accept button text.
        /// </summary>
        /// <remarks>
        /// Not used on some platforms.
        /// </remarks>
        public string? AcceptButtonText;

        /// <summary>
        /// Gets or sets cancel button text.
        /// </summary>
        /// <remarks>
        /// Not used on some platforms.
        /// </remarks>
        public string? CancelButtonText;

        /// <summary>
        /// Returns <see cref="AcceptButtonText"/> if it's specified;
        /// otherwise returns default text for the accept button.
        /// </summary>
        public virtual string SafeAcceptButtonText => CommonStrings.Default.ButtonOk;

        /// <summary>
        /// Returns <see cref="CancelButtonText"/> if it's specified;
        /// otherwise returns default text for the cancel button.
        /// </summary>
        public virtual string SafeCancelButtonText => CommonStrings.Default.ButtonCancel;

        /// <summary>
        /// Gets <see cref="Message"/> or an empty string.
        /// </summary>
        public virtual string SafeMessage => Message ?? string.Empty;

        /// <summary>
        /// Gets <see cref="DefaultValue"/> as string or an empty string if it is not specified.
        /// </summary>
        public virtual string SafeDefaultValueAsString => DefaultValue?.ToString() ?? string.Empty;

        /// <summary>
        /// Gets <see cref="Title"/> or default input window title.
        /// </summary>
        public virtual string SafeTitle => Title ?? CommonStrings.Default.WindowTitleInput;

        /// <summary>
        /// Raises dialog result actions after dialog is closed.
        /// </summary>
        /// <param name="result"></param>
        public virtual void RaiseActions(T? result)
        {
            OnClose?.Invoke(result);

            if (result is null)
                OnCancel?.Invoke();
            else
                OnApply?.Invoke(result);
        }
    }
}