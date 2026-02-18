using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Enumerates known layout methods. This is for internal use only.
        /// </summary>
        public enum DefaultLayoutMethod
        {
            /// <summary>
            /// Original layout method.
            /// </summary>
            Original,

            /// <summary>
            /// New layout method.
            /// </summary>
            New,
        }

        /// <summary>
        /// Enumerates known handler types. This is for internal use only.
        /// </summary>
        public enum HandlerType
        {
            /// <summary>
            /// Native handler. Depends on the type of control for which it is created.
            /// </summary>
            Native,

            /// <summary>
            /// Generic handler. Creates handler suitable for owner drawn controls.
            /// </summary>
            Generic,
        }

        /// <summary>
        /// Enumerates known suspend flags.
        /// </summary>
        /// <remarks>
        /// This enumeration supports a bitwise combination of its member values.
        /// </remarks>
        [Flags]
        public enum SuspendFlags
        {
            /// <summary>
            /// Whether to suspend layout.
            /// </summary>
            Layout,

            /// <summary>
            /// Whether to suspend paint.
            /// </summary>
            Update,

            /// <summary>
            /// Whether to suspend selection changed events.
            /// It is up to control to decide what exactly events are suspended.
            /// </summary>
            Selection,

            /// <summary>
            /// Whether to suspend property changed events.
            /// It is up to control to decide what exactly events are suspended.
            /// </summary>
            PropChange,

            /// <summary>
            /// Whether to call <see cref="BeginInit"/> and <see cref="EndInit"/>.
            /// </summary>
            Init,
        }
    }
}
