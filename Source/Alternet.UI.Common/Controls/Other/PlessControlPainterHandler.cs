using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides methods to paint and measure control parts such as checkboxes, expanders, and header buttons.
    /// </summary>
    public class PlessControlPainterHandler : DisposableObject, IControlPainterHandler
    {
        /// <summary>
        /// Gets or sets the default size of a check mark in device-independent units.
        /// </summary>
        public static SizeD DefaultCheckMarkSize = (16, 16);

        /// <summary>
        /// Gets or sets the default size of a checkbox in device-independent units.
        /// </summary>
        public static SizeD DefaultCheckBoxSize = (13, 13);

        /// <summary>
        /// Gets or sets the default size of an expander in device-independent units.
        /// </summary>
        public static SizeD DefaultExpanderSize = (9, 9);

        /// <summary>
        /// Gets or sets the default height of a header button in device-independent units.
        /// </summary>
        public static Coord DefaultHeaderButtonHeight = 23;

        /// <summary>
        /// Gets or sets the default margin of a
        /// header button's label in device-independent units.
        /// </summary>
        public static Coord DefaultHeaderButtonMargin = 6;

        /// <summary>
        /// Gets or sets the default size of a collapse button in device-independent units.
        /// </summary>
        public static SizeD DefaultCollapseButtonSize = (19, 21);

        /// <inheritdoc/>
        public virtual void DrawCheckBox(
            AbstractControl control,
            Graphics canvas,
            RectD rect,
            CheckState checkState,
            VisualControlState controlState)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public SizeD GetCheckBoxSize(
            AbstractControl control,
            CheckState checkState = CheckState.Unchecked,
            VisualControlState controlState = VisualControlState.Normal)
        {
            return DefaultCheckBoxSize;
        }

        /// <inheritdoc/>
        public virtual SizeD GetCheckMarkSize(AbstractControl control)
        {
            return DefaultCheckMarkSize;
        }

        /// <inheritdoc/>
        public virtual SizeD GetCollapseButtonSize(AbstractControl control, Graphics dc)
        {
            return DefaultCollapseButtonSize;
        }

        /// <inheritdoc/>
        public virtual SizeD GetExpanderSize(AbstractControl control)
        {
            return DefaultExpanderSize;
        }

        /// <inheritdoc/>
        public virtual Coord GetHeaderButtonHeight(AbstractControl control)
        {
            return DefaultHeaderButtonHeight;
        }

        /// <inheritdoc/>
        public virtual Coord GetHeaderButtonMargin(AbstractControl control)
        {
            return DefaultHeaderButtonMargin;
        }
    }
}
