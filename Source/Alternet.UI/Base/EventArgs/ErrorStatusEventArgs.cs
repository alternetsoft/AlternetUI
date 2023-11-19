using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle <see cref="CustomTextEdit.ErrorStatusChanged"/>
    /// and similar events.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">An object that contains an event data.</param>
    public delegate void ErrorStatusEventHandler(
        object? sender,
        ErrorStatusEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="CustomTextEdit.ErrorStatusChanged"/> and similar events.
    /// </summary>
    public class ErrorStatusEventArgs : EventArgs
    {
        private readonly bool showError;
        private readonly string? errorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorStatusEventArgs"/> class.
        /// </summary>
        /// <param name="showError">Indicates whether to show/hide error.</param>
        /// <param name="errorMessage">Specifies error message.</param>
        public ErrorStatusEventArgs(bool showError, string? errorMessage)
        {
            this.showError = showError;
            this.errorMessage = errorMessage;
        }

        /// <summary>
        /// Gets whether to show/hide error.
        /// </summary>
        public bool ShowError => showError;

        /// <summary>
        /// Gets an error message.
        /// </summary>
        public string? ErrorMessage => errorMessage;
    }
}
