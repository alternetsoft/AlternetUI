using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class PlessCaretHandler : DisposableObject, ICaretHandler
    {
        private static int blinkTime;

        private SizeI size;
        private PointI position;
        private bool visible;
        private bool isOk = true;
        private Control? control;

        public PlessCaretHandler()
        {

        }

        public PlessCaretHandler(Control control, int width, int height)
        {
            size = (width, height);
            this.control = control;
        }

        public Control? Control
        {
            get => control;

            set => control = value;
        }

        public int BlinkTime
        {
            get => blinkTime;
            set => blinkTime = value;
        }

        public SizeI Size
        {
            get => size;
            set => size = value;
        }

        public PointI Position
        {
            get => position;
            set => position = value;
        }

        public bool Visible
        {
            get => visible;
            set => visible = value;
        }

        public bool IsOk
        {
            get => isOk;
            set => isOk = value;
        }
    }
}
