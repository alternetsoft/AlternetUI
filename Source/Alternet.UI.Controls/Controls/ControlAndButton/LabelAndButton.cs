using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="Label"/> with side buttons.
    /// </summary>
    public partial class LabelAndButton : ControlAndButton<Label>
    {
        /// <summary>
        /// Default margin for the label in <see cref="LabelAndButton"/>.
        /// </summary>
        public static Thickness DefaultLabelMargin = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelAndButton"/> class.
        /// </summary>
        public LabelAndButton()
        {
            InnerOuterBorder = InnerOuterSelector.Outer;
            Label.Margin = DefaultLabelMargin;
        }

        /// <summary>
        /// Gets main child control, same as <see cref="ControlAndButton{T}.MainControl"/>.
        /// </summary>
        [Browsable(false)]
        public Label Label => (Label)MainControl;
    }
}
