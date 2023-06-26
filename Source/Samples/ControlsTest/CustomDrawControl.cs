using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI;

namespace ControlsTest
{
    public class CustomDrawControl : Control
    {
        public CustomDrawControl()
        {
        }

        protected override ControlHandler CreateHandler()
        {
            return new CustomHandler();
        }

        public class CustomHandler : ControlHandler<CustomDrawControl>
        {

            protected override bool NeedsPaint => true;

            public override void OnPaint(DrawingContext dc)
            {
                var bounds = ClientRectangle;
                var brush = Control.Background;
                if (brush != null)
                    dc.FillRectangle(brush, bounds);
            }

            protected override void OnAttach()
            {
                base.OnAttach();

                UserPaint = true;
            }

            protected override void OnDetach()
            {
                base.OnDetach();
            }
        }
    }
}
