using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        internal static SizeD GetPreferredSizeWhenHorizontal(
            AbstractControl container,
            SizeD availableSize)
        {
            return OldLayout.GetPreferredSizeWhenHorizontal(container, availableSize);
        }

        internal static SizeD GetPreferredSizeWhenVertical(
            AbstractControl container,
            SizeD availableSize)
        {
            return OldLayout.GetPreferredSizeWhenVertical(container, availableSize);
        }

        internal static void LayoutWhenHorizontal(
            AbstractControl container,
            RectD childrenLayoutBounds,
            IReadOnlyList<AbstractControl> controls)
        {
            OldLayout.LayoutWhenHorizontal(container, childrenLayoutBounds, controls);
        }

        internal static void LayoutWhenVertical(
            AbstractControl container,
            RectD lBounds,
            IReadOnlyList<AbstractControl> items)
        {
            OldLayout.LayoutWhenVertical(container, lBounds, items);
        }

        // On return, 'bounds' has an empty space left after docking the controls to sides
        // of the container (fill controls are not counted).
        // On return, 'result' has number of controls with Dock != None.
        internal static int LayoutWhenDocked(
            AbstractControl parent,
            ref RectD bounds,
            IReadOnlyList<AbstractControl> children)
        {
            return OldLayout.LayoutWhenDocked(parent, ref bounds, children);
        }
    }
}
