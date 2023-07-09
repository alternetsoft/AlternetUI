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

        protected override void OnPaint(PaintEventArgs e)
        {
        }
    }

}
