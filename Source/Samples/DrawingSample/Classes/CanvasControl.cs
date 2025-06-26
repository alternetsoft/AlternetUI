using Alternet.Drawing;
using Alternet.UI;

namespace DrawingSample
{
    public class CanvasControl : HiddenBorder
    {
        public CanvasControl()
        {
            UserPaint = true;
            BackgroundColor = Color.White;
        }
    }
}