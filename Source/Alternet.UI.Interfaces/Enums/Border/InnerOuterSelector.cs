using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known inner/outer selector members.
    /// </summary>
    public enum InnerOuterSelector
    {
        /// <summary>
        /// None is visible.
        /// </summary>
        None,

        /// <summary>
        /// Inner is selected.
        /// </summary>
        Inner,

        /// <summary>
        /// Outer is selected.
        /// </summary>
        Outer,

        /// <summary>
        /// Both inner and outer are selected.
        /// </summary>
        Both,
    }
}
