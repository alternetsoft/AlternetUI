using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the possible results of an exception dialog, which is a dialog that is shown when an unhandled exception occurs
    /// in the application. The user can choose to continue running the application,
    /// quit the application, or throw the exception to be handled by the default exception handler.
    /// </summary>
    public enum ExceptionDialogResult
    {
        /// <summary>
        /// Indicates that no action was taken by the user.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that the user chose to continue running the application, ignoring the exception.
        /// The application will attempt to skip the error and proceed with normal execution.
        /// </summary>
        Continue,
        
        /// <summary>
        /// Indicates that the user chose to quit the application after an exception has occurred.
        /// The application will terminate immediately.
        /// </summary>
        Quit,

        /// <summary>
        /// Indicates that the user chose to throw the exception.
        /// The exception will be propagated to the development environment or the default exception handler.
        /// </summary>
        Throw,
    }

}
