using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxCaretHandler : DisposableObject<IntPtr>, ICaretHandler
    {
        private readonly AbstractControl? control;

        public WxCaretHandler()
            : base(UI.Native.WxOtherFactory.CreateCaret(), true)
        {
        }

        public WxCaretHandler(AbstractControl control, int width, int height)
            : base(UI.Native.WxOtherFactory.CreateCaret2(
                WxApplicationHandler.WxWidget(control),
                width,
                height), true)
        {
            this.control = control;
        }

        public int BlinkTime
        {
            get
            {
                return UI.Native.WxOtherFactory.CaretGetBlinkTime();
            }

            set
            {
                UI.Native.WxOtherFactory.CaretSetBlinkTime(value);
            }
        }

        public SizeI Size
        {
            get
            {
                return UI.Native.WxOtherFactory.CaretGetSize(Handle);
            }

            set
            {
                UI.Native.WxOtherFactory.CaretSetSize(Handle, value.Width, value.Height);
            }
        }

        public PointI Position
        {
            get
            {
                return UI.Native.WxOtherFactory.CaretGetPosition(Handle);
            }

            set
            {
                UI.Native.WxOtherFactory.CaretMove(Handle, value.X, value.Y);
            }
        }

        public bool IsOk
        {
            get
            {
                return UI.Native.WxOtherFactory.CaretIsOk(Handle);
            }
        }

        public bool Visible
        {
            get
            {
                return UI.Native.WxOtherFactory.CaretIsVisible(Handle);
            }

            set
            {
                UI.Native.WxOtherFactory.CaretShow(Handle, value);
            }
        }

        public AbstractControl? Control
        {
            get => control;
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            UI.Native.WxOtherFactory.DeleteCaret(Handle);
        }
    }
}