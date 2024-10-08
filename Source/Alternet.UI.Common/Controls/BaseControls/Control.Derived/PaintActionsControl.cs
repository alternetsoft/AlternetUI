﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="UserControl"/> descendant with <see cref="PaintActions"/> property.
    /// Extends it's ancestor with an ability to call custom paint actions.
    /// </summary>
    public class PaintActionsControl : UserControl
    {
        private List<Action<Control, Graphics, RectD>>? paintActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaintActionsControl"/> class.
        /// </summary>
        public PaintActionsControl()
        {
        }

        /// <summary>
        /// Gets list of custom paint actions.
        /// </summary>
        public IList<Action<Control, Graphics, RectD>> PaintActions
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
        public void SetPaintAction(Action<Control, Graphics, RectD> action)
        {
            PaintActions.Clear();
            PaintActions.Add(action);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;
            var bounds = DrawClientRectangle;

            var brush = this.Background;
            if (brush != null)
                dc.FillRectangle(brush, bounds);

            if (paintActions is null)
                return;

            foreach(var paintAction in paintActions)
            {
                paintAction(this, e.Graphics, e.ClipRectangle);
            }
        }
    }
}
