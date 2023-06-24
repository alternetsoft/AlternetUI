using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extended types of errors that can cause navigation to fail.  Used in the <see cref="WebBrowser"/> control.
    /// </summary>
    internal enum WebBrowserNavigationErrorEx
    {
        ErrorFirst,
        InvalidUrl,
        NoSession,
        CannotConnect,
        ResourceNotFound,
        ObjectNotFound,
        DataNotAvailable,
        DownloadFailure,
        AuthenticationRequired,
        NoValidMedia,
        ConnectionTimeout,
        InvalidRequest,
        UnknownProtocol,
        SecurityProblem,
        CannotLoadData,
        CannotInstantiateObject,
        QueryoptionUnknown,
        RedirectFailed,
        RedirectToDir,
        CannotLockRequest,
        UseExtendBinding,
        TerminatedBind,
        InvalidCertificate,
        CodeDownloadDeclined,
        ResultDispatched,
        CannotReplaceSfpFile,
        CodeInstallBlockedByHashPolicy,
        CodeInstallSuppressed,
    }
    
    /// <summary>
    /// Types of errors that can cause navigation in the <see cref="WebBrowser"/> control to fail. 
    /// </summary>
    public enum WebBrowserNavigationError
    {
        
        /// <summary>
        ///     Connection error (timeout, etc.)
        /// </summary>
        Connection = 0,
        
        /// <summary>
        ///     Invalid certificate.
        /// </summary>
        Certificate = 1,
        
        /// <summary>
        /// Authentication required. 
        /// </summary>
    
        Auth = 2,
        /// <summary>
        /// Other security error. 
        /// </summary>
        Security = 3,
        
        /// <summary>
        /// Requested resource not found. 
        /// </summary>
        NotFound = 4,
        
        /// <summary>
        ///     Invalid request or parameters (e.g. bad URL or protocol, 
        ///     unsupported resource type). 
        /// </summary>
        Request = 5,
        
        /// <summary>
        /// The user cancelled (e.g. in a dialog). 
        /// </summary>
        UserCancelled = 6,
        
        /// <summary>
        ///     Another type of error that didn't fit in other categories.     
        /// </summary>        
        Other = 7,
    };
}
