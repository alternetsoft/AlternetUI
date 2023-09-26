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
#if DEBUG
        private bool drawDebugPointsBefore;
        private bool drawDebugPointsAfter;
#endif

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

        /// <summary>
        /// Gets or sets whether to draw debug related points for the owner draw controls.
        /// </summary>
#if DEBUG
#else
        [Browsable(false)]
#endif
        public bool DrawDebugPointsBefore
        {
            get
            {
#if DEBUG
                return drawDebugPointsBefore;
#else
                return false;
#endif
            }

            set
            {
#if DEBUG
                if (drawDebugPointsBefore == value)
                    return;
                drawDebugPointsBefore = value;
                Refresh();
#else
#endif
            }
        }

        /// <summary>
        /// Gets or sets whether to draw debug related points for the owner draw controls.
        /// </summary>
#if DEBUG
#else
        [Browsable(false)]
#endif
        public bool DrawDebugPointsAfter
        {
            get
            {
#if DEBUG
                return drawDebugPointsAfter;
#else
                return false;
#endif
            }

            set
            {
#if DEBUG
                if (drawDebugPointsAfter == value)
                    return;
                drawDebugPointsAfter = value;
                Refresh();
#else
#endif
            }
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
        }
    }
}
