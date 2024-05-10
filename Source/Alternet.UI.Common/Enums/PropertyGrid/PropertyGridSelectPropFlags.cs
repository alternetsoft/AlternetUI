using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies <see cref="PropertyGrid"/> select property flags.
    /// </summary>
    [Flags]
    public enum PropertyGridSelectPropFlags
    {
        /// <summary>
        /// Focuses to created editor.
        /// </summary>
        Focus = 0x0001,

        /// <summary>
        /// Forces deletion and recreation of editor.
        /// </summary>
        Force = 0x0002,

        /// <summary>
        /// For example, doesn't cause EnsureVisible.
        /// </summary>
        NonVisible = 0x0004,

        /// <summary>
        /// Do not validate editor's value before selecting.
        /// </summary>
        NoValidate = 0x0008,

        /// <summary>
        /// Property being deselected is about to be deleted.
        /// </summary>
        Deleting = 0x0010,

        /// <summary>
        /// Property's values was set to unspecified by the user.
        /// </summary>
        SetUnspecified = 0x0020,

        /// <summary>
        /// Property's event handler changed the value
        /// </summary>
        DialogValue = 0x0040,

        /// <summary>
        /// Set to disable sending of Selected event.
        /// </summary>
        DontSendEvent = 0x0080,

        /// <summary>
        /// Don't make any graphics updates.
        /// </summary>
        NoRefresh = 0x0100,
    }
}
