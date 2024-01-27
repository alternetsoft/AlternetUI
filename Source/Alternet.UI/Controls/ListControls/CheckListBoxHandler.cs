using System;
using System.Collections.Generic;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides base functionality for implementing a specific
    /// <see cref="CheckListBoxHandler"/> behavior and appearance.
    /// </summary>
    internal abstract class CheckListBoxHandler : ListBoxHandler
    {
        /// <summary>
        /// Gets a <see cref="CheckListBox"/> this handler provides
        /// the implementation for.
        /// </summary>
        public new CheckListBox Control => (CheckListBox)base.Control;
    }
}