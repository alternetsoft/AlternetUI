using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for all non visual controls like <see cref="Menu"/>,
    /// <see cref="StatusBar"/> and other.
    /// </summary>
    [ControlCategory("Hidden")]
    public class NonVisualControl : Control
    {
        /// <summary>
        /// This property has no meaning in the <see cref="NonVisualControl"/> descendants.
        /// </summary>
        [Browsable(false)]
        public override double Left { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override double Top { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override double Width { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override double Height { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override Thickness Margin { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override Thickness Padding { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override Color? BackgroundColor { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override Color? ForegroundColor { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override Font? Font { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override VerticalAlignment VerticalAlignment { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override HorizontalAlignment HorizontalAlignment { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override bool TabStop { get; set; }
    }
}
