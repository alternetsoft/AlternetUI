using Alternet.Drawing;
using Alternet.UI;

namespace DrawingContextTutorial
{
    public class DrawingControl : Control
    {
        public DrawingControl()
        {
            UserPaint = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }
    }
}