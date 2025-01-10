using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Gets a value indicating which of the modifier keys (SHIFT, CTRL, and ALT) is in
        /// a pressed state.</summary>
        /// <returns>
        /// A bitwise combination of the <see cref="Keys" /> values.
        /// The default is <see cref="Keys.None" />.</returns>
        protected static Keys ModifierKeys
        {
            get
            {
                var modifiers = Keyboard.Modifiers;
                return modifiers.ToKeys();
            }
        }

        /// <summary>
        /// Gets whether this control is dummy control.
        /// </summary>
        protected virtual bool IsDummy => false;

        /// <summary>
        /// Gets minimal margin value.
        /// </summary>
        [Browsable(false)]
        protected virtual Thickness MinMargin
        {
            get
            {
                if (minMargin == null)
                {
                    var value = AllPlatformDefaults.
                        GetAsThickness(ControlKind, ControlDefaultsId.MinMargin);
                    minMargin = value;
                }

                return minMargin.Value;
            }
        }

        /// <summary>
        /// Gets minimal padding value.
        /// </summary>
        [Browsable(false)]
        protected virtual Thickness MinPadding
        {
            get
            {
                if (minPadding == null)
                {
                    minPadding = AllPlatformDefaults.
                        GetAsThickness(ControlKind, ControlDefaultsId.MinPadding);
                }

                return minPadding.Value;
            }
        }
    }
}
