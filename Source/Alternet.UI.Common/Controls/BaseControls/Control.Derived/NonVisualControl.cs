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
    /// Base class for all non visual controls.
    /// </summary>
    [ControlCategory("Hidden")]
    public partial class NonVisualControl : Control
    {
        /// <summary>
        /// This property has no meaning.
        /// </summary>
        [Browsable(false)]
        public override Coord Left { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public override Coord Top { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public new Coord Width { get; set; }

        /// <inheritdoc cref="Left"/>
        [Browsable(false)]
        public new Coord Height { get; set; }

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
