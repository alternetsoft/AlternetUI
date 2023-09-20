using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Parent class for all owner draw controls.
    /// </summary>
    [ControlCategory("Other")]
    public class UserPaintControl : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPaintControl"/> class.
        /// </summary>
        public UserPaintControl()
            : base()
        {
            UserPaint = true;
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.UserPaintControl;

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
        }
    }
}
