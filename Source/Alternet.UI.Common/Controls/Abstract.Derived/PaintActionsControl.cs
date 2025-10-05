using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// A custom control for executing paint actions.
    /// </summary>
    public partial class PaintActionsControl : HiddenBorder
    {
        private List<Action<AbstractControl, Graphics, RectD>>? paintActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintActionsControl"/> class.
        /// </summary>
        public PaintActionsControl()
        {
        }

        /// <summary>
        /// Gets list of custom paint actions.
        /// </summary>
        public IList<Action<AbstractControl, Graphics, RectD>> PaintActions
        {
            get
            {
                return paintActions ??= new();
            }
        }

        /// <summary>
        /// Clears <see cref="PaintActions"/> and adds single paint action there.
        /// </summary>
        /// <param name="action">Custom paint action.</param>
        public void SetPaintAction(Action<AbstractControl, Graphics, RectD> action)
        {
            PaintActions.Clear();
            PaintActions.Add(action);
            Invalidate();
        }

        /// <inheritdoc/>
        public override void DefaultPaint(PaintEventArgs e)
        {
            base.DefaultPaint(e);

            if (paintActions is null)
                return;

            foreach(var paintAction in paintActions)
            {
                paintAction(this, e.Graphics, e.ClientRectangle);
            }
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }
    }
}
