#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Specialized;
using System.Configuration;


namespace Alternet.UI.Port
{
    internal static class BaseCompatibilityPreferences
    {
        public const bool ReuseDispatcherSynchronizationContextInstance = false;

        public const bool FlowDispatcherSynchronizationContextPriority = true;

        public const bool InlineDispatcherSynchronizationContextSend = true;

        public const bool MatchPackageSignatureMethodToPackagePartDigestMethod = true;

        /// <summary>
        ///     A Dispatcher can become unresponsive when it is unable to
        ///     set a timer or post a message to itself.  This failure is usually
        ///     the fault of the application, for posting messages faster than
        ///     the Dispatcher can handle them, or for starving the Dispatcher's
        ///     message pump (or both).
        ///
        ///     As an aid in diagnosing the root cause of this non-responsiveness,
        ///     an app can control how Dispatcher reacts to these failures by setting
        ///     the HandleDispatcherRequestProcessingFailure property.
        /// </summary>
        public static HandleDispatcherRequestProcessingFailureOptions
            HandleDispatcherRequestProcessingFailure
        {
            // no need for lock or seal - this value can be changed at any time, and
            // the "last writer wins" behavior is fine if multiple threads are involved.
            get { return handleDispatcherRequestProcessingFailure; }
            set { handleDispatcherRequestProcessingFailure = value; }
        }

        /// <summary>
        /// The HandleDispatcherRequestProcessingFailureOptions enumeration describes
        /// how Dispatcher reacts to failures encountered while requesting processing.
        /// Dispatcher tries to set a timer or post a message to itself, either
        /// of which can fail if the underlying OS resource is exhausted.
        /// </summary>
        public enum HandleDispatcherRequestProcessingFailureOptions
        {
            /// <summary>
            ///     Continue after the failure.
            ///     The Dispatcher may become unresponsive.
            ///     This is the default, and the behavior of Alternet UI prior to version 4.7.1.
            /// </summary>
            Continue = 0,

            /// <summary>
            ///     Throw an exception.
            ///     This brings the problem to the attention of the app author immediately.
            /// </summary>
            Throw = 1,

            /// <summary>
            ///     Reset the Dispatcher's state to try another request the next
            ///     time one is needed.
            ///     While this can sometimes "repair" non-responsiveness, it cannot honor
            ///     the usual timing of processing, which can be crucial.
            ///     Using this option can lead to unexpected behavior.
            /// </summary>
            Reset = 2,
        }

        private static HandleDispatcherRequestProcessingFailureOptions
                        handleDispatcherRequestProcessingFailure;
    }
}
