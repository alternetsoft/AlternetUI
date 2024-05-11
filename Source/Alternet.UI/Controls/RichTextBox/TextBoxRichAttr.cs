using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class TextBoxRichAttr : TextBoxTextAttr, ITextBoxRichAttr
    {
        public TextBoxRichAttr()
            : base(Native.TextBoxTextAttr.CreateRichTextAttr())
        {
        }

        public TextBoxRichAttr(IntPtr handle)
            : base(handle)
        {
        }

        protected override void DisposeUnmanagedResources()
        {
            Native.TextBoxTextAttr.DeleteRichTextAttr(Handle);
        }
    }
}
