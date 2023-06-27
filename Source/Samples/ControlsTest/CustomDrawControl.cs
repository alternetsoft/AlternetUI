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
            private readonly Control leftControl = new ();
            private readonly Control fillControl = new ();

            public CustomHandler()
                : base()
            {
                leftControl.Bounds = new (0, 0, 50, 50);
                fillControl.Bounds = new (0, 0, 50, 50);
                leftControl.Background = new SolidBrush(Color.Green);
                fillControl.Background = new SolidBrush(Color.Red);
            }

            protected override bool NeedsPaint => true;

            public override void OnPaint(DrawingContext dc)
            {
                var bounds = ClientRectangle;
                LayoutFactory.PerformLayoutLeftFill(Control, leftControl, fillControl);

                var brush = Control.Background;
                if (brush != null)
                    dc.FillRectangle(brush, bounds);
                dc.FillRectangle(leftControl.Background!, leftControl.Bounds);
                dc.FillRectangle(fillControl.Background!, fillControl.Bounds);
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
