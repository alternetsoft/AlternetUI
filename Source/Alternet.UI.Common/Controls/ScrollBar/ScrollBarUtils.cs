using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which are related to the scrollbars
    /// and control scrolling.
    /// </summary>
    public static class ScrollBarUtils
    {
        /// <summary>
        /// Converts <see cref="RichTextBoxScrollBars"/> to the tuple with two
        /// <see cref="HiddenOrVisible"/> values which specify horizontal and vertical
        /// scrollbar visibility.
        /// </summary>
        /// <param name="scrollbars">Value to convert.</param>
        /// <returns></returns>
        public static (HiddenOrVisible Horizontal, HiddenOrVisible Vertical)
            AsHiddenOrVisible(RichTextBoxScrollBars scrollbars)
        {
            (HiddenOrVisible Horizontal, HiddenOrVisible Vertical) result;

            switch (scrollbars)
            {
                default:
                case RichTextBoxScrollBars.None:
                    result.Vertical = HiddenOrVisible.Hidden;
                    result.Horizontal = HiddenOrVisible.Hidden;
                    break;
                case RichTextBoxScrollBars.Horizontal:
                    result.Vertical = HiddenOrVisible.Hidden;
                    result.Horizontal = HiddenOrVisible.Auto;
                    break;
                case RichTextBoxScrollBars.Vertical:
                    result.Vertical = HiddenOrVisible.Auto;
                    result.Horizontal = HiddenOrVisible.Hidden;
                    break;
                case RichTextBoxScrollBars.Both:
                    result.Vertical = HiddenOrVisible.Auto;
                    result.Horizontal = HiddenOrVisible.Auto;
                    break;
                case RichTextBoxScrollBars.ForcedHorizontal:
                    result.Vertical = HiddenOrVisible.Hidden;
                    result.Horizontal = HiddenOrVisible.Visible;
                    break;
                case RichTextBoxScrollBars.ForcedVertical:
                    result.Vertical = HiddenOrVisible.Visible;
                    result.Horizontal = HiddenOrVisible.Hidden;
                    break;
                case RichTextBoxScrollBars.ForcedBoth:
                    result.Vertical = HiddenOrVisible.Visible;
                    result.Horizontal = HiddenOrVisible.Visible;
                    break;
            }

            return result;
        }
    }
}