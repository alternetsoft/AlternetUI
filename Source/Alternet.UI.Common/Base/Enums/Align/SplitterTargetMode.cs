using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies the target element that a splitter should act upon,
    /// based on its relative position in the layout.
    /// </summary>
    public enum SplitterTargetMode
    {
        /// <summary>
        /// Automatically determine the target based on layout context or default behavior.
        /// </summary>
        Auto,

        /// <summary>
        /// Targets the next sibling element in the visual or logical tree.
        /// Typically used when the splitter is placed before the element it resizes.
        /// </summary>
        NextVisibleSibling,

        /// <summary>
        /// Targets the previous sibling element in the visual or logical tree.
        /// Useful when the splitter is placed after the element it resizes.
        /// </summary>
        PreviousVisibleSibling,
    }
}
