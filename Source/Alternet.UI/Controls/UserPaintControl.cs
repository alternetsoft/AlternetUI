using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public class UserPaintControl : Control
    {
        public UserPaintControl()
            : base()
        {
            UserPaint = true;
        }

        public virtual Rect ClientRectangle
        {
            get
            {
                var size = ClientSize;
                return new (0,0, size.Width-1,size.Height-1);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }
    }

}
