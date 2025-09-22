using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxDisplayHandler : DisposableObject<IntPtr>, IDisplayHandler
    {
        public bool IsOk
        {
            get
            {
                return UI.Native.WxOtherFactory.DisplayIsOk(Handle);
            }
        }

        public WxDisplayHandler(int index)
            : base(UI.Native.WxOtherFactory.CreateDisplay2((uint)index), true)
        {
        }

        public WxDisplayHandler()
            : base(UI.Native.WxOtherFactory.CreateDisplay(), true)
        {
        }

        public string GetName()
        {
            return UI.Native.WxOtherFactory.DisplayGetName(Handle);
        }

        public SizeI GetDPI()
        {
            return UI.Native.WxOtherFactory.DisplayGetPPI(Handle);
        }

        public Coord GetScaleFactor()
        {
            if (App.IsWindowsOS)
                return UI.Native.WxOtherFactory.DisplayGetScaleFactor(Handle);
            return 1f;
        }

        public bool IsPrimary()
        {
            return UI.Native.WxOtherFactory.DisplayIsPrimary(Handle);
        }

        public RectI GetClientArea()
        {
            return UI.Native.WxOtherFactory.DisplayGetClientArea(Handle);
        }

        public RectI GetGeometry()
        {
            return UI.Native.WxOtherFactory.DisplayGetGeometry(Handle);
        }

        protected override void DisposeUnmanaged()
        {
            base.DisposeUnmanaged();
            UI.Native.WxOtherFactory.DeleteDisplay(Handle);
        }
    }
}