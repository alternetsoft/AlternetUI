using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// This interface is used when object is assigned to <see cref="CustomTextBox.ValidatorReporter"/>
    /// property and validation error is shown.
    /// </summary>
    public interface IValidatorReporter
    {
        /// <summary>
        /// Sets validation error status for the specified control.
        /// </summary>
        /// <param name="sender">Control with a validation error.</param>
        /// <param name="showError">Indicates whether to show/hide error.</param>
        /// <param name="errorText">Specifies error text.</param>
        void SetErrorStatus(object? sender, bool showError, string? errorText);
    }
}
