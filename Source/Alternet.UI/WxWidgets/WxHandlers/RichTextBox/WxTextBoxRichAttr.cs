using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class WxTextBoxRichAttr : WxTextBoxTextAttr, ITextBoxRichAttr
    {
        public WxTextBoxRichAttr()
            : base(Native.TextBoxTextAttr.CreateRichTextAttr())
        {
        }

        public WxTextBoxRichAttr(IntPtr handle)
            : base(handle)
        {
        }

        protected override void DisposeUnmanaged()
        {
            Native.TextBoxTextAttr.DeleteRichTextAttr(Handle);
        }
    }
}
