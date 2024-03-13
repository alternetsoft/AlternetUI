using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    public class CustomDrawControl : UserControl
    {
        private List<Action<Control, Graphics, RectD>>? paintActions;

        public CustomDrawControl()
        {
        }

        public IList<Action<Control, Graphics, RectD>> PaintActions
        {
            get
            {
                return paintActions ??= new();
            }
        }

        public void SetPaintAction(Action<Control, Graphics, RectD> action)
        {
            PaintActions.Clear();
            PaintActions.Add(action);
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

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
