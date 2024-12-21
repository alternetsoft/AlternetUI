#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Alternet.UI.Port
{
    /// <summary>
    ///     An enunmeration describing the status of a DispatcherOperation.
    /// </summary>
    ///
    internal enum DispatcherOperationStatus
    {
        /// <summary>
        ///     The operation is still pending.
        /// </summary>
        Pending,

        /// <summary>
        ///     The operation has been aborted.
        /// </summary>
        Aborted,

        /// <summary>
        ///     The operation has been completed.
        /// </summary>
        Completed,
        
        /// <summary>
        ///     The operation has started executing, but has not completed yet.
        /// </summary>
        Executing
    }
}


