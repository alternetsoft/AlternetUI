using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class SizerItem : DisposableObject, ISizerItem
    {
        public SizerItem(
            Control window,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
            : this(
                  Native.SizerItem.CreateSizerItem(
                    window.WxWidget,
                    proportion,
                    (int)flag,
                    border,
                    default), true)
        {
        }

        public SizerItem(Control window, ISizerFlags sizerFlags)
            : this(Native.SizerItem.CreateSizerItem2(window.WxWidget, sizerFlags.Handle), true)
        {
        }

        public SizerItem(
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
            : this(
                  Native.SizerItem.CreateSizerItem3(
                      sizer.Handle,
                      proportion,
                      (int)flag,
                      border,
                      default), true)
        {
        }

        public SizerItem(ISizer sizer, ISizerFlags sizerFlags)
            : this(Native.SizerItem.CreateSizerItem4(sizer.Handle, sizerFlags.Handle), true)
        {
        }

        public SizerItem(
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
            : this(
                  Native.SizerItem.CreateSizerItem5(
                    width,
                    height,
                    proportion,
                    (int)flag,
                    border,
                    default), true)
        {
        }

        public SizerItem(int width, int height, ISizerFlags sizerFlags)
            : this(Native.SizerItem.CreateSizerItem6(width, height, sizerFlags.Handle), true)
        {
        }

        public SizerItem()
            : this(Native.SizerItem.CreateSizerItem7(), true)
        {
        }

        public SizerItem(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }
    }
}
